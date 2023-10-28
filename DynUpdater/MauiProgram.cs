using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
namespace DynUpdater
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            builder.Services.AddSingleton<MainPage>();
#if DEBUG
            builder.Logging.AddDebug();
#endif

#if ANDROID
            builder.Services.AddTransient<IServiceTest, DemoServices>();
#endif
            return builder.Build();
        }
    }

    public interface IServiceTest
    {
        void Start();
        void Stop();
    }
}