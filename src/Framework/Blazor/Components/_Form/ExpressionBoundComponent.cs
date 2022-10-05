using System.Collections.Specialized;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

namespace Shipwreck.ViewModelUtils.Components;

public abstract class ExpressionBoundComponent<T> : BindableComponentBase
{
    #region Expression

    private LambdaExpression _Expression;

    [Parameter]
    public LambdaExpression Expression
    {
        get => _Expression;
        set
        {
            if (SetProperty(ref _Expression, value))
            {
                OnExpressionChanged();
            }
        }
    }

    protected virtual void OnExpressionChanged()
    {
        _Member = null;
        _ValidatorErrorMessage = null;
    }

    private MemberInfo _Member;

    protected MemberInfo Member
        => _Member ??= (_Expression?.Body as MemberExpression)?.Member;

    protected string GetDisplayNameFromExpression()
        => Member?.GetCustomAttribute<DisplayAttribute>()?.GetName()
        ?? Member?.GetCustomAttribute<DisplayNameAttribute>()?.DisplayName
        ?? Member.Name;

    protected int? GetMinLengthFromExpression()
        => Member?.GetCustomAttribute<StringLengthAttribute>()?.MinimumLength;

    protected int? GetMaxLengthFromExpression()
        => Member?.GetCustomAttribute<StringLengthAttribute>()?.MaximumLength
        ?? Member?.GetCustomAttribute<MaxLengthAttribute>()?.Length;

    protected object GetMinFromExpression()
        => Member?.GetCustomAttribute<RangeAttribute>()?.Minimum;

    protected object GetMaxFromExpression()
        => Member?.GetCustomAttribute<RangeAttribute>()?.Maximum;

    protected string GetRegularExpressionFromExpression()
        => Member?.GetCustomAttribute<RegularExpressionAttribute>()?.Pattern;

    protected bool GetIsRequiredFromExpression()
        => Member?.GetCustomAttribute<RequiredAttribute>() != null;

    #endregion Expression

    #region Validator

    private ObjectValidator _Validator;

    [CascadingParameter]
    public ObjectValidator Validator
    {
        get => _Validator;
        set
        {
            var old = _Validator;
            if (SetProperty(ref _Validator, value))
            {
                _ValidatorErrorMessage = null;
                if (old != null)
                {
                    old.ValidationResults.CollectionChanged -= ValidationResults_CollectionChanged;
                    if (old.UseExpressionBinding)
                    {
                        (old.Target as INotifyPropertyChanged).RemovePropertyChanged(InputComponentBase_PropertyChanged);
                    }
                }
                if (Validator != null)
                {
                    Validator.ValidationResults.CollectionChanged += ValidationResults_CollectionChanged;
                    if (Validator.UseExpressionBinding)
                    {
                        (Validator.Target as INotifyPropertyChanged).AddPropertyChanged(InputComponentBase_PropertyChanged);
                        SetValueFromTarget();
                    }
                }
            }
        }
    }

