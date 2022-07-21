using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FleetTemplate
{
    public string Name;
    public List<ShipData> ships = new List<ShipData>(); 

    public int TotalPointCost { get {
        int total = 0;
        foreach(ShipData data in ships)
        {
            total += data.PointValue;
        }
        return total;
      }
    }
   
}
