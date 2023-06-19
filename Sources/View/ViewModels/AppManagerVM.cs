using System;
using System.Windows.Input;
using View.Pages;
using ViewModel;
using CommunityToolkit.Maui.Views;

namespace View.ViewModels
{
	public class AppManagerVM
	{
        public INavigation Navigation { get; set; }

        public ChampionManagerVM ChampionManagerVM => (Application.Current as App)!.ChampionManagerVM;

        public ICommand NavigateToUpsertChampionCommand { get; private set; }
        public ICommand NavigateToAddSkillCommand { get; private set; }
        public ICommand NavigateToSelectChampionCommand { get; private set; }
        public ICommand NavigateBackAfterUpsertingChampionCommand { get; private set; }
        public ICommand NavigateBackAfterCancelingChampionEditCommand { get; private set; }
        public ICommand NavigateBackAfterUpsertingSkillCommand { get; private set; }
        public ICommand NavigateBackAfterCancelingSkillEditCommand { get; private set; }

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
                canExecute: editedChampion => ChampionManagerVM is not null && editedChampion is not null
            );

            NavigateBackAfterCancelingChampionEditCommand = new Command(NavigateBackAfterCancelingChampionEdit);

            NavigateBackAfterUpsertingSkillCommand = new Command<EditableChampionVM>(
                execute: NavigateBackAfterUpsertingSkill,
                canExecute: editedChampion => ChampionManagerVM is not null && editedChampion is not null && editedChampion.EditedSkill is not null
                            && !string.IsNullOrEmpty(editedChampion.EditedSkill.Name)
            );

            NavigateBackAfterCancelingSkillEditCommand = new Command(NavigateBackAfterCancelingSkillEdit);

            NavigateToAddSkillCommand = new Command<EditableChampionVM>(
                execute: async (editedChampion) => await NavigateToAddSkill(editedChampion),
                canExecute: editedChampion => ChampionManagerVM is not null && editedChampion is not null
            );
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

        private async Task NavigateToAddSkill(EditableChampionVM editedChampion)
        {
            string result =
                await (Application.Current as App)!.MainPage!
                    .DisplayPromptAsync("New Skill", "Choose a name for your skill", cancel: "Cancel", accept: "Select", maxLength: 255, initialValue: "");

            if (result == null) return;
            if (string.IsNullOrWhiteSpace(result))
            {
                await (Application.Current as App)!.MainPage!
                    .DisplayAlert("Error", "You must choose a name not empty for your skill.", "OK");
                return;
            }

            editedChampion.AddEditSkillCommand.Execute(result);
            await Navigation.PushModalAsync(new AddEditSkillPage(this, editedChampion));
        }

        private void NavigateBackAfterUpsertingSkill(EditableChampionVM editedChampion)
        {
            editedChampion.UpsertSkillCommand.Execute(null);
            Navigation.PopModalAsync();
        }

        private void NavigateBackAfterCancelingSkillEdit()
        {
            if (Navigation is null) return;
            Navigation.PopModalAsync();
        }
    }
}