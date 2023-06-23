using System;
using System.Windows.Input;
using View.Pages;
using ViewModel;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Maui.Converters;
using Model;

namespace View.ViewModels
{
	public class AppManagerVM
	{
        public INavigation Navigation { get; set; }

        public ChampionManagerVM ChampionManagerVM => (Application.Current as App)!.ChampionManagerVM;

        public ICommand NavigateToUpsertChampionCommand { get; private set; }
        public ICommand NavigateToDeleteChampionCommand { get; private set; }
        public ICommand NavigateToAddSkillCommand { get; private set; }
        public ICommand NavigateToSelectChampionCommand { get; private set; }
        public ICommand NavigateBackAfterUpsertingChampionCommand { get; private set; }
        public ICommand NavigateBackAfterDeletingChampionCommand { get; private set; }
        public ICommand NavigateBackAfterCancelingChampionEditCommand { get; private set; }
        public ICommand NavigateBackAfterUpsertingSkillCommand { get; private set; }
        public ICommand NavigateBackAfterCancelingSkillEditCommand { get; private set; }
        public ICommand UploadChampionIconCommand { get; private set; }
        public ICommand UploadChampionImageCommand { get; private set; }

        public AppManagerVM()
		{
            NavigateToUpsertChampionCommand = new Command<ChampionVM>(
                execute: async (champion) => await NavigateToUpsertChampion(champion),
                canExecute: champion => ChampionManagerVM is not null
            );
            NavigateToDeleteChampionCommand = new Command<ChampionVM>(
                execute: NavigateToDeleteChampion,
                canExecute: champion => ChampionManagerVM is not null && champion != null
            );
            NavigateToSelectChampionCommand = new Command<ChampionVM>(
                execute: async (selectedChampion) => await NavigateToSelectChampion(selectedChampion),
                canExecute: selectedChampion => ChampionManagerVM is not null && selectedChampion is not null
            );
            NavigateBackAfterUpsertingChampionCommand = new Command<EditableChampionVM>(
                execute: async (editedChampion) => await NavigateBackAfterUpsertingChampion(editedChampion),
                canExecute: editedChampion => ChampionManagerVM is not null && editedChampion is not null
            );

            NavigateBackAfterCancelingChampionEditCommand = new Command(NavigateBackAfterCancelingChampionEdit);

            NavigateBackAfterUpsertingSkillCommand = new Command<EditableChampionVM>(
                execute: NavigateBackAfterUpsertingSkill,
                canExecute: editedChampion => ChampionManagerVM is not null && editedChampion is not null && editedChampion.EditedSkill is not null
                            && !string.IsNullOrEmpty(editedChampion.EditedSkill.Name)
            );

            NavigateBackAfterDeletingChampionCommand = new Command<ChampionVM>(
                execute: async (champion) => await NavigateBackAfterDeletingChampion(champion),
                canExecute: champion => ChampionManagerVM is not null && champion != null
            );

            NavigateBackAfterCancelingSkillEditCommand = new Command(NavigateBackAfterCancelingSkillEdit);

            NavigateToAddSkillCommand = new Command<EditableChampionVM>(
                execute: async (editedChampion) => await NavigateToAddSkill(editedChampion),
                canExecute: editedChampion => ChampionManagerVM is not null && editedChampion is not null
            );

            UploadChampionIconCommand = new Command<EditableChampionVM>(
                execute: async (editedChampion) => await UploadChampionIcon(editedChampion),
                canExecute: editedChampion => ChampionManagerVM is not null && editedChampion is not null
            );

            UploadChampionImageCommand = new Command<EditableChampionVM>(
                execute: async (editedChampion) => await UploadChampionImage(editedChampion),
                canExecute: editedChampion => ChampionManagerVM is not null && editedChampion is not null
            );
        }

        private async Task NavigateToUpsertChampion(ChampionVM champion)
        {
            if (Navigation is null) return;
            if (champion is not null) await Navigation.PushModalAsync(new AddEditChampionPage(this, new EditableChampionVM(champion)));
            else
            {
                string result = await (Application.Current as App)!.MainPage!
                    .DisplayPromptAsync("New Champion", "Choose a name for your champion", cancel: "Cancel", accept: "Select", maxLength: 255, initialValue: "");

                if (result == null) return;
                if (string.IsNullOrWhiteSpace(result))
                {
                    await (Application.Current as App)!.MainPage!
                        .DisplayAlert("Error", "You must choose a name not empty for your champion.", "OK");
                    return;
                }
                await Navigation.PushModalAsync(new AddEditChampionPage(this, new EditableChampionVM(result)));
            }
        }

        private void NavigateToDeleteChampion(ChampionVM champion)
        {
            if (Navigation is null) return;
            ChampionManagerVM.DeleteChampionCommand.Execute(champion);
        }

        private async Task NavigateToSelectChampion(ChampionVM selectedChampion)
        {
            if (Navigation is null) return;
            await Navigation.PushAsync(new ChampionDetailPage(this, selectedChampion));
        }

        private async Task NavigateBackAfterUpsertingChampion(EditableChampionVM editedChampion)
        {
            if (Navigation is null) return;
            ChampionManagerVM.UpsertChampionCommand.Execute(editedChampion);
            await Shell.Current.GoToAsync("//ChampionsPage");
        }

        private async Task NavigateBackAfterDeletingChampion(ChampionVM champion)
        {
            if (Navigation is null) return;
            ChampionManagerVM.DeleteChampionCommand.Execute(champion);
            await Shell.Current.GoToAsync("//ChampionsPage");
        }

        private void NavigateBackAfterCancelingChampionEdit()
        {
            if (Navigation is null) return;
            Navigation.PopModalAsync();
        }

        private async Task NavigateToAddSkill(EditableChampionVM editedChampion)
        {
            if (Navigation is null) return;
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
            if (Navigation is null) return;
            editedChampion.UpsertSkillCommand.Execute(null);
            Navigation.PopModalAsync();
        }

        private void NavigateBackAfterCancelingSkillEdit()
        {
            if (Navigation is null) return;
            Navigation.PopModalAsync();
        }

        private async Task UploadChampionIcon(EditableChampionVM editedChampion)
        {
            var bytes = await LoadImage();
            editedChampion.ChampionVM.Icon = Convert.ToBase64String(bytes);
        }
        
        private async Task UploadChampionImage(EditableChampionVM editedChampion)
        {
            var bytes = await LoadImage();
            editedChampion.ChampionVM.Image = Convert.ToBase64String(bytes);
        }

        private async Task<byte[]> LoadImage()
        {
            var result = await FilePicker.PickAsync(new PickOptions { PickerTitle = "Pick Icon", FileTypes = FilePickerFileType.Images });
            if (result is null) return null;

            byte[] bytes;
            using var stream = await result.OpenReadAsync();
            var image = ImageSource.FromStream(() => stream);
            var converter = new ByteArrayToImageSourceConverter();
            bytes = converter.ConvertBackTo(image);
            return bytes;
        }
    }
}