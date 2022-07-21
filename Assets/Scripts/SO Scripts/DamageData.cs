using UnityEngine;

public class DamageData
{
    public ShipLogic attacker;
    public ShipLogic defender;
    public int damage;
    public float TotalHitChance { get {return baseHitMod + bonusHitMod - evadeHitMod; }}
    public float evadeHitMod { get; private set; }
    public float baseHitMod { get; private set; }
    public float bonusHitMod { get; private set; }

    WeaponSlot weaponSlot;

    public bool isMiss;
    public bool isHit;
    public bool isBlocked;
    public bool isCrit;


    public DamageData(ShipLogic _attacker, ShipLogic _defender, WeaponSlot weaponModule)
    {
        attacker = _attacker;
        defender = _defender;
        baseHitMod = weaponModule.accuracy;
        evadeHitMod = defender.evasionValue/100;
        damage = weaponModule.damage;
        weaponSlot = weaponModule;

        CalculateHitChance();
        // TODO bonus
    }

    public void CalculateHitChance()
    {
        float value = Random.value;
        
        if(value < TotalHitChance)
        {
            isHit = true;
            isMiss = false;
        }else

        {
            isHit = false;
            isMiss = true;
        }
    }
}
