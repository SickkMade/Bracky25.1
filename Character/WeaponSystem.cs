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
            _weaponIndex = value % weaponInventory.Count;
        }
    }

    void Start()
    {
        CreateObjectsAndAddToList();
        SetWeapon(0);
    }

    void Update()
    {
        if(!PlayerManager.Instance.playerData.isCharacterActive) return;

        if(Input.GetKeyDown(KeyCode.Tab)){
            DeactivateWeapon();
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
                GameObject Instantiated = Instantiate(weaponPrefab, transform);
                Instantiated.SetActive(false);
                weaponInventory.Add(Instantiated.GetComponent<IWeapon>());
            } //lol needs refactor
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

    void SetWeapon(int weaponIndex){
        weaponInventory[WeaponIndex].WeaponObject.SetActive(false);
        WeaponIndex = weaponIndex;
        weaponInventory[WeaponIndex].WeaponObject.SetActive(true);
    }

}
