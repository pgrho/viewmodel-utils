namespace Shipwreck.ViewModelUtils.JSInterop
{
    public sealed class ItemsControllLineInfo
    {
        public int FirstIndex { get; set; }
        public int LastIndex { get; set; }
        public float Top { get; set; }
        public float Height { get; set; }

        public float Bottom => Top + Height;

        public override string ToString()
            => $"{FirstIndex}-{LastIndex} {{{Top}-{Bottom}}}";
    }
}
