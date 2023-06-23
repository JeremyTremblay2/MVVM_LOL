using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System;
using System.Collections.ObjectModel;
using VMToolkit;

namespace VMToolkit;

public  class BaseVM : INotifyPropertyChanged 
{
    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    protected void UpdateCommand(ICommand command)
        => (command as Command)?.ChangeCanExecute();
}