using System.Runtime.Serialization;

namespace Shipwreck.ViewModelUtils.Client
{
    [DataContract]
    public partial class BooleanQueryPropertyInfo : QueryPropertyInfo
    {
        [DataMember]
        public string TrueString { get; set; }

        [DataMember]
        public string FalseString { get; set; }
    }
}
