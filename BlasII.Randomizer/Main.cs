using MelonLoader;

namespace BlasII.Randomizer;

internal class Main : MelonMod
{
    public static Randomizer Randomizer { get; private set; }

    public override void OnLateInitializeMelon()
    {
        Randomizer = new Randomizer();
    }
}