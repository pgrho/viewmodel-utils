using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace Shipwreck.ViewModelUtils.Client
{
    [DataContract]
    public partial class EnumQueryPropertyInfo : QueryPropertyInfo
    {
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
    }
}
