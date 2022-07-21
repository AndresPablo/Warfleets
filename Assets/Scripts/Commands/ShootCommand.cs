using UnityEngine;

public class ShootCommand : Command
{
    DamageData damageData;
    bool complete;

    public ShootCommand(DamageData dd)
    {
        damageData = dd;
    }

    public override void Execute()
    {
        AttackDirector.instance.PlayShooting(damageData);
        // El comando se completa desde el attackDirector cuando ya no haya balas en escena
    }



    public override bool IsCompleted()
    {
                
        return complete;
    }

    public override void SetComplete()
    {
        complete = true;
    }
}
