using UnityEngine;

public class DeployArea : MonoBehaviour
{
    [SerializeField] Player myPlayer;
    [SerializeField] string cursorTag = "Cursor";

    private void Start()
    {
        GameManager.OnTurnPassed += CheckTurn;
    }

    void CheckTurn()
    {
        if (GameManager.State != GameState.DEPLOY)
            return;
        if(Player.ActivePlayer == myPlayer)
        {

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(cursorTag))
        {

            if (GameManager.ActiveShip.Owner == myPlayer)
                GameManager.instance.SetDeployArea(true);

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(cursorTag))
        {
            GameManager.instance.SetDeployArea(false);
        }
    }


}
