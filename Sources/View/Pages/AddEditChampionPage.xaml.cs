using View.ViewModels;
using ViewModel;

namespace View.Pages;

public partial class AddEditChampionPage : ContentPage
{
	public AppManagerVM AppManagerVM { get; private set; }

	public EditableChampionVM ChampionVM { get; private set; }

	public AddEditChampionPage(AppManagerVM appManagerVM, EditableChampionVM editableChampionVM)
	{
		AppManagerVM = appManagerVM;
		ChampionVM = editableChampionVM;
		InitializeComponent();
		BindingContext = this;
	}
}
