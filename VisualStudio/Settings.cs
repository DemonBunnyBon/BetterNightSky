using System.Reflection;
using ModSettings;

namespace BetterNightSky;

internal class Settings : JsonModSettings
{
    internal static Settings instance = new Settings();

    [Section("Shooting Stars")]
    [Name("Enable Shooting Stars")]
    [Description("Changes whether or not shooting stars will appear in the sky.")]
    public bool ShootingStars = true;

    [Name("Colorize Shooting Stars")] 
    [Description("Enabling this option will make shooting stars have a ranging variety of colors. If disabled they will all be white.")]
    public bool Colorize = true;
    
    [Name("Constant Shooting Stars")] 
    [Description("Enabling this option will make shooting stars appear constantly.")]
    public bool ConstantShootingStars = false;
    
    [Name("Shooting Stars: Minimum Delay")]
    [Description("The minimum possible amount of time before shooting stars appear in the sky, in real world minutes.")]
    [Slider(1, 30, NumberFormat = "{0:0.##}m")]
    public int ShootingStarsTimeMin = 1;
    
    [Name("Shooting Stars: Maximum Delay")]
    [Description("The maximum possible amount of time before shooting stars appear in the sky, in real world minutes.")]
    [Slider(1, 30, NumberFormat = "{0:0.##}m")]
    public int ShootingStarsTimeMax = 15;
    
    [Name("Shooting Stars: Minimum Duration")]
    [Description("The minimum amount of time that shooting stars will stay in the sky, in real world seconds.")]
    [Slider(1, 60, NumberFormat = "{0:0.##}s")]
    public int ShootingStarsDurationMin = 20;
    
    [Name("Shooting Stars: Maximum Duration")]
    [Description("The maximum amount of time that shooting stars will stay in the sky, in real world seconds.")]
    [Slider(1, 60, NumberFormat = "{0:0.##}s")]
    public int ShootingStarsDurationMax = 45;
    
    

    
    [Section("Reset Settings")]
    [Name("Reset To Default")]
    [Description("Resets all settings to Default. (Confirm and scene reload/transition required.)")]
    public bool ResetSettings = false;
    
        protected override void OnChange(FieldInfo field, object? oldValue, object? newValue) => RefreshFields();
        protected override void OnConfirm()
        {
           ApplyReset();
           instance.ResetSettings = false;
           base.OnConfirm();
        }
        internal static void OnLoad()
        {
            instance.RefreshFields();
        }
        internal void RefreshFields()
        {
            if (instance.ShootingStars)
            {
                SetFieldVisible(nameof(ConstantShootingStars), true);
                if (ConstantShootingStars)
                {
                    SetFieldVisible(nameof(ShootingStarsTimeMin), false);
                    SetFieldVisible(nameof(ShootingStarsDurationMin), false);
                    SetFieldVisible(nameof(ShootingStarsTimeMax), false);
                    SetFieldVisible(nameof(ShootingStarsDurationMax), false);
                    return;
                }
                SetFieldVisible(nameof(ShootingStarsTimeMin), true);
                SetFieldVisible(nameof(ShootingStarsDurationMin), true);
                SetFieldVisible(nameof(ShootingStarsTimeMax), true);
                SetFieldVisible(nameof(ShootingStarsDurationMax), true);
                SetFieldVisible(nameof(Colorize), true);
            }
            else
            {
                SetFieldVisible(nameof(ConstantShootingStars), false);
                SetFieldVisible(nameof(ShootingStarsTimeMin), false);
                SetFieldVisible(nameof(ShootingStarsDurationMin), false);
                SetFieldVisible(nameof(ShootingStarsTimeMax), false);
                SetFieldVisible(nameof(ShootingStarsDurationMax), false);
                SetFieldVisible(nameof(Colorize), false);
            }


        }
        public static void ApplyReset()
        {
            if (instance.ResetSettings)
            {
                instance.ShootingStars = true;
                instance.ConstantShootingStars = false;
                instance.Colorize = true;
                instance.ShootingStarsTimeMin = 1;
                instance.ShootingStarsTimeMax = 15;
                instance.ShootingStarsDurationMin = 20;
                instance.ShootingStarsDurationMax = 45;
                instance.ResetSettings = false;
                instance.RefreshFields();
                instance.RefreshGUI();
            }
        }
}
