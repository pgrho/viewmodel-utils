using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;

namespace Shipwreck.ViewModelUtils
{
    public class ObjectValidator : ObservableModel
    {
        public ObjectValidator(object target, bool useExpressionBinding = false)
        {
            Target = target;
            ValidationResults = new BulkUpdateableCollection<ValidationResult>();
            UseExpressionBinding = useExpressionBinding;
        }

        public object Target { get; }
        public bool UseExpressionBinding { get; }
        public BulkUpdateableCollection<ValidationResult> ValidationResults { get; }

        public bool ValidateProperty(string propertyName, object value)
        {
            var vc = new ValidationContext(Target)
            {
                MemberName = propertyName
            };
            var results = new List<ValidationResult>();
            var r = Validator.TryValidateProperty(value, vc, results);
            ValidationResults.RemoveAll(a => a.MemberNames.Contains(propertyName));
            ValidationResults.AddRange(results);
            return r;
        }

        public bool Validate()
        {
            var vc = new ValidationContext(Target);

            var results = new List<ValidationResult>();
            Validator.TryValidateObject(Target, vc, results, true);
            ValidationResults.Set(results);

            return ValidationResults.Count == 0;
        }

        #region IsEditable

        private bool _IsEditable = true;

        public bool IsEditable
        {
            get => _IsEditable;
            set => SetProperty(ref _IsEditable, value);
        }

        public void SetEditable(bool value)
            => IsEditable = value;

        #endregion IsEditable
    }

    public sealed partial class ObjectValidator<T> : ObjectValidator
    {
        public ObjectValidator(T target, bool useExpressionBinding = false)
            : base(target, useExpressionBinding: useExpressionBinding)
        {
        }

        public new T Target => (T)base.Target;

        public Expression<Func<T, TProperty>> Lambda<TProperty>(Expression<Func<T, TProperty>> expression)
            => expression;
    }
}
