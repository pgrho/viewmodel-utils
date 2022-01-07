namespace Shipwreck.ViewModelUtils.Configuration;

public interface IUpgradableSettings
{
    bool IsSettingsUpgraded { get; set; }

    void Upgrade();
}
