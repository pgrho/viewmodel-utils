namespace Shipwreck.ViewModelUtils.Client;

[DataContract]
public partial class BooleanQueryPropertyInfo : QueryPropertyInfo
{
    [RequiresUnreferencedCode("Default Constructor")]
    public BooleanQueryPropertyInfo() { }


    [DataMember]
    public string TrueString { get; set; }

    [DataMember]
    public string FalseString { get; set; }

    protected override QueryPropertyInfo CreateInstance()
        => new BooleanQueryPropertyInfo();

    public override void CopyTo(QueryPropertyInfo other)
    {
        base.CopyTo(other);
        if (other is BooleanQueryPropertyInfo b)
        {
            b.TrueString = TrueString;
            b.FalseString = FalseString;
        }
    }
}
