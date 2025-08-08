using UnityEngine;


[System.Serializable]
public class WeaponData
{
    public int extraDamage;
    public GameObject weaponPrefab;
}


public class WeaponCore : MonoBehaviour
{
    public WeaponData[] possibleWeapons;
    private WeaponData selectedWeapon;


    void Start()
    {
        Debug.Log($"{name}: WeaponCore Start called.");


        if (possibleWeapons == null || possibleWeapons.Length == 0)
        {
            Debug.LogWarning($"{name} has no possible weapons assigned!");
            return;
        }


        AssignRandomWeapon();
    }


    void AssignRandomWeapon()
    {
        selectedWeapon = possibleWeapons[Random.Range(0, possibleWeapons.Length)];


        if (selectedWeapon == null)
        {
            Debug.LogWarning($"{name} selectedWeapon is null after random selection!");
            return;
        }


        Debug.Log($"{name} assigned weapon with {selectedWeapon.extraDamage} bonus damage.");


        if (selectedWeapon.weaponPrefab != null)
        {
            Instantiate(selectedWeapon.weaponPrefab, transform.position, transform.rotation, transform);
        }
    }


    public int GetBonusDamage()
    {
        if (selectedWeapon == null)
        {
            Debug.LogWarning($"{name} tried to get bonus damage but selectedWeapon is null.");
            return 0;
        }


        return selectedWeapon.extraDamage;
    }
}



