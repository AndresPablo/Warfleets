using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wait_Command : Command
{
    private bool finished;
    float maxTime;

    public Wait_Command(float _time)
    {
        maxTime = _time;
    }

    public override void Execute()
    {
        GameManager.instance.SetWait(maxTime);
    }

    public override bool IsCompleted()
    {
        if(finished)
            GameManager.instance.SetGameState(GameState.TACTIC);
        return finished;
    }

    public override void SetComplete()
    {
        finished = true;
    }
}
