using BlasII.ModdingAPI.Helpers;
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

    private void ShowInternal(DisplayInfo info)
    {
        _currentCoroutine = MelonCoroutines.Start(FadeCoroutine(info));
    }

    public void OnExitGame()
    {
        _itemQueue.Clear();

        if (_currentCoroutine != null)
        {
            MelonCoroutines.Stop(_currentCoroutine);
            _currentCoroutine = null;
        }
    }

    private IEnumerator FadeCoroutine(DisplayInfo info)
    {
        // Hold min fade
        _display.UpdateAlpha(ALPHA_MIN);
        yield return GenericCoroutine(FADE_HIDE, _ => { });

        // Start showing
        _display.UpdateDisplay(info);
        CoreCache.AudioManager.InstantiateEvent("event:/SFX/UI/Get Item Popup");
        CoreCache.AudioManager.PlayOneShot("event:/SFX/UI/Get Item Popup");

        // Fade in
        yield return GenericCoroutine(FADE_DURATION, percent => _display.UpdateAlpha(Mathf.Lerp(ALPHA_MIN, ALPHA_MAX, percent)));
        _display.UpdateAlpha(ALPHA_MAX);

        // Hold max fade
        yield return GenericCoroutine(FADE_SHOW, _ => { });

        // Fade out
        yield return GenericCoroutine(FADE_DURATION, percent => _display.UpdateAlpha(Mathf.Lerp(ALPHA_MAX, ALPHA_MIN, percent)));
        _display.UpdateAlpha(ALPHA_MIN);

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

    private IEnumerator GenericCoroutine(float duration, System.Action<float> action)
    {
        float timer = 0f;

        while (timer < duration)
        {
            if (SceneHelper.GameSceneLoaded && !CoreCache.Time.IsRootPaused())
                timer += Time.deltaTime;

            action(timer / duration);
            yield return null;
        }
    }

    private const float FADE_DURATION = 0.75f;
    private const float FADE_SHOW = 2f;
    private const float FADE_HIDE = 0.5f;
    private const float ALPHA_MIN = 0f;
    private const float ALPHA_MAX = 0.9f;
}
