using Il2Cpp;
using Il2CppInterop.Runtime.Attributes;
using UnityEngine;

namespace BetterNightSky;

[MelonLoader.RegisterTypeInIl2Cpp]
internal class UpdateShootingStar : MonoBehaviour
{
    private const float ALPHA_MAX = 0.85f;

    private int delay_max = Settings.instance.ShootingStarsTimeMax*60;
    private int delay_min = Settings.instance.ShootingStarsTimeMin*60;
    private int duration_max = Settings.instance.ShootingStarsDurationMin;
    private int duration_min = Settings.instance.ShootingStarsDurationMax;

    private const float HEIGHT = 2000;
    private const float POSITION_MAX = 2000;
    private const float POSITION_MIN = -2000;

    private ParticleSystem? _particleSystem;

    private ParticleSystem ParticleSystem 
    { 
        get
        {
			if (_particleSystem == null)
            {
				_particleSystem = GetComponentInChildren<ParticleSystem>();
				if (_particleSystem == null)
				{
					gameObject.SetActive(false);
					throw new System.NullReferenceException("Particle system not found!");
				}
			}
            return _particleSystem;
		}
        set => _particleSystem = value; 
    }

    public UpdateShootingStar(System.IntPtr intPtr) : base(intPtr) { }

    [HideFromIl2Cpp]
    internal void Trigger(int duration = 0)
    {
        CancelInvoke();

        int actualDuration = duration < 1 ? UnityEngine.Random.Range(duration_min, duration_max) : duration;
        Invoke("StopEmitting", actualDuration);

        StartEmitting();
    }

    private int GetNextDelay()
    {
        if (Settings.instance.ConstantShootingStars)
        {
            return 3;
        }
        
        int result = UnityEngine.Random.Range(delay_min, delay_max);

        if (Utilities.IsMenu())
        {
            result /= 10;
        }

        return result;
    }

    private int GetNextDuration()
    {
        if (Settings.instance.ConstantShootingStars)
        {
            return 3;
        }
        return UnityEngine.Random.Range(duration_min, duration_max);
    }

    [HideFromIl2Cpp]
    private bool CanEmit()
    {
        if (Utilities.IsMenu() && Settings.instance.ShootingStars)
        {
            return true;
        }
        return !GameManager.GetWeatherComponent().IsIndoorScene() 
            && GameManager.GetUniStorm().GetActiveTODState().m_MoonAlpha >= 0.05 && Settings.instance.ShootingStars;
    }

    private void Start()
    {
        StopEmitting();
    }

    private void StartEmitting()
    {
        if (!CanEmit())
        {
            return;
        }

        UpdatePosition();
        ParticleSystem.MainModule mainModule = ParticleSystem.main;
        mainModule.startColor = GetCometColor();
        //mainModule.maxParticles = 60;

        ParticleSystem.EmissionModule emissionModule = ParticleSystem.emission;
        emissionModule.enabled = true;
    }

    private void StopEmitting()
    {
        ParticleSystem.EmissionModule emissionModule = ParticleSystem.emission;
        emissionModule.enabled = false;
    }

    internal void Reschedule()
    {
        CancelInvoke();
        StopEmitting();

        int delay = GetNextDelay();
        Invoke("StartEmitting", delay);

        int duration = GetNextDuration();
        Invoke("Reschedule", delay + duration);
        
    }
    
    private Color GetCometColor()
    {
        if (!Settings.instance.Colorize)
        {
            return Color.white;
        }
        var color = Utilities.ChooseRandomCometColor();    
        color.a = Mathf.Clamp(GameManager.GetUniStorm().GetActiveTODState().m_MoonAlpha, 0, ALPHA_MAX);
        return color;
    }

    private void UpdatePosition()
    {
        if (Utilities.IsMenu())
        {
            ParticleSystem.transform.position = new Vector3(-2500, 2000, -2000);
            ParticleSystem.transform.rotation = Quaternion.identity;
            return;
        }

        ParticleSystem.transform.position = new Vector3(UnityEngine.Random.Range(POSITION_MIN, POSITION_MAX), HEIGHT, UnityEngine.Random.Range(POSITION_MIN, POSITION_MAX));
        ParticleSystem.transform.rotation = Quaternion.Euler(0, UnityEngine.Random.value * 360, 0);
    }
}