using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Shipwreck.ViewModelUtils.Searching
{
    public sealed class NumberConditionViewModel : ConditionViewModel
    {
        public static OperatorViewModel DefaultOperator { get; } = new OperatorViewModel("=", SR.Equal);

        public static ReadOnlyCollection<OperatorViewModel> Operators { get; } = Array.AsReadOnly(new[] {
            DefaultOperator,
            new OperatorViewModel("!=",  SR.NotEqual),
            new OperatorViewModel("<", SR.LessThan),
            new OperatorViewModel("<=", SR.LessThanOrEqual),
            new OperatorViewModel(">", SR.GreaterThan),
            new OperatorViewModel(">=", SR.GreaterThanOrEqual)
        });

        public NumberConditionViewModel(SearchPropertyViewModel property)
            : base(property)
        {
            _Operator = Operators.FirstOrDefault(e => e.Token == property.Model.DefaultOperator)?.Token ?? DefaultOperator.Token;
        }

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

        private double? _Value;

        public double? Value
        {
            get => _Value;
            set => SetProperty(ref _Value, value);
        }

        #endregion Value

        public bool IsFloat
        {
            get
            {
                switch (Property.Model.TypeName)
                {
                    case nameof(Single):
                    case nameof(Double):
                    case nameof(Decimal):
                        return true;
                }
                return false;
            }
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
                    else
                    {
                        @operator = Property.Model.DefaultOperator?.TrimOrNull() ?? DefaultOperator.Token;
                    }
                }

                Operator = @operator;
                var d = double.TryParse(value, out var v) ? v : (double?)null;
                Value = d;
            }
        }

        public override bool HasValue => Value != null;

        public override void AppendValueTo(StringBuilder builder)
        {
            builder.Append(Operator);

            builder.Append(Value.Value.ToString("r"));
        }
        public override bool TryCreateDefaultValueExpression(out string @operator, out string defaultValue)
        {
            if (Value != null)
            {
                @operator = Operator;
                defaultValue = Value.Value.ToString("r");

                return true;
            }
            @operator = defaultValue = null;

            return false;
        }
    }
}
