using UnityEngine;

namespace BlasII.Randomizer.ItemDisplay;

public class DisplayInfo(string message, string item, Sprite image)
{
    public string Message { get; } = message;

    public string Item { get; } = item;

    public Sprite Image { get; } = image;
}
