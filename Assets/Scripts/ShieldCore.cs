using UnityEngine;


[System.Serializable]
public class ShieldData
{
    public int extraProtection;
    public GameObject ShieldPrefab;
}


public class ShieldCore : MonoBehaviour
{
    public ShieldData[] possibleShields;
    private ShieldData selectedShield;


    void Start()
    {
        Debug.Log($"{name}: WeaponCore Start called.");


        if (possibleShields == null || possibleShields.Length == 0)
        {
            Debug.LogWarning($"{name} has no possible weapons assigned!");
            return;
        }


        AssignRandomShield();
    }


    void AssignRandomShield()
    {
        selectedShield = possibleShields[Random.Range(0, possibleShields.Length)];


        if (selectedShield == null)
        {
            Debug.LogWarning($"{name} selectedWeapon is null after random selection!");
            return;
        }


        Debug.Log($"{name} assigned weapon with {selectedShield.ShieldPrefab} bonus damage.");


        if (selectedShield.ShieldPrefab != null)
        {
            Instantiate(selectedShield.ShieldPrefab, transform.position, transform.rotation, transform);
        }
    }


    public int GetBonusProtection()
    {
        if (selectedShield == null)
        {
            return 0;
        }


        return selectedShield.extraProtection;
    }
}



