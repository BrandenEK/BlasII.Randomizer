﻿using BlasII.Framework.Menus;
using BlasII.Framework.Menus.Options;
using UnityEngine;

namespace BlasII.Randomizer.Services;

public class RandomizerMenu : ModMenu
{
    /// <inheritdoc/>
    protected override int Priority { get; } = 100;

    /// <summary>
    /// Stores or loads the entire settings menu into or from a settings object
    /// </summary>
    private RandomizerSettings MenuSettings
    {
        get
        {
            int logicDifficulty = 1;
            int requiredKeys = _setRequiredKeys.CurrentOption;
            int startingWeapon = _setStartingWeapon.CurrentOption;
            bool shuffleLongQuests = _setShuffleLongQuests.Toggled;
            bool shuffleShops = _setShuffleShops.Toggled;

            int seed = _setSeed.CurrentNumericValue == 0 ? RandomizerSettings.RandomSeed : _setSeed.CurrentNumericValue;
            return new RandomizerSettings(seed, logicDifficulty, requiredKeys, 0, startingWeapon, 0, shuffleLongQuests, shuffleShops, true, 0, 0);
        }
        set
        {
            _setLogicDifficulty.CurrentOption = 0;
            _setRequiredKeys.CurrentOption = value.requiredKeys;
            _setStartingWeapon.CurrentOption = value.startingWeapon;
            _setShuffleLongQuests.Toggled = value.shuffleLongQuests;
            _setShuffleShops.Toggled = value.shuffleShops;

            _setSeed.CurrentValue = string.Empty;
        }
    }

    /// <summary>
    /// Set the menu options to default when opening it
    /// </summary>
    public override void OnStart()
    {
        MenuSettings = RandomizerSettings.DefaultSettings;
    }

    /// <summary>
    /// Update the randomizer settings when closing it
    /// </summary>
    public override void OnFinish()
    {
        Main.Randomizer.CurrentSettings = MenuSettings;
    }

    protected override void CreateUI(Transform ui)
    {
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
        };

        var text = new TextCreator(this)
        {
            LineSize = 150,
            TextColor = SILVER,
            TextColorAlt = YELLOW,
            TextSize = TEXT_SIZE,
        };

        _setSeed = text.CreateOption("Seed", ui, new Vector2(0, 300),
            "seed", true, false, 6);

        _setLogicDifficulty = arrow.CreateOption("LD", ui, new Vector2(-300, 80),
            "opld", _opLogic);

        _setRequiredKeys = arrow.CreateOption("RQ", ui, new Vector2(-300, -80),
            "oprq", _opKeys);

        _setStartingWeapon = arrow.CreateOption("SW", ui, new Vector2(-300, -240),
            "opsw", _opWeapon);

        _setShuffleLongQuests = toggle.CreateOption("SL", ui, new Vector2(150, 70),
            "opsl");

        _setShuffleShops = toggle.CreateOption("SS", ui, new Vector2(150, -10),
            "opss");
    }

    private const int TEXT_SIZE = 55;
    private readonly Color SILVER = new Color32(192, 192, 192, 255);
    private readonly Color YELLOW = new Color32(255, 231, 65, 255);

    private readonly string[] _opLogic = new string[] { "o2ld" }; // "Easy", "Normal", "Hard"
    private readonly string[] _opKeys = new string[] { "rand", "o1rq", "o2rq", "o3rq", "o4rq", "o5rq", "o6rq" };
    private readonly string[] _opWeapon = new string[] { "rand", "o1sw", "o2sw", "o3sw" };

    private TextOption _setSeed;

    private ArrowOption _setLogicDifficulty;
    private ArrowOption _setRequiredKeys;
    private ArrowOption _setStartingWeapon;

    private ToggleOption _setShuffleLongQuests;
    private ToggleOption _setShuffleShops;
}