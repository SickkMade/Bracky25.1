using UnityEngine;

public interface IHeldObject: IInteractable    
{
    public void OnPickup();
    public void Drop();

    public void DoInteraction();
    public float getHoldDistance();

    public GameObject getObject();

}
