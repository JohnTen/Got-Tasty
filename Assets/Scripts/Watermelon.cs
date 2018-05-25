using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Watermelon : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField]
    float maturity;
    public float Maturity
    {
        get { return maturity; }
        set { maturity = value; }
    }

    [SerializeField]
    float wholeness;
    public float Wholeness
    {
        get { return wholeness; }
        set { wholeness = value; }
    }

    [SerializeField]
    Sprite brokenSprite;

    public void BrokenDown()
    {
        SoundManager.SmashWatermelon();
        GetComponent<SpriteRenderer>().sprite = brokenSprite;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (wholeness <= 0) return;
        if (GameManager.isManipulatingWatermelon)
        {
            if (GameManager.CurrentManipulatingWatermelon != this) return;
            wholeness -= 0.15f;
            if (wholeness <= 0)
            {
                BrokenDown();
            }

            GameManager.PatMelon(this);
        }
        else
        {
            GameManager.PickUpWaterMelon(this);
        }

    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // nothing, yet.
    }

    private void Awake()
    {
        maturity = Random.Range(0f, 2f);
        wholeness = 1;
    }
}

