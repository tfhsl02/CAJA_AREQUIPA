using Microsoft.Extensions.DependencyInjection;
using CajaArequipa.UI;

namespace CajaArequipa
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            UserAppTheme = AppTheme.Dark;
        }

        protected override Window CreateWindow(IActivationState activationState)
        {

            MarcoPage marco = IPlatformApplication.Current.Services.GetRequiredService<MarcoPage>();

            Window ventana = new Window(marco);
            ventana.Title = "Caja Arequipa";
            return ventana;
        }
    }
}
