using UnityEngine;

public class AttackCommand : Command
{
    ShipLogic attacker;
    ShipLogic defender;
    bool complete;

    public AttackCommand(ShipLogic _attacker, ShipLogic _defender)
    {
        attacker = _attacker;
        defender = _defender;
    }

    public override void Execute()
    {
        //GameManager.instance.cameraHandler.LookAtAttackerShip(attacker, defender);
        DamageData dd = GameManager.instance.CalculateAttackData();
        CommandManager.QueueCommand( new ShootCommand(dd) );
        CommandManager.QueueCommand( new DealDamage_Command(dd) );
        CommandManager.QueueCommand( new Wait_Command(2f));
        SetComplete();
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
