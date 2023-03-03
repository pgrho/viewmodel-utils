using System.Collections.ObjectModel;

namespace Shipwreck.ViewModelUtils.Client;

[DataContract]
public partial class EnumQueryPropertyInfo : QueryPropertyInfo
{
    [RequiresUnreferencedCode("Default Constructor")]
    public EnumQueryPropertyInfo() { }

    [DataMember]
    public bool IsFlags { get; set; }

    #region Fields

    private Collection<EnumFieldInfo> _Fields;

    [DataMember]
    public IList<EnumFieldInfo> Fields
    {
        get => _Fields ??= new Collection<EnumFieldInfo>();
        set
        {
            if (value != _Fields)
            {
                _Fields?.Clear();
                if (value?.Count > 0)
                {
                    foreach (var e in value)
                    {
                        Fields.Add(e);
                    }
                }
            }
        }
    }

    #endregion Fields

    protected override QueryPropertyInfo CreateInstance()
        => new EnumQueryPropertyInfo();

    public override void CopyTo(QueryPropertyInfo other)
    {
        base.CopyTo(other);
        if (other is EnumQueryPropertyInfo d)
        {
            d.IsFlags = IsFlags;
            d.Fields = Fields;
        }
    }
}
