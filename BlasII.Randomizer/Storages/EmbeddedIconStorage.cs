using BlasII.ModdingAPI;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace BlasII.Randomizer.Storages;

/// <inheritdoc/>
public class EmbeddedIconStorage : BaseSpriteStorage<string>
{
    /// <summary>
    /// Initializes the storage by searching in Resources
    /// </summary>
    public EmbeddedIconStorage()
    {
        foreach (var sprite in FindAllSprites())
        {
            if (!_spriteNames.TryGetValue(sprite.name, out string type))
                continue;

            _images.Add(type, sprite);
        }

        ModLog.Info($"Loaded {_images.Count} embedded images!");
    }

    private IEnumerable<Sprite> FindAllSprites()
    {
        return Resources.FindObjectsOfTypeAll<Sprite>()
            .Where(x => !string.IsNullOrEmpty(x.name))
            .OrderBy(x => x.name);
    }

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
