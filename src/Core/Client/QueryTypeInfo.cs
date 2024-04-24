namespace Shipwreck.ViewModelUtils.Client;

[DataContract]
public sealed partial class QueryTypeInfo : FrameworkMessageObject
{
    public QueryTypeInfo()
        : base(null)
    {
    }

    [DataMember]
    public string TypeName { get; set; }

    [DataMember]
    public QueryPropertyInfo[] Properties { get; set; }
}
