using System.ComponentModel;

namespace Shipwreck.ViewModelUtils.Client;

[DataContract]
[TypeConverter(typeof(ExtensionDataConverter))]
public sealed partial class ExtensionData : Dictionary<string, string>
{
    [RequiresUnreferencedCode("Default Constructor")]
    public ExtensionData()
    {
    }

    public ExtensionData(IEnumerable<KeyValuePair<string, string>> entries)
    {
        if (entries != null)
        {
            foreach (var kv in entries)
            {
                this[kv.Key] = kv.Value;
            }
        }
    }

    private bool _ShouldSerialize;

    [IgnoreDataMember]
    public bool ShouldSerialize
    {
        get => _ShouldSerialize = (_ShouldSerialize || Count > 0);
        set
        {
            if (value)
            {
                _ShouldSerialize = true;
            }
            else
            {
                _ShouldSerialize = false;
                Clear();
            }
        }
    }

    public void Set(IEnumerable<KeyValuePair<string, string>> value)
    {
        if (value != this)
        {
            Clear();
            if (value != null)
            {
                foreach (var kv in value)
                {
                    this[kv.Key] = kv.Value;
                }
            }
        }
    }
}
