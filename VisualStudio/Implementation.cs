using System.IO;
using System.Reflection;
using Il2Cpp;
using MelonLoader;
using UnityEngine;

namespace BetterNightSky;

internal sealed class BetterNightSkyMelon : MelonMod
{

    private static AssetBundle assetBundle = Utilities.LoadFromStream("Better-Night-Sky.res.better-night-sky");

	private static GameObject? moon;
    private static UpdateMoon? updateMoon;
    private static GameObject? starSphere;
    private static GameObject? shootingStar;
    private static UpdateShootingStar? updateShootingStar;

    public override void OnInitializeMelon()
    {
        Settings.instance.AddToModSettings("Better Night Sky");
        Settings.OnLoad();
        uConsole.RegisterCommand("shooting-star", new System.Action(ShootingStar));
        
    }

    internal static void Install()
    {
        if (starSphere == null)
        {
            starSphere = UnityEngine.Object.Instantiate(assetBundle.LoadAsset<GameObject>("assets/StarSphere.prefab"));
            if (starSphere == null)
            {
                return;
            }

            starSphere.transform.parent = GameManager.GetUniStorm()?.m_StarSphere?.transform?.parent;
            starSphere.transform.localEulerAngles = new Vector3(0, 90, 0);
            starSphere.layer = GameManager.GetUniStorm().m_StarSphere.layer;
            starSphere?.AddComponent<UpdateStars>();

            moon = UnityEngine.Object.Instantiate(assetBundle.LoadAsset<GameObject>("assets/Moon.prefab"));
            if (moon == null)
            {
                return;
            }

            moon.transform.parent = GameManager.GetUniStorm()?.m_StarSphere?.transform?.parent?.parent;
            moon.layer = GameManager.GetUniStorm().m_StarSphere.layer;
            updateMoon = moon?.AddComponent<UpdateMoon>();

            GameManager.GetUniStorm()?.m_StarSphere?.SetActive(false);
        }

        if (starSphere != null)
        {
            UnityEngine.Object.Destroy(starSphere);
            UnityEngine.Object.Destroy(moon);
            GameManager.GetUniStorm().m_StarSphere.SetActive(true);
        }

        if (Settings.instance.ShootingStars && shootingStar == null)
        {
            shootingStar = UnityEngine.Object.Instantiate(assetBundle.LoadAsset<GameObject>("assets/ShootingStar.prefab"));
            shootingStar.transform.parent = GameManager.GetUniStorm().m_StarSphere.transform.parent.parent;
            updateShootingStar = shootingStar.AddComponent<UpdateShootingStar>();
        }

        if (!Settings.instance.ShootingStars && shootingStar != null)
        {
            UnityEngine.Object.Destroy(shootingStar);
        }
    }

    internal static void RescheduleShootingStars()
    {
        if (updateShootingStar != null)
        {
            updateShootingStar.Reschedule();
        }
    }

    internal static void Log(string message) => MelonLoader.MelonLogger.Msg(message);

    internal static void UpdateMoonPhase()
    {
        if (updateMoon != null)
        {
            updateMoon.UpdatePhase();
        }
    }

    internal static Texture2D GetMoonPhaseTexture(int i)
    {
        return assetBundle.LoadAsset<Texture2D>("assets/MoonPhase/Moon_" + i + ".png");
    }


    private static void ShootingStar()
    {
        if (updateShootingStar == null)
        {
            uConsole.Log("Shooting Stars are disabled");
            return;
        }

        int duration = 5;
        if (uConsole.GetNumParameters() == 1)
        {
            duration = uConsole.GetInt();
        }

        updateShootingStar.Trigger(duration);
    }
    
}