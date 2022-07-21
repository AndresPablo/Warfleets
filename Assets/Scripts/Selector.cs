using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selector : MonoBehaviour
{
    /* GameManager gm {get {return GameManager.instance;}}

    public static SelectMode selectMode;
    //public static int ActiveShip { get; private set; }
    public static ShipLogic SelectedShip { get {return GameManager.ActiveShip; } }
    public static int ActiveWeapon { get; private set; }
    public static int TargetShip { get; private set; }
    public static int ShipToDeploy { get; private set; }

    public delegate void EmptyVoidDelegate();
    public delegate void ActionModeDelegate(SelectMode mode);
    public delegate void ShipDataDelegate(ShipData shipLogic);
    public delegate void ShipDelegate(ShipLogic shipLogic);
    public delegate void AttackDelegate(ShipLogic attacker, ShipLogic defender);
    public delegate void WeaponDelegate(WeaponSlot weapon);
    public static event WeaponDelegate OnWeaponSelected;    
    public static event ShipDelegate OnShipMoveState;    
    public static event ShipDelegate OnShipSelected;    
    public static event ShipDelegate OnSelectShipToDeploy;    
    public static event ShipDelegate OnShipTargeted;    
    public static event ActionModeDelegate OnModeChange; 
    public static event ShipDelegate OnHoldAction;   
    public static event AttackDelegate OnAttackAction; 
    public static event EmptyVoidDelegate OnCancelButton;
      

    void Start()
    {
        //GameManager.OnTurnPassed += CheckState;
        UI_Cursor.OnMoveOrder += SelectedShipEndedMovement;
        GameManager.OnAttackSequenceEnd += CheckState;
    }

    void SelectedShipEndedMovement(Vector3 p, Quaternion q)
    {
        ChangeSelectMode(SelectMode.ACTIVE_SHIP);
    }

    public void CycleShipSelect(int i)
    {
        ShipLogic nextShip = null;
        foreach(ShipLogic ship in gm.allShips)
        {
            if(ship.Owner != Player.ActivePlayer)
                continue;

            if(!ship.CanBeActivated)
                continue;

            nextShip = ship;
            if(OnShipSelected != null)
                OnShipSelected(nextShip);
        }
        

        /*ActiveShip += i;

        if(ActiveShip >= gm.allShips.Count)
        {
            ActiveShip = 0;
        }else
        if(ActiveShip < 0)
        {
            ActiveShip = gm.allShips.Count-1;
        }

        bool skipSelect = false;
        if(gm.allShips[ActiveShip].CanBeActivated == false)
            skipSelect = true;      

        if(gm.allShips[ActiveShip].Owner != Player.ActivePlayer)
            skipSelect = true;  

        if(skipSelect)
        {
            ActiveShip += i;
            CycleShipSelect(0);
            return;
        }
        //if(gm.allShips[ActiveShip].Owner != Player.ActivePlayer)
        //    CycleShipSelect(i);

        ShipLogic ship = gm.allShips[ActiveShip];

        if(OnShipSelected != null)
            OnShipSelected(ship);
    }

    public void CycleWeaponSelect(int i)
    {
        ActiveWeapon += i;
        if(GameManager.ActiveShip.Weapons.Length <= 0)
            {
                Debug.LogWarning("ESTA NAVE NO TIENE ARMAS");
                ChangeSelectMode(SelectMode.ACTIVE_SHIP);
                return;
            }
        if(ActiveWeapon < 0)
        {
            ActiveWeapon = GameManager.ActiveShip.Weapons.Length-1;
        }else
        if(ActiveWeapon >= GameManager.ActiveShip.Weapons.Length)
        {
            ActiveWeapon = 0;
        }

        if(OnWeaponSelected != null)
            OnWeaponSelected(GameManager.ActiveShip.Weapons[ActiveWeapon]);
    }

    public void CycleTargetSelect(int i)
    {
        TargetShip += i;
        ShipLogic activeShip  =GameManager.ActiveShip;

        if(TargetShip < 0)
        {
            TargetShip = activeShip.validTargets.Count;
        }else
        if(TargetShip > activeShip.validTargets.Count)
        {
            TargetShip = 0;
        }

        int targetShipNumber = activeShip.validTargets[i];
        ShipLogic targetShip = GameManager.instance.allShips[targetShipNumber];

        for (int j = 0; j < activeShip.validTargets.Count-1; j++)
        {
            
        }

        if(OnShipTargeted != null)
            OnShipTargeted(targetShip);
    }

    public void CycleDeploySelect(int i)
    {
        bool readyToBattle = true;
        foreach(ShipLogic _ship in gm.allShips)
        {
            if(_ship.state == ShipState.WAIT_DEPLOY || _ship.state == ShipState.DEPLOY)
                readyToBattle = false;
        }
        if(readyToBattle)
        {
            Debug.Log("Ya no hay naves disponibles");
            return;
        }

        ShipLogic shipScript = null;

        foreach(ShipLogic ship in gm.allShips)
        {
            if(ship.state == ShipState.IDLE)
            {
                continue;
            }else
            if(ship.state == ShipState.WAIT_DEPLOY || ship.state == ShipState.DEPLOY)
                shipScript = ship;
        }

        if(shipScript == null)
        {
            Debug.LogError("NO HAY NAVE APTA");
            return;
        }

        shipScript.gameObject.SetActive(true);
        shipScript.state = ShipState.DEPLOY;

        if(OnSelectShipToDeploy != null)
            OnSelectShipToDeploy(shipScript);
    }

    void GoBackToShipTactics()
    {
        if(GameManager.State == GameState.TACTIC)
        {
            ChangeSelectMode(SelectMode.ACTIVE_SHIP);
        }
    }

    void CheckState()
    {
        if(GameManager.State == GameState.DEPLOY)
        {
            ChangeSelectMode(SelectMode.ACTIVE_SHIP);
        }
        if(GameManager.State == GameState.TACTIC)
        {
            ChangeSelectMode(SelectMode.ACTIVE_SHIP);
        }
    }

    void HandleWeaponSelectListener()
    {
        // Si estamos en batalle eligiendo nave y tocamos el boton de ataque y la nave es del jugador activo
        // pasamos a seleccionar armas.

        if(GameManager.State == GameState.TACTIC)
        {
            if(selectMode == SelectMode.ACTIVE_SHIP)
            {
                if(SelectedShip.Owner == Player.ActivePlayer)
                {
                    if(!SelectedShip.canAttack)
                        return;
                    if(SelectedShip.validTargets.Count > 0)
                        ChangeSelectMode(SelectMode.ACTIVE_WEAPON);
                }
            }else
            if(selectMode == SelectMode.ACTIVE_WEAPON)
            {
                ChangeSelectMode(SelectMode.TARGET_SHIP);
            }else
            if(selectMode == SelectMode.TARGET_SHIP)
            {
                ShipLogic defender = GameManager.instance.allShips[GameManager.ActiveShip.validTargets[TargetShip]];
                if(OnAttackAction != null)
                    OnAttackAction(GameManager.ActiveShip, defender );
            }
        }
    }

    void ChangeSelectMode(SelectMode newMode)
    {
        selectMode = newMode;

        switch(selectMode)
        { 
            case SelectMode.ACTIVE_SHIP:
                if(GameManager.State == GameState.DEPLOY)
                {
                    CycleDeploySelect(0);
                }
                else
                if(GameManager.State == GameState.TACTIC)
                    CycleShipSelect(0);
            break;
            case SelectMode.ACTIVE_WEAPON:
                CycleWeaponSelect(0);
            break;
            case SelectMode.TARGET_SHIP:
                CycleTargetSelect(0);
            break;
        }

        if(OnModeChange != null)
            OnModeChange(selectMode);
    }

    void Update()
    {
        // ATACAR?
        if(Input.GetButtonDown("Accion"))
        {
            HandleWeaponSelectListener();
        }
    }*/
}
