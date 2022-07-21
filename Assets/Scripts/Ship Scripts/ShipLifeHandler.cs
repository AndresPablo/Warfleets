using UnityEngine;

public class ShipLifeHandler : MonoBehaviour
{
    public int maxHealth;
    public int currentHealth;
    public int armor;
    GameObject dieVFX;
    ShipLogic logic;

    #region EVENTS
    public delegate void EmptyVoidDelegate();
    public delegate void EmptyIntDelegate(int amount);
    public delegate void EmptyDamageDelegate(DamageData dd);
    public delegate void ShipDelegate(ShipLogic shipLogic);
    //public static event EmptyVoidDelegate OnTurnPassed;  
    public static event EmptyDamageDelegate OnShipDamaged;  
    public static event EmptyIntDelegate OnDamageToShip;
    public static event ShipDelegate OnShipDestroyed;
    public static event EmptyVoidDelegate OnAnyShipDestroyed;

    #endregion EVENTS

    public bool IsDestroyed { get { if( currentHealth <= 0) return true; else return false;}}


    void Start()
    {
        
    }

    public void LoadData(ShipLogic _logic, ShipData data)
    {
        this.logic = _logic;
        maxHealth = data.hullPoints;
        currentHealth = maxHealth;
        armor = data.armorPoints;
        dieVFX = data.destroy_VFX;
    }

    public void TakeDamage(DamageData dd)
    {
        currentHealth -= dd.damage;

        if(OnDamageToShip != null)
            OnDamageToShip(currentHealth);
        if (OnShipDamaged != null)
            OnShipDamaged(dd);
        if (currentHealth <= 0)
            Die();
    }

    void Die()
    {
        logic.SetState(ShipState.DEAD);
        gameObject.SetActive(false);

        GameObject destroyVFX_GO = Instantiate(dieVFX, logic.ModelTrans.position, Quaternion.identity);


        if (OnShipDestroyed != null)
            OnShipDestroyed(logic);
        if (OnAnyShipDestroyed != null)
            OnAnyShipDestroyed();
    }


}
