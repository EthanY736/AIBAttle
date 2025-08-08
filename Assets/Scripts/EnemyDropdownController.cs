using UnityEngine;
using TMPro;
using System.Collections.Generic;


public class EnemyDropdownController : MonoBehaviour
{
    private TMP_Dropdown enemyDropdown;
    private List<GameObject> enemyList = new List<GameObject>();


    private Camera mainCam;
    private Camera activeEnemyCam;
    private GameObject currentWatchedEnemy;


    public GameObject enemyStatsUI;
    public TMP_Text healthText;
    public TMP_Text damageText;
    public TMP_Text armorText;
    public TMP_Text killsText;


    private int currentIndex = -1;


    void OnEnable()
    {
        MeleeFighter.OnEnemyDied += HandleEnemyDeath;
        MeleeFighter.OnEnemyGotKill += HandleEnemyGotKill;
    }


    void OnDisable()
    {
        MeleeFighter.OnEnemyDied -= HandleEnemyDeath;
        MeleeFighter.OnEnemyGotKill -= HandleEnemyGotKill;
    }




    void Start()
    {
        mainCam = Camera.main;
        if (mainCam == null)
        {
            Debug.LogError("Main camera not found.");
            return;
        }


        foreach (Camera cam in Camera.allCameras)
        {
            if (cam != mainCam)
                cam.enabled = false;
        }


        GameObject dropdownObj = GameObject.FindGameObjectWithTag("Leaderboard");
        if (dropdownObj == null)
        {
            Debug.LogError("No GameObject with tag 'Leaderboard' found.");
            return;
        }


        enemyDropdown = dropdownObj.GetComponent<TMP_Dropdown>();
        if (enemyDropdown == null)
        {
            Debug.LogError("TMP_Dropdown component missing from object tagged 'Leaderboard'.");
            return;
        }


        enemyDropdown.onValueChanged.AddListener(OnDropdownChanged);
        Invoke(nameof(PopulateDropdown), 1f);
    }


