using Input;
using UnityEngine;

/// <summary>
/// Main entry point for the game. This will initialize the backend during Awake.
/// </summary>
public class Main : MonoBehaviour
{
    static Main()
    {
        Avace.Backend.Main.Initialize();   
    }
    
    private void Awake()
    {
        InputManager.LoadCallbacks();
    }
}