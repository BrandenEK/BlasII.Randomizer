using BlasII.Framework.Menus;
using BlasII.Framework.Menus.Options;
using BlasII.Framework.UI;
using BlasII.ModdingAPI;
using BlasII.Randomizer.Settings;
using Il2CppTGK.Game.Components.UI;
using Il2CppTMPro;
using System.Diagnostics;
using System.Text;
using UnityEngine;
using UnityEngine.TextCore;

namespace BlasII.Randomizer.Services;

/// <summary>
/// Displays generation settings for the Randomizer
/// </summary>
public class RandomizerMenu : ModMenu
{
    private int _generatedSeed = 0;

    /// <inheritdoc/>
    protected override int Priority { get; } = 100;

    /// <summary>
    /// Stores or loads the entire settings menu into or from a settings object
    /// </summary>
    private RandomizerSettings MenuSettings
    {
        get
        {
            return new RandomizerSettings()
            {
                Seed = _setSeed.CurrentNumericValue == 0 ? _generatedSeed : _setSeed.CurrentNumericValue,
                LogicType = 1,
                RequiredKeys = _setRequiredKeys.CurrentOption - 1,
                StartingWeapon = _setStartingWeapon.CurrentOption - 1,
                ShopMultiplier = _setShopCosts.CurrentOption,
                MartyrdomExperience = _setMarksExperience.CurrentOption,
                AddPenitenceRewards = _setAddPenitence.Toggled,
                ShuffleCherubs = _setShuffleCherubs.Toggled,
                ShuffleLongQuests = true,
            };
        }
        set
        {
            _setLogicDifficulty.CurrentOption = 0;
            _setRequiredKeys.CurrentOption = value.RequiredKeys + 1;
            _setStartingWeapon.CurrentOption = value.StartingWeapon + 1;
            _setShopCosts.CurrentOption = value.ShopMultiplier;
            _setMarksExperience.CurrentOption = value.MartyrdomExperience;
            _setAddPenitence.Toggled = value.AddPenitenceRewards;
            _setShuffleCherubs.Toggled = value.ShuffleCherubs;
            //_setShuffleLongQuests.Toggled = true;
        }
    }

    /// <summary>
    /// Set the menu options to default when opening it
    /// </summary>
    public override void OnStart()
    {
        RandomizerSettings settings = SettingsGenerator.CreateFromPreset(Preset.Standard);

        _generatedSeed = SettingsGenerator.GetRandomSeed();
        ModLog.Info($"Generating default seed: {_generatedSeed}");

        MenuSettings = settings;
        _setSeed.CurrentValue = string.Empty;
        _setPreset.CurrentOption = 1;

        UpdateUniqueIdText(settings.CalculateUID());
    }

    /// <summary>
    /// Update the randomizer settings when closing it
    /// </summary>
    public override void OnFinish()
    {
        Main.Randomizer.CurrentSettings = MenuSettings;
    }

    /// <summary>
    /// Handle changing an option (Apply presets, update UID)
    /// </summary>
    public override void OnOptionsChanged(string option)
    {
        base.OnOptionsChanged(option);

        if (option == "Preset")
            UpdateSettingsFromPresetChange();
        else if (option != "Seed")
            _setPreset.CurrentOption = 0;

        UpdateUniqueIdText(MenuSettings.CalculateUID());
    }

    private void UpdateSettingsFromPresetChange()
    {
        if (_setPreset.CurrentOption == 0)
            return;

        Preset preset = (Preset)(_setPreset.CurrentOption - 1);

        ModLog.Info($"Changed preset to {preset}");
        MenuSettings = SettingsGenerator.CreateFromPreset(preset);
    }

    private void UpdateUniqueIdText(ulong id)
    {
        var sb = new StringBuilder();
        ulong targetBase = (ulong)ID_CHARS.Length;

        do
        {
            sb.Append($" {ID_CHARS[(int)(id % targetBase)]}");
            id /= targetBase;
        }
        while (id > 0);

        while (sb.Length < ID_DIGITS * 2)
            sb.Append(" 0");

        _idText.SetText($"Unique ID:<color=#B3E5B3>{sb}");
    }

