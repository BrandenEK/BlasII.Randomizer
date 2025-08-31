using System.Collections.Generic;
using UnityEngine;

namespace BlasII.Randomizer.ItemDisplay;

public class ItemDisplayer
{
    private readonly DisplayUI _display = new();
    private readonly Queue<DisplayInfo> _itemQueue = [];

    public void Show(string message, string item, Sprite image)
    {
        // Queue this instead
        _display.UpdateDisplay(new DisplayInfo(message, item, image));
    }

    public void OnUpdate()
    {
        // Update popup
    }

    public void OnExitGame()
    {
        _itemQueue.Clear();
        // Hide popup
    }
}
