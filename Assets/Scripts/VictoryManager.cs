using UnityEngine;
using TMPro;

public class VictoryManager : MonoBehaviour
{
    public GameObject victoryPanel;
    public TMP_Text victoryText;

    // Make aliveCount public so other scripts/UI can access it
    public int aliveCount = 0;

    private bool gameEnded = false;

    void Update()
    {
        if (gameEnded) return;

        MeleeFighter[] fighters = FindObjectsOfType<MeleeFighter>();
        MeleeFighter aliveFighter = null;
        aliveCount = 0;  // reset every frame

        foreach (MeleeFighter f in fighters)
        {
            if (f != null && f.currentHealth > 0)
            {
                aliveCount++;
                aliveFighter = f;
            }
        }

        if (aliveCount == 1 && aliveFighter != null)
        {
            int kills = aliveFighter.kills;
            string playerName = aliveFighter.gameObject.name;

            Debug.Log($"Victory Detected: {playerName} with {kills} kills");

            victoryPanel.SetActive(true);
            victoryText.text = $"{playerName} wins!\nKills: {kills}";
            gameEnded = true;
        }
    }
}


