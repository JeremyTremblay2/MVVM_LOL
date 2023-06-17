namespace View.Pages;
using ViewModel;
using StubLib;
using View.ViewModels;

public partial class ChampionsPage : ContentPage
{
	public AppManagerVM AppManagerVM { get; } = (Application.Current as App)!.AppManagerVM;

    public ChampionsPage()
	{
		InitializeComponent();
        AppManagerVM.Navigation = this.Navigation;
        BindingContext = AppManagerVM;
	}
}
