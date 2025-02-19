using UnityEngine;

public class CircutPuzzleManager : MonoBehaviour
{
    [SerializeField] private LayerMask layerToIgnore;
    public void OnUpdate() //checking even when not playing, neeed to fix
    {
        if(Input.GetMouseButtonDown(0)){
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 100, ~layerToIgnore)) {
                WireHit(hit.collider.gameObject);
            }
        }
    }

    private void WireHit(GameObject hit){
        if(hit.TryGetComponent<IGrabbable>(out IGrabbable component)){
            component.OnGrabbed();
        }
    }
}
