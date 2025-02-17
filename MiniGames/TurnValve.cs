using UnityEngine;

public class TurnValve : MonoBehaviour
{

    private bool isActive = false;
    private IGrabbable crank = null;
    [SerializeField] private LayerMask layerToIgnore;

    void Start()
    {
        MiniGameScript.CallMiniGame += OnUpdate;
    }
    void OnUpdate() //checking even when not playing, neeed to fix
    {
        if(Input.GetMouseButtonDown(0)){
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 100, ~layerToIgnore)) {
                CrankHit(hit.collider.gameObject);
            }
        }
        else if(Input.GetMouseButtonUp(0)){
            crank = null;
        }

        crank?.OnGrabbed();
    }

    private void CrankHit(GameObject hit){
        if(hit.TryGetComponent<IGrabbable>(out IGrabbable component)){
            crank = component;
        }
    }
}
