using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player ActivePlayer;

    public GameObject ship_prefab;
    public FactionData factionData;
    public Fleet Fleet;

    #region  EVENTOS
    public delegate void PlayerDelegate(Player _player);
    public delegate void FactionSelectDelegate(FactionData _faction);
    public static event FactionSelectDelegate OnFactionChosen; 
    public static event PlayerDelegate OnTurnStart; 
    #endregion EVENTOS


    public void BeginBattleTurn()
    {
        ActivePlayer = this;

        foreach(ShipLogic ship in Fleet.shipsInPlay)
        {
            ship.GiveActionPoints(true);
        }
        if(OnTurnStart != null)
            OnTurnStart(Player.ActivePlayer);
    }

    public void SelectFaction(FactionData _data)
    {
        factionData = _data;

        if(OnFactionChosen != null)
            OnFactionChosen(factionData);
    }

    public bool CanActivateShips()
    {
        bool resultado = false;
        foreach(ShipLogic ship in Fleet.shipsInPlay)
        {
            if(ship.CanBeActivated)
                resultado = true;
        }
        return resultado;
    }

}
