using System.Collections.Generic;
using UnityEngine;
using UnityUtility;


public class SoundManager : MonoBehaviour
{
    [SerializeField]
    AudioSource patWatermelon;

    [SerializeField]
    AudioSource smashWatermelon;

    [SerializeField]
    AudioSource angrySeller;

    [SerializeField]
    AnimationCurve patPitch;

    static SoundManager _instance;
    static SoundManager Instance
    {
        get
        {
            if (_instance != null)
                return _instance;

            _instance = FindObjectOfType<SoundManager>();
            if (_instance != null)
                return _instance;

            _instance = GlobalObject.GetOrAddComponent<SoundManager>();
            return _instance;
        }
    }

    /// <summary>
    /// Play sound of patting watermelon by maturity
    /// </summary>
    /// <remarks>
    /// The range of maturity should be 0~2
    /// </remarks>
    /// <param name="maturity"></param>
    public static void PatWatermelon(float maturity)
    {
        Instance.patWatermelon.pitch = Instance.patPitch.Evaluate(maturity);
        Instance.patWatermelon.Play();  
    }

    /// <summary>
    /// Play sound of patting watermelon by maturity
    /// </summary>
    /// <remarks>
    /// The range of maturity should be 0~2
    /// </remarks>
    /// <param name="maturity"></param>
    public static void SmashWatermelon()
    {
        Instance.smashWatermelon.Play();
    }

    public static void AngrySeller()
    {
        Instance.angrySeller.Play();
    }
}
