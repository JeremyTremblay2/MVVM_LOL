using System;
namespace VMToolkit
{
	public class GenericBaseVM<T> : BaseVM
	{
        protected internal T Model
        {
            get => model;
            private set
            {
                this.model = value;
            }
        }
        private T model;

        public GenericBaseVM(T model)
        {
            Model = model ?? throw new ArgumentNullException(nameof(model), "The parameter given cannot be null.");
        }
    }
}

