
namespace BlasII.Randomizer.Benchmarks;

public class SettingsWithDescription : RandomizerSettings
{
    public string Description { get; set; }

    public override string ToString()
    {
        return Description;
    }

    public static new SettingsWithDescription DEFAULT
    {
        get
        {
            RandomizerSettings settings = RandomizerSettings.DEFAULT;
            return new SettingsWithDescription()
            {
                Description = "Default",
                LogicType = settings.LogicType,
                RequiredKeys = settings.RequiredKeys,
                StartingWeapon = settings.StartingWeapon,
                ShuffleLongQuests = settings.ShuffleLongQuests,
                ShuffleShops = settings.ShuffleShops,
            };
        }
    }
}
