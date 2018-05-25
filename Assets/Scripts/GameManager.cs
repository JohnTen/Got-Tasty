using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityUtility;

public class GameManager : MonoSingleton<GameManager>
{
    MelonManager melonManager;

    public static Watermelon CurrentManipulatingWatermelon
    {
        get
        {
            return Instance.melonManager.CurrentManipulatingWatermelon;
        }
    }

    public static bool isManipulatingWatermelon
    {
        get
        {
            return Instance.melonManager.isManipulatingWatermelon;
        }
    }

    public static void StartNewGame()
    {
        if (Instance.melonManager == null)
            Instance.melonManager = FindObjectOfType<MelonManager>();

        Instance.melonManager.StartNewGame();
    }

    public static void RateMelon()
    {

    }

    public static void PickUpWaterMelon(Watermelon melon)
    {
        Instance.melonManager.PickUpWaterMelon(melon);
    }

    public static void QuitManipulatingWatermelon()
    {
        Instance.melonManager.QuitManipulatingWatermelon();
    }

    public void Awake()
    {
        melonManager = FindObjectOfType<MelonManager>();
    }
}
