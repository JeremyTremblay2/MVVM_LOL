using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Model;

namespace ViewModel;

public class ChampionVM : INotifyPropertyChanged
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

    public event PropertyChangedEventHandler PropertyChanged;

    public ChampionVM(Champion model)
    {
        Model = model ?? throw new ArgumentNullException(nameof(model), "The parameter given cannot be null.");
        skillsVM = new ObservableCollection<SkillVM>();
        SkillsVM = new ReadOnlyObservableCollection<SkillVM>(skillsVM);
    }

    public ChampionVM() : this(new Champion("", ChampionClass.Tank))
    {
    }

    public void OnPropertyChanged([CallerMemberName] string propertyName = "")
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}