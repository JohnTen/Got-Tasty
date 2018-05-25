using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonAgent : MonoBehaviour
{
    public void StartNewGame()
    {
        GameManager.StartNewGame();
    }

    public void RateMelon()
    {
        GameManager.RateMelon();
    }

    public void QuitManipulatingWatermelon()
    {
        GameManager.QuitManipulatingWatermelon();
    }
}
