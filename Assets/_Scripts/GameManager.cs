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

    public GameObject startButton;

    public GameObject gameOverPanel;
    public Text gameOverText;

    // --- Layout Configuration ---
    [System.Serializable]
    public struct GridLayout
    {
        public int rows;
        public int cols;
    }

    public GridLayout[] availableLayouts = new GridLayout[]
    {
        new GridLayout { rows = 2, cols = 2 },
        new GridLayout { rows = 2, cols = 3 },
        new GridLayout { rows = 3, cols = 4 },
        new GridLayout { rows = 4, cols = 4 },
        new GridLayout { rows = 4, cols = 5 },
        new GridLayout { rows = 5, cols = 6 },
    };

    public int selectedLayoutIndex = 3; // Default: 4x4

    // Padding & spacing (in pixels)
    public float cellSpacing = 10f;
    public float gridPadding = 20f;

    // --- Private State ---
    private List<Card> flippedCards = new List<Card>();
    private List<Card> allCards = new List<Card>();

    private int matches = 0;
    private int turns = 0;
    private bool isChecking = false;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        GenerateCards();
        UpdateUI();
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
    }

    void GenerateCards()
    {
        // Clear previous cards
        foreach (Card c in allCards)
            if (c != null) Destroy(c.gameObject);
        allCards.Clear();
        flippedCards.Clear();
        matches = 0;
        turns = 0;

        GridLayout layout = availableLayouts[selectedLayoutIndex];
        int rows = layout.rows;
        int cols = layout.cols;
        int totalCards = rows * cols;

        // Ensure even number of cards
        if (totalCards % 2 != 0)
        {
            Debug.LogWarning("Odd number of cards — reducing by 1.");
            totalCards--;
        }

        int pairCount = totalCards / 2;

        // Build shuffled ID list
        List<int> ids = new List<int>();
        for (int i = 0; i < pairCount; i++)
        {
            ids.Add(i % cardSprites.Length);
            ids.Add(i % cardSprites.Length);
        }
        Shuffle(ids);

        // Configure GridLayoutGroup
        GridLayoutGroup glg = gridParent.GetComponent<GridLayoutGroup>();
        if (glg == null) glg = gridParent.gameObject.AddComponent<GridLayoutGroup>();

        RectTransform parentRect = gridParent.GetComponent<RectTransform>();

        // Calculate cell size to fill the parent rect
        float availableWidth  = parentRect.rect.width  - gridPadding * 2 - cellSpacing * (cols - 1);
        float availableHeight = parentRect.rect.height - gridPadding * 2 - cellSpacing * (rows - 1);

        float cellW = availableWidth  / cols;
        float cellH = availableHeight / rows;
       // float cellSize = Mathf.Min(cellW, cellH); // keep cards square

        glg.constraint      = GridLayoutGroup.Constraint.FixedColumnCount;
        glg.constraintCount = cols;
       // glg.cellSize        = new Vector2(cellSize, cellSize);
        glg.spacing         = new Vector2(cellSpacing, cellSpacing);
        glg.padding         = new RectOffset(
            (int)gridPadding, (int)gridPadding,
            (int)gridPadding, (int)gridPadding);
        glg.childAlignment  = TextAnchor.MiddleCenter;

        // Instantiate cards
        for (int i = 0; i < totalCards; i++)
        {
            GameObject obj = Instantiate(cardPrefab, gridParent);
            Card card = obj.GetComponent<Card>();
            card.Setup(ids[i], cardSprites[ids[i]]);
            allCards.Add(card);
        }
    }

    // Call this from UI buttons to switch layout
    public void SetLayout(int index)
    {
        if (index < 0 || index >= availableLayouts.Length) return;
        selectedLayoutIndex = index;
        GenerateCards();
        UpdateUI();
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        if (startButton != null) startButton.SetActive(true);
    }

    public void StartGame()
    {
        StartCoroutine(HideAllCards());
        if (startButton != null) startButton.SetActive(false);
    }

    IEnumerator HideAllCards()
    {
        yield return new WaitForSeconds(1f);

        foreach (Card card in allCards)
            card.Flip(false);

        yield return new WaitForSeconds(0.5f);

        foreach (Card card in allCards)
            card.canClick = true;
    }

    public void CardFlipped(Card card)
    {
        if (isChecking) return;

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
        isChecking = true;
        yield return new WaitForSeconds(0.5f);

        Card a = flippedCards[0];
        Card b = flippedCards[1];

        if (a.id == b.id)
        {
            a.SetMatched();
            b.SetMatched();
            matches++;
        }
        else
        {
            a.ResetCard();
            b.ResetCard();
        }

        flippedCards.Clear();
        isChecking = false;
        UpdateUI();

        int totalPairs = allCards.Count / 2;
        if (matches == totalPairs)
        {
            Debug.Log("Game Over");
            if (gameOverPanel != null)
            {
                gameOverPanel.SetActive(true);
                if (gameOverText != null)
                    gameOverText.text = "You Win!\nMatches: " + matches + "\nTurns: " + turns;
            }
        }
    }

    void UpdateUI()
    {
        if (matchesText != null)
            matchesText.text = "Matches\n" + matches;

        if (turnsText != null)
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
