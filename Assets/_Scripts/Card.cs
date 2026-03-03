using UnityEngine;
using UnityEngine.UI;
using System.Collections;
public class Card : MonoBehaviour
{
    public int id;

    public Image iconImage;
    public Sprite frontSprite;
    public Sprite backSprite;

    private bool isFlipped = false;
    private bool isMatched = false;

    public void Setup(int newId, Sprite sprite)
    {
        id = newId;
        frontSprite = sprite;
        iconImage.sprite = backSprite;
    }

    public void OnClick()
    {
        if (isFlipped || isMatched) return;

        Flip(true);
        GameManager.Instance.CardFlipped(this);
    }

    public void Flip(bool showFront)
    {
        StartCoroutine(FlipAnimation(showFront));
    }

    IEnumerator FlipAnimation(bool showFront)
    {
        float duration = 0.2f;
        float time = 0;

        float start = showFront ? 0 : 180;
        float end = showFront ? 180 : 0;

        while (time < duration)
        {
            float angle = Mathf.Lerp(start, end, time / duration);
            transform.rotation = Quaternion.Euler(0, angle, 0);
            time += Time.deltaTime;
            yield return null;
        }

        transform.rotation = Quaternion.Euler(0, end, 0);

        iconImage.sprite = showFront ? frontSprite : backSprite;
        isFlipped = showFront;
    }

    public void SetMatched()
    {
        isMatched = true;
    }

    public void ResetCard()
    {
        Flip(false);
    }
}