using System;
using Model;
using VMToolkit;

namespace ViewModel
{
	public class SkillVM : BaseVM
	{
        public Skill Model
        {
            get => model;
            private set
            {
                if (value.Equals(model)) return;
                model = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Name));
                OnPropertyChanged(nameof(Type));
                OnPropertyChanged(nameof(Description));
            }
        }
        private Skill model;

        public string Name
        {
            get => Model.Name;
        }

        public string Type
        {
            get => Model.Type.ToString();
        }

        public string Description
        {
            get => Model.Description;
            set
            {
                if (value.Equals(Model.Description)) return;
                Model.Description = value;
                OnPropertyChanged();
            }
        }

        public SkillVM(Skill model)
        {
            Model = model ?? throw new ArgumentNullException(nameof(model), "The given parameter cannot be null.");
        }

        public SkillVM() : this(new Skill("Skill", SkillType.Unknown))
        {
        }
    }
}

