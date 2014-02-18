using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using CompreAqui.Resources;
using CompreAqui.Modelos;
using Newtonsoft.Json;
using System.IO;
using System.Windows.Resources;
using CompreAqui.Auxiliar;

namespace CompreAqui
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            //Categoria Dispositivos = new Categoria();
            //Dispositivos.Id = 1;
            //Dispositivos.Descricao = "Dispositivos";
            //Dispositivos.DescricaoDetalhada = "Dispositivos para uso pessoal, nesta categoria você encontra os melhores tablets, smartphones, notebooks e computadores de mesa";
            ////Dispositivos.Produtos = new List<Produto>();

            //Categoria Cameras = new Categoria();
            //Cameras.Id = 2;
            //Cameras.Descricao = "Câmeras";
            //Cameras.DescricaoDetalhada = "Câmeras de todos os tipos, nesta categoria você encontra desde dipositivos para tirar ótimas fotos quanto para fazer filmagens fantásticas";
            ////Cameras.Produtos = new List<Produto>();

            //Categoria Acessorios = new Categoria();
            //Acessorios.Id = 3;
            //Acessorios.Descricao = "Acessórios";
            //Acessorios.DescricaoDetalhada = "Acessórios para todos os gostos, nesta categoria você encontra os melhores acessórios para turbinar seu computador";
            ////Acessorios.Produtos = new List<Produto>();

            //Produto camera = new Produto();
            //camera.Id = 1;
            //camera.Descricao = "Câmera Fotográfica PXR";
            //camera.DescricaoDetalhada = "Câmera Fotográfica PXR, ótimas configurações: 18 MP, sensor de luminosidade, zoom 5x e muito mais.";
            //camera.AvaliacaoMedia = 3.5;
            //camera.Preco = 400.00;
            //camera.IconName = "camera.png";
            //camera.Categoria = Cameras;
            ////Cameras.Produtos.Add(camera);

            //Produto filmadora = new Produto();
            //filmadora.Id = 2;
            //filmadora.Descricao = "Câmera Filmadora Z650N";
            //filmadora.DescricaoDetalhada = "Câmera Filmadora Z650N, possui filmagem em ultra HD e 120 FPS.";
            //filmadora.AvaliacaoMedia = 4.5;
            //filmadora.Preco = 2400.00;
            //filmadora.PrecoPromocao = 2000.00;
            //camera.IconName = "filmadora.png";
            //filmadora.Categoria = Cameras;
            ////Cameras.Produtos.Add(filmadora);

            //Produto computador = new Produto();
            //computador.Id = 3;
            //computador.Descricao = "Computador PH 220";
            //computador.DescricaoDetalhada = "Computador PH 220 com as seguintes configurações: 250 GB SSD, 8 GB RAM e sistema operacional Doors.";
            //computador.AvaliacaoMedia = 4;
            //computador.Preco = 1400.00;
            //computador.IconName = "desktop.png";
            //computador.Categoria = Dispositivos;
            ////Dispositivos.Produtos.Add(computador);

            //Produto tablet = new Produto();
            //tablet.Id = 4;
            //tablet.Descricao = "Tablet Pinapple iTab";
            //tablet.DescricaoDetalhada = "Tablet Pinapple iTab, ótimo para trabalhar e para se divertir, conta com 64 GB armazenamento, 1 GB RAM e sistema operacional SOI.";
            //tablet.AvaliacaoMedia = 5;
            //tablet.Preco = 2500.00;
            //tablet.IconName = "tablet.png";
            //tablet.Categoria = Dispositivos;
            ////Dispositivos.Produtos.Add(tablet);

            //Produto notebook = new Produto();
            //notebook.Id = 5;
            //notebook.Descricao = "Notebook lemoderno";
            //notebook.DescricaoDetalhada = "Notebook lemoderno, possui luzes integradas no teclado e ótimas configurações, além de uma garantia de 3 anos.";
            //notebook.AvaliacaoMedia = 4;
            //notebook.Preco = 2100.00;
            //notebook.PrecoPromocao = 1900.00;
            //notebook.IconName = "notebook.png";
            //notebook.Categoria = Dispositivos;
            ////Dispositivos.Produtos.Add(notebook);

            //Produto smartphone1 = new Produto();
            //smartphone1.Id = 6;
            //smartphone1.Descricao = "Smartphone Cyborgue";
            //smartphone1.DescricaoDetalhada = "Smartphone Cyborgue, com a versão 5.0 do sistema operacional mais utilizado no mercado.";
            //smartphone1.AvaliacaoMedia = 3.5;
            //smartphone1.Preco = 650.00;
            //smartphone1.IconName = "smartphone.jpg";
            //smartphone1.Categoria = Dispositivos;
            ////Dispositivos.Produtos.Add(smartphone1);

            //Produto smartphone2 = new Produto();
            //smartphone2.Id = 7;
            //smartphone2.Descricao = "Smartphone Pineapple";
            //smartphone2.DescricaoDetalhada = "Smartphone Pineapple, ótimo smartphone, conta com uma performance excepcional e ótimos aplicativos.";
            //smartphone2.AvaliacaoMedia = 5;
            //smartphone2.Preco = 2500.00;
            //smartphone2.IconName = "smartphone.jpg";
            //smartphone2.Categoria = Dispositivos;
            ////Dispositivos.Produtos.Add(smartphone2);

            //Produto smartphone3 = new Produto();
            //smartphone3.Id = 8;
            //smartphone3.Descricao = "Smartphone Doors Phone";
            //smartphone3.DescricaoDetalhada = "Smartphone Doors Phone, smartphone vivo e com uma proposta diferente dos demais, custo benefício ótimo e você mesmo pode criar aplicativos.";
            //smartphone3.AvaliacaoMedia = 5;
            //smartphone3.Preco = 1000.00;
            //smartphone3.PrecoPromocao = 500.00;
            //smartphone3.IconName = "smartphone.jpg";
            //smartphone3.Categoria = Dispositivos;
            ////Dispositivos.Produtos.Add(smartphone3);

            //Produto fones = new Produto();
            //fones.Id = 9;
            //fones.Descricao = "Headset RC2000";
            //fones.DescricaoDetalhada = "O melhor Headset, tanto para jogos, quanto para filmes e músicas.";
            //fones.AvaliacaoMedia = 4;
            //fones.Preco = 1000.00;
            //fones.IconName = "fones.jpg";
            //fones.Categoria = Acessorios;
            ////Acessorios.Produtos.Add(fones);

            //Produto mouse = new Produto();
            //mouse.Id = 10;
            //mouse.Descricao = "Mouse PRT10";
            //mouse.DescricaoDetalhada = "Mouse com excelente precisão, compatível com vários sistemas operacionais.";
            //mouse.AvaliacaoMedia = 4.1;
            //mouse.Preco = 90.00;
            //mouse.IconName = "mouse.jpg";
            //mouse.Categoria = Acessorios;
            ////Acessorios.Produtos.Add(mouse);

            //Produto teclado = new Produto();
            //teclado.Id = 11;
            //teclado.Descricao = "Teclado YX";
            //teclado.DescricaoDetalhada = "teclado compatível com vários sistemas operacionais.";
            //teclado.AvaliacaoMedia = 2.1;
            //teclado.Preco = 130.00;
            //teclado.IconName = "keyboard.ico";
            //teclado.Categoria = Acessorios;
            ////Acessorios.Produtos.Add(teclado);

            //Loja compreaqui = new Loja();
            //compreaqui.Produtos = new List<Produto>();
            //compreaqui.Produtos.Add(camera);
            //compreaqui.Produtos.Add(filmadora);
            //compreaqui.Produtos.Add(computador);
            //compreaqui.Produtos.Add(tablet);
            //compreaqui.Produtos.Add(notebook);
            //compreaqui.Produtos.Add(smartphone1);
            //compreaqui.Produtos.Add(smartphone2);
            //compreaqui.Produtos.Add(smartphone3);
            //compreaqui.Produtos.Add(fones);
            //compreaqui.Produtos.Add(mouse);
            //compreaqui.Produtos.Add(teclado);

            //string completeJson = JsonConvert.SerializeObject(compreaqui);
            //completeJson = null;

            string s = LeitorArquivo.Ler ("/CompreAqui;component/Resources/dados.txt");
            Loja loja = JsonConvert.DeserializeObject<Loja>(s);
        }

    }
}