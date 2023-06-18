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

        public ICommand NavigateToUpsertChampionCommand { get; private set; }
        public ICommand NavigateToSelectChampionCommand { get; private set; }
        public ICommand NavigateBackAfterUpsertingChampionCommand { get; private set; }
        public ICommand NavigateBackAfterCancelingChampionEditCommand { get; private set; }

        public AppManagerVM()
		{
            NavigateToUpsertChampionCommand = new Command<ChampionVM>(
                execute: async (champion) => await NavigateToUpsertChampion(champion),
                canExecute: champion => ChampionManagerVM is not null
            );
            NavigateToSelectChampionCommand = new Command<ChampionVM>(
                execute: async (selectedChampion) => await NavigateToSelectChampion(selectedChampion),
                canExecute: selectedChampion => ChampionManagerVM is not null && selectedChampion is not null
            );
            NavigateBackAfterUpsertingChampionCommand = new Command<EditableChampionVM>(
                execute: NavigateBackAfterUpsertingChampion,
                canExecute: championFormVM => ChampionManagerVM is not null && championFormVM is not null
            );

            NavigateBackAfterCancelingChampionEditCommand = new Command(NavigateBackAfterCancelingChampionEdit);

        }

        private async Task NavigateToUpsertChampion(ChampionVM champion)
        {
            if (Navigation is null) return;
            await Navigation.PushModalAsync(new AddEditChampionPage(this, new EditableChampionVM(champion)));
        }

        private async Task NavigateToSelectChampion(ChampionVM selectedChampion)
        {
            if (Navigation is null) return;
            await Navigation.PushAsync(new ChampionDetailPage(this, selectedChampion));
        }

        private void NavigateBackAfterUpsertingChampion(EditableChampionVM editedChampion)
        {
            ChampionManagerVM.UpsertChampionCommand.Execute(editedChampion);
            Navigation.PopModalAsync();
        }

        private void NavigateBackAfterCancelingChampionEdit()
        {
            if (Navigation is null) return;
            Navigation.PopModalAsync();
        }
    }
}