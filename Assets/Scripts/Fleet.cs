using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Fleet 
{
    public string fleetName;
    public List<ShipLogic> shipsInPlay = new List<ShipLogic>(); 
    public List<ShipData> shipsToDeploy = new List<ShipData>(); 

    public int TotalPointCost { get {
        int total = 0;
        foreach(ShipData data in shipsToDeploy)
        {
            total += data.PointValue;
        }
        return total;
      }
    }

    
}
