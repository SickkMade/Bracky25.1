using UnityEngine;

public interface IInteractor
{
    public bool PickupObject(IHeldObject obj);

    public IHeldObject GetHeldObject();
}
