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
    private int CenserLevel { get; set; }
    [ResolvableAs("censer")]
    private bool HasCenser => CenserLevel > 0;

    [CollectableAs("RosaryBlade")]
    private int BladeLevel { get; set; }
    [ResolvableAs("blade")]
    private bool HasBlade => BladeLevel > 0;

    [CollectableAs("Rapier")]
    private int RapierLevel { get; set; }
    [ResolvableAs("rapier")]
    private bool HasRapier => RapierLevel > 0;

    [CollectableAs("MeaCulpa")]
    private int MeaCulpaLevel { get; set; }
    [ResolvableAs("meaculpa")]
    private bool HasMeaCulpa => MeaCulpaLevel > 0;

    // Abilities

    [CollectableAs("WallClimb")]
    [ResolvableAs("wallclimb")]
    private bool WallClimb { get; set; }

    [CollectableAs("AirJump")]
    [ResolvableAs("doublejump")]
    private bool DoubleJump { get; set; }

    [CollectableAs("AirDash")]
    [ResolvableAs("airdash")]
    private bool AirDash { get; set; }

    [CollectableAs("MagicRingClimb")]
    [ResolvableAs("ringclimb")]
    private bool RingClimb { get; set; }

    [CollectableAs("GlassWalk")]
    [ResolvableAs("glasswalk")]
    private bool GlassWalk { get; set; }

    // Bosses

    [CollectableAs("QI63", "QI64", "QI65", "QI66", "QI67")]
    [ResolvableAs("bossKeys")]
    private int NumBossKeys { get; set; }

    // Cherub quest

    [CollectableAs("Cherub")]
    [ResolvableAs("cherubs")]
    private int NumCherubs { get; set; }

    [CollectableAs("QI54")]
    [ResolvableAs("rattle")]
    private bool Rattle { get; set; }

    // Elder quest

    [CollectableAs("QI07")]
    [ResolvableAs("elderScroll")]
    private bool ElderScroll { get; set; }

    [CollectableAs("QI08")]
    [ResolvableAs("elderCloth")]
    private bool ElderCloth { get; set; }

    // Gold quest

    private int GoldLumps { get; set; } // ???

    // Hand quest

    [CollectableAs("QI37", "QI38", "QI39", "QI40", "QI41")]
    [ResolvableAs("kisses")]
    private int NumKisses { get; set; }

    [CollectableAs("QI28")]
    [ResolvableAs("brokenKey")]
    private bool BrokenKey { get; set; }

    // Letter quest

    [CollectableAs("QI14")]
    [ResolvableAs("letter1")]
    private bool LetterOne { get; set; }

    [CollectableAs("QI16")]
    [ResolvableAs("letter2")]
    private bool LetterTwo { get; set; }

    [CollectableAs("QI18")]
    [ResolvableAs("letter3")]
    private bool LetterThree { get; set; }

    [CollectableAs("QI20")]
    [ResolvableAs("letter4")]
    private bool LetterFour { get; set; }

    [CollectableAs("QI22")]
    [ResolvableAs("letter5")]
    private bool LetterFive { get; set; }

    // Lullaby quest

    private int NumLullabies { get; set; } // ???

    // Mud quest

    [CollectableAs("QI101")]
    [ResolvableAs("mudKey")]
    private bool MudKey { get; set; }

    [CollectableAs("QI103")]
    [ResolvableAs("ceramicKey")]
    private bool CeramicKey { get; set; }

    // Regula quest

    [CollectableAs("QI05")]
    [ResolvableAs("regulaCloth")]
    private bool RegulaCloth { get; set; }

    // Sculptor quest

    [CollectableAs("QI01", "QI02", "QI03", "QI11", "QI12")]
    [ResolvableAs("tools")]
    private int NumTools { get; set; }

    // Tribute quest

    [CollectableAs("QI29", "QI30", "QI31")]
    [ResolvableAs("tributes")]
    private int NumTributes { get; set; }

    // Wax quest

    [CollectableAs("QI56", "QI57", "QI58", "QI59", "QI60", "QI61")]
    [ResolvableAs("waxSeeds")]
    private int NumWaxSeeds { get; set; }

    // Yerma quest

    [CollectableAs("QI68")]
    [ResolvableAs("holyOil")]
    private bool HolyOil { get; set; }

    // Rooms

    [ResolvableAs("daughterRooms")]
    private int DaughterRooms
    {
        get
        {
            int rooms = 0;
            if (WallClimb && DoubleJump) rooms++;
            if (WallClimb && DoubleJump && CenserLevel > 0 && BladeLevel > 0 && RapierLevel > 0) rooms++;
            if (WallClimb && DoubleJump && AirDash && BladeLevel > 0) rooms++;
            if (DoubleJump && AirDash && RingClimb) rooms++;
            if (WallClimb && DoubleJump && AirDash && RingClimb && CenserLevel > 0 && BladeLevel > 0 && RapierLevel > 0) rooms++;
            return rooms;
        }
    }

    [ResolvableAs("shopRooms")]
    private int ShopRooms
    {
        get
        {
            if (!WallClimb) // City
                return 0;
            if (BladeLevel == 0) // Towers
                return 1;
            if (!DoubleJump || CenserLevel == 0) // Temples + Towers
                return 2;
            if (!AirDash) // Cathedral
                return 4;
            if (!RingClimb || RapierLevel == 0) // Severed
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
