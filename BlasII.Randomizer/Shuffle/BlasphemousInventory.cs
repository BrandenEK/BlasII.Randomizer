using Basalt.LogicParser;
using Basalt.LogicParser.Attributes;
using Basalt.LogicParser.Calculators;
using Basalt.LogicParser.Collectors;
using Basalt.LogicParser.Formatters;
using Basalt.LogicParser.Parsers;
using Basalt.LogicParser.Resolvers;

namespace BlasII.Randomizer.Shuffle;

/// <summary>
/// Represents the current inventory of the player during the fill process
/// </summary>
public class BlasphemousInventory
{
    // Weapons

    [CollectableAs("Censer")]
    int CenserLevel { get; set; }
    [ResolvableAs("censer")]
    bool HasCenser => CenserLevel > 0;
    [ResolvableAs("censerfx")]
    bool HasCenserEffect => CenserLevel > 0; // or liberated

    [CollectableAs("RosaryBlade")]
    int RosaryLevel { get; set; }
    [ResolvableAs("rosary")]
    bool HasRosary => RosaryLevel > 0;
    [ResolvableAs("rosaryfx")]
    bool HasRosaryEffect => RosaryLevel > 0 || MeaCulpaLevel > 0;

    [CollectableAs("Rapier")]
    int RapierLevel { get; set; }
    [ResolvableAs("rapier")]
    bool HasRapier => RapierLevel > 0;
    [ResolvableAs("rapierfx")]
    bool HasRapierEffect => RapierLevel > 0; // or liberated

    [CollectableAs("MeaCulpa")]
    int MeaCulpaLevel { get; set; }
    [ResolvableAs("meaculpa")]
    bool HasMeaCulpa => MeaCulpaLevel > 0;

    // Abilities

    [CollectableAs("WallClimb")]
    [ResolvableAs("wallclimb")]
    bool WallClimb { get; set; }

    [CollectableAs("AirJump")]
    [ResolvableAs("doublejump")]
    bool DoubleJump { get; set; }

    [CollectableAs("AirDash")]
    [ResolvableAs("airdash")]
    bool AirDash { get; set; }

    [CollectableAs("MagicRingClimb")]
    [ResolvableAs("ringclimb")]
    bool RingClimb { get; set; }

    [CollectableAs("GlassWalk")]
    [ResolvableAs("glasswalk")]
    bool GlassWalk { get; set; }

    // Bosses

    [CollectableAs("QI63", "QI64", "QI65", "QI66", "QI67")]
    [ResolvableAs("bosskeys")]
    int NumBossKeys { get; set; }

    // Cherub quest

    [CollectableAs("CH")]
    [ResolvableAs("cherubs")]
    int NumCherubs { get; set; }

    [CollectableAs("QI54")]
    [ResolvableAs("rattle")]
    bool Rattle { get; set; }

    // Elder quest

    [CollectableAs("QI07")]
    [ResolvableAs("elderscroll")]
    bool ElderScroll { get; set; }

    [CollectableAs("QI08")]
    [ResolvableAs("eldercloth")]
    bool ElderCloth { get; set; }

    // Gold quest

    [CollectableAs("GL")]
    [ResolvableAs("goldlumps")]
    int GoldLumps { get; set; }

    // Hand quest

    [CollectableAs("QI37", "QI38", "QI39", "QI40", "QI41")]
    [ResolvableAs("kisses")]
    int NumKisses { get; set; }

    [CollectableAs("QI28")]
    [ResolvableAs("brokenkey")]
    bool BrokenKey { get; set; }

    // Letter quest

    [CollectableAs("QI14")]
    [ResolvableAs("letter1")]
    bool LetterOne { get; set; }

    [CollectableAs("QI16")]
    [ResolvableAs("letter2")]
    bool LetterTwo { get; set; }

    [CollectableAs("QI18")]
    [ResolvableAs("letter3")]
    bool LetterThree { get; set; }

    [CollectableAs("QI20")]
    [ResolvableAs("letter4")]
    bool LetterFour { get; set; }

    [CollectableAs("QI22")]
    [ResolvableAs("letter5")]
    bool LetterFive { get; set; }

    // Lullaby quest

    [CollectableAs("UL")]
    [ResolvableAs("lullabies")]
    int NumLullabies { get; set; }

    // Mud quest

    [CollectableAs("QI101")]
    [ResolvableAs("mudkey")]
    bool MudKey { get; set; }

    [CollectableAs("QI103")]
    [ResolvableAs("ceramickey")]
    bool CeramicKey { get; set; }

    // Regula quest

    [CollectableAs("QI05")]
    [ResolvableAs("regulacloth")]
    bool RegulaCloth { get; set; }

    // Sculptor quest

    [CollectableAs("ST")]
    [ResolvableAs("tools")]
    int NumTools { get; set; }

    // Tribute quest

    [CollectableAs("QI29", "QI30", "QI31")]
    [ResolvableAs("tributes")]
    int NumTributes { get; set; }

    // Wax quest

    [CollectableAs("QI56", "QI57", "QI58", "QI59", "QI60", "QI61")]
    [ResolvableAs("waxseeds")]
    int NumWaxSeeds { get; set; }

    // Yerma quest

    [CollectableAs("QI68")]
    [ResolvableAs("holyoil")]
    bool HolyOil { get; set; }

    // Rooms

    [ResolvableAs("daughterrooms")]
    int DaughterRooms
    {
        get
        {
            int rooms = 0;
            if (WallClimb && DoubleJump) rooms++;
            if (WallClimb && DoubleJump && HasCenser && HasRosary && HasRapier) rooms++;
            if (WallClimb && DoubleJump && AirDash && HasRosary) rooms++;
            if (DoubleJump && AirDash && RingClimb) rooms++;
            if (WallClimb && DoubleJump && AirDash && RingClimb && HasCenser && HasRosary && HasRapier) rooms++;
            return rooms;
        }
    }

    [ResolvableAs("shoprooms")]
    int ShopRooms
    {
        get
        {
            if (!WallClimb) // City
                return 0;
            if (!HasRosary) // Towers
                return 1;
            if (!DoubleJump || !HasCenser) // Temples + Towers
                return 2;
            if (!AirDash) // Cathedral
                return 4;
            if (!RingClimb || !HasRapier) // Severed
                return 5;
            if (NumBossKeys < 5) // Crimson
                return 6;
            return 7;
        }
    }

    /// <summary>
    /// Creates a new empty inventory
    /// </summary>
    public static GameInventory CreateNewInventory(RandomizerSettings settings)
    {
        var inventory = new BlasphemousInventory(settings);

        return new GameInventory(
            new PostfixCalculator(),
            new ReflectionCollector(inventory),
            new ParenthesisPaddingFormatter(),
            new PostfixParser(),
            new ReflectionResolver(inventory));
    }

    private BlasphemousInventory(RandomizerSettings settings)
    {
        _settings = settings;
    }

    private readonly RandomizerSettings _settings;
}
