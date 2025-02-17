using UnityEngine;

public class Flashlight : MonoBehaviour, IWeapon
{
    [SerializeField] private GameObject flashlightLight;

    public GameObject WeaponObject => gameObject;

    void IWeapon.ActionOn()
    {
        flashlightLight.SetActive(true);
    }
    void IWeapon.ActionOff()
    {
        flashlightLight.SetActive(false);
    }
}
