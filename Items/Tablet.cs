using UnityEngine;

public class Tablet : MonoBehaviour, IWeapon
{
    public GameObject WeaponObject => gameObject;

    [SerializeField] private GameObject tabletCanvas;


    public void ActionOff()
    {
        //un implimented 
    }

    public void ActionOn()
    {
        tabletCanvas.SetActive(!tabletCanvas.activeSelf);
    }
}
