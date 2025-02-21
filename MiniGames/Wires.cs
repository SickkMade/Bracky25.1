using System;
using UnityEngine;

public enum WireTypes{
    straight,
    curve,
    triple,
    cross,
    gen,
}
public class Wires : MonoBehaviour, IGrabbable
{
    public int gridX;
    public int gridY;
    public int connections;
    public bool isGenerator = false;
    public bool isActive = false;
    private Material material;
    private Color colorActive = Color.white;
    private Color colorInactive = Color.black;

    void OnEnable()
    {
        material = GetComponentInChildren<Renderer>().material;
    }

    public void SetConnections(int mask, WireTypes wireType)
    {
        connections = mask;
        //prerotate everything
        int rotations = 0;
        switch(wireType){ //LMAO THIS SWITCH STATEMENT I CANT
            case WireTypes.straight:
                if((mask & 2) > 0) rotations = 1; 
                break;
            case WireTypes.gen:
                rotations = (int)Math.Log(mask, 2);
                break;
            case WireTypes.curve:
                switch(mask){
                    case 3:
                        rotations = 1;
                        break;
                    case 6:
                        rotations = 2;
                        break;
                    case 12:
                        rotations = 3;
                        break;
                }
                break;
                case WireTypes.triple:
                switch(mask){
                    case 14:
                        rotations = 3;
                        break;
                    case 11:
                        rotations = 1;
                        break;
                    case 7:
                        rotations = 2;
                        break;
                }
                break;
        }
        // print($"x:{gridX}, y:{gridY}, rotations:{rotations}, wiretype:{wireType}, mask:{mask}");
        for(int i = 0; i < rotations; i++){
            transform.Rotate(0, 0, -90);
        }

    }

    public void RotateTile()
    {
        connections = RotateMask(connections);
        transform.Rotate(0, 0, -90);
        CheckNeighborActivity();
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
        if (!isGenerator) RotateTile();
    }

    public void SetActivity(bool newActivity)
    {
        isActive = newActivity;
        if (isActive)
        {
            material.EnableKeyword("_EMISSION");
            material.SetColor("_EmissionColor", colorActive);
        }
        else
        {
            material.SetColor("_EmissionColor", colorInactive);
            material.DisableKeyword("_EMISSION");
        }
    }

    public void CheckNeighborActivity()
    {
        if (isGenerator)
        {
            SetActivity(true);
        }
        bool wasActive = isActive;
        if (!isGenerator)
        {
            bool foundActiveNeighbor = false;
            if ((connections & 1) != 0)
            {
                Wires n = CircuitPuzzleGenerator.instance.GetWireAt(gridX, gridY+1);
                if (n && (n.connections & 4) != 0 && n.isActive) foundActiveNeighbor = true;
            }
            if ((connections & 2) != 0)
            {
                Wires n = CircuitPuzzleGenerator.instance.GetWireAt(gridX+1, gridY);
                if (n && (n.connections & 8) != 0 && n.isActive) foundActiveNeighbor = true;
            }
            if ((connections & 4) != 0)
            {
                Wires n = CircuitPuzzleGenerator.instance.GetWireAt(gridX, gridY-1);
                if (n && (n.connections & 1) != 0 && n.isActive) foundActiveNeighbor = true;
            }
            if ((connections & 8) != 0)
            {
                Wires n = CircuitPuzzleGenerator.instance.GetWireAt(gridX-1, gridY);
                if (n && (n.connections & 2) != 0 && n.isActive) foundActiveNeighbor = true;
            }
            SetActivity(foundActiveNeighbor);
        }
        if (!wasActive && isActive)
        {
            if ((connections & 1) != 0)
            {
                Wires n = CircuitPuzzleGenerator.instance.GetWireAt(gridX, gridY+1);
                if (n && (n.connections & 4) != 0) n.CheckNeighborActivity();
            }
            if ((connections & 2) != 0)
            {
                Wires n = CircuitPuzzleGenerator.instance.GetWireAt(gridX+1, gridY);
                if (n && (n.connections & 8) != 0) n.CheckNeighborActivity();
            }
            if ((connections & 4) != 0)
            {
                Wires n = CircuitPuzzleGenerator.instance.GetWireAt(gridX, gridY-1);
                if (n && (n.connections & 1) != 0) n.CheckNeighborActivity();
            }
            if ((connections & 8) != 0)
            {
                Wires n = CircuitPuzzleGenerator.instance.GetWireAt(gridX-1, gridY);
                if (n && (n.connections & 2) != 0) n.CheckNeighborActivity();
            }
        }
        CircuitPuzzleGenerator.instance.CheckWinCondition();
    }
}
