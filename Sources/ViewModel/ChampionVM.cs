using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Model;
using Utils;
using VMToolkit;

namespace ViewModel;

public class ChampionVM : BaseVM
{
    private Champion Model
    {
        get => model;
        set
        {
            if (value.Equals(model)) return;
            this.model = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(Name));
            OnPropertyChanged(nameof(Icon));
            OnPropertyChanged(nameof(Bio));
            OnPropertyChanged(nameof(Class));
            OnPropertyChanged(nameof(Image));
            OnPropertyChanged(nameof(SkillsVM));
        }
    }
    private Champion model;

    public string Name
    {
        get => Model.Name;
        set
        {
            if (value.Equals(Model.Name)) return;
            Model.Name = value;
            OnPropertyChanged();
        }
    }

    public string Icon
    {
        get => Model.Icon;
        set
        {
            if (value.Equals(Model.Icon)) return;
            Model.Icon = value;
            OnPropertyChanged();
        }
    }

    public string Bio
    {
        get => Model.Bio;
        set
        {
            if (value.Equals(Model.Bio)) return;
            Model.Bio = value;
            OnPropertyChanged();
        }
    }

    public string Class
    {
        get => Model.Class.ToString();
        set
        {
            if (value.Equals(Model.Class.ToString())) return;
            if (Enum.TryParse(value, out ChampionClass classValue))
            {
                Model.Class = classValue;
                OnPropertyChanged();
            }
        }
    }

    public string Image
    {
        get => Model.Image.Base64;
        set
        {
            if (value.Equals(Model.Image.Base64)) return;
            Model.Image.Base64 = value;
            OnPropertyChanged();
        }
    }

    public ReadOnlyObservableCollection<SkillVM> SkillsVM { get; private set; }

    private ObservableCollection<SkillVM> skillsVM;

    public ReadOnlyObservableDictionary<string, int> CharacteristicsVM { get; private set; }

    public ObservableDictionary<string, int> characteristics;

    public ChampionVM(Champion model)
    {
        Model = model ?? throw new ArgumentNullException(nameof(model), "The parameter given cannot be null.");
        skillsVM = new ObservableCollection<SkillVM>();
        SkillsVM = new ReadOnlyObservableCollection<SkillVM>(skillsVM);
        characteristics = new ObservableDictionary<string, int>();
        CharacteristicsVM = new ReadOnlyObservableDictionary<string, int>(characteristics);
        AddCharacteristics(Model.Characteristics.Select(c => new Tuple<string, int>(c.Key, c.Value)).ToArray());
        UpsertSkills(Model.Skills.Select(s => new SkillVM(s)));
    }

    public ChampionVM() : this(new Champion("", ChampionClass.Tank))
    {
    }

    public void AddCharacteristic(Tuple<string, int> value)
    {
        if (string.IsNullOrWhiteSpace(value.Item1))
        {
            throw new ArgumentException("The given parameter cannot be null.", nameof(value.Item1));
        }
        if (Model.Characteristics.ContainsKey(value.Item1))
        {
            RemoveCharacteristic(value.Item1);
        }
        Model.AddCharacteristics(value);
        characteristics.Add(value.Item1, value.Item2);
    }

    public void AddCharacteristics(Tuple<string, int>[] values)
    {
        foreach (var value in values)
        {
            AddCharacteristic(value);
        }
    }

    public void RemoveCharacteristic(string key)
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            throw new ArgumentException("The given parameter cannot be null.", nameof(key));
        }
        if (Model.RemoveCharacteristics(key))
        {
            characteristics.Remove(key);
        }
    }

    public void UpsertSkills(IEnumerable<SkillVM> skills)
    {
        foreach (var skill in skills)
        {
            UpsertSkill(skill);
        }
    }

    public void UpsertSkill(SkillVM skill)
    {
        if (skill is null)
        {
            throw new ArgumentNullException(nameof(skill), "The skill given in parameter cannot be null.");
        }

        Model.AddSkill(skill.Model);
        skillsVM.Add(skill);
    }

    public void UpsertSkill(string name, string type, string description)
    {
        if (string.IsNullOrWhiteSpace(type) || string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("The skill type or the name cannot be empty");
        }

        if (Enum.TryParse(type, out SkillType typeAsEnum))
        {
            UpsertSkill(new SkillVM(new Skill(name, typeAsEnum, description ?? "")));
        }
    }

    public void RemoveSkill(SkillVM skillVM)
    {
        if (skillVM is null)
        {
            throw new ArgumentNullException(nameof(skillVM), "The skill given cannot be null.");
        }

        var skill = Model.Skills.FirstOrDefault(s => s.Equals(skillVM.Model));
        if (skill != null)
        {
            Model.RemoveSkill(skill);
            skillsVM.Remove(skillVM);
        }
    }
}