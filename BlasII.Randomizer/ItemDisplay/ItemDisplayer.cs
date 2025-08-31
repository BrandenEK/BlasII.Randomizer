using System.Collections;
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

    private IEnumerator FadeCoroutine()
    {
        float timer;

        timer = 0f;
        while (timer < FADE_DURATION)
        {
            timer += Time.deltaTime;
            _display.UpdateAlpha(Mathf.Lerp(ALPHA_MIN, ALPHA_MAX, timer / FADE_DURATION));
            yield return null;
        }
        _display.UpdateAlpha(ALPHA_MAX);

        yield return new WaitForSecondsRealtime(FADE_HOLD);

        timer = 0f;
        while (timer < FADE_DURATION)
        {
            timer += Time.deltaTime;
            _display.UpdateAlpha(Mathf.Lerp(ALPHA_MAX, ALPHA_MIN, timer / FADE_DURATION));
            yield return null;
        }
        _display.UpdateAlpha(ALPHA_MIN);

        yield return new WaitForSecondsRealtime(FADE_HOLD);
    }

    private float FADE_DURATION = 3f;
    private float FADE_HOLD = 1f;
    private float ALPHA_MIN = 0f;
    private float ALPHA_MAX = 0.9f;
}
