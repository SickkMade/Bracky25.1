using Unity.VisualScripting;
using UnityEngine;

public class AirFilter : TestPickup
{
    [SerializeField] Material goodMat;
    [SerializeField] Material badMat;

    [SerializeField] MeshRenderer mesh;
    private bool _isGood = true;
    public bool IsGood
    {
        get { return _isGood; }
        set
        {
            _isGood = value;
            if (value)
            {
                mesh.material = goodMat;
                updateId("GoodFilter");
            }
            else
            {
                mesh.material = badMat;
                updateId("BadFilter");
            }
        }
    }
}
