using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameObject cardPrefab;
    public Transform gridParent;

    public Sprite[] cardSprites;

    public Text matchesText;
    public Text turnsText;

    private List<Card> flippedCards = new List<Card>();

    private int matches = 0;
    private int turns = 0;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        GenerateCards();
        UpdateUI();
    }

    void GenerateCards()
    {
        List<int> ids = new List<int>();

        for (int i = 0; i < 8; i++)
        {
            ids.Add(i);
            ids.Add(i);
        }

        Shuffle(ids);

        foreach (int id in ids)
        {
            GameObject obj = Instantiate(cardPrefab, gridParent);
            Card card = obj.GetComponent<Card>();
            card.Setup(id, cardSprites[id]);
        }
    }

    public void CardFlipped(Card card)
    {
        flippedCards.Add(card);

        if (flippedCards.Count == 2)
        {
            turns++;
            UpdateUI();
            StartCoroutine(CheckMatch());
        }
    }

    IEnumerator CheckMatch()
    {
        yield return new WaitForSeconds(0.5f);

        if (flippedCards[0].id == flippedCards[1].id)
        {
            flippedCards[0].SetMatched();
            flippedCards[1].SetMatched();

            matches++;

            if (matches == 8)
            {
                Debug.Log("Game Over");
            }
        }
        else
        {
            flippedCards[0].ResetCard();
            flippedCards[1].ResetCard();
        }

        flippedCards.Clear();
        UpdateUI();
    }

    void UpdateUI()
    {
        matchesText.text = "Matches\n" + matches;
        turnsText.text = "Turns\n" + turns;
    }

    void Shuffle(List<int> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int rand = Random.Range(0, list.Count);
            int temp = list[i];
            list[i] = list[rand];
            list[rand] = temp;
        }
    }
}
