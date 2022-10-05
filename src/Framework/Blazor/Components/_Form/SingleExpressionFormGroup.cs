namespace Shipwreck.ViewModelUtils.Components;

public class SingleExpressionFormGroup : NumberExpressionFormGroup<float>
{
    protected override double? GetStep() => base.GetStep() ?? 0.0001;
}
