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

    public void Pat(float force)
    {
        wholeness -= force;
        if (wholeness > 0) return;
        BrokeDown();
    }

    public void BrokeDown()
    {
        
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        
    }
}
