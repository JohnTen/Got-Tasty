using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Watermelon : MonoBehaviour
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
}
