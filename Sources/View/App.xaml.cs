using View.ViewModels;
using ViewModel;

namespace View;

public partial class App : Application
{
	public AppManagerVM AppManagerVM { get; private set; }

    public ChampionManagerVM ChampionManagerVM { get; set; }

    public App(IServiceProvider serviceProvider)
	{
		InitializeComponent();
        AppManagerVM = serviceProvider.GetService<AppManagerVM>()!;
        ChampionManagerVM = serviceProvider.GetService<ChampionManagerVM>()!;
        ChampionManagerVM.InitializeDataCommand.Execute(null);
        MainPage = new AppShell();
	}
}