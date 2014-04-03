using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Windows.Devices.Geolocation;
using System.Windows.Shapes;
using System.Windows.Media;
using Microsoft.Phone.Maps.Controls;
using System.Device.Location;
using Microsoft.Phone.Maps.Services;
using System.Threading.Tasks;

namespace CompreAqui.Paginas
{
    public partial class FinalizarCompra : PhoneApplicationPage
    {
        public FinalizarCompra()
        {
            InitializeComponent();
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            ObterPosicaoAtual();
        }

        private async void ObterPosicaoAtual()
        {
            Geolocator localizador = new Geolocator();
            localizador.DesiredAccuracy = PositionAccuracy.Default;

            try
            {
                Geoposition posicaoAtual = await localizador.GetGeopositionAsync();
                MarcarPosicaoNoMapa(posicaoAtual.Coordinate.Latitude, posicaoAtual.Coordinate.Longitude, Colors.Red);
                Mapa.Center = new GeoCoordinate(posicaoAtual.Coordinate.Latitude, posicaoAtual.Coordinate.Longitude);
                Mapa.ZoomLevel = 15.5;
            }
            catch
            {
                MessageBox.Show("Não foi possível encontrar sua localização. Por favor, verifique se suas configurações de localização estão habilitadas.");
            }
        }

        private void MarcarPosicaoNoMapa(double latitude, double longitude, Color cor)
        {
            Ellipse marcacao = new Ellipse();
            marcacao.Fill = new SolidColorBrush(cor);
            marcacao.Height = 10;
            marcacao.Width = 10;

            MapLayer camada = new MapLayer();

            MapOverlay sobrecamada = new MapOverlay();
            sobrecamada.Content = marcacao;
            sobrecamada.GeoCoordinate = new GeoCoordinate(latitude, longitude);

            camada.Add(sobrecamada);

            Mapa.Layers.Add(camada);
        }

        private void AumentarZoom_Tap(object sender, RoutedEventArgs e)
        {
            AlterarAcumulativoZoom(3);
        }

        private void DiminuirZoom_Tap(object sender, RoutedEventArgs e)
        {
            AlterarAcumulativoZoom(-3);
        }

        private void AlterarAcumulativoZoom(double zoomLevel)
        {
            double zoomLevelLimite = Mapa.ZoomLevel + zoomLevel;

            if (zoomLevel > 0)
                zoomLevelLimite = Math.Min(20, zoomLevelLimite);
            else
                zoomLevelLimite = Math.Max(1, zoomLevelLimite);

            Mapa.ZoomLevel = zoomLevelLimite;
        }

        private void AdicionarEndereco_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            BuscarPorEndereco(Logradouro.Text, Cidade.Text);
        }

        private void BuscarPorEndereco(string logradouro, string cidade)
        {
            GeocodeQuery pesquisa = new GeocodeQuery();
            pesquisa.MaxResultCount = 1;
            pesquisa.SearchTerm = string.Concat(logradouro, ", ", cidade);
            pesquisa.GeoCoordinate = new GeoCoordinate(0, 0);
            pesquisa.QueryCompleted += pesquisa_QueryCompleted;
            pesquisa.QueryAsync();

            Logradouro.Text = string.Empty;
            Cidade.Text = string.Empty;
        }

        private void pesquisa_QueryCompleted(object sender, QueryCompletedEventArgs<IList<MapLocation>> e)
        {
            if( e.Result.Count >0)
            {
                GeoCoordinate coordenadas = e.Result.First().GeoCoordinate;
                MarcarPosicaoNoMapa(coordenadas.Latitude, coordenadas.Longitude, Colors.Blue);
            }
            else
            {
                MessageBox.Show("Desculpe, mas o endereço não foi encontrado.");
            }
        }

        private void FinalizarCompra_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            MessageBox.Show("Compra efetuada com sucesso!");
            NavigationService.GoBack();
        }



    }
}