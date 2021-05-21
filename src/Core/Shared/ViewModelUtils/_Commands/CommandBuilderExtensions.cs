using System;

namespace Shipwreck.ViewModelUtils
{
    public static class CommandBuilderExtensions
    {
        #region CommandBuilderBase

        public static T SetTitle<T>(this T builder, string title)
            where T : CommandBuilderBase
        {
            builder.Title = title;
            return builder;
        }

        public static T SetTitle<T>(this T builder, Func<string> titleGetter)
            where T : CommandBuilderBase
        {
            builder.TitleGetter = titleGetter;
            return builder;
        }

        public static T SetMnemonic<T>(this T builder, string mnemonic)
            where T : CommandBuilderBase
        {
            builder.Mnemonic = mnemonic;
            return builder;
        }

        public static T SetMnemonic<T>(this T builder, Func<string> mnemonicGetter)
            where T : CommandBuilderBase
        {
            builder.MnemonicGetter = mnemonicGetter;
            return builder;
        }

        public static T SetDescription<T>(this T builder, string description)
            where T : CommandBuilderBase
        {
            builder.Description = description;
            return builder;
        }

        public static T SetDescription<T>(this T builder, Func<string> descriptionGetter)
            where T : CommandBuilderBase
        {
            builder.DescriptionGetter = descriptionGetter;
            return builder;
        }

        public static T SetIcon<T>(this T builder, string icon)
            where T : CommandBuilderBase
        {
            builder.Icon = icon;
            return builder;
        }

        public static T SetIcon<T>(this T builder, Func<string> iconGetter)
            where T : CommandBuilderBase
        {
            builder.IconGetter = iconGetter;
            return builder;
        }

        public static T SetStyle<T>(this T builder, BorderStyle? style)
            where T : CommandBuilderBase
        {
            builder.Style = style;
            return builder;
        }

        public static T SetStyle<T>(this T builder, Func<BorderStyle> styleGetter)
            where T : CommandBuilderBase
        {
            builder.StyleGetter = styleGetter;
            return builder;
        }

        public static T SetIsEnabled<T>(this T builder, bool? isEnabled)
            where T : CommandBuilderBase
        {
            builder.IsEnabled = isEnabled;
            return builder;
        }

        public static T SetIsEnabled<T>(this T builder, Func<bool> isEnabledGetter, bool isOverride = true)
            where T : CommandBuilderBase
        {
            if (isOverride || builder.IsEnabledGetter == null)
            {
                builder.IsEnabledGetter = isEnabledGetter;
            }
            else
            {
                var baseGetter = builder.IsEnabledGetter;
                builder.IsEnabledGetter = () => isEnabledGetter() && baseGetter();
            }
            return builder;
        }

        public static T SetIsVisible<T>(this T builder, bool? isVisible)
            where T : CommandBuilderBase
        {
            builder.IsVisible = isVisible;
            return builder;
        }

        public static T SetIsVisible<T>(this T builder, Func<bool> isVisibleGetter, bool isOverride = true)
            where T : CommandBuilderBase
        {
            if (isOverride || builder.IsVisibleGetter == null)
            {
                builder.IsVisibleGetter = isVisibleGetter;
            }
            else
            {
                var baseGetter = builder.IsVisibleGetter;
                builder.IsVisibleGetter = () => isVisibleGetter() && baseGetter();
            }
            return builder;
        }

        public static T SetBadgeCount<T>(this T builder, int? badgeCount)
            where T : CommandBuilderBase
        {
            builder.BadgeCount = badgeCount;
            return builder;
        }

        public static T SetBadgeCount<T>(this T builder, Func<int> badgeCountGetter)
            where T : CommandBuilderBase
        {
            builder.BadgeCountGetter = badgeCountGetter;
            return builder;
        }

        #endregion CommandBuilderBase
    }
}
