using UnityEngine;
using TMPro;

public class AliveCountDisplay : MonoBehaviour
{
    public VictoryManager victoryManager;  // assign in inspector
    public TMP_Text aliveCountText;

    void Update()
    {
        aliveCountText.text = $"Enemies Alive: {victoryManager.aliveCount}";
    }
}


