namespace Shipwreck.ViewModelUtils.Components;

public class ByteExpressionFormGroup : NumberExpressionFormGroup<byte>
{
    protected override double? GetStep() => base.GetStep() ?? 1;

    protected override double? GetMin() => Min ?? byte.MinValue;

    protected override double? GetMax() => Max ?? byte.MaxValue;
}
