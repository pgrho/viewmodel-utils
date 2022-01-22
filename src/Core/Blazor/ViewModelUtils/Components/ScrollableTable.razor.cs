namespace Shipwreck.ViewModelUtils.Components
{
    public partial class ScrollableTable<T> : StackPanel<T>
        where T : class
    {
        public ScrollableTable()
        {
            ItemSelector = ":scope > table > tbody > tr[data-itemindex]";
        }

        #region Theme

        private ScrollableTableTheme _Theme;

        [CascadingParameter]
        public ScrollableTableTheme Theme
        {
            get => _Theme;
            set => SetProperty(ref _Theme, value);
        }

        #endregion Theme

        #region HeaderHeight

        private string _HeaderHeight;

        [Parameter]
        public string HeaderHeight
        {
            get => _HeaderHeight;
            set => SetProperty(ref _HeaderHeight, value);
        }

        #endregion HeaderHeight

        #region Wrapper

        #region ElementClass

        private string _ElementClass;

        [Parameter]
        public string ElementClass
        {
            get => _ElementClass;
            set => SetProperty(ref _ElementClass, value);
        }

        #endregion ElementClass

        protected virtual IEnumerable<KeyValuePair<string, object>> GetElementAttributes()
        {
            var attrs = AdditionalAttributes.MergeAttributes(Theme?.ElementAttributes).AppendClass(ElementClass ?? Theme?.ElementClass);

            var h = HeaderHeight ?? Theme?.HeaderHeight;
            if (h != null)
            {
                attrs = attrs.PrependStyle("padding-top:" + h);
            }
            return attrs;
        }

        #endregion Wrapper

        #region Header Background

        #region HeaderBackgroundClass

        private string _HeaderBackgroundClass;

        [Parameter]
        public string HeaderBackgroundClass
        {
            get => _HeaderBackgroundClass;
            set => SetProperty(ref _HeaderBackgroundClass, value);
        }

        #endregion HeaderBackgroundClass

        #region HeaderBackgroundBackground

        private string _HeaderBackgroundBackground;

        [Parameter]
        public string HeaderBackgroundBackground
        {
            get => _HeaderBackgroundBackground;
            set => SetProperty(ref _HeaderBackgroundBackground, value);
        }

        #endregion HeaderBackgroundBackground

        #region HeaderBackgroundAttributes

        private Dictionary<string, object> _HeaderBackgroundAttributes;

        [Parameter]
        public Dictionary<string, object> HeaderBackgroundAttributes
        {
            get => _HeaderBackgroundAttributes;
            set => SetProperty(ref _HeaderBackgroundAttributes, value);
        }

        #endregion HeaderBackgroundAttributes

        #region HeaderTemplate

        private RenderFragment _HeaderTemplate;

        [Parameter]
        public RenderFragment HeaderTemplate
        {
            get => _HeaderTemplate;
            set => SetProperty(ref _HeaderTemplate, value);
        }

        #endregion HeaderTemplate

        #region FooterTemplate

        private RenderFragment _FooterTemplate;

        [Parameter]
        public RenderFragment FooterTemplate
        {
            get => _FooterTemplate;
            set => SetProperty(ref _FooterTemplate, value);
        }

        #endregion FooterTemplate

        protected virtual IEnumerable<KeyValuePair<string, object>> GetHeaderBackgroundAttributes()
        {
            var attrs = HeaderBackgroundAttributes.MergeAttributes(Theme?.HeaderBackgroundAttributes)
                            .AppendClass(HeaderBackgroundClass ?? Theme?.HeaderBackgroundClass);

            var s = "position: absolute;top: 0;width: 100%;";
            var h = HeaderHeight ?? Theme?.HeaderHeight;
            if (h != null)
            {
                s += ";height:" + h;
            }
            var bg = HeaderBackgroundBackground ?? Theme?.HeaderBackgroundBackground;
            if (bg != null)
            {
                s += ";background:" + bg;
            }
            return attrs.PrependStyle(s);
        }

        #endregion Header Background

        #region Scroller

        #region ScrollerClass

        private string _ScrollerClass;

        [Parameter]
        public string ScrollerClass
        {
            get => _ScrollerClass;
            set => SetProperty(ref _ScrollerClass, value);
        }

        #endregion ScrollerClass

        #region ScrollerAttributes

        private Dictionary<string, object> _ScrollerAttributes;

        [Parameter]
        public Dictionary<string, object> ScrollerAttributes
        {
            get => _ScrollerAttributes;
            set => SetProperty(ref _ScrollerAttributes, value);
        }

        #endregion ScrollerAttributes

        protected virtual IEnumerable<KeyValuePair<string, object>> GetScrollerAttributes()
            => ScrollerAttributes.MergeAttributes(Theme?.ScrollerAttributes)
                    .AppendClass(ScrollerClass ?? Theme?.ScrollerClass)
                    .PrependStyle("overflow-x: hidden;overflow-y: auto;min-height: 80%;max-height: 100%;-webkit-overflow-scrolling: touch;");

        #endregion Scroller

        #region Table

        #region TableClass

        private string _TableClass;

        [Parameter]
        public string TableClass
        {
            get => _TableClass;
            set => SetProperty(ref _TableClass, value);
        }

        #endregion TableClass

        #region TableAttributes

        private Dictionary<string, object> _TableAttributes;

        [Parameter]
        public Dictionary<string, object> TableAttributes
        {
            get => _TableAttributes;
            set => SetProperty(ref _TableAttributes, value);
        }

        #endregion TableAttributes

        protected virtual IEnumerable<KeyValuePair<string, object>> GetTableAttributes()
            => TableAttributes.MergeAttributes(Theme?.TableAttributes)
                    .AppendClass(TableClass ?? Theme?.TableClass)
                    .PrependStyle("overflow-anchor: none");

        #endregion Table

        #region Table Head

        #region TableHeadClass

        private string _TableHeadClass;

        [Parameter]
        public string TableHeadClass
        {
            get => _TableHeadClass;
            set => SetProperty(ref _TableHeadClass, value);
        }

        #endregion TableHeadClass

        #region TableHeadAttributes

        private Dictionary<string, object> _TableHeadAttributes;

        [Parameter]
        public Dictionary<string, object> TableHeadAttributes
        {
            get => _TableHeadAttributes;
            set => SetProperty(ref _TableHeadAttributes, value);
        }

        #endregion TableHeadAttributes



        protected virtual IEnumerable<KeyValuePair<string, object>> GetTableHeadAttributes()
            => TableHeadAttributes.MergeAttributes(Theme?.TableHeadAttributes)
                .AppendClass(TableHeadClass ?? Theme?.TableHeadClass);

        #endregion Table Head

        #region Body

        #region TableBodyClass

        private string _TableBodyClass;

        [Parameter]
        public string TableBodyClass
        {
            get => _TableBodyClass;
            set => SetProperty(ref _TableBodyClass, value);
        }

        #endregion TableBodyClass

        #region TableBodyAttributes

        private Dictionary<string, object> _TableBodyAttributes;

        [Parameter]
        public Dictionary<string, object> TableBodyAttributes
        {
            get => _TableBodyAttributes;
            set => SetProperty(ref _TableBodyAttributes, value);
        }

        #endregion TableBodyAttributes

        protected virtual IEnumerable<KeyValuePair<string, object>> GetTableBodyAttributes()

            => TableBodyAttributes.MergeAttributes(Theme?.TableBodyAttributes)
                .AppendClass(TableBodyClass ?? Theme?.TableBodyClass)
                .PrependStyle("overflow-anchor:none");

        #endregion Body

        protected override void RenderFirstPadding(RenderTreeBuilder builder, ref int sequence, int firstIndex)
        {
            float height;
            if (firstIndex <= 0)
            {
                height = 0;
            }
            else
            {
                var el = Lines.FirstOrDefault(e => e.FirstIndex <= firstIndex);
                if (el != null)
                {
                    height = el.Top + el.Height * (firstIndex - el.FirstIndex) / (el.LastIndex - el.FirstIndex + 1);
                }
                else
                {
                    height = firstIndex * ItemHeight;
                }
            }

            RenderPaddingCore(
                  builder,
                  ref sequence,
                  0,
                  firstIndex - 1,
                  Math.Max(0, height),
                  tagName: "tr");
        }

        protected override void RenderLastPadding(RenderTreeBuilder builder, ref int sequence, int lastIndex)
            => RenderPaddingCore(
                builder,
                ref sequence,
                lastIndex + 1,
                Source.Count - 1,
                Math.Max(0, (Source.Count - lastIndex + ColumnCount - 2) / ColumnCount * ItemHeight), tagName: "tr");
    }
}
