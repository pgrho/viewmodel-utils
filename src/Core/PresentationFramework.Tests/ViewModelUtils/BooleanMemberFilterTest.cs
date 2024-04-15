namespace Shipwreck.ViewModelUtils;

using D = InlineDataAttribute;

public sealed class BooleanMemberFilterTest
{
    [Theory]
    [D(null, null, true)]
    [D(true, null, true)]
    [D(false, null, true)]
    [D(true, "1", true)]
    [D(true, "+1", true)]
    [D(true, "-1", true)]
    [D(true, "t", true)]
    [D(true, "true", true)]
    [D(true, "y", true)]
    [D(true, "yes", true)]
    [D(false, "0", true)]
    [D(false, "+0", true)]
    [D(false, "-0", true)]
    [D(false, "f", true)]
    [D(false, "false", true)]
    [D(false, "n", true)]
    [D(false, "no", true)]
    [D(null, "null", true)]
    [D(null, "other", false)]
    [D(true, "other", false)]
    [D(false, "other", false)]
    [D(true, "t,f", true)]
    [D(false, "t,f", true)]
    [D(null, "t,f", false)]
    public void Test(bool? testValue, string filter, bool isMatch)
    {
        var f = new BooleanMemberFilter<bool?>(e => e, _ => { })
        {
            Filter = filter
        };
        Assert.Equal(isMatch, f.IsMatch(testValue));

        if (testValue != null
            && !string.IsNullOrWhiteSpace(filter)
            && filter.IndexOf(',') < 0
            && isMatch)
        {
            Assert.False(f.IsMatch(!testValue));
            Assert.False(f.IsMatch(null));
        }
    }
}
