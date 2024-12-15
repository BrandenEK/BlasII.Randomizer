using BlasII.ModdingAPI;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace BlasII.Randomizer.Storages;

/// <summary>
/// Stores Sprite objects for the Randomizer
/// </summary>
public class IconStorage
{
    private readonly Dictionary<string, Sprite> _images = [];

    /// <summary>
    /// Gets an image by its type
    /// </summary>
    public Sprite GetImage(string type) => _images.TryGetValue(type, out var sprite) ? sprite : null;

    /// <summary>
    /// Initializes a new IconStorage
    /// </summary>
    public IconStorage()
    {
        foreach (var sprite in FindAllSprites())
        {
            if (!_spriteNames.TryGetValue(sprite.name, out string type))
                continue;

            _images.Add(type, sprite);
        }

        ModLog.Info($"Loaded {_images.Count} icons!");
    }

    /// <summary>
    /// Initializes the storage
    /// </summary>
    public void Initialize()
    {
        //Main.Randomizer.FileHandler.LoadDataAsFixedSpritesheet("rando-items.png", new Vector2(30, 30),
        //    out Sprite[] images, new SpriteImportOptions() { PixelsPerUnit = 32 });

        //for (int i = 0; i < images.Length; i++)
        //    _images.Add((ImageType)i, images[i]);
        //ModLog.Info($"Loaded {_images.Count} images!");

        foreach (var sprite in FindAllSprites())
        {
            if (!_spriteNames.TryGetValue(sprite.name, out string type))
                continue;

            _images.Add(type, sprite);
        }

        ModLog.Info($"Loaded {_images.Count} icons!");
    }

    private IEnumerable<Sprite> FindAllSprites()
    {
        return Resources.FindObjectsOfTypeAll<Sprite>()
            .Where(x => !string.IsNullOrEmpty(x.name))
            .OrderBy(x => x.name);
    }

    //public enum ImageType
    //{
    //    Cherub = 0,
    //    WallClimb = 1,
    //    AirJump = 2,
    //    AirDash = 3,
    //    MagicRingClimb = 4,
    //    Censer = 5,
    //    RosaryBlade = 6,
    //    Rapier = 7,
    //    Tears = 8,
    //    Invalid = 9,
    //    Chest = 10,
    //}

    private readonly Dictionary<string, string> _spriteNames = new()
    {
        // Weapons
        { "UI-general-save-slot-censer-icon", "Censer" },
        { "UI-general-save-slot-sword-icon", "RosaryBlade" },
        { "UI-general-save-slot-rapier-icon" , "Rapier" },
        { "UI-general-save-slot-meaculpa-icon" , "MeaCulpa" },
        // Abilities
        { "UI-stats-progression-skill-wallclimb-icon" , "WallClimb" },
        { "UI-stats-progression-skill-doublejump-icon" , "AirJump" },
        { "UI-stats-progression-skill-airdash-icon" , "AirDash" },
        { "UI-stats-progression-skill-rings-icon" , "MagicRingClimb" },
        { "UI-stats-progression-skill-crystalplats-icon" , "GlassWalk" },
        // Other
        { "UI-general-elements-tears-icon" , "Tears" },
        { "UI-general-elements-mark-icon-small" , "Marks" },
        { "UI-general-elements-MOP-icon-small" , "PreMarks" },
        { "cherub-counter-pop-up-icon" , "Cherub" },
    };
}
