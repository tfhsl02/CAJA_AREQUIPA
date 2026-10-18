using System.Collections.Generic;
using CajaArequipa.Helpers;
using CajaArequipa.Servicios;
using Microsoft.Maui.Controls.Shapes;

namespace CajaArequipa.UI
{
    // Marco de la aplicación: cabecera, menú (lateral en pantallas anchas, horizontal
    // en celulares) y la sección que se está mostrando.
    public partial class MarcoPage : ContentPage
    {
        private class Seccion
        {
            public string Titulo;
            public string Icono;
            public string IconoActivo;
            public ContentView Vista;
            public Border ItemLateral;
            public Border ItemHorizontal;
        }

        private const double ANCHO_MINIMO_MENU_LATERAL = 720;

        private List<Seccion> secciones = new List<Seccion>();
        private int indiceActual = -1;

        public MarcoPage(ProcesadorOperaciones procesador)
        {
            InitializeComponent();

            AgregarSeccion("Inicio", "ico_inicio", new InicioPage(procesador));
            AgregarSeccion("Agencias", "ico_agencias", new AgenciasPage(procesador));
            AgregarSeccion("Conexiones", "ico_conexiones", new ConexionesPage(procesador));
            AgregarSeccion("Operaciones", "ico_operaciones", new OperacionesPage(procesador));
            AgregarSeccion("Búsqueda", "ico_busqueda", new BusquedaPage(procesador));
            AgregarSeccion("Rutas", "ico_rutas", new RutasPage(procesador));

            for (int i = 0; i < secciones.Count; i++)
            {
                secciones[i].ItemLateral = CrearItemMenu(secciones[i], i);
                secciones[i].ItemHorizontal = CrearItemMenu(secciones[i], i);
                navV.Children.Add(secciones[i].ItemLateral);
                navH.Children.Add(secciones[i].ItemHorizontal);
            }

            SizeChanged += (sender, e) => AjustarDiseno();

            Navegar(0);
        }

        private void AgregarSeccion(string titulo, string icono, ContentView vista)
        {
            Seccion seccion = new Seccion();
            seccion.Titulo = titulo;
            // En MAUI los SVG se referencian con extensión .png (se convierten al compilar).
            seccion.Icono = icono + ".png";
            seccion.IconoActivo = icono + "_on.png";
            seccion.Vista = vista;
            secciones.Add(seccion);
        }

        private Border CrearItemMenu(Seccion seccion, int indice)
        {
            Image icono = new Image();
            icono.WidthRequest = 18;
            icono.HeightRequest = 18;
            icono.VerticalOptions = LayoutOptions.Center;

            Label texto = new Label();
            texto.Text = seccion.Titulo;
            texto.FontSize = 14;
            texto.VerticalOptions = LayoutOptions.Center;

            HorizontalStackLayout contenido = new HorizontalStackLayout();
            contenido.Spacing = 10;
            contenido.Children.Add(icono);
            contenido.Children.Add(texto);

            RoundRectangle forma = new RoundRectangle();
            forma.CornerRadius = new CornerRadius(8);

            Border item = new Border();
            item.Padding = new Thickness(12, 10);
            item.StrokeShape = forma;
            item.StrokeThickness = 1;
            item.Content = contenido;

            TapGestureRecognizer toque = new TapGestureRecognizer();
            toque.Tapped += (sender, e) => Navegar(indice);
            item.GestureRecognizers.Add(toque);

            return item;
        }

        private void PintarItem(Border item, Seccion seccion, bool activo)
        {
            HorizontalStackLayout contenido = (HorizontalStackLayout)item.Content;
            Image icono = (Image)contenido.Children[0];
            Label texto = (Label)contenido.Children[1];

            icono.Source = activo ? seccion.IconoActivo : seccion.Icono;
            texto.TextColor = activo ? Colors.White : Paleta.TextoMenu;
            item.BackgroundColor = activo ? Paleta.NavActivo : Colors.Transparent;
            item.Stroke = activo ? Paleta.NavActivoBorde : Colors.Transparent;
        }

        private void Navegar(int indice)
        {
            indiceActual = indice;

            for (int i = 0; i < secciones.Count; i++)
            {
                bool activo = i == indice;
                PintarItem(secciones[i].ItemLateral, secciones[i], activo);
                PintarItem(secciones[i].ItemHorizontal, secciones[i], activo);
            }

            Seccion seccion = secciones[indice];
            contenedor.Content = seccion.Vista;
            ((IVistaRefrescable)seccion.Vista).AlMostrar();
        }

        // En pantallas anchas: menú lateral. En celulares: menú horizontal bajo la cabecera.
        private void AjustarDiseno()
        {
            if (Width <= 0)
            {
                return;
            }

            bool ancho = Width >= ANCHO_MINIMO_MENU_LATERAL;

            cuerpo.ColumnDefinitions[0].Width = new GridLength(ancho ? 260 : 0);
            cuerpo.ColumnSpacing = ancho ? 10 : 0;
            panelMenu.IsVisible = ancho;
            scrMenuH.IsVisible = !ancho;
        }
    }
}
