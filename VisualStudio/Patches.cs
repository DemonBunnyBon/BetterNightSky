using Il2Cpp;
using UnityEngine;
using HarmonyLib;

namespace BetterNightSky;

[HarmonyPatch(typeof(TODStateData), nameof(TODStateData.SetBlended))]
internal static class BloomPatch
{
    public static void Postfix(TODStateData __instance, int nightStates)
    {
        if (nightStates > 0)
        {
            __instance.m_BloomIntensity *= 0.3f;
        }
    }
}

[HarmonyPatch(typeof(UniStormWeatherSystem), nameof(UniStormWeatherSystem.Init))]
internal static class WeatherInitPatch
{
    public static void Prefix()
    {
        BetterNightSkyMelon.Install();
    }
}

[HarmonyPatch(typeof(UniStormWeatherSystem), nameof(UniStormWeatherSystem.SetMoonPhaseIndex))]
internal static class MoonPhaseIndexPatch
{
    public static void Postfix()
    {
        BetterNightSkyMelon.UpdateMoonPhase();
    }
}

[HarmonyPatch(typeof(UniStormWeatherSystem), nameof(UniStormWeatherSystem.SetMoonPhase))]
internal static class MoonPhasePatch
{
    public static void Postfix()
    {
        BetterNightSkyMelon.UpdateMoonPhase();
    }
}

[HarmonyPatch(typeof(GameManager), nameof(GameManager.Awake))]
internal static class ShootingStarsSchedulerPatch
{
    public static void Postfix()
    {
        if(!Utilities.IsMenu())
        {
            BetterNightSkyMelon.RescheduleShootingStars();
        }
    }
}