using UnityEngine;

public class DealDamage_Command : Command
{
    private DamageData data;
    private bool finished;

    public delegate void EmptyVectorDelegate(Vector3 point);
    public static event EmptyVectorDelegate OnShipEvades;


    public DealDamage_Command(DamageData _dd)
    {
        data = _dd;
    }

    public override void Execute()
    {
        if(data.isHit)
        {
            data.defender.LifeHandler.TakeDamage(data);
            //Debug.Log("Enemigo dañado " + (int)(hitRoll*100) +"%");
        }else
        {
            if(OnShipEvades != null)
                OnShipEvades(data.defender.ModelTrans.position);
            //Debug.Log("Enemigo evade!! " + (int)(hitRoll*100) +"%");
        }
        SetComplete();
    }

    public override bool IsCompleted()
    {
        return finished;
    }

    public override void SetComplete()
    {
        finished = true;
    }
}
