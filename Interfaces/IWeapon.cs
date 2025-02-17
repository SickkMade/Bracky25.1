using UnityEngine;

public interface IWeapon
{
    GameObject WeaponObject { get; }
    void ActionOn();
    void ActionOff();
}
