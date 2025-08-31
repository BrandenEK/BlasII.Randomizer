using Il2CppTGK.Game;
using MelonLoader;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlasII.Randomizer.ItemDisplay;

public class ItemDisplayer
{
    private readonly DisplayUI _display = new();
    private readonly Queue<DisplayInfo> _itemQueue = [];

    private object _currentCoroutine;

    public void Show(string message, string item, Sprite image)
    {
        var info = new DisplayInfo(message, item, image);

        if (_currentCoroutine != null)
        {
            _itemQueue.Enqueue(info);
        }
        else
        {
            ShowInternal(info);
        }
    }

    //public void OnUpdate()
    //{
    //    // Update popup
    //}

    public void OnExitGame()
    {
        _itemQueue.Clear();
    }

    private void ShowInternal(DisplayInfo info)
    {
        _display.UpdateDisplay(info);
        CoreCache.AudioManager.InstantiateEvent("event:/SFX/UI/Get Item Popup");
        CoreCache.AudioManager.PlayOneShot("event:/SFX/UI/Get Item Popup");

        _currentCoroutine = MelonCoroutines.Start(FadeCoroutine());
    }

    private IEnumerator FadeCoroutine()
    {
        float timer;

        // Fade in
        timer = 0f;
        while (timer < FADE_DURATION)
        {
            if (!CoreCache.Time.IsRootPaused())
                timer += Time.deltaTime;

            _display.UpdateAlpha(Mathf.Lerp(ALPHA_MIN, ALPHA_MAX, timer / FADE_DURATION));
            yield return null;
        }
        _display.UpdateAlpha(ALPHA_MAX);

        // Hold max fade
        yield return new WaitForSeconds(FADE_SHOW);

        // Fade out
        timer = 0f;
        while (timer < FADE_DURATION)
        {
            if (!CoreCache.Time.IsRootPaused())
                timer += Time.deltaTime;

            _display.UpdateAlpha(Mathf.Lerp(ALPHA_MAX, ALPHA_MIN, timer / FADE_DURATION));
            yield return null;
        }
        _display.UpdateAlpha(ALPHA_MIN);

        // Hold min fade
        yield return new WaitForSeconds(FADE_HIDE);

        // Check for next in queue
        if (_itemQueue.Count > 0)
        {
            ShowInternal(_itemQueue.Dequeue());
        }
        else
        {
            _currentCoroutine = null;
        }
    }

    private const float FADE_DURATION = 0.75f;
    private const float FADE_SHOW = 10f;
    private const float FADE_HIDE = 0.5f;
    //private const float FADE_DURATION = 1f;
    //private const float FADE_SHOW = 1.5f;
    //private const float FADE_HIDE = 0.5f;
    private const float ALPHA_MIN = 0f;
    private const float ALPHA_MAX = 0.9f;
}
