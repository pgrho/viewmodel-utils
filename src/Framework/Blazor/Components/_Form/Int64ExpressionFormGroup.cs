namespace Shipwreck.ViewModelUtils.Components;

public class Int64ExpressionFormGroup : NumberExpressionFormGroup<long>
{
    protected override double? GetStep() => base.GetStep() ?? 1;

    protected override double? GetMin() => Min ?? long.MinValue;

    protected override double? GetMax() => Max ?? long.MaxValue;
}
