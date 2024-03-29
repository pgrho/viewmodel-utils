﻿namespace Shipwreck.ViewModelUtils.Client;

[DataContract]
public sealed partial class EnumFieldInfo
{
    public EnumFieldInfo()
    {
    }

    [DataMember]
    public long Value { get; set; }

    [DataMember]
    public string Name { get; set; }

    [DataMember]
    public string DisplayName { get; set; }
}
