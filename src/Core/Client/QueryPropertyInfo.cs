using System.Runtime.Serialization;

namespace Shipwreck.ViewModelUtils.Client
{
    [DataContract]
    public partial class QueryPropertyInfo
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string DisplayName { get; set; }

        [DataMember]
        public string TypeName { get; set; }

        [DataMember]
        public string DefaultOperator { get; set; }
    }
}
