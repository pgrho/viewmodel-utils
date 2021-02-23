using System;

namespace Shipwreck.ViewModelUtils.Demo.PresentationFramework
{
    public sealed class CalculatorModalViewModel : DialogViewModel
    {
        #region Input

        public string _Input = "0";

        public string Input
        {
            get => _Input;
            private set => SetProperty(ref _Input, value);
        }

        #endregion Input

        #region Buffer

        private string _Buffer;

        public string Buffer
        {
            get => _Buffer;
            private set => SetProperty(ref _Buffer, value);
        }

        #endregion Buffer

        #region AppendCommand

        private ParameteredCommand _AppendCommand;

        public ParameteredCommand AppendCommand
            => _AppendCommand ??= new ParameteredCommand(obj =>
            {
                var v = ((IConvertible)obj).ToInt32(null);
                if (_Input == "0")
                {
                    if (v == 0)
                    {
                        return;
                    }
                    else if (0 < v && v <= 9)
                    {
                        Input = v.ToString();
                    }
                    else
                    {
                        throw new NotSupportedException();
                    }
                }
                else
                {
                    if (0 <= v && v <= 9)
                    {
                        Input += v;
                    }
                    else
                    {
                        throw new NotSupportedException();
                    }
                }
            });

        #endregion AppendCommand

        #region AddCommand

        private Command _AddCommand;

        public Command AddCommand
            => _AddCommand ??= new Command(() =>
            {
                var nv = ComputeCore();
                Buffer = nv + "+";
                Input = "0";
            });

        #endregion AddCommand

        #region SubtractCommand

        private Command _SubtractCommand;

        public Command SubtractCommand
            => _SubtractCommand ??= new Command(() =>
            {
                var nv = ComputeCore();
                Buffer = nv + "-";
                Input = "0";
            });

        #endregion SubtractCommand

        #region MultiplyCommand

        private Command _MultiplyCommand;

        public Command MultiplyCommand
            => _MultiplyCommand ??= new Command(() =>
            {
                var nv = ComputeCore();
                Buffer = nv + "*";
                Input = "0";
            });

        #endregion MultiplyCommand

        #region ExecuteCommand

        private Command _ExecuteCommand;

        public Command ExecuteCommand
            => _ExecuteCommand ??= new Command(() =>
            {
                var nv = ComputeCore();
                Buffer = string.Empty;
                Input = nv.ToString();
            });

        #endregion ExecuteCommand

        #region ClearCommand

        private Command _ClearCommand;

        public Command ClearCommand
            => _ClearCommand ??= new Command(() =>
            {
                Buffer = string.Empty;
                Input = "0";
            });

        #endregion ClearCommand

        private long ComputeCore()
        {
            var i = _Input;
            var b = _Buffer;
            if (string.IsNullOrEmpty(_Buffer))
            {
                return long.Parse(i);
            }
            else if (_Buffer.EndsWith("+"))
            {
                return long.Parse(_Buffer.Substring(0, _Buffer.Length - 1)) + long.Parse(i);
            }
            else if (_Buffer.EndsWith("-"))
            {
                return long.Parse(_Buffer.Substring(0, _Buffer.Length - 1)) - long.Parse(i);
            }
            else if (_Buffer.EndsWith("*"))
            {
                return long.Parse(_Buffer.Substring(0, _Buffer.Length - 1)) * long.Parse(i);
            }
            else
            {
                throw new NotSupportedException();
            }
        }
    }
}
