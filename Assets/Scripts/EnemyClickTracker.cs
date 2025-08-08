using UnityEngine;


public class EnemyClickTracker : MonoBehaviour
{
    private Camera mainCam;
    private Camera activePlayerCam;
    private GameObject selectedEnemy;


    public static GameObject ActiveControlledEnemy; // Only gets input after pressing Space


    void Start()
    {
        GameObject mainCamObj = GameObject.FindWithTag("MainCamera");


        if (mainCamObj == null)
        {
            Debug.LogError("No GameObject with tag 'MainCamera' found!");
            return;
        }


        mainCam = mainCamObj.GetComponent<Camera>();


        if (mainCam == null)
        {
            Debug.LogError("The object tagged 'MainCamera' does not have a Camera component!");
            return;
        }


        foreach (Camera cam in Camera.allCameras)
        {
            if (cam != mainCam)
                cam.enabled = false;
        }


        mainCam.enabled = true;
    }


    void Update()
    {
        // Left-click to select enemy
        if (Input.GetMouseButtonDown(0))
        {
            if (mainCam == null) return;


            Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.CompareTag("Enemy"))
                {
                    Transform root = hit.collider.transform.root;
                    selectedEnemy = root.gameObject;


                    Camera[] cams = root.GetComponentsInChildren<Camera>(true);


                    if (cams.Length == 0)
                    {
                        Debug.LogWarning("No cameras found in clicked player prefab: " + root.name);
                        return;
                    }


                    Camera newPlayerCam = cams[0];


                    if (activePlayerCam != null && activePlayerCam != newPlayerCam)
                    {
                        activePlayerCam.enabled = false;
                        activePlayerCam.gameObject.SetActive(false);
                    }


                    newPlayerCam.gameObject.SetActive(true);
                    newPlayerCam.enabled = true;
                    activePlayerCam = newPlayerCam;


                    mainCam.enabled = false;


                    // Update stat UI via EnemyDropdownController (if present)
                    EnemyDropdownController statUI = FindObjectOfType<EnemyDropdownController>();
                    if (statUI != null)
                    {
                        statUI.ShowStatsFor(selectedEnemy);
                    }


                    Debug.Log("Viewing enemy camera: " + newPlayerCam.name);
                }
            }
        }


        // Right-click to return to main camera
        if (Input.GetMouseButtonDown(1))
        {
            if (activePlayerCam != null)
            {
                activePlayerCam.enabled = false;
                activePlayerCam.gameObject.SetActive(false);
                activePlayerCam = null;
            }


            if (mainCam != null)
            {
                mainCam.enabled = true;
            }


            if (selectedEnemy != null)
            {
                var control = selectedEnemy.GetComponent<EnemyPlayerController>();
                if (control != null)
                    control.enabled = false;


                var aiComponents = selectedEnemy.GetComponentsInChildren<EnemyAI>(true);
                foreach (var ai in aiComponents)
                {
                    ai.enabled = true;
                }


                selectedEnemy = null;
            }


            ActiveControlledEnemy = null;


            // Hide the UI stats panel
            EnemyDropdownController statUI = FindObjectOfType<EnemyDropdownController>();
            if (statUI != null)
            {
                statUI.HideStats();
            }


            Debug.Log("Returned to main camera.");
        }


        // Press space to enable WASD control
        if (Input.GetKeyDown(KeyCode.Space) && selectedEnemy != null)
        {
            EnableWASDControl();
        }
    }


    void EnableWASDControl()
    {
        var control = selectedEnemy.GetComponent<EnemyPlayerController>();
        if (control == null)
            control = selectedEnemy.AddComponent<EnemyPlayerController>();


        control.enabled = true;


        var aiComponents = selectedEnemy.GetComponentsInChildren<EnemyAI>(true);
        foreach (var ai in aiComponents)
        {
            ai.enabled = false;
        }


        ActiveControlledEnemy = selectedEnemy;


        Debug.Log("WASD control enabled for: " + selectedEnemy.name);
    }
}



