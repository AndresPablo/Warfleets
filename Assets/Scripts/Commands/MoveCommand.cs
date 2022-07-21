using UnityEngine;

public class MoveCommand : Command
{
    public Vector3 targetPos, originPos;
    private GameObject actor;


    public MoveCommand(Vector3 _originPos, Vector3 _targetPos)
    {
        originPos = _originPos;
        targetPos = _targetPos;
    }

    public override void Execute()
    {
        //GameManager.ActiveShip.transform.position = targetPos;
        GameManager.ActiveShip.MoveOrder(originPos, targetPos);
    }


    public override bool IsCompleted()
    {
        bool complete = Vector3.Distance(GameManager.ActiveShip.transform.position, targetPos) < 0.01f;
        if(complete == true)
        {
            GameManager.ActiveShip.canMove = false;
        }
        return complete;
    }

    public override void SetComplete()
    {
    }
}
