using System.Collections.Generic;
using UnityEngine;

public class TextRepo : MonoBehaviour
{
    [SerializeField] Dictionary<Room, string> flavorText;
    [SerializeField] Dictionary<Room, string> descText;


    public string getFlavorText(Room room)
    {
        return flavorText[room];
    }

    public string getDescription(Room room)
    {
        return descText[room];
    }
}

public enum Room
{
    Turbine,
    BoilerHouse,
    Assembly,
    WaterTreatment,
    Tokomak,
    PumpHouse,
    General,
    None
}
