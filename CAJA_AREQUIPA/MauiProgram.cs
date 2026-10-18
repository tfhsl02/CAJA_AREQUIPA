using CajaArequipa.Helpers;
using CajaArequipa.Servicios;
using CajaArequipa.UI;

namespace CajaArequipa
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            MauiAppBuilder builder = MauiApp.CreateBuilder();

            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });


            builder.Services.AddSingleton(new ProcesadorOperaciones(CarpetaDatos));

            builder.Services.AddSingleton<MarcoPage>();

            EstiloControles.Aplicar();

            return builder.Build();
        }

        public static string CarpetaDatos
        {
            get { return Path.Combine(FileSystem.AppDataDirectory, "Data"); }
        }
    }
}
