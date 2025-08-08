using UnityEngine;


public class CrownManager : MonoBehaviour
{
    public string crownChildName = "Crown";


    void Update()
    {
        MeleeFighter[] players = FindObjectsOfType<MeleeFighter>();
        MeleeFighter topPlayer = null;
        bool tie = false;
        int highestKills = -1;


        foreach (MeleeFighter player in players)
        {
            if (player.kills > highestKills)
            {
                highestKills = player.kills;
                topPlayer = player;
                tie = false;
            }
            else if (player.kills == highestKills)
            {
                tie = true;
            }
        }


        foreach (MeleeFighter player in players)
        {
            Transform crown = player.transform.Find(crownChildName);


            if (crown != null)
            {
                bool shouldShow = (player == topPlayer && !tie);
                crown.gameObject.SetActive(shouldShow);
            }
            else
            {
                Debug.LogWarning($"Crown not found on {player.name}");
            }
        }
    }
}



