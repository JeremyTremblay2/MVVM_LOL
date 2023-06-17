using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using Model;
using StubLib;
using View.ViewModels;
using ViewModel;

namespace View;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
            .UseMauiCommunityToolkit()
			.AddModels()
			.AddViewModels()
			.AddViews()
            .ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}

    public static MauiAppBuilder AddModels(this MauiAppBuilder builder)
    {
        builder.Services.AddSingleton<IDataManager, StubData>();
        return builder;
    }

    public static MauiAppBuilder AddViewModels(this MauiAppBuilder builder)
    {
        builder.Services.AddSingleton<ChampionManagerVM>();
        return builder;
    }

    public static MauiAppBuilder AddViews(this MauiAppBuilder builder)
    {
        builder.Services.AddSingleton<AppManagerVM>();
        return builder;
    }
}