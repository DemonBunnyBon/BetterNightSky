
using System.Reflection;
using Il2Cpp;
using UnityEngine;

namespace BetterNightSky;

public class Utilities
{
    public static AssetBundle LoadFromStream(string name)
    {
        using (Stream? stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(name))
        {
            MemoryStream? memory = new((int)stream.Length);
            stream!.CopyTo(memory);

            Il2CppSystem.IO.MemoryStream memoryStream = new Il2CppSystem.IO.MemoryStream(memory.ToArray());
                
            AssetBundle loadFromMemoryInternal = AssetBundle.LoadFromStream(memoryStream);
            return loadFromMemoryInternal;
        };
    }
    internal static bool IsMenu()
    {
        if (GameManager.m_ActiveScene.Contains("MainMenu") || InterfaceManager.IsMainMenuEnabled())
        {
            return true;
        }
        return false;

    }

    internal static Color ChooseRandomCometColor()
    {
        Color c = Color.white;
        switch (UnityEngine.Random.Range(0, 7))
        {
            case 0:
                c = new Color32(139, 227, 188, 255);
                break;
            case 1:
                c = new Color32(188, 166, 255, 255);
                break;
            case 2:
                c = new Color32(199, 99, 180, 255);
                break;
            case 3:
                c = new Color32(217, 211, 111, 255);
                break;
            case 4:
                c = new Color32(163, 227, 120, 255);
                break;
            case 5:
                c = new Color32(204, 141, 135, 255);
                break;
            case 6:
                c = new Color32(232, 233, 224, 255);
                break;
            case 7:
                c = new Color32(135, 179, 228, 255);
                break;
        }
        return c;
    }
}