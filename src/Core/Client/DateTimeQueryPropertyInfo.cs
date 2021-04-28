using System.Runtime.Serialization;

namespace Shipwreck.ViewModelUtils.Client
{
    [DataContract]
    public partial class DateTimeQueryPropertyInfo : QueryPropertyInfo
    {
        [DataMember]
        public bool IsDate { get; set; }
    }
}
