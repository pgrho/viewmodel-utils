namespace Shipwreck.ViewModelUtils;

public interface IPropertyChangedRaisable : INotifyPropertyChanged
{
    void RaisePropertyChanged(string propertyName);
}
