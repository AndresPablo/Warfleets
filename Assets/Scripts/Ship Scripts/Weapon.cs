using UnityEngine;

[System.Serializable]
public class Weapon
{
    public string name = "Weapon";
    public WeaponType type;
    public Sprite icon;
    [Range(1,10f)]public float range = 2f;
    public int damage = 1;
    [Range(0,1f)]public float accuracyValue = .6f;
    public GameObject bulletPrefab;
    public GameObject fire_VFX;
    public AudioClip fire_SFX;
}
