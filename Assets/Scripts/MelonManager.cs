using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MelonManager : MonoBehaviour
{
    [SerializeField]
    AudioMixerSnapshot pickUp;

    [SerializeField]
    AudioMixerSnapshot putDown;

    [SerializeField]
    Transform patPosition;

    [SerializeField]
    float transferDuration;

    [SerializeField]
    BoxArea melonArea;

    [SerializeField]
    int melonCount;

    [SerializeField]
    float melonDistance;

    [SerializeField, MinMaxSlider(0.1f, 0.5f)]
    Vector2 melonScaleRange;

    [SerializeField]
    SpriteRenderer pickupMask;

    [SerializeField]
    ChangeSprite sellerSprite;

    [SerializeField]
    RateMelon melonRator;

    [SerializeField]
    Watermelon[] melonPrefabs;
    
    Watermelon[] melons;

    Vector3 melonPosition;
    Vector3 melonScale;
    bool transfering;

    int patTimes;

    public Watermelon CurrentManipulatingWatermelon { get; private set; }

    public bool isManipulatingWatermelon
    {
        get
        {
            return CurrentManipulatingWatermelon != null;
        }
    }

    public void StartNewGame()
    {
        if (melonArea == null)
            melonArea = GameObject.FindGameObjectWithTag("MelonArea").GetComponent<BoxArea>();

        if (melons != null && melons.Length > 0)
        {
            for (int i = 0; i < melons.Length; i++)
            {
                if (melons[i] != null)
                {
                    Destroy(melons[i].gameObject);
                }
            }
        }

        melons = new Watermelon[melonCount];
        for (int i = 0; i < melonCount; i++)
        {
            melons[i] = Instantiate(melonPrefabs[Random.Range(0, melonPrefabs.Length)].gameObject).GetComponent<Watermelon>();
            melons[i].transform.localScale = Vector3.one * Random.Range(melonScaleRange.x, melonScaleRange.y);
            melons[i].transform.rotation = Quaternion.Euler(0, 0, Random.value * 180 - 90);

            Vector3 pos = Vector3.zero;
            for (int j = 0; j < 100; j++)
            {
                pos = melonArea.GetRandomPoint();
                bool foundSuitablePos = true;
                foreach (var m in melons)
                {
                    if (m == null) break;
                    if (((Vector2)(m.transform.position - pos)).sqrMagnitude < melonDistance * melonDistance)
                    {
                        foundSuitablePos = false;
                        break;
                    }
                }

                if (foundSuitablePos) break;
            }

            pos.z = pos.y;
            melons[i].transform.position = pos;
        }
    }

    public void RateMelon()
    {
        if (!CurrentManipulatingWatermelon) return;

        CurrentManipulatingWatermelon.gameObject.SetActive(false);
        melonRator.RateWatermelon(CurrentManipulatingWatermelon);
    }

    public void PatMelon(Watermelon melon)
    {
        SoundManager.PatWatermelon(melon.Maturity);

        if (melon.Wholeness <= 0)
        {
            sellerSprite.ChangeToLastSprite();
            SoundManager.AngrySeller();
            RateMelon();
            return;
        }

        patTimes++;
        var sellerPicIndex = patTimes / 5;
        sellerPicIndex = sellerPicIndex > 2 ? 2 : sellerPicIndex;
        sellerSprite.ChangeSpriteTo(sellerPicIndex);
    }

    public void PickUpWaterMelon(Watermelon melon)
    {
        if (transfering) return;
        if (CurrentManipulatingWatermelon)
        {
            Debug.LogWarning("Picking up melon while already manipulating a melon.");
        }
        CurrentManipulatingWatermelon = melon;
        StartCoroutine(PickUpMelon());
        pickUp.TransitionTo(0.5f);
    }

    public void QuitManipulatingWatermelon()
    {
        if (transfering) return;
        if (!CurrentManipulatingWatermelon) return;

        StartCoroutine(PutBackMelon());
        CurrentManipulatingWatermelon = null;
        putDown.TransitionTo(0.5f);
    }

    IEnumerator PickUpMelon()
    {
        transfering = true;
        var melonTransform = CurrentManipulatingWatermelon.transform;
        melonPosition = melonTransform.position;
        melonScale = melonTransform.localScale;

        if (patPosition == null)
            patPosition = GameObject.FindGameObjectWithTag("PatPosition").transform;

        var time = transferDuration;
        var color = pickupMask.color;
        while (time > 0)
        {
            time -= Time.deltaTime;
            melonTransform.position = Vector3.Lerp(patPosition.position, melonPosition, time / transferDuration);
            melonTransform.localScale = Vector3.Lerp(Vector3.one, melonScale, time / transferDuration);
            color.a = Mathf.Lerp(0.6f, 0f, time / transferDuration);
            pickupMask.color = color;
            yield return null;
        }

        melonTransform.position = patPosition.position;
        melonTransform.localScale = Vector3.one;
        transfering = false;
    }

    IEnumerator PutBackMelon()
    {
        transfering = true;
        var melonTransform = CurrentManipulatingWatermelon.transform;

        var time = transferDuration;
        var color = pickupMask.color;
        while (time > 0)
        {
            time -= Time.deltaTime;
            melonTransform.position = Vector3.Lerp(melonPosition, patPosition.position, time / transferDuration);
            melonTransform.localScale = Vector3.Lerp(melonScale, Vector3.one, time / transferDuration);
            color.a = Mathf.Lerp(0f, 0.6f, time / transferDuration);
            pickupMask.color = color;
            yield return null;
        }

        melonTransform.position = melonPosition;
        melonTransform.localScale = melonScale;
        transfering = false;
    }
}
