using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class TestPickup : MonoBehaviour, IHeldObject
{
    [SerializeField]  float holdDistance;
    public float HoldDistance => holdDistance;

    [SerializeField] string id;
    public string ID => id;



    [SerializeField] private AudioClip sfHit;

    #region Dropping vars
    IInteractor holder;
    Rigidbody rb;
    Collider objCollider;
    private readonly float respawnY = -20;


    public GameObject GObject => gameObject;

    Vector3 objHalfSize;
    public Vector3 HalfSize => objHalfSize;

    private Animator animator;
    bool destroyOnUse => animator == null;

    private float curSoundCooldown = 0f;
    private float soundCooldown = 0.2f;
    #endregion

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        objCollider = GetComponent<Collider>();
        objHalfSize = objCollider.bounds.size / 2;
        TryGetComponent<Animator>(out animator);
    }

    // Update is called once per frame
    void Update()
    {
        curSoundCooldown -= Time.deltaTime;
        if(transform.position.y < respawnY)
        {
            transform.position = PlayerManager.Instance.playerData.position + Vector3.up;
            rb.linearVelocity = UnityEngine.Random.insideUnitSphere;
            rb.angularVelocity = Vector3.zero;
        }
    }

    public void UseWithObject()
    {
        holder.RemoveHeldObject();
        if (destroyOnUse)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            // Play Animation?
        }
    }

    public void Drop()
    {
        objCollider.enabled = true;
        rb.isKinematic = false;
        rb.angularVelocity = UnityEngine.Random.insideUnitSphere * 5;
        rb.linearVelocity = UnityEngine.Random.insideUnitSphere;
        holder = null;

    }

    public void OnInteract(IInteractor interactor)
    {
        if (interactor == null)
            return;

        // If the interactor can pick us up, do X
        if (interactor.PickupObject(this))
        {
            Debug.Log("Object Picked Up!");
            objCollider.enabled = false;
            holder = interactor;
        }
    }

    public void OnPickup()
    {

    }

    public void OnCollisionEnter(Collision collision)
    {
        if (curSoundCooldown < 0 && sfHit)
        {
            curSoundCooldown = soundCooldown;
            float volumeAdj = Mathf.Max(rb.linearVelocity.magnitude / 3, 0.1f);
            AudioManager.Instance.PlayOneShot(sfHit, transform.position, volumeAdj);
        }
    }
}
