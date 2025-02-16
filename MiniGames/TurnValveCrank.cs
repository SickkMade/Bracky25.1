using UnityEngine;

public class TurnValveCrank : MonoBehaviour, IGrabbable
{
    private Vector2 lastMousePosition = new();
    [SerializeField] private float rotationSpeed = 5;
    public void OnGrabbed()
    {
        if(lastMousePosition == Vector2.zero) lastMousePosition = Input.mousePosition;

        Vector2 newMousePosition = Input.mousePosition;
        float mouseDistance = (newMousePosition - lastMousePosition).magnitude;
        lastMousePosition = newMousePosition;

        this.transform.eulerAngles = new Vector3(
        this.transform.eulerAngles.x,
        this.transform.eulerAngles.y,
        this.transform.eulerAngles.z - (mouseDistance * Time.deltaTime * rotationSpeed)
        );
    }
}
