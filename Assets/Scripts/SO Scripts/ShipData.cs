using UnityEngine;

[CreateAssetMenu(menuName = "Warfleets/Ship")]
public class ShipData : ScriptableObject
{
    public GameObject model_Prefab;
    public ShipClass shipClass;
    public int PointValue;
    public string[] names;
    [Space]
    [Range(0, 10f)] public float moveRange = 5f;
    [Range(1, 10)] public int hullPoints = 1;
    [Range(1, 10)] public int armorPoints = 1;
    [Range(1, 10)] public int evasionPoints = 1 ;
    [Range(1, 10)] public int AttackPoints = 1;
    [Space]
    public WeaponModuleData[] weaponsData;
    public ModuleData[] modulosData;
    [Space]
    public GameObject destroy_VFX;
    public SFX SFX;

    public float Size { get {return (float)shipClass;}}
}

[System.Serializable]
public struct SFX {
    public AudioClip selected;
    public AudioClip destroyed;
    public AudioClip attackOrder;
    public AudioClip takingDamage;
    public AudioClip shieldsHolding;
    public AudioClip shieldDamage;
}