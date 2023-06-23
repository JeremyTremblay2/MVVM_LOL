using System;
using Model;
using System.Collections.ObjectModel;
using System.Windows.Input;
using VMToolkit;

namespace ViewModel
{
	public class EditableChampionVM : BaseVM
	{
        public ChampionVM ChampionVM
        {
            get => championVM;
            private set
            {
                if (championVM is not null && championVM.Equals(value)) return;
                championVM = value;
                UpdateCommand(UpsertCharacteristicCommand);
                UpdateCommand(RemoveCharacteristicCommand);
                UpdateCommand(UpsertSkillCommand);
                UpdateCommand(RemoveSkillCommand);
                UpdateCommand(UpsertIconCommand);
                UpdateCommand(UpsertImageCommand);
            }
        }
        private ChampionVM championVM;

        public SkillVM EditedSkill
        {
            get => editedSkill;
            private set
            {
                if (editedSkill is not null && editedSkill.Equals(value)) return;
                editedSkill = value;
                UpdateCommand(UpsertSkillCommand);
            }
        }
        private SkillVM editedSkill;

        public ICommand UpsertCharacteristicCommand { get; private set; }
        public ICommand RemoveCharacteristicCommand { get; private set; }
        public ICommand AddEditSkillCommand { get; private set; }
        public ICommand UpsertSkillCommand { get; private set; }
        public ICommand RemoveSkillCommand { get; private set; }
        public ICommand UpsertIconCommand { get; private set; }
        public ICommand UpsertImageCommand { get; private set; }

        private static readonly ReadOnlyCollection<string> types = new(Enum.GetNames(typeof(SkillType)).ToList());
        public ReadOnlyCollection<string> SkillTypes => types;

        private static readonly ReadOnlyCollection<string> classes = new(Enum.GetNames(typeof(ChampionClass)).Skip(1).ToList());
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

            AddEditSkillCommand = new Command<string?>(
                execute: AddEditSkill,
                canExecute: skill =>
                    ChampionVM is not null
            );

            UpsertSkillCommand = new Command(UpsertSkill);

            RemoveSkillCommand = new Command<SkillVM>(
                execute: DeleteSkill,
                    canExecute: skill => ChampionVM is not null && skill is not null
            );
        }

        public EditableChampionVM(string name) : this(new ChampionVM(new Champion(name)))
        {

        }

        private void UpsertCharacteristic(Tuple<string, int> value)
        {
            ChampionVM.UpsertCharacteristic(value);
            UpdateCommand(UpsertCharacteristicCommand);
        }

        private void RemoveCharacteristic(string key)
        {
            ChampionVM.RemoveCharacteristic(key);
            UpdateCommand(UpsertCharacteristicCommand);
        }

        private void UpsertIcon(byte[] image)
            => ChampionVM.Icon = Convert.ToBase64String(image);

        private void UpsertImage(byte[] image)
            => ChampionVM.Image = Convert.ToBase64String(image);

        private void AddEditSkill(string skillName)
        {
            var skillVM = ChampionVM.SkillsVM.FirstOrDefault(s => s.Name == skillName);
            EditedSkill = skillVM == null ? new SkillVM(new Skill(skillName, SkillType.Unknown))
                : new SkillVM(new Skill(skillVM.Model.Name, skillVM.Model.Type, skillVM.Model.Description));
        }

        private void UpsertSkill()
            => ChampionVM.UpsertSkill(EditedSkill);

        private void DeleteSkill(SkillVM skill)
            => ChampionVM.RemoveSkill(skill);
    }
}