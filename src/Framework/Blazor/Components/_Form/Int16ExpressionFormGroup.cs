namespace Shipwreck.ViewModelUtils.Components;

public class Int16ExpressionFormGroup : NumberExpressionFormGroup<short>
{
    protected override double? GetStep() => base.GetStep() ?? 1;

    protected override double? GetMin() => Min ?? short.MinValue;

    protected override double? GetMax() => Max ?? short.MaxValue;
}
