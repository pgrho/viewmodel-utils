using System.ComponentModel;

namespace Shipwreck.ViewModelUtils.Client;

[DataContract]
public partial class SearchPageDefaultConditionInfo : FrameworkMessageObject
{
    [RequiresUnreferencedCode("Default Constructor")]
    public SearchPageDefaultConditionInfo()
        : base(null)
    {
    }

    [DataMember]
    [DefaultValue(null)]
    public string Name { get; set; }

    [DataMember]
    [DefaultValue(null)]
    public string Operator { get; set; }

    [DataMember]
    [DefaultValue(null)]
    public string DefaultValue { get; set; }
}
