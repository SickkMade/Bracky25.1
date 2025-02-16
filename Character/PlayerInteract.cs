using Unity.Cinemachine;
using UnityEngine;

[RequireComponent(typeof(MainPlayer))]
public class PlayerInteract : MonoBehaviour, IInteractor
{
    CinemachineCamera cam;
    IHeldObject heldObject;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cam = GetComponentInChildren<CinemachineCamera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }


        if (heldObject != null)
        {
            heldObject.getObject().transform.position = cam.transform.position + (cam.transform.forward * heldObject.getHoldDistance());
            heldObject.getObject().transform.rotation = cam.transform.rotation;

            if (Input.GetKeyDown(KeyCode.F))
            {
                heldObject.Drop();
            }
        }
    }

    private void Interact()
    {
        // Default 1st person mode
        Vector3 rayD = cam.transform.forward;
        Vector3 rayPos = cam.transform.position;
        if (heldObject != null)
        {
            rayPos += rayD.normalized * (heldObject.getHoldDistance() + 0.01f);
        }
        Ray ray = new Ray(rayPos, rayD);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            Debug.Log("Hit object: " + hit.collider.gameObject.name);
        }
        // If we hit nothing, do nothing
        if (hit.collider == null)
        {
            return;
        }

        IInteractable iObj = hit.collider.gameObject.GetComponent<IInteractable>();
        if (iObj != null)
        {
            iObj.OnInteract(this);
            return;
        }
    }

    public bool PickupObject(IHeldObject obj)
    {
        if (heldObject != null)
        {
            return false;
        }
        return true;
    }

    public IHeldObject GetHeldObject()
    {
        return heldObject;
    }
}
