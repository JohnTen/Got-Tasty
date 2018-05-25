using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RateMelon : MonoBehaviour
{
    [System.Serializable]
    struct Rate
    {
        public string rating;
        public float maturaity;
        public Sprite image;
    }

    [SerializeField]
    Text ratingText;

    [SerializeField]
    Text maturityText;

    [SerializeField]
    Image cutOpenImage;

    [SerializeField]
    Rate[] ratingLevels;


    public void RateWatermelon(Watermelon melon)
    {
        if (melon.Wholeness <= 0)
        {
            var r = ratingLevels[ratingLevels.Length - 1];

            ratingText.text = r.rating;
            maturityText.text = melon.Maturity.ToString();
            cutOpenImage.sprite = r.image;
        }
        else
        {
            foreach (var r in ratingLevels)
            {
                if (melon.Maturity < r.maturaity)
                    continue;

                ratingText.text = r.rating;
                maturityText.text = melon.Maturity.ToString();
                cutOpenImage.sprite = r.image;
                break;
            }
        }

        this.gameObject.SetActive(true);
    }
}
