using Misc;
using UnityEngine;

/// <summary>
/// Main entry point for the game. This will initialize the backend during Awake.
/// </summary>
public class Main : CustomMonoBehaviour
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void Initialize()
    {
        Avace.Backend.Main.Initialize();
    }
}