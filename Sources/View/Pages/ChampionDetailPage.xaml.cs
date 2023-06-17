using View.ViewModels;
using ViewModel;

namespace View.Pages;

public partial class ChampionDetailPage : ContentPage
{
    public ChampionVM ChampionVM => championVM;
    private readonly ChampionVM championVM;

    public AppManagerVM AppManagerVM { get; private set; }

    public ChampionDetailPage(AppManagerVM mainAppVM, ChampionVM championVM)
    {
        AppManagerVM = mainAppVM;
        this.championVM = championVM;
        InitializeComponent();
        BindingContext = ChampionVM;
    }
}
