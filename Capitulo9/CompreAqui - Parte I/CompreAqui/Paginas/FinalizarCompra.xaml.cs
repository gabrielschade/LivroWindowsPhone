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

namespace CompreAqui.Paginas
{
    public partial class FinalizarCompra : PhoneApplicationPage
    {
        public FinalizarCompra()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            ObterPosicaoAtual();
        }

        private async void ObterPosicaoAtual()
        {
            Geolocator localizador = new Geolocator();
            localizador.DesiredAccuracy = PositionAccuracy.Default;

            try
            {
                await System.Threading.Tasks.Task.Delay(1000);
                Geoposition posicaoAtual = await localizador.GetGeopositionAsync();
                MarcarPosicaoNoMapa(posicaoAtual.Coordinate.Latitude, posicaoAtual.Coordinate.Longitude, Colors.Red);
                Mapa.Center = new GeoCoordinate(posicaoAtual.Coordinate.Latitude, posicaoAtual.Coordinate.Longitude);
                Mapa.ZoomLevel = 15.5;
            }
            catch (Exception ex)
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

        private void AumentarZoom_Click(object sender, RoutedEventArgs e)
        {
            AlterarAcumulativoZoom(3);
        }

        private void DiminuirZoom_Click(object sender, RoutedEventArgs e)
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

        private void LocalizarEndereco_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {

        }


    }
}