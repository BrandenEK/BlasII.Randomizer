using BlasII.Randomizer.Models;
using System.Collections.Generic;

namespace BlasII.Randomizer.ItemDisplay;

public class ItemDisplay
{
    private readonly Queue<int> _itemQueue = [];

    public void Show(Item item)
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
