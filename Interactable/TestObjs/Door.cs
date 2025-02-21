using UnityEngine;
using static UnityEngine.UIElements.UxmlAttributeDescription;

public class Door : MonoBehaviour, IInteractable
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    bool _open = false;
    [SerializeField] Animator animator;
    public bool IsOpen
    {
        get { return _open; }
        set
        {
            _open = value;
            animator.SetBool("IsOpen", _open);
        }
    }
    public void OnInteract(IInteractor interactor)
    {
        IsOpen = !IsOpen;

    }
}
