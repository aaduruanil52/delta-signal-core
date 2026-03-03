using UnityEngine;
using UnityEngine.UI;
using System.Collections;
public class Card : MonoBehaviour
{
    public int cardID;
    public bool isFlipped;
    public bool isMatched;

    private Image image;

    void Awake()
    {
        image = GetComponent<Image>();
    }

    public void OnClick()
    {
        if (isMatched || isFlipped) return;
        Flip();
    }

    public void Flip()
    {
        StartCoroutine(FlipAnimation());
    }

    IEnumerator FlipAnimation()
    {
        isFlipped = true;

        for (float i = 0; i <= 180; i += 10)
        {
            transform.rotation = Quaternion.Euler(0, i, 0);
            yield return null;
        }
    }
}