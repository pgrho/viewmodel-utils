using System;

namespace Shipwreck.ViewModelUtils
{
    public static class CommandBuilderExtensions
    {
        #region CommandBuilderBase

        public static T SetTitle<T>(this T builder, string title, bool isOverride = true)
            where T : CommandBuilderBase
        {
            builder.Title = title;
            if (isOverride)
            {
                builder.TitleGetter = null;
            }
            return builder;
        }

        public static T SetTitle<T>(this T builder, Func<string> titleGetter, bool isOverride = true)
            where T : CommandBuilderBase
        {
            builder.TitleGetter = titleGetter;
            if (isOverride)
            {
                builder.Title = null;
            }
            return builder;
        }

        public static T SetMnemonic<T>(this T builder, string mnemonic, bool isOverride = true)
            where T : CommandBuilderBase
        {
            builder.Mnemonic = mnemonic;
            if (isOverride)
            {
                builder.MnemonicGetter = null;
            }
            return builder;
        }

        public static T SetMnemonic<T>(this T builder, Func<string> mnemonicGetter, bool isOverride = true)
            where T : CommandBuilderBase
        {
            builder.MnemonicGetter = mnemonicGetter;
            if (isOverride)
            {
                builder.Mnemonic = null;
            }
            return builder;
        }

        public static T SetDescription<T>(this T builder, string description, bool isOverride = true)
            where T : CommandBuilderBase
        {
            builder.Description = description;
            if (isOverride)
            {
                builder.DescriptionGetter = null;
            }
            return builder;
        }

        public static T SetDescription<T>(this T builder, Func<string> descriptionGetter, bool isOverride = true)
            where T : CommandBuilderBase
        {
            builder.DescriptionGetter = descriptionGetter;
            if (isOverride)
            {
                builder.Description = null;
            }
            return builder;
        }

        public static T SetIcon<T>(this T builder, string icon, bool isOverride = true)
            where T : CommandBuilderBase
        {
            builder.Icon = icon;
            if (isOverride)
            {
                builder.IconGetter = null;
            }
            return builder;
        }

        public static T SetIcon<T>(this T builder, Func<string> iconGetter, bool isOverride = true)
            where T : CommandBuilderBase
        {
            builder.IconGetter = iconGetter;
            if (isOverride)
            {
                builder.Icon = null;
            }
            return builder;
        }

        public static T SetStyle<T>(this T builder, BorderStyle? style, bool isOverride = true)
            where T : CommandBuilderBase
        {
            builder.Style = style;
            if (isOverride)
            {
                builder.StyleGetter = null;
            }
            return builder;
        }

        public static T SetStyle<T>(this T builder, Func<BorderStyle> styleGetter, bool isOverride = true)
            where T : CommandBuilderBase
        {
            builder.StyleGetter = styleGetter;
            if (isOverride)
            {
                builder.Style = null;
            }
            return builder;
        }

        public static T SetIsEnabled<T>(this T builder, bool? isEnabled, bool isOverride = true)
            where T : CommandBuilderBase
        {
            builder.IsEnabled = isEnabled;
            if (isOverride)
            {
                builder.IsEnabledGetter = null;
            }
            return builder;
        }

        public static T SetIsEnabled<T>(this T builder, Func<bool> isEnabledGetter, bool isOverride = true)
            where T : CommandBuilderBase
        {
            if (isOverride)
            {
                builder.IsEnabled = null;
            }
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

        public static T SetIsVisible<T>(this T builder, bool? isVisible, bool isOverride = true)
            where T : CommandBuilderBase
        {
            builder.IsVisible = isVisible;
            if (isOverride)
            {
                builder.IsVisibleGetter = null;
            }
            return builder;
        }

        public static T SetIsVisible<T>(this T builder, Func<bool> isVisibleGetter, bool isOverride = true)
            where T : CommandBuilderBase
        {
            if (isOverride)
            {
                builder.IsVisible = null;
            }
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

        public static T SetBadgeCount<T>(this T builder, int? badgeCount, bool isOverride = true)
            where T : CommandBuilderBase
        {
            builder.BadgeCount = badgeCount;
            if (isOverride)
            {
                builder.BadgeCountGetter = null;
            }
            return builder;
        }

        public static T SetBadgeCount<T>(this T builder, Func<int> badgeCountGetter, bool isOverride = true)
            where T : CommandBuilderBase
        {
            builder.BadgeCountGetter = badgeCountGetter;
            if (isOverride)
            {
                builder.BadgeCount = null;
            }
            return builder;
        }

        #endregion CommandBuilderBase
    }
}
