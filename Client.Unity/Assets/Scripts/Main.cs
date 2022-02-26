using UnityEngine;

/// <summary>
/// Main entry point for the game. This will initialize the backend during Awake.
/// </summary>
public class Main : MonoBehaviour
{
    private void Awake()
    {
        Avace.Backend.Main.Initialize();
    }
}