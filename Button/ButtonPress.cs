using Unity.VisualScripting;
using UnityEngine;

public class ButtonPress : MonoBehaviour, IInteractable
{
    [SerializeField] private string id;

    public void OnInteract(IInteractor interactor)
    {
        GameManager.Instance.InvokeButtonPressedEvent(id);
    }
}
