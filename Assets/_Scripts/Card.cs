using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Card : MonoBehaviour
{
    public int id;

    public Image iconImage;
    public Sprite frontSprite;
    public Sprite backSprite;

    [HideInInspector] public bool canClick = false;

    private bool isFlipped = false;
    private bool isMatched = false;
    private Coroutine flipCoroutine;

    public void Setup(int newId, Sprite sprite)
    {
        id = newId;
        frontSprite = sprite;

        iconImage.sprite = frontSprite;
        isFlipped = true;
    }

    public void OnClick()
    {
        if (!canClick) return;
        if (isFlipped || isMatched) return;

        Flip(true);
        GameManager.Instance.CardFlipped(this);
    }

    public void Flip(bool showFront)
    {
        if (flipCoroutine != null)
            StopCoroutine(flipCoroutine);
        flipCoroutine = StartCoroutine(FlipAnimation(showFront));

        SoundManager.Instance.PlayFlip();
    }

    IEnumerator FlipAnimation(bool showFront)
    {
        float duration = 0.15f;
        float time = 0f;

        // First half: fold to 90°
        Quaternion startRot = transform.rotation;
        Quaternion midRot = Quaternion.Euler(0, 90, 0);

        while (time < duration)
        {
            transform.rotation = Quaternion.Lerp(startRot, midRot, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        // Swap sprite at the midpoint
        transform.rotation = midRot;
        iconImage.sprite = showFront ? frontSprite : backSprite;
        isFlipped = showFront;

        // Second half: unfold from 90°
        time = 0f;
        Quaternion endRot = Quaternion.Euler(0, 0, 0);

        while (time < duration)
        {
            transform.rotation = Quaternion.Lerp(midRot, endRot, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        transform.rotation = endRot;
        flipCoroutine = null;
    }

    public void SetMatched()
    {
        isMatched = true;
        // Optional: tint to show matched state
        iconImage.color = new Color(0.7f, 1f, 0.7f);
    }

    public void ResetCard()
    {
        Flip(false);
    }
}