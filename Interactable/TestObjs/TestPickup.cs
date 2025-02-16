using System;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class TestPickup : MonoBehaviour, IHeldObject
{
    [SerializeField] float holdDistance;

    #region Dropping vars
    float physTimeAfterGrounded = 2f;
    bool isDropped;
    bool isHeld = false;
    Rigidbody rb;
    Collider[] objColliders;
    private float lastYPos;
    private bool grounded;
    private float countToLock = 0;
    private bool lockPos = false;
    private float timeOutTime = 5.0f;
    private float countToRespawn = 5.0f;
    #endregion

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        objColliders = GetComponents<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        #region Dropping Code
        if (!isHeld)
        {

            grounded = (lastYPos == transform.position.y);
            if (grounded)
            {
                countToRespawn = timeOutTime;
            }
            else
            {
                countToRespawn -= Time.deltaTime;
                if (countToRespawn < 0)
                {
                    transform.position = PlayerManager.Instance.playerData.position + Vector3.up;
                    Drop();
                }
            }

            if (isDropped)
            {
                if (grounded)
                {
                    isDropped = false;
                    countToLock = physTimeAfterGrounded;
                    lockPos = false;
                }

            }
            else if (!lockPos)
            {
                countToLock -= Time.deltaTime;
                if (countToLock < 0)
                {
                    rb.angularVelocity = Vector3.zero;
                    rb.linearVelocity = new Vector3(0, rb.linearVelocity.y, 0);
                    lockPos = true;

                }
            }
        }
            #endregion
        }

        public void DoInteraction()
    {
        throw new System.NotImplementedException();
    }

    public void Drop()
    {
        isHeld = false;
        foreach (Collider c in objColliders)
        {
            c.enabled = true;
        }
        // Prevent player from dropping objects in other objects / out of the map.
        isDropped = true;
        rb.isKinematic = false;
        lastYPos = transform.position.y;
        lockPos = false;
    }

    public void OnInteract(IInteractor interactor)
    {
        if (interactor == null)
            return;

        // If the interactor can pick us up, do X
        if (interactor.PickupObject(this))
        {
            Debug.Log("Object Picked Up!");
            foreach (Collider c in objColliders)
            {
                c.enabled = false;
            }
            isHeld = true;
            countToRespawn = timeOutTime;
        }
    }

    public void OnPickup()
    { 

    }

    public float getHoldDistance()
    {
        return holdDistance;
    }

    public GameObject getObject()
    {
        return gameObject;
    }
}
