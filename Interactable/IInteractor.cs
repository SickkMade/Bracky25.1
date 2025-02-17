using UnityEngine;

public interface IInteractor
{
    public bool PickupObject(IHeldObject obj);

    IHeldObject HeldObject
    {
        get;
    }

    /// <summary>
    /// Call when the object is "Used"
    /// Player no longer holds that object.
    /// </summary>
    public void RemoveHeldObject();
}
