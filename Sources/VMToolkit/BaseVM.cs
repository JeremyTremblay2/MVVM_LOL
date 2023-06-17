using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace VMToolkit;

public  class BaseVM : INotifyPropertyChanged //<T> where T : IEquatable<T>, INotifyPropertyChanged
{
    //protected T Model { get; private set; }

    public event PropertyChangedEventHandler? PropertyChanged;

    /*public BaseVM(T model)
    {
        Model = model;
    }*/

    public void OnPropertyChanged([CallerMemberName] string propertyName = "")
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}