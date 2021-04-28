using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Shipwreck.ViewModelUtils.Searching
{
    public sealed class StringConditionViewModel : ConditionViewModel
    {
        public static OperatorViewModel DefaultOperator { get; } = new OperatorViewModel("*=", SR.Contains);

        public static ReadOnlyCollection<OperatorViewModel> Operators { get; } = Array.AsReadOnly(new[]
        {
            new OperatorViewModel("=", SR.Equal),
            new OperatorViewModel("!=", SR.NotEqual),
            new OperatorViewModel("^=", SR.StartsWith),
            new OperatorViewModel("$=", SR.EndsWith),
            DefaultOperator,
            new OperatorViewModel("<", SR.LessThan),
            new OperatorViewModel("<=", SR.LessThanOrEqual),
            new OperatorViewModel(">", SR.GreaterThan),
            new OperatorViewModel(">=", SR.GreaterThanOrEqual)
        });

        public StringConditionViewModel(SearchPropertyViewModel property)
            : base(property)
        {
            _Operator = GetDefaultOperator();
        }

        private string GetDefaultOperator()
            => Operators.FirstOrDefault(e => e.Token == Property.Model.DefaultOperator)?.Token ?? DefaultOperator.Token;

        #region Operator

        private string _Operator;

        public string Operator
        {
            get => _Operator;
            set
            {
                if (SetProperty(ref _Operator, value))
                {
                    RaisePropertyChanged(nameof(SelectedOperator));
                }
            }
        }

        public OperatorViewModel SelectedOperator
        {
            get => Operators.FirstOrDefault(e => e.Token == _Operator) ?? DefaultOperator;
            set => Operator = (value ?? DefaultOperator).Token;
        }

        #endregion Operator

        #region Value

        private string _Value;

        public string Value
        {
            get => _Value;
            set => SetProperty(ref _Value, value);
        }

        #endregion Value

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
                    else
                    {
                        @operator = Property.Model.DefaultOperator?.TrimOrNull() ?? DefaultOperator.Token;
                    }
                }

                Operator = @operator;
                Value = value.TrimOrNull();
            }
        }

        public override bool HasValue => !string.IsNullOrEmpty(Value);

        public override void AppendValueTo(StringBuilder builder)
        {
            if (Operator != GetDefaultOperator())
            {
                builder.Append(Operator);
            }
            else if (Value.IndexOfAny(new[] { '"', ':', ' ', '<', '>', '=' }) < 0)
            {
                builder.Append(Value);
                return;
            }

            builder.Append('"');
            foreach (var c in Value)
            {
                if (c == '"')
                {
                    builder.Append('"');
                }
                builder.Append(c);
            }
            builder.Append('"');
        }
    }
}
