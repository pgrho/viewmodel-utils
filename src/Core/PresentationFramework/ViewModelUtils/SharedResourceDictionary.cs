namespace Shipwreck.ViewModelUtils;

public sealed class SharedResourceDictionary : ResourceDictionary
{
    public static readonly Dictionary<Uri, ResourceDictionary> _Instances = new Dictionary<Uri, ResourceDictionary>();

    private Uri _Source;

    public new Uri Source
    {
        get => _Source;
        set
        {
            var old = _Source;
            if (value != _Source)
            {
                _Source = value;

                if (!_Instances.ContainsKey(value))
                {
                    base.Source = value;

                    _Instances.Add(value, this);
                }
                else
                {
                    MergedDictionaries.Add(_Instances[value]);
                }
                if (old != null)
                {
                    var od = MergedDictionaries.FirstOrDefault(e => e.Source == old);
                    if (od != null)
                    {
                        MergedDictionaries.Remove(od);
                    }
                }
            }
        }
    }
}
