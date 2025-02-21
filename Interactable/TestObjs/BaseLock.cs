using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public abstract class BaseLock : MonoBehaviour, IInteractable
{
    [SerializeField] string useId;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public abstract void PerformTargetInteraction();
    public abstract void PerformGenericInteraction();

    public void OnInteract(IInteractor interactor)
    {
        if (interactor.HeldObject == null)
        {

        }
        else
        {
            string id = interactor.HeldObject.ID;
            if (id == useId)
            {
                PerformTargetInteraction();
            }
            else
            {
                PerformGenericInteraction();
            }
        }
        
    }
}
