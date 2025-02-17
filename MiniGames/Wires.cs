using UnityEngine;

public class Wires : MonoBehaviour
{
    public int connections; // Bitmask: 1 = up, 2 = right, 4 = down, 8 = left
    public bool isGenerator = false;

    public void SetConnections(int mask)
    {
        connections = mask;
        UpdateVisual();
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

    void UpdateVisual()
    {
        if(isGenerator){
            
        }
    }

    void OnMouseDown()
    {
        RotateTile();
    }
}
