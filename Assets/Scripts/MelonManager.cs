using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;

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
    Animator cutMelon;

    [SerializeField]
    UnityEvent CuttingMelon;

    [SerializeField]
    UnityEvent CuttedMelon;

    [SerializeField]
    UnityEvent BrokenMelon;

    [SerializeField]
    Watermelon[] melonPrefabs;
    
    Watermelon[] melons;

    Vector3 melonPosition;
    Vector3 melonScale;
    bool transfering;
    bool cutting;

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
            if (i == 0 && (melons[i].Maturity > 1.2f || melons[i].Maturity < 0.8f))
            {
                Destroy(melons[i].gameObject);
                i--;
                continue;
            }

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

        if (CurrentManipulatingWatermelon.Wholeness > 0)
        {
            StartCoroutine(CutMelon());
            return;
        }

        BrokenMelon?.Invoke();
    }

    public void AngrySeller()
    {
        SoundManager.AngrySeller();
    }

    public void PatMelon(Watermelon melon)
    {
        SoundManager.PatWatermelon(melon.Maturity);

        if (melon.Wholeness <= 0)
        {
            sellerSprite.ChangeToLastSprite();
            Invoke("AngrySeller", 0.8f);
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

    IEnumerator CutMelon()
    {
        cutting = true;
        CuttingMelon?.Invoke();
        cutMelon.SetBool("Cut", true);
        SoundManager.Knife();
        yield return new WaitForSeconds(0.5f);
        cutMelon.SetBool("Cut", false);
        CuttedMelon?.Invoke();
        cutting = false;
        
        CurrentManipulatingWatermelon.gameObject.SetActive(false);
        melonRator.RateWatermelon(CurrentManipulatingWatermelon);
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
