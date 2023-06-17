namespace View.Pages;
using ViewModel;
using StubLib;
using View.ViewModels;

public partial class ChampionsPage : ContentPage
{
	public AppManagerVM Manager { get; } = (Application.Current as App)!.AppManagerVM;

    public ChampionsPage()
	{
		InitializeComponent();
		BindingContext = Manager;
	}
}
