namespace Shipwreck.ViewModelUtils.Controls
{
    public sealed class PaginatorLinkModel
    {
        internal PaginatorLinkModel(int index, PaginatorLinkType type = PaginatorLinkType.Number, bool isActive = false)
        {
            Index = index;
            Type = type;
            IsActive = isActive;
        }

        public int Index { get; }

        public PaginatorLinkType Type { get; }
        public bool IsActive { get; }
        public int Number => Index + 1;

        public override int GetHashCode() => Index ^ (IsActive ? unchecked((int)0x80000000) : 0) ^ ((int)Type << 24);

        public override bool Equals(object obj)
            => obj is PaginatorLinkModel other
            && other.Index == Index
            && other.IsActive == IsActive
            && other.Type == Type;
    }
}
