
namespace BlasII.Randomizer.Benchmarks;

public class SettingsWithDescription : RandomizerSettings
{
    public string Description { get; set; }

    public override string ToString()
    {
        return Description;
    }

    public static SettingsWithDescription CreateDefault(string description)
    {
        RandomizerSettings settings = DEFAULT;
        return new SettingsWithDescription()
        {
            Description = description,
            LogicType = settings.LogicType,
            RequiredKeys = settings.RequiredKeys,
            StartingWeapon = settings.StartingWeapon,
            ShuffleLongQuests = settings.ShuffleLongQuests,
            ShuffleShops = settings.ShuffleShops,
        };
    }

    public SettingsWithDescription SetLogicType(int logic)
    {
        LogicType = logic;
        return this;
    }

    public SettingsWithDescription SetRequiredKeys(int keys)
    {
        RequiredKeys = keys;
        return this;
    }

    public SettingsWithDescription SetStartingWeapon(int weapon)
    {
        StartingWeapon = weapon;
        return this;
    }

    public SettingsWithDescription SetLongQuests(bool quests)
    {
        ShuffleLongQuests = quests;
        return this;
    }

    public SettingsWithDescription SetShops(bool shops)
    {
        ShuffleShops = shops;
        return this;
    }
}