    void Update()
    {
        CleanupDeadEnemies();


        if (currentWatchedEnemy == null && activeEnemyCam != null)
        {
            ReturnToMainCamera();
        }


        if (activeEnemyCam != null && enemyStatsUI.activeSelf && currentWatchedEnemy != null)
        {
            MeleeFighter stats = currentWatchedEnemy.GetComponent<MeleeFighter>();
            if (stats != null)
            {
                healthText.text = $"Health: {stats.currentHealth}";
                damageText.text = $"Damage: {stats.damage}";
                armorText.text = $"Armor: {stats.armor}";
                killsText.text = $"Kills: {stats.kills}";
            }
        }


        if (enemyList.Count > 0 && currentIndex != -1)
        {
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                currentIndex = (currentIndex + 1) % enemyList.Count;
                enemyDropdown.value = currentIndex;
                OnDropdownChanged(currentIndex);
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                currentIndex = (currentIndex - 1 + enemyList.Count) % enemyList.Count;
                enemyDropdown.value = currentIndex;
                OnDropdownChanged(currentIndex);
            }
        }
    }


    void PopulateDropdown()
    {
        enemyList.Clear();
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");


        foreach (GameObject enemy in enemies)
        {
            enemyList.Add(enemy);
        }


        enemyList.Sort((a, b) =>
        {
            MeleeFighter statsA = a.GetComponent<MeleeFighter>();
            MeleeFighter statsB = b.GetComponent<MeleeFighter>();


            int killsA = statsA != null ? statsA.kills : 0;
            int killsB = statsB != null ? statsB.kills : 0;


            return killsB.CompareTo(killsA);
        });


        List<string> options = new List<string>();
        foreach (GameObject enemy in enemyList)
        {
            options.Add(enemy.name);
        }


        enemyDropdown.ClearOptions();
        enemyDropdown.AddOptions(options);
        currentIndex = -1;
    }
    void ResortAndRefreshDropdown()
    {


        enemyList.Sort((a, b) =>
        {
            MeleeFighter statsA = a.GetComponent<MeleeFighter>();
            MeleeFighter statsB = b.GetComponent<MeleeFighter>();


            int killsA = statsA != null ? statsA.kills : 0;
            int killsB = statsB != null ? statsB.kills : 0;


            return killsB.CompareTo(killsA);
        });




        List<string> options = new List<string>();
        foreach (GameObject enemy in enemyList)
        {
            options.Add(enemy.name);
        }


        enemyDropdown.ClearOptions();
        enemyDropdown.AddOptions(options);


        if (currentWatchedEnemy != null)
        {
            int index = enemyList.IndexOf(currentWatchedEnemy);
            if (index != -1)
            {
                currentIndex = index;
                enemyDropdown.value = index;
            }
        }
    }




    void OnDropdownChanged(int index)
    {
        if (index < 0 || index >= enemyList.Count) return;


        currentIndex = index;
        currentWatchedEnemy = enemyList[index];


        Camera[] cams = currentWatchedEnemy.GetComponentsInChildren<Camera>(true);
        if (cams.Length == 0)
        {
            Debug.LogWarning("No camera found on selected enemy.");
            return;
        }


        Camera newCam = cams[0];


        if (activeEnemyCam != null)
        {
            activeEnemyCam.enabled = false;
            activeEnemyCam.gameObject.SetActive(false);
        }


        newCam.gameObject.SetActive(true);
        newCam.enabled = true;
        activeEnemyCam = newCam;


        mainCam.enabled = false;
        enemyStatsUI.SetActive(true);
    }


    public void ReturnToMainCamera()
    {
        if (activeEnemyCam != null)
        {
            activeEnemyCam.enabled = false;
            activeEnemyCam.gameObject.SetActive(false);
            activeEnemyCam = null;
        }


        mainCam.enabled = true;
        enemyStatsUI.SetActive(false);
        currentWatchedEnemy = null;
        currentIndex = -1;
    }


    void CleanupDeadEnemies()
    {
        bool changed = false;
        for (int i = enemyList.Count - 1; i >= 0; i--)
        {
            if (enemyList[i] == null)
            {
                enemyList.RemoveAt(i);
                changed = true;
            }
        }


        if (changed)
        {
            List<string> options = new List<string>();
            foreach (GameObject enemy in enemyList)
            {
                options.Add(enemy.name);
            }
            enemyDropdown.ClearOptions();
            enemyDropdown.AddOptions(options);


            if (currentIndex >= enemyList.Count)
                currentIndex = enemyList.Count - 1;
        }
    }


    void HandleEnemyDeath(GameObject deadEnemy, GameObject killer)
    {
        if (deadEnemy == currentWatchedEnemy)
        {
            if (killer != null)
            {
                int killerIndex = enemyList.IndexOf(killer);
                if (killerIndex != -1)
                {
                    currentIndex = killerIndex;
                    enemyDropdown.value = currentIndex;
                    OnDropdownChanged(currentIndex);
                    return;
                }
            }


            ReturnToMainCamera();
        }


        enemyList.Remove(deadEnemy);
        PopulateDropdown();
    }
    void HandleEnemyGotKill(GameObject killer)
    {
        if (killer != null && enemyList.Contains(killer))
        {
            ResortAndRefreshDropdown();
        }
    }
    public void ShowStatsFor(GameObject enemy)
    {
        if (enemy == null) return;


        currentWatchedEnemy = enemy;


        Camera[] cams = currentWatchedEnemy.GetComponentsInChildren<Camera>(true);
        if (cams.Length > 0)
        {
            if (activeEnemyCam != null)
            {
                activeEnemyCam.enabled = false;
                activeEnemyCam.gameObject.SetActive(false);
            }


            Camera newCam = cams[0];
            newCam.gameObject.SetActive(true);
            newCam.enabled = true;
            activeEnemyCam = newCam;


            mainCam.enabled = false;
        }


        enemyStatsUI.SetActive(true);


        int index = enemyList.IndexOf(enemy);
        if (index != -1)
        {
            currentIndex = index;
            enemyDropdown.value = index;
        }
    }
    public void HideStats()
    {
        if (enemyStatsUI != null)
            enemyStatsUI.SetActive(false);
    }




}



