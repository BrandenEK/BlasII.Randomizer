using BlasII.ModdingAPI;
using HarmonyLib;
using Il2CppSystem;
using Il2CppTGK.Audio;
using Il2CppTGK.Game;
using Il2CppTGK.Game.Components.UI;
using Il2CppTGK.Game.Managers;
using System.Collections.Generic;
using UnityEngine;

namespace BlasII.Randomizer.Patches;

[HarmonyPatch(typeof(AudioManager), nameof(AudioManager.InstantiateEvent), typeof(string))]
class t
{
    public static void Prefix(ref string id)
    {
        ModLog.Warn($"Audio1 {id}");

        //if (!_sfx.Contains(id))
        //    _sfx.Add(id);

        //int idx = UnityEngine.Random.RandomRangeInt(0, _sfx.Count);
        //id = _sfx[idx];
    }

    public static readonly List<string> _sfx = [];
}

[HarmonyPatch(typeof(AudioManager), nameof(AudioManager.InstantiateEvent), typeof(string), typeof(Transform))]
class t2
{
    public static void Prefix(ref string id)
    {
        ModLog.Warn($"Audio2 {id}");

        //if (!t._sfx.Contains(id))
        //    t._sfx.Add(id);

        //int idx = UnityEngine.Random.RandomRangeInt(0, t._sfx.Count);
        //id = t._sfx[idx];
    }
}

[HarmonyPatch(typeof(AudioManagerRef), nameof(AudioManagerRef.PlayOneShotFX), typeof(ScriptableAudioEvent))]
class t3
{
    public static void Postfix(ScriptableAudioEvent audioEvent)
    {
        ModLog.Warn($"Audio3 {audioEvent.GetEventID()}");
    }
}

[HarmonyPatch(typeof(AudioManagerRef), nameof(AudioManagerRef.PlayOneShotFX), typeof(AudioManagerRequest))]
class t4
{
    public static void Postfix(AudioManagerRequest request)
    {
        ModLog.Warn($"Audio4 {request.audioEvent.GetEventID()}");
    }
}
