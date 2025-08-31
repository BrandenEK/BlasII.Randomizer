using BlasII.Randomizer.Models;
using System.Collections.Generic;
using UnityEngine;

namespace BlasII.Randomizer.ItemDisplay;

public class ItemDisplay
{
    private readonly Queue<DisplayInfo> _itemQueue = [];

    public void Show(string message, string item, Sprite image)
    {

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
