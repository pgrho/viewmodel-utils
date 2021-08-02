using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Shipwreck.ViewModelUtils.Searching
{
    public sealed class DateTimeConditionViewModel : ConditionViewModel
    {
        private static readonly string[] _SupportedFormats
            = new[] {
                "yyyy-MM-dd HH:mm:ss",
                "yyyy-MM-dd HH:mm",
                "yyyy-MM-dd HH",
                "yyyy-MM-dd",
                "yyyy-MM",
                "yyyy",
            }
            .SelectMany(e => new[] { e, e.Replace('-', '/'), e.Replace("-", "") })
            .SelectMany(e => new[] { e, e.Replace(' ', 'T'), e.Replace(" ", "") })
            .SelectMany(e => new[] { e, e.Replace(":", "") })
            .Distinct()
            .ToArray();

        public static OperatorViewModel YearOperator { get; } = new OperatorViewModel(SR.YearToken, SR.YearOperator);
        public static OperatorViewModel MonthOperator { get; } = new OperatorViewModel(SR.MonthToken, SR.MonthOperator);
        public static OperatorViewModel DateOperator { get; } = new OperatorViewModel(SR.DayToken, SR.DayOperator);
        public static OperatorViewModel HourOperator { get; } = new OperatorViewModel(SR.HourToken, SR.HourOperator);
        public static OperatorViewModel MinuteOperator { get; } = new OperatorViewModel(SR.MinuteToken, SR.MinuteOperator);

        public static ReadOnlyCollection<OperatorViewModel> Operators = Array.AsReadOnly(new[] {
            YearOperator,
            MonthOperator,
            DateOperator,
            HourOperator,
            MinuteOperator,
            new OperatorViewModel("<", SR.LessThan),
            new OperatorViewModel("<=", SR.LessThanOrEqual),
            new OperatorViewModel(">", SR.GreaterThan),
            new OperatorViewModel(">=", SR.GreaterThanOrEqual)
        });

        public DateTimeConditionViewModel(SearchPropertyViewModel property)
            : base(property)
        {
            _Operator = Operators.FirstOrDefault(e => e.Token == property.Model.DefaultOperator)?.Token ?? DateOperator.Token;
        }

        #region Operator

        private string _Operator;

        public string Operator
        {
            get => _Operator;
            set
            {
                var y = IsYear;
                var mo = IsMonth;
                var d = IsDate;
                var h = IsHour;
                var mi = IsMinute;
                if (SetProperty(ref _Operator, value))
                {
                    RaisePropertyChanged(nameof(SelectedOperator));
                    if (y != IsYear)
                    {
                        RaisePropertyChanged(nameof(IsYear));
                    }
                    if (mo != IsMonth)
                    {
                        RaisePropertyChanged(nameof(IsMonth));
                    }
                    if (d != IsDate)
                    {
                        RaisePropertyChanged(nameof(IsDate));
                    }
                    if (h != IsHour)
                    {
                        RaisePropertyChanged(nameof(IsHour));
                    }
                    if (mi != IsMinute)
                    {
                        RaisePropertyChanged(nameof(IsMinute));
                    }
                }
            }
        }

        public OperatorViewModel SelectedOperator
        {
            get => Operators.FirstOrDefault(e => e.Token == _Operator) ?? DateOperator;
            set => Operator = (value ?? DateOperator).Token;
        }

        public bool IsYear => Operator == YearOperator.Token;
        public bool IsMonth => Operator == MonthOperator.Token;
        public bool IsDate => Operator == DateOperator.Token;
        public bool IsHour => Operator == HourOperator.Token;
        public bool IsMinute => !(IsYear || IsMonth || IsDate || IsHour);

        #endregion Operator

        #region Value

        private DateTime? _Value;

        public DateTime? Value
        {
            get => _Value;
            set => SetProperty(ref _Value, value);
        }

        #endregion Value

        public int? Year
        {
            get => _Value?.Year;
            set => Value = (value != null ? new DateTime(value.Value, 1, 1) : (DateTime?)null);
        }

        public override void SetValue(string @operator, string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                Value = null;
            }
            else
            {
                if (string.IsNullOrEmpty(@operator))
                {
                    var op = Operators.OrderByDescending(e => e.Token.Length).FirstOrDefault(e => e.Token.Length < value.Length && value.StartsWith(e.Token));
                    if (op != null)
                    {
                        @operator = op.Token;
                        value = value.Substring(op.Token.Length);
                    }
                }

                value = value.Replace('T', ' ').Replace('/', '-');

                DateTime? d;
                if (DateTime.TryParseExact(value, _SupportedFormats, null, DateTimeStyles.None, out var v) || DateTime.TryParse(value, out v))
                {
                    d = v;
                }
                else
                {
                    d = null;
                    if (value?.Length > 0)
                    {
                        Page.LogWarning("Invalid DateTime: \"{0}\"", value);
                    }
                }

                if (string.IsNullOrEmpty(@operator))
                {
                    var comps = value.Split(new[] { '-', ':', ' ', '.', 'z', '+' }, StringSplitOptions.RemoveEmptyEntries);

                    switch (comps.Length)
                    {
                        case 1:
                            @operator = YearOperator.Token;
                            d = d ?? (int.TryParse(value, out var i) ? new DateTime(i, 1, 1) : (DateTime?)null);
                            break;

                        case 2:
                            @operator = MonthOperator.Token;
                            break;

                        case 3:
                        default:
                            @operator = DateOperator.Token;
                            break;

                        case 4:
                            @operator = HourOperator.Token;
                            break;

                        case 5:
                            @operator = MinuteOperator.Token;
                            break;
                    }
                }

                Operator = @operator;
                Value = d;
            }
        }

        public override bool HasValue => Value != null;

        public override void AppendValueTo(StringBuilder builder)
        {
            if (Operator == DateOperator.Token || string.IsNullOrEmpty(Operator))
            {
                builder.Append(Value.Value.ToString("yyyy-MM-dd"));
            }
            else if (Operator == YearOperator.Token)
            {
                builder.Append(Value.Value.ToString("yyyy"));
            }
            else if (Operator == MonthOperator.Token)
            {
                builder.Append(Value.Value.ToString("yyyy-MM"));
            }
            else if (Operator == HourOperator.Token)
            {
                builder.Append(Value.Value.ToString("yyyy-MM-ddTHH"));
            }
            else if (Operator == MinuteOperator.Token)
            {
                builder.Append(Value.Value.ToString("yyyy-MM-ddTHH:mm"));
            }
            else
            {
                builder.Append(Operator);
                builder.Append(Value.Value.ToString("yyyy-MM-ddTHH:mm"));
            }
        }

        private static readonly Regex _DefaultValuePattern = new Regex(@"^\{(?<source>Today|Now)(\.(?<method>Add(?:Year|Month|Day|Hour|Minute)s)\((?<offset>-?\d+(?:\.\d+)?)\))?\:(?<format>[^}]+)\}$");

        public override bool TryCreateDefaultValueExpression(out string @operator, out string defaultValue)
        {
            if (Value != null)
            {
                var now = DateTime.Now;
                if (SelectedOperator == YearOperator)
                {
                    var offset = (Value ?? now).Year - now.Year;
                    @operator = Operator;
                    defaultValue = offset == 0 ? "{Today:yyyy}" : $"{{Today.AddYears({offset}):yyyy}}";
                }
                else if (SelectedOperator == MonthOperator)
                {
                    var tm = Value ?? now;
                    var offset = tm.Year * 12 + tm.Month - now.Year * 12 - now.Month;
                    @operator = Operator;
                    defaultValue = offset == 0 ? "{Today:yyyy-MM}" : $"{{Today.AddMonths({offset}):yyyy-MM}}";
                }
                else if (SelectedOperator == HourOperator)
                {
                    var offset = (int)Math.Round(((Value ?? now) - now).TotalHours);
                    @operator = Operator;
                    defaultValue = offset == 0 ? "{Now:yyyy-MM-dd HH}" : $"{{Now.AddHours({offset}):yyyy-MM-dd HH}}";
                }
                else if (SelectedOperator == MinuteOperator
                    || SelectedOperator != DateOperator
                    || Value?.Hour != 0
                    || Value?.Minute != 0)
                {
                    var offset = (int)Math.Round(((Value ?? now) - now).TotalMinutes);
                    @operator = Operator;
                    defaultValue = offset == 0 ? "{Now:yyyy-MM-dd HH:mm}" : $"{{Now.AddMinutes({offset}):yyyy-MM-dd HH:mm}}";
                }
                else
                {
                    var offset = (int)Math.Round(((Value ?? now).Date - now.Date).TotalDays);
                    @operator = Operator;
                    defaultValue = offset == 0 ? "{Today:yyyy-MM-dd}" : $"{{Today.AddDays({offset}):yyyy-MM-dd}}";
                }

                return true;
            }
            @operator = defaultValue = null;

            return false;
        }

        public override void SetDefaultValueExpression(string @operator, string defaultValue)
        {
            if (defaultValue != null && _DefaultValuePattern.Match(defaultValue) is var tdm && tdm.Success)
            {
                var dv = tdm.Groups["source"].Value == "Today" ? DateTime.Today : DateTime.Now;

                var method = tdm.Groups["method"].Value;
                var os = tdm.Groups["offset"].Value;
                var offset = int.TryParse(os, out var i) ? i : 0;

                switch (method)
                {
                    case nameof(DateTime.AddYears):
                        dv = dv.AddYears(offset);
                        break;

                    case nameof(DateTime.AddMonths):
                        dv = dv.AddMonths(offset);
                        break;

                    case nameof(DateTime.AddDays):
                        dv = dv.AddDays(offset);
                        break;

                    case nameof(DateTime.AddHours):
                        dv = dv.AddHours(offset);
                        break;

                    case nameof(DateTime.AddMinutes):
                        dv = dv.AddMinutes(offset);
                        break;
                }

                var f = tdm.Groups["format"].Value;

                SetValue(@operator, dv.ToString(f));
            }
            else
            {
                base.SetDefaultValueExpression(@operator, defaultValue);
            }
        }
    }
}
