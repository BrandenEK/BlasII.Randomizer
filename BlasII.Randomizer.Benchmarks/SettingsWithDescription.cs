using BlasII.Randomizer.Settings;

namespace BlasII.Randomizer.Benchmarks;

public class SettingsWithDescription : RandomizerSettings
{
    public string Description { get; set; }

    public SettingsWithDescription(string description)
    {
        Description = description;

        RandomizerSettings settings = SettingsGenerator.CreateFromPreset(Preset.Standard);
        LogicType = settings.LogicType;
        RequiredKeys = settings.RequiredKeys;
        StartingWeapon = settings.StartingWeapon;
        ShopMultiplier = settings.ShopMultiplier;

        AddPenitenceRewards = settings.AddPenitenceRewards;
        ShuffleCherubs = settings.ShuffleCherubs;
        ShuffleLongQuests = settings.ShuffleLongQuests;
    }

    public override string ToString()
    {
        return Description;
    }
}
