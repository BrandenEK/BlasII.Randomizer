using UnityEngine;

namespace BlasII.Randomizer.ItemDisplay;

public class DisplayInfo(string message, string itemName, Sprite image)
{
    public string Message { get; } = message;

    public string ItemName { get; } = itemName;

    public Sprite Image { get; } = image;
}
