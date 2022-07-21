using UnityEngine;

[CreateAssetMenu(menuName = "Warfleets/Faction")]
public class FactionData : ScriptableObject
{
    public Color color_1;
    public Color color_2;
    public Sprite icon;
    public ShipData[] shipList;
    public FleetTemplate[] premadeFleets;   // BUG estamo borrando de aca cuando hacemos deploy

}
