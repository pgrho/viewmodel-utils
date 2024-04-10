namespace Shipwreck.ViewModelUtils;

using D = InlineDataAttribute;

public class Int64MemberFilterTest
{
    [Theory]
    [D(null, null, true), D(1, null, true)]
    [D(0, "1", false), D(1, "1", true), D(2, "1", false), D(null, "1", false)]
    [D(0, "!=1", true), D(1, "!=1", false), D(2, "!=1", true), D(null, "!=1", true)]
    [D(0, "<=1", true), D(1, "<=1", true), D(2, "<=1", false), D(null, "<=1", false)]
    [D(0, "<1", true), D(1, "<1", false), D(2, "<1", false), D(null, "<1", false)]
    [D(0, ">=1", false), D(1, ">=1", true), D(2, ">=1", true), D(null, ">=1", false)]
    [D(0, ">1", false), D(1, ">1", false), D(2, ">1", true), D(null, ">1", false)]
    [D(0, "1..3", false), D(1, "1..3", true), D(2, "1..3", true), D(3, "1..3", true), D(4, "1..3", false)]
    public void Test(int? testValue, string filter, bool isMatch)
        => Assert.Equal(
            isMatch,
            new Int64MemberFilter<long?>(e => e, _ => { })
            {
                Filter = filter
            }.IsMatch(testValue));
}
