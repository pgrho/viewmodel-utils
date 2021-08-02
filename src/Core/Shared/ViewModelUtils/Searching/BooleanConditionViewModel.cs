using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Shipwreck.ViewModelUtils.Client;

namespace Shipwreck.ViewModelUtils.Searching
{
    public sealed class BooleanConditionViewModel : OptionsConditionViewModel<bool?>
    {
        public BooleanConditionViewModel(SearchPropertyViewModel property)
            : base(property)
        {
        }

        protected override IEnumerable<OptionViewModel<bool?>> CreateOptions()
            => new[]
            {
                CreateOption(null, string.Empty),
                CreateOption(true, (Property.Model as BooleanQueryPropertyInfo)?.TrueString ?? bool.TrueString),
                CreateOption(false, (Property.Model as BooleanQueryPropertyInfo)?.FalseString ?? bool.FalseString),
            };

        protected override string GetString(bool? value) => value?.ToString() ?? string.Empty;

        protected override bool? Parse(string value)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                value = value.TrimOrNull();
                if (Regex.IsMatch(value, "^([-+]?1|y(es)?|t(rue)?)$", RegexOptions.IgnoreCase))
                {
                    return true;
                }
                if (Regex.IsMatch(value, "^(0|no?|f(alse)?)$", RegexOptions.IgnoreCase))
                {
                    return false;
                }
            }
            return null;
        }

        public override bool HasValue => Value != null;

        public override void AppendValueTo(StringBuilder builder)
            => builder.Append(Value.Value ? '1' : '0');

        public override bool TryCreateDefaultValueExpression(out string @operator, out string defaultValue)
        {
            @operator = null;
            if (Value != null)
            {
                defaultValue = Value.Value ? "1" : "0";

                return true;
            }
            defaultValue = null;

            return false;
        }
    }
}
