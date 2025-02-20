using UnityEngine;

public class Wires : MonoBehaviour, IGrabbable
{
    public int connections; // Bitmask: 1 = up, 2 = right, 4 = down, 8 = left
    public bool isGenerator = false;

    private bool isActive = false;

    private Material material;
    private Color colorActive = Color.white;
    private Color colorInactive = Color.black;

    public void SetActivity(bool newActivity){
        isActive = newActivity;
        if(isActive){
            material.EnableKeyword("_EMISSION");
            material.SetColor("_EmissionColor", colorActive);
        }
        else{
            material.SetColor("_EmissionColor", colorInactive);
            material.DisableKeyword("_EMISSION");
        }

    }

    void OnEnable()
    {
        material = GetComponentInChildren<Renderer>().material;
    }

    public void SetConnections(int mask)
    {
        connections = mask;
        if(isGenerator){ //okay stfu its a jam :sob:
            int rotations = 0;
            if(connections == 2) rotations = 3;
            if(connections == 1) rotations = 2;
            if(connections == 8) rotations = 1;
            for(int i = 0; i < rotations; i++){
                RotateTile();
            }
        }
    }

    public void RotateTile()
    {
        connections = RotateMask(connections);
        transform.Rotate(0, 0, -90);
    }

    int RotateMask(int mask)
    {
        int up = (mask & 1) > 0 ? 1 : 0;
        int right = (mask & 2) > 0 ? 1 : 0;
        int down = (mask & 4) > 0 ? 1 : 0;
        int left = (mask & 8) > 0 ? 1 : 0;
        int newMask = 0;
        if (left == 1) newMask |= 1;
        if (up == 1) newMask |= 2;
        if (right == 1) newMask |= 4;
        if (down == 1) newMask |= 8;
        return newMask;
    }

    public void OnGrabbed()
    {
        if(isGenerator) return;
        RotateTile();
    }
}
