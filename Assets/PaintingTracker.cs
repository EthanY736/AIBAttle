using UnityEngine;
using TMPro;

public class PaintingTracker : MonoBehaviour
{
    public static PaintingTracker instance;
    public TextMeshProUGUI scoreText;
    public int totalPaintings = 10; // Set this to the total number of paintings in your game
    private int foundPaintings = 0;
    void Awake()
    {
        instance = this;
    }
    void Start()
    {
        UpdateScoreText();
    }

    public void AddPainting()
    {
        foundPaintings++;
        UpdateScoreText();
    }

    private void UpdateScoreText()
    {
        scoreText.text = $"{foundPaintings}/{totalPaintings}";
    }
}


