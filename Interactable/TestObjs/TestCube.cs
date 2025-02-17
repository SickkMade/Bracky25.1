using UnityEngine;

public class TestCube : MonoBehaviour, IInteractable
{
    MeshRenderer m_Renderer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_Renderer = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void OnInteract(IInteractor interactor)
    {
        if (interactor.HeldObject == null)
        {

            Vector3 newColor = Random.insideUnitSphere;
            m_Renderer.material.color = new Color(newColor.x, newColor.y, newColor.z);
        }
        else if(interactor.HeldObject.ID == "Sphere")
        {
            transform.localScale *= 2f;
            interactor.HeldObject.UseWithObject();
        }
    }
}
