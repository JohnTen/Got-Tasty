using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class ChangeSprite : MonoBehaviour
{
    [SerializeField]
    Sprite[] sprites;

    SpriteRenderer renderer;

    int currentIndex;

    private void Awake()
    {
        renderer = GetComponent<SpriteRenderer>();
    }

    public void ChangeSpriteTo(int index)
    {
        index = index < 0 ? 0 : index;
        index = index >= sprites.Length ? sprites.Length - 1 : index;
        currentIndex = index;

        renderer.sprite = sprites[index];
    }

    public void ChangeToNextSprite()
    {
        ChangeSpriteTo(currentIndex + 1);
    }

    public void ChangeToPreviousSprite()
    {
        ChangeSpriteTo(currentIndex - 1);
    }

    public void ChangeToFirstSprite()
    {
        ChangeSpriteTo(0);
    }

    public void ChangeToLastSprite()
    {
        ChangeSpriteTo(sprites.Length - 1);
    }
}
