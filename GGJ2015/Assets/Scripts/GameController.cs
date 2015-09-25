using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour
{
    public enum Scenes
    {
        Menu = 0, Game = 1, End = 2
    }

    public static string winner = string.Empty;    
}