    /// <inheritdoc/>
    protected override void CreateUI(Transform ui)
    {
        FixFontUnderline();

        var toggle = new ToggleCreator(this)
        {
            BoxSize = 55,
            TextColor = SILVER,
            TextSize = TEXT_SIZE,
        };

        var arrow = new ArrowCreator(this)
        {
            ArrowSize = 55,
            TextColor = SILVER,
            TextColorAlt = YELLOW,
            TextSize = TEXT_SIZE,
            ElementSpacing = 140,
        };

        var text = new TextCreator(this)
        {
            LineSize = 200,
            TextColor = SILVER,
            TextColorAlt = YELLOW,
            TextSize = TEXT_SIZE,
        };

        _setSeed = text.CreateOption("Seed", ui, new Vector2(0, 300), "option/seed", true, false, SettingsGenerator.MAX_SEED.ToString().Length);

        _setLogicDifficulty = arrow.CreateOption("LD", ui, new Vector2(-300, 80), "option/logic",
        [
            "option/logic/normal",
        ]);

        _setRequiredKeys = arrow.CreateOption("RQ", ui, new Vector2(-300, -80), "option/keys",
        [
            "option/random",
            "option/keys/zero",
            "option/keys/one",
            "option/keys/two",
            "option/keys/three",
            "option/keys/four",
            "option/keys/five",
        ]);

        _setStartingWeapon = arrow.CreateOption("SW", ui, new Vector2(-300, -240), "option/weapon",
        [
            "option/random",
            "option/weapon/censer",
            "option/weapon/rosary",
            "option/weapon/rapier",
            "option/weapon/meaculpa",
        ]);

        _setShopCosts = arrow.CreateOption("SH", ui, new Vector2(300, 80), "option/cost",
        [
            "option/cost/none",
            "option/cost/less",
            "option/cost/normal",
            "option/cost/more",
            "option/vanilla",
        ]);

        _setMarksExperience = arrow.CreateOption("ME", ui, new Vector2(300, -80), "option/marks",
        [
            "option/vanilla",
            "option/marks/double",
            "option/marks/items",
            "option/marks/bosses",
        ]);

        _setAddPenitence = toggle.CreateOption("AP", ui, new Vector2(150, -200), "option/penitence");

        _setShuffleCherubs = toggle.CreateOption("SC", ui, new Vector2(150, -280), "option/cherubs");

        //_setShuffleLongQuests = toggle.CreateOption("SL", ui, new Vector2(150, 50), "option/long");
        //_setShuffleLongQuests.Enabled = false;

        arrow.ArrowSize = 40;
        arrow.TextSize = 40;

        _setPreset = arrow.CreateOption("Preset", ui, new Vector2(600, 400), "option/preset",
        [
            "option/preset/custom",
            "option/preset/standard",
            "option/preset/quick",
        ]);

        //UIModder.Create(new RectCreationOptions()
        //{
        //    Name = "Temp text",
        //    Parent = ui,
        //    Position = new Vector2(0, 200),
        //}).AddText(new TextCreationOptions()
        //{
        //    Contents = "More options coming in the next update!",
        //    Color = Color.cyan,
        //    Alignment = TextAlignmentOptions.Center,
        //    FontSize = 40,
        //});

        _idText = UIModder.Create(new RectCreationOptions()
        {
            Name = "UniqueID",
            Parent = ui,
            Position = new Vector2(0, -130),
            Pivot = Vector2.zero,
            XRange = Vector2.zero,
            YRange = Vector2.zero,
        }).AddText(new TextCreationOptions()
        {
            Contents = "Unique ID: ---",
            Color = SILVER,
            Alignment = TextAlignmentOptions.Left,
            FontSize = 42,
            UseRichText = true,
        }).AddShadow();

        _helpText = UIModder.Create(new RectCreationOptions()
        {
            Name = "HelpText",
            Parent = ui,
            Position = new Vector2(0, -130),
            Size = new Vector2(400, 100),
            Pivot = new Vector2(1, 0),
            XRange = Vector2.one,
            YRange = Vector2.zero,
        }).AddText(new TextCreationOptions()
        {
            Contents = "<u>Click here for more info</u>",
            Color = SILVER,
            Alignment = TextAlignmentOptions.Right,
            FontSize = 42,
            UseRichText = true,
        }).AddShadow();

        AddClickable(_helpText.normalText.rectTransform, false, OpenGithubLink);
    }

    private void FixFontUnderline()
    {
        ModLog.Warn("Fixing blasphemous font underline offset");

        FaceInfo info = UIModder.Fonts.Blasphemous.faceInfo.MemberwiseClone().Cast<FaceInfo>();
        info.underlineOffset = -1.8f;
        UIModder.Fonts.Blasphemous.faceInfo = info;
    }

    private void OpenGithubLink()
    {
        ModLog.Info("Opening github link to settings info");

        Process.Start(new ProcessStartInfo()
        {
            FileName = "https://github.com/BrandenEK/BlasII.Randomizer/blob/main/docs/SETTINGS.md",
            UseShellExecute = true,
        });
    }

    private TextOption _setSeed;
    private ArrowOption _setPreset;

    private ArrowOption _setLogicDifficulty;
    private ArrowOption _setRequiredKeys;
    private ArrowOption _setStartingWeapon;
    private ArrowOption _setShopCosts;
    private ArrowOption _setMarksExperience;
    private ToggleOption _setAddPenitence;
    private ToggleOption _setShuffleCherubs;
    //private ToggleOption _setShuffleLongQuests;

    private UIPixelTextWithShadow _idText;
    private UIPixelTextWithShadow _helpText;

    private const int TEXT_SIZE = 55;
    private const int ID_DIGITS = 12;
    private const string ID_CHARS = "0123456789ABCDEFGHJKLMNPQRSTUVWXYZ";
    private readonly Color SILVER = new Color32(192, 192, 192, 255);
    private readonly Color YELLOW = new Color32(255, 231, 65, 255);
}
