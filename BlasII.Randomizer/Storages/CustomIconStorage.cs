using BlasII.ModdingAPI;
using BlasII.ModdingAPI.Files;
using UnityEngine;

namespace BlasII.Randomizer.Storages;

/// <inheritdoc/>
public class CustomIconStorage : BaseSpriteStorage<CustomIconStorage.IconType>
{
    /// <summary>
    /// Initializes the storage by loading a texture from the data folder
    /// </summary>
    public CustomIconStorage()
    {
        Main.Randomizer.FileHandler.LoadDataAsFixedSpritesheet("rando-items.png", new Vector2(30, 30),
            out Sprite[] images, new SpriteImportOptions() { PixelsPerUnit = 32 });

        for (int i = 0; i < images.Length; i++)
            _images.Add((IconType)i, images[i]);
        ModLog.Info($"Loaded {_images.Count} custom images!");
    }

    public enum IconType
    {
        Invalid,
        Chest,
    }
}
