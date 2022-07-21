
using UnityEngine;

[CreateAssetMenu(menuName = "Warfleets/Weapon Module")]
public class WeaponModuleData : ModuleData
{ 
    public int damage = 1;
    [Range(0,1f)]public float accuracyValue = .6f;
    //public WeaponType wType;
    [Space]
    [Range(1,10)]public int gunsAmount = 1;
    public int volleyAmount = 1;
    [Range(0.05f,.5f)]public float fireRate = .3f;
    public float originSpread = .1f;
    public float targetSpread = .1f;
    [Space]
    public GameObject bulletPrefab;
    public GameObject fire_VFX;
    public AudioClip fire_SFX;

    public override void Activate()
    {
        
    }
}
