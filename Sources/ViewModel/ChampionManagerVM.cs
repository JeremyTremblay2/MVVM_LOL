﻿using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Model;
using VMToolkit;
using Utils;

namespace ViewModel
{
	public class ChampionManagerVM : BaseVM
	{
        private IDataManager DataManager
        {
            get => dataManager;
            set
            {
                if (dataManager is not null && dataManager.Equals(value)) return;
                dataManager = value;
                UpdateCommand(LoadChampionsCommand);
                UpdateCommand(InitializeDataCommand);
                UpdateCommand(NextPageCommand);
                UpdateCommand(PreviousPageCommand);
            }
        }
        private IDataManager dataManager;

        public ReadOnlyObservableCollection<ChampionVM> ObservableChampionsVM { get; private set; }

		private ObservableCollection<ChampionVM> championsVM { get; set; }

        public int Index
        {
            get => index;
            set
            {
                if (index == value) return;
                index = value;
                OnPropertyChanged(nameof(Index));
                UpdateCommand(NextPageCommand);
                UpdateCommand(PreviousPageCommand);
            }
        }
        private int index = 1;

        public int Count
        {
            get => count;
            set
            {
                if (count == value) return;
                count = value;
                OnPropertyChanged();
                UpdateCommand(NextPageCommand);
                UpdateCommand(PreviousPageCommand);
            }
        }
        private int count = 5;

        public int NumberOfPages
        {
            get
            {
                int nbPagesRoundedDown = TotalItemCount / Count;
                bool noRemainderOrNoItems = TotalItemCount % Count == 0 || TotalItemCount == 0;

                return nbPagesRoundedDown + (noRemainderOrNoItems ? 0 : 1);
            }
        }

        public int TotalItemCount
        {
            get => totalItemCount;
            set
            {
                if (totalItemCount == value) return;
                if (totalItemCount == 0 && value == 1)
                {
                    Index = 1;
                    OnPropertyChanged(nameof(Index));
                }
                totalItemCount = value;
                OnPropertyChanged();
                UpdateCommand(NextPageCommand);
            }
        }
        private int totalItemCount;

        public bool CanNavigatePreviousPage => Index > 1;

		public bool CanNavigateNextPage => Index < NumberOfPages;

        public ICommand LoadChampionsCommand { get; private set; }
        public ICommand InitializeDataCommand { get; private set; }
        public ICommand NextPageCommand { get; private set; }
        public ICommand PreviousPageCommand { get; private set; }
        public ICommand UpsertChampionCommand { get; private set; }
        public ICommand DeleteChampionCommand { get; private set; }

        public ChampionManagerVM(IDataManager dataManager)
		{
            DataManager = dataManager ?? throw new ArgumentNullException(nameof(dataManager)); ;

            championsVM = new ObservableCollection<ChampionVM>();
            ObservableChampionsVM = new ReadOnlyObservableCollection<ChampionVM>(championsVM);

            NextPageCommand = new Command(
                execute: NextPage,
                canExecute: () => DataManager is not null && CanNavigateNextPage
            );

            PreviousPageCommand = new Command(
                execute: PreviousPage,
                canExecute: () => DataManager is not null && CanNavigatePreviousPage
            );

            LoadChampionsCommand = new Command(
                execute: async () => await LoadChampions(Index, Count),
                canExecute: () => DataManager is not null
            );

            InitializeDataCommand = new Command(
                execute: async () =>
                {
                    await UpdateTotalItemCount();
                    LoadChampionsCommand.Execute(null);
                },
                canExecute: () => DataManager is not null
            );

            UpsertChampionCommand = new Command<EditableChampionVM>(
                execute: async (editableChampion) => await UpsertChampion(editableChampion),
                canExecute: editableChampion => dataManager is not null && editableChampion is not null
            );

            DeleteChampionCommand = new Command<ChampionVM>(
                execute: async (championVM) => await DeleteChampion(championVM),
                canExecute: championVM => dataManager is not null && championVM is not null
            );
        }

        public async Task LoadChampions(int index, int count)
		{
            championsVM.Clear();

            var champions = await DataManager.ChampionsMgr.GetItems(index - 1, count, "Name");
            championsVM.AddRange(champions.Where(c => c is not null).Select(c => new ChampionVM(c)));
        }

        private async Task UpdateTotalItemCount()
        {
            TotalItemCount = await DataManager.ChampionsMgr.GetNbItems();
            Index = Index > NumberOfPages ? NumberOfPages : Index;
            OnPropertyChanged(nameof(NumberOfPages));
        }

        private void NextPage()
        {
            Index += 1;
            LoadChampionsCommand.Execute(null);
        }

        private void PreviousPage()
        {
            Index = Math.Max(1, Index - 1);
            LoadChampionsCommand.Execute(null);
        }

        private async Task UpsertChampion(EditableChampionVM editableChampion)
        {
            Champion champion = await GetChampionByName(editableChampion.ChampionVM.Name);

            if (champion is not null)
            {
                await DataManager.ChampionsMgr.UpdateItem(champion, editableChampion.ChampionVM.Model);
            }
            else
            {
                if (await DataManager.ChampionsMgr.AddItem(editableChampion.ChampionVM.Model) is not null)
                    await UpdateTotalItemCount();
            }
            LoadChampionsCommand.Execute(null);
        }

        private async Task DeleteChampion(ChampionVM championVM)
        {
            if (championVM is not null && await DataManager.ChampionsMgr.DeleteItem(championVM.Model))
            {
                await UpdateTotalItemCount();
                LoadChampionsCommand.Execute(null);
            }
        }

        private async Task<Champion> GetChampionByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name) || dataManager is null) return null;
            return (await DataManager.ChampionsMgr.GetItemsByName(name, 0, 1)).FirstOrDefault();
        }
    }
}

