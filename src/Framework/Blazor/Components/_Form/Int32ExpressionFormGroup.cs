namespace Shipwreck.ViewModelUtils.Components;

public class Int32ExpressionFormGroup : NumberExpressionFormGroup<int>
{
    protected override double? GetStep() => base.GetStep() ?? 1;

    protected override double? GetMin() => Min ?? int.MinValue;

    protected override double? GetMax() => Max ?? int.MaxValue;
}
