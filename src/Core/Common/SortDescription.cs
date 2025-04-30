namespace Shipwreck.ViewModelUtils;

public struct SortDescription
{
    public SortDescription(string member, bool isDescending)
    {
        Member = member;
        IsDescending = isDescending;
    }

    public string Member { get; }
    public bool IsDescending { get; }

    public static string? ToString(IEnumerable<SortDescription>? sortDescriptions)
        => sortDescriptions == null ? null : string.Join(",", sortDescriptions.Select(e => e.IsDescending ? e.Member + " desc" : e.Member));

    public static SortDescription[] Parse(string? order)
    {
        if (!string.IsNullOrWhiteSpace(order))
        {
            var ss = order.Split(',');
            var list = new List<SortDescription>(ss.Length);
            var wp = new[] { ' ', '\t', '\r', '\n' };

            foreach (var s in ss)
            {
                var comps = s.Split(wp);

                var n = comps.FirstOrDefault();

                if (string.IsNullOrWhiteSpace(n) || list.Any(e => e.Member == n))
                {
                    continue;
                }

                list.Add(new SortDescription(n, "desc".Equals(comps.ElementAtOrDefault(1), StringComparison.InvariantCultureIgnoreCase)));
            }
            if (list.Count > 0)
            {
                return list.ToArray();
            }
        }
        return [];
    }
}
