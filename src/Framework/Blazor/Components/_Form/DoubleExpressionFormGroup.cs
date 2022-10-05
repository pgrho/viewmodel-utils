namespace Shipwreck.ViewModelUtils.Components;

public class DoubleExpressionFormGroup : NumberExpressionFormGroup<double>
{
    protected override double? GetStep() => base.GetStep() ?? 0.0000001;
}