    private void InputComponentBase_PropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == Member?.Name)
        {
            SetValueFromTarget();
        }
    }

    private void SetValueFromTarget()
    {
        if (Validator?.Target != null && Member != null)
        {
            var p = TypeDescriptor.GetProperties(Validator.Target).Find(Member.Name, false);
            if (p != null)
            {
                var v = p.GetValue(Validator.Target);
                Value = (T)(v is IConvertible c ? c.ToType(Nullable.GetUnderlyingType(typeof(T)) ?? typeof(T), null) : v);
            }
        }
    }

    private void UpdateValueToTarget()
    {
        if (Validator?.Target != null && Member != null)
        {
            var p = TypeDescriptor.GetProperties(Validator.Target).Find(Member.Name, false);
            if (p != null)
            {
                p.SetValue(
                    Validator.Target,
                    Value != null && p.Converter?.CanConvertFrom(Value.GetType()) == true
                    ? p.Converter.ConvertFrom(Value)
                    : Value);
            }
        }
    }

    private void ValidationResults_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        if (ShowErrorMessage == true
            && _ErrorMessage == null
            && Member?.Name != null)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Move:
                    return;

                case NotifyCollectionChangedAction.Add:
                case NotifyCollectionChangedAction.Remove:
                case NotifyCollectionChangedAction.Replace:
                    if ((e.Action != NotifyCollectionChangedAction.Remove
                            && e.NewItems?.Cast<ValidationResult>().Any(r => r.MemberNames.Contains(Member.Name)) != false)
                        || (e.Action != NotifyCollectionChangedAction.Add
                            && e.OldItems?.Cast<ValidationResult>().Any(r => r.MemberNames.Contains(Member.Name)) != false))
                    {
                        UpdateValidatorErrorMessage();
                    }
                    return;

                case NotifyCollectionChangedAction.Reset:
                default:
                    UpdateValidatorErrorMessage();
                    break;
            }
        }
    }

    #endregion Validator

    #region InputId

    private static int _UsedId;
    private string _InputId;

    [Parameter]
    public string InputId
    {
        get => _InputId ??= GetCoreId() + "--" + (++_UsedId);
        set => SetProperty(ref _InputId, value);
    }

    protected virtual string GetCoreId()
        => Member != null ? Member.Name : "FormGroup";

    #endregion InputId

    #region ErrorMessage

    private string _ErrorMessage;
    private string _ValidatorErrorMessage;

    [Parameter]
    public string ErrorMessage
    {
        get => _ErrorMessage;
        set => SetProperty(ref _ErrorMessage, value);
    }

    protected virtual string GetValidatorErrorMessage()
        => Validator?.ValidationResults.FirstOrDefault(e => e.MemberNames.Contains(Member.Name))?.ErrorMessage;

    protected void UpdateValidatorErrorMessage()
    {
        var nv = GetValidatorErrorMessage() ?? string.Empty;
        var changed = _ValidatorErrorMessage != null && _ValidatorErrorMessage != nv;
        _ValidatorErrorMessage = nv;
        if (changed)
        {
            StateHasChanged();
        }
    }

    protected virtual string GetErrorMessage()
    {
        if (_ErrorMessage != null)
        {
            return _ErrorMessage;
        }
        else if (Member != null)
        {
            return _ValidatorErrorMessage ??= GetValidatorErrorMessage() ?? string.Empty;
        }
        return null;
    }

    #endregion ErrorMessage

    #region Value

    private T _Value;

    [Parameter]
    public T Value
    {
        get => _Value;
        set => SetValue(value);
    }

    protected T InternalValue
    {
        get => _Value;
        set => SetValue(value, false);
    }

    [Parameter]
    public Action<T> ValueChanged { get; set; }

    protected void SetValue(T value, bool shouldNotify = true)
    {
        if (SetProperty(ref _Value, value, shouldNotify: shouldNotify, propertyName: nameof(Value)))
        {
            OnValueChanged();
        }
    }

    protected virtual void OnValueChanged()
    {
        if (Validator?.UseExpressionBinding == true)
        {
            UpdateValueToTarget();
        }
        ValueChanged?.Invoke(_Value);

        if (_Validator != null
            && ShowErrorMessage == true
            && Member != null)
        {
            _Validator.ValidateProperty(Member.Name, _Value);
        }
    }

    #endregion Value

    #region Description

    private string _Description;

    [Parameter]
    public string Description
    {
        get => _Description;
        set => SetProperty(ref _Description, value);
    }

    protected virtual string GetDescription()
        => _Description;

    #endregion Description

    #region IsRequired

    private bool _IsRequired;

    [Parameter]
    public bool IsRequired
    {
        get => _IsRequired;
        set => SetProperty(ref _IsRequired, value);
    }

    protected virtual bool GetIsRequired()
        => IsRequired;

    #endregion IsRequired

    #region IsEnabled

    private bool _IsEnabled = true;

    [Parameter]
    public bool IsEnabled
    {
        get => _IsEnabled;
        set => SetProperty(ref _IsEnabled, value);
    }

    [Parameter]
    public Action Focused { get; set; }

    [Parameter]
    public Action Blurred { get; set; }

    protected virtual bool GetIsDisabled()
        => !IsEnabled || (Validator?.IsEditable == false);

    #endregion IsEnabled

    #region ShowErrorMessage

    private bool? _ShowErrorMessage;

    [Parameter]
    public bool? ShowErrorMessage
    {
        get => _ShowErrorMessage;
        set => SetProperty(ref _ShowErrorMessage, value);
    }

    #endregion ShowErrorMessage

    protected override bool ImplicitRender => false;

    public abstract ValueTask FocusAsync(bool selectAll = false);

    protected virtual void OnFocus()
    {
        Focused?.Invoke();
        OnFocusCommand?.Execute(OnFocusCommandParameter);
    }

    [Parameter]
    public ICommand OnFocusCommand { get; set; }

    [Parameter]
    public object OnFocusCommandParameter { get; set; }

    [Parameter]
    public ICommand EnterCommand { get; set; }

    [Parameter]
    public object EnterCommandParameter { get; set; }

    protected virtual void OnBlur()
    {
        ShowErrorMessage = ShowErrorMessage ?? true;
        Blurred?.Invoke();
        EnterCommand?.Execute(EnterCommandParameter);
    }
}
