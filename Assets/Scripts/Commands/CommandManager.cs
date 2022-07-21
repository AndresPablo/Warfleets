using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandManager : MonoBehaviour
{
    public static List<Command> commands = new List<Command>();
    public static int currentCommandIndex = 0;
    public static Command CurrentCommand {get { return commands[currentCommandIndex];}}

    public string debug_currentName;
    public int debug_currentIndex;
    public int debug_totalCommands;
    #region EVENTS
    public delegate void EmptyVoidDelegate();
    public static event EmptyVoidDelegate OnQueueStop; 
    public static event EmptyVoidDelegate OnQueueStart; 
    #endregion EVENTS

    public void ExecuteCommand(Command command)
    {
        commands.Add(command);
        command.Execute();
        currentCommandIndex = commands.Count -1;
        debug_currentIndex = currentCommandIndex;
        debug_currentName = command.name;
        debug_totalCommands = commands.Count;
    }

    public static void QueueCommand(Command command)
    {
        commands.Add(command);
    }

    public void PlayCommands()
    {
        currentCommandIndex = 0;
        StartCoroutine("PlayCommandQueue");
        if(OnQueueStart != null)
            OnQueueStart();
    }

    IEnumerator PlayCommandQueue()
    {
        while(currentCommandIndex != commands.Count)
        {
            commands[currentCommandIndex].Execute();
            while(!CurrentCommand.IsCompleted())
            {
                yield return new WaitForSeconds(.002f);
            }
            //Debug.Log("CMD: COMANDO " + currentCommandIndex + " COMPLETADO");
            currentCommandIndex++;
        }
        //Debug.Log("CMD: COLA COMPLETADA");

        if(OnQueueStop != null)
            OnQueueStop();
        DeleteQueue();
    }

    public void DeleteQueue()
    {
        foreach(Command c in commands)
        {
            //Destroy(c);
        }
        commands.Clear();
        currentCommandIndex = 0;
    }

    public static void CompleteCurrentCommand()
    {
        CurrentCommand.SetComplete();
    }
}
