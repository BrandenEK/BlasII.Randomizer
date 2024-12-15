using System.Collections.Generic;
using UnityEngine;

namespace BlasII.Randomizer.Storages;

/// <summary>
/// Stores Sprite objects for the Randomizer
/// </summary>
public abstract class BaseSpriteStorage<TKey>
{
    /// <summary>
    /// The internal image storage
    /// </summary>
    protected readonly Dictionary<TKey, Sprite> _images = [];

    /// <summary>
    /// Gets an image by its type
    /// </summary>
    public Sprite GetImage(TKey type) => _images.TryGetValue(type, out var sprite) ? sprite : null;
}
