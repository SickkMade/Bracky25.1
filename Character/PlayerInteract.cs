using NUnit.Framework;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

[RequireComponent(typeof(MainPlayer))]
public class PlayerInteract : MonoBehaviour, IInteractor
{
    CinemachineCamera cam;
    IHeldObject heldObject;
    public IHeldObject HeldObject => heldObject;

    [SerializeField] float interactRange = 4;
    [SerializeField] AudioClip audPickupSound;

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
            if (heldObject != null)
            {
                Interact();
                Debug.Log("Dropping Object");
                if (heldObject != null)
                {
                    heldObject.Drop();
                    heldObject = null;
                }
            }
            else
            {
                Interact();
            }

        }

    }
    void LateUpdate()
    {
        if (heldObject != null)
        {

            float holdDistance = heldObject.HoldDistance;
            Vector3 dir = cam.transform.forward.normalized;
            List<Vector3> objCorners = new List<Vector3>();
            // Find the 4 Corners that are in the direction the player is looking?
            // For now, just check all corners.
            Vector3 halfSize = heldObject.HalfSize;
            Vector3 rSize = (cam.transform.forward * halfSize.z) + (cam.transform.right * halfSize.x) + (cam.transform.up * halfSize.y);
            Vector3 bMin = cam.transform.position - rSize;
            Vector3 bMax = cam.transform.position + rSize;
            #region Define corners
            objCorners.Add(new Vector3(bMin.x, bMin.y, bMin.z));
            objCorners.Add(new Vector3(bMin.x, bMin.y, bMax.z));
            objCorners.Add(new Vector3(bMin.x, bMax.y, bMin.z));
            objCorners.Add(new Vector3(bMin.x, bMax.y, bMax.z));
            objCorners.Add(new Vector3(bMax.x, bMin.y, bMin.z));
            objCorners.Add(new Vector3(bMax.x, bMin.y, bMax.z));
            objCorners.Add(new Vector3(bMax.x, bMax.y, bMin.z));
            objCorners.Add(new Vector3(bMax.x, bMax.y, bMax.z));
            #endregion
            foreach (Vector3 v in objCorners)
            {
                //Offset from camera.
                Ray ray = new Ray(v, dir);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, maxDistance: interactRange, layerMask: 7))
                {
                    holdDistance = Mathf.Min(holdDistance, hit.distance);
                    Debug.Log("Blocked by Object: " + hit.collider.gameObject.name);
                }
            }
            holdDistance = Mathf.Max(Mathf.Abs(heldObject.HalfSize.z * 1.5f) + 0.25f, holdDistance);
            //Debug.Log(holdDistance);
            heldObject.GObject.transform.position = cam.transform.position + (dir * holdDistance);
            heldObject.GObject.transform.rotation = cam.transform.rotation;

        }
    }

    private void Interact()
    {
        RaycastHit hit = centerCamRay();
        if (hit.collider != null)
        {
            Debug.Log("Hit object: " + hit.collider.gameObject.name);
        }
        else
        {
            return;
        }
        if (hit.distance > interactRange)
            return;

        IInteractable iObj;
        hit.collider.gameObject.TryGetComponent<IInteractable>(out iObj);
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
        // Pickup Sound
        AudioManager.Instance.PlayOneShot(audPickupSound);
        heldObject = obj;
        return true;
    }

    public IHeldObject GetHeldObject()
    {
        return heldObject;
    }

    public void RemoveHeldObject()
    {
        heldObject = null;
    }

    private RaycastHit centerCamRay()
    {
        Vector3 rayD = cam.transform.forward;
        Vector3 rayPos = cam.transform.position;
        Ray ray = new Ray(rayPos, rayD);
        RaycastHit hit;

        Physics.Raycast(ray, out hit);

        return hit;


    }
}
