using UnityEngine;

public interface IHeldObject : IInteractable
{
    float HoldDistance
    {
        get;
    }
    GameObject GObject
    {
        get;
    }
    string ID
    {
        get;
    }

    Vector3 HeldRotation { get; }

    Vector3 HalfSize { get; }

    public void OnPickup();
    public void Drop();

    public void UseWithObject();

    

}
