using Microsoft.Extensions.DependencyInjection;
using CajaArequipa.UI;

namespace CajaArequipa
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            // La interfaz está diseñada solo en modo oscuro.
            UserAppTheme = AppTheme.Dark;
        }

        protected override Window CreateWindow(IActivationState activationState)
        {
            // La página se crea aquí (y no en el constructor) para que los recursos
            // de App.xaml ya estén cargados cuando el XAML pide los estilos.
            MarcoPage marco = IPlatformApplication.Current.Services.GetRequiredService<MarcoPage>();

            Window ventana = new Window(marco);
            ventana.Title = "Caja Arequipa";
            return ventana;
        }
    }
}
