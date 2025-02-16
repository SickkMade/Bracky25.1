using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class TestPickup : MonoBehaviour, IHeldObject
{
    [SerializeField] float holdDistance;
    float physTimeAfterGrounded = 2f;
    bool isDropped;
    Rigidbody rb;
    private float lastYPos;
    private bool grounded;
    private float countToLock = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isDropped)
        {
            if (grounded)
            {
                countToLock = physTimeAfterGrounded;
                isDropped = false;
            }
            else
            {
                grounded = (lastYPos == transform.position.y);
            }
            
        }
        else if(rb.isKinematic == false)
        {
            countToLock -=Time.deltaTime;
            if(countToLock < 0)
            {
                rb.isKinematic = true;
            }
        }
    }

    public void DoInteraction()
    {
        throw new System.NotImplementedException();
    }

    public void Drop()
    {
        isDropped = true;
        rb.isKinematic = false;
        lastYPos = transform.position.y;
    }

    public void OnInteract(IInteractor interactor)
    {
        if (interactor == null)
            return;

        // If the interactor can pick us up, do X
        if (interactor.PickupObject(this))
        {
            // Something?
        }
    }

    public void OnPickup()
    {
        return;
    }

    public float getHoldDistance()
    {
        throw new System.NotImplementedException();
    }

    public GameObject getObject()
    {
        throw new System.NotImplementedException();
    }
}
