using System.Collections.Generic;
using UnityEngine;


public class WeaponSystem : MonoBehaviour
{
    [SerializeField, Header("Please IWeapon Only")] private List<GameObject> inventory = new();
    private List<IWeapon> weaponInventory = new();
    private int _weaponIndex = 0;
    private int WeaponIndex {
        get {
            return _weaponIndex;
        }
        set {
            _weaponIndex = (_weaponIndex + value) % inventory.Count;
        }
    }

    void Start()
    {
        CreateObjectsAndAddToList();
    }

    void Update()
    {
        if(!PlayerManager.Instance.playerData.isCharacterActive) return;

        if(Input.GetKeyDown(KeyCode.Tab)){
            ChangeWeapon();
        }
        if(Input.GetKeyDown(KeyCode.F)){
            ActivateWeapon();
        }
        else if(Input.GetKeyUp(KeyCode.F)){
            DeactivateWeapon();
        }
    }

    void CreateObjectsAndAddToList(){
        foreach(GameObject weaponPrefab in inventory){
            if(weaponPrefab.TryGetComponent<IWeapon>(out IWeapon weapon)){
                weaponInventory.Add(weapon);
                Instantiate(weaponPrefab, transform);
            }
        }
    }

    void ChangeWeapon(){
        weaponInventory[WeaponIndex].WeaponObject.SetActive(false);
        WeaponIndex += 1;
        weaponInventory[WeaponIndex].WeaponObject.SetActive(true);
    }

    void ActivateWeapon(){
        weaponInventory[WeaponIndex].ActionOn();
    }

    void DeactivateWeapon(){
        weaponInventory[WeaponIndex].ActionOff();
    }

}
