using System;
using Model;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace ViewModel
{
	public class EditableChampionVM
	{
        public ChampionVM ChampionVM
        {
            get => championVM;
            private set
            {
                if (championVM is not null && championVM.Equals(value)) return;
                championVM = value;
                (UpsertCharacteristicCommand as Command)?.ChangeCanExecute();
                (RemoveCharacteristicCommand as Command)?.ChangeCanExecute();
                (UpsertSkillCommand as Command)?.ChangeCanExecute();
                (RemoveSkillCommand as Command)?.ChangeCanExecute();
                (UpsertIconCommand as Command)?.ChangeCanExecute();
                (UpsertImageCommand as Command)?.ChangeCanExecute();
            }
        }
        private ChampionVM championVM;

        public ICommand UpsertCharacteristicCommand { get; private set; }
        public ICommand RemoveCharacteristicCommand { get; private set; }
        public ICommand UpsertSkillCommand { get; private set; }
        public ICommand RemoveSkillCommand { get; private set; }
        public ICommand UpsertIconCommand { get; private set; }
        public ICommand UpsertImageCommand { get; private set; }

        private static readonly ReadOnlyCollection<string> types = new(Enum.GetNames(typeof(SkillType)).ToList());
        public ReadOnlyCollection<string> SkillTypes => types;

        private static readonly ReadOnlyCollection<string> classes = new(Enum.GetNames(typeof(ChampionClass)).ToList());
        public ReadOnlyCollection<string> Classes => classes;

        public EditableChampionVM(ChampionVM championVM)
        {
            ChampionVM = championVM is null
                ? new ChampionVM(new Champion("Champion"))
                : championVM.Clone() as ChampionVM;


            UpsertCharacteristicCommand = new Command<Tuple<string, int>>(
                execute: UpsertCharacteristic,
                canExecute: tuple =>
                    ChampionVM is not null && tuple is not null && !string.IsNullOrWhiteSpace(tuple.Item1)
                    && !ChampionVM.CharacteristicsVM.ContainsKey(tuple.Item1)
            );

            RemoveCharacteristicCommand = new Command<string>(
                execute: RemoveCharacteristic,
                canExecute: characteristic =>
                    ChampionVM is not null && characteristic is not null && !string.IsNullOrWhiteSpace(characteristic)
            );

            UpsertIconCommand = new Command<byte[]>(
                execute: UpsertIcon,
                canExecute: image => ChampionVM is not null && image.Any()
            );

            UpsertImageCommand = new Command<byte[]>(
                execute: UpsertImage,
                canExecute: image => ChampionVM is not null && image.Any()
            );

            UpsertSkillCommand = new Command<SkillVM>(
                execute: UpsertSkill,
                canExecute: skill =>
                    ChampionVM is not null && skill is not null && skill.Type is not null && !string.IsNullOrWhiteSpace(skill.Name)
            );

            RemoveSkillCommand = new Command<SkillVM>(
                execute: DeleteSkill,
                    canExecute: skill => ChampionVM is not null && skill is not null
            );
        }

        private void UpsertCharacteristic(Tuple<string, int> value)
        {
            ChampionVM.UpsertCharacteristic(value);
            (UpsertCharacteristicCommand as Command).ChangeCanExecute();
        }

        private void RemoveCharacteristic(string key)
        {
            ChampionVM.RemoveCharacteristic(key);
            (UpsertCharacteristicCommand as Command).ChangeCanExecute();
        }

        private void UpsertIcon(byte[] image)
            => ChampionVM.Icon = Convert.ToBase64String(image);

        private void UpsertImage(byte[] image)
            => ChampionVM.Image = Convert.ToBase64String(image);

        private void UpsertSkill(SkillVM skill)
            => ChampionVM.UpsertSkill(skill);

        private void DeleteSkill(SkillVM skill)
            => ChampionVM.RemoveSkill(skill);
    }
}