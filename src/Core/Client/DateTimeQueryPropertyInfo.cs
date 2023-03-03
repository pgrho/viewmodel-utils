using System.Security.Permissions;

namespace Shipwreck.ViewModelUtils.Client;

[DataContract]
public partial class DateTimeQueryPropertyInfo : QueryPropertyInfo
{
    [RequiresUnreferencedCode("Default Constructor")]
    public DateTimeQueryPropertyInfo() { }

    [DataMember]
    public bool IsDate { get; set; }

    protected override QueryPropertyInfo CreateInstance()
        => new DateTimeQueryPropertyInfo();

    public override void CopyTo(QueryPropertyInfo other)
    {
        base.CopyTo(other);
        if (other is DateTimeQueryPropertyInfo d)
        {
            d.IsDate = IsDate;
        }
    }
}
