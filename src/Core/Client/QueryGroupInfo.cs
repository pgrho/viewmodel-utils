namespace Shipwreck.ViewModelUtils.Client;

[DataContract]
public sealed partial class QueryGroupInfo : FrameworkMessageObject
{
    public QueryGroupInfo()
        : base(null)
    {
    }

    [DataMember]
    public string Path { get; set; }

    [DataMember]
    public string TypeName { get; set; }

    [DataMember]
    public string DisplayName { get; set; }
}
