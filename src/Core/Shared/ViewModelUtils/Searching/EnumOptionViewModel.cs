using Shipwreck.ViewModelUtils.Client;

namespace Shipwreck.ViewModelUtils.Searching
{
    public sealed class EnumOptionViewModel : MultipleOptionViewModel<EnumFieldInfo>
    {
        public EnumOptionViewModel(MultipleOptionConditionViewModel<EnumFieldInfo> condition, EnumFieldInfo value, string displayName, bool isSelected = false)
            : base(condition, value, displayName, isSelected)
        {
        }
    }
}
