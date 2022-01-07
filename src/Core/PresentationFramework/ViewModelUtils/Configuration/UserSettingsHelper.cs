using System.Configuration;

namespace Shipwreck.ViewModelUtils.Configuration;

public static class UserSettingsHelper
{
    public static void SaveAndProtect(this ApplicationSettingsBase settings)
    {
        settings.Save();
        settings.Protect();
    }

    public static bool Protect(this ApplicationSettingsBase settings)
    {
        var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.PerUserRoamingAndLocal);
        var sec = config.GetSectionGroup("userSettings")?.Sections[settings.GetType().FullName];
        if (sec != null)
        {
            try
            {
                if (!sec.SectionInformation.IsProtected)
                {
                    sec.SectionInformation.ProtectSection(nameof(RsaProtectedConfigurationProvider));
                    config.Save(ConfigurationSaveMode.Minimal);
                }

                return true;
            }
            catch
            {
            }
        }

        return false;
    }

    public static bool Unprotect(this ApplicationSettingsBase settings)
    {
        var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.PerUserRoamingAndLocal);
        var sec = config.GetSectionGroup("userSettings")?.Sections[settings.GetType().FullName];
        if (sec != null)
        {
            try
            {
                if (sec.SectionInformation.IsProtected)
                {
                    sec.SectionInformation.UnprotectSection();
                    config.Save(ConfigurationSaveMode.Minimal);
                }

                return true;
            }
            catch
            {
            }
        }

        return false;
    }

    public static bool UpgradeIfNeeded(this IUpgradableSettings settings)
    {
        if (!settings.IsSettingsUpgraded)
        {
            try
            {
                settings.Upgrade();
            }
            catch { }

            settings.IsSettingsUpgraded = true;
            return true;
        }

        return false;
    }

    public static void CancelPropertyChangedIfSameValue(this ApplicationSettingsBase settings)
    {
        settings.SettingChanging += (sender, e) =>
        {
            if (Equals(((ApplicationSettingsBase)sender)[e.SettingName], e.NewValue))
            {
                e.Cancel = true;
            }
        };
    }
}
