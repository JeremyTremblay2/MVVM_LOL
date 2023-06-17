using System;
using System.Windows.Input;
using View.Pages;
using ViewModel;

namespace View.ViewModels
{
	public class AppManagerVM
	{
        public INavigation Navigation { get; set; }

        public ChampionManagerVM ChampionManagerVM => (Application.Current as App)!.ChampionManagerVM;

        public ICommand NavToAddChampionCommand { get; private set; }
        public ICommand NavToUpdateChampionCommand { get; private set; }
        public ICommand NavToSelectChampionCommand { get; private set; }

        public AppManagerVM()
		{
            NavToAddChampionCommand = new Command(
                execute: async () => await NavToAddChampion(),
                canExecute: () => ChampionManagerVM is not null
            );
            NavToUpdateChampionCommand = new Command<ChampionVM>(
                execute: async (championToUpdate) => await NavToUpdateChampion(championToUpdate),
                canExecute: championToUpdate => ChampionManagerVM is not null && championToUpdate is not null
            );
            NavToSelectChampionCommand = new Command<ChampionVM>(
                execute: async (selectedChampion) => await NavToSelectChampion(selectedChampion),
                canExecute: selectedChampion => ChampionManagerVM is not null && selectedChampion is not null
            );
        }

        private async Task NavToAddChampion()
        {
            if (Navigation is null) return;
            await Navigation.PushModalAsync(new HomePage());
        }

        private async Task NavToUpdateChampion(ChampionVM championToUpdate)
        {
            if (Navigation is null) return;
            await Navigation.PushModalAsync(new HomePage());
        }

        private async Task NavToSelectChampion(ChampionVM selectedChampion)
        {
            if (Navigation is null) return;
            await Navigation.PushAsync(new HomePage());
        }
    }
}