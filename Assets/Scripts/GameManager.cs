using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region SINGLETON
    public static GameManager instance;
    private void Awake()
    {
        instance = this;
    }
    #endregion SINGLETON

    #region VARS
    public static GameState State;
    public int totalPoints = 500;
    public int currentRound;
    public float timeBetweenActions = 2f;
    public Selector selector;
    public CommandManager commandManager;
    public CameraHandler cameraHandler;
    [Space]
    [SerializeField] GameObject shipBasePrefab;
    [SerializeField] Transform entityContainer;
    [Space]
    public FactionData[] factions;
    public List <ShipLogic> allShips = new List<ShipLogic>();
    public Player player_A;
    public Player player_B;
    public Player[] players;
    public bool canDeploy;

    public int selectedShipIndex;
    public int selectedModuleIndex;
    public int selectedTargetIndex;
    public static ShipLogic ActiveShip { get; private set;}
    public static ShipLogic TargetShip { get; private set;}
    public WeaponSlot ActiveWeapon {get{return ActiveShip.Weapons[selectedModuleIndex];}}
    #endregion VARS

    #region EVENTS
    public delegate void EmptyVoidDelegate();
    public delegate void EmptyIntDelegate(int amount);
    public delegate void EmptyVectorDelegate(Vector3 amount);
    public delegate void EmptyDamageDelegate(DamageData dd);
    public delegate void ShipDelegate(ShipLogic shipLogic);
    public delegate void ShipsDelegate(ShipLogic a, ShipLogic b);
    public delegate void GameStateDelegate(GameState state);
    public static event GameStateDelegate OnChangeState;
    public static event EmptyVoidDelegate OnTurnPassed;
    //public static event EmptyVoidDelegate OnTurnStarter;
    public static event EmptyIntDelegate OnChangeScore;
    public static event EmptyVoidDelegate OnGameStart;
    //public static event EmptyVoidDelegate OnBattleStart; 
    public static event ShipDelegate OnShipSelected; 
    public static event EmptyDamageDelegate OnShipTargeted; 
    public static event EmptyIntDelegate OnModuleSelected; 
    public static event EmptyVoidDelegate OnShipDeployed;    
    public static event EmptyVoidDelegate OnActionFinished; 
    #endregion EVENTS


    void Start()
    {
        UserInput.OnFireInput += OnFireInput;
        UserInput.OnCancelInput += OnCancelInput;
        UserInput.OnMoveInput += OnMoveInput;
        UserInput.OnArrowInput += OnArrowInput;
        UserInput.OnMoveClick += OnMouseMoveInput;
        UserInput.OnDeployClick += OnDeployShip;
        ShipMovement.OnShipStopsMoving += OnActionEnded;
        Invoke("TestStart", .2f); // DEBUG ONLY
    }

    public void TestStart()
    {        
        GameStart();
        SetGameState(GameState.PREP);
    }

    // TODO: refactorizar, atado con alambre para testear
    public void QuickPlayStart()
    {
        player_A.Fleet.shipsToDeploy = new List<ShipData>(player_A.factionData.premadeFleets[0].ships);
        player_B.Fleet.shipsToDeploy = new List<ShipData>(player_B.factionData.premadeFleets[0].ships);

        SpawnAllShips();
            SetActiveShip(allShips[0]);
        RandomizeStartingPlayer();
        SetGameState(GameState.DEPLOY);
        NextTurn();
                Debug.LogWarning("Quick start");
    }

    public void GameStart()
    {
       if(OnGameStart != null)
            OnGameStart();
    }

    void RandomizeStartingPlayer()
    {
        if(Random.value < .5f)
        {
            Player.ActivePlayer = player_A;
        }else
        {
            Player.ActivePlayer = player_B;
        }
    }

    void SetActiveShip(ShipLogic shipLogic)
    {
        if(shipLogic != null)
        {
            ActiveShip = shipLogic;
            if(State == GameState.DEPLOY)
                ActiveShip.SetState(ShipState.DEPLOY);
            if(OnShipSelected != null)
                OnShipSelected(ActiveShip);
        }else
        {
            Debug.LogWarning("No existe la nave a activar");
        }


    }

    void OnDeployShip(Vector3 deployPos, Quaternion deployRot)
    {
        if(!ActiveShip){ Debug.LogError("NO HAY NAVE PARA COLOCAR"); return;}
        if(!canDeploy) { return; }

        ShipLogic ship = ActiveShip;
        Player player = ship.Owner;
        player.Fleet.shipsInPlay.Add(ship);
        
        ship.SetState(ShipState.IDLE);
        ship.gameObject.SetActive(true);

        bool readyToBattle = true;
        foreach(ShipLogic _ship in allShips)
        {
            if(_ship.state == ShipState.WAIT_DEPLOY || _ship.state == ShipState.DEPLOY)
                readyToBattle = false;
        }

        Debug.Log("NAVE COLOCADA, pasando el turno");

        if(readyToBattle == true)
        {
            SetGameState(GameState.TACTIC);
        }

        NextTurn();
    }

    #region INPUT_RESPONSES
    void OnArrowInput(int i)
    {
        switch(State)
        {
            case GameState.DEPLOY:
                CycleActiveShip(i);
                break;
            case GameState.TACTIC:
                CycleActiveShip(i);
                break;
            case GameState.MODULE_SELECT:
                CycleSelectedModule(i);
            break;
            case GameState.TARGET_SELECT:
                CycleTargetShip(i);
            break;
        }
    }
    
    void OnCancelInput()
    {
        switch(State)
        {
            case GameState.DEPLOY:
                break;
            case GameState.TACTIC:
                if(ActiveShip.CanBeActivated)
                    HoldAction();
                break;
            case GameState.MOVE_POINT:
                SetGameState(GameState.TACTIC);
                break;
            case GameState.MODULE_SELECT:
                SetGameState(GameState.TACTIC);
                break;
            case GameState.TARGET_SELECT:
                SetGameState(GameState.MODULE_SELECT);
                break;
        }
    }

    void OnFireInput()
    {
        switch(State)
        {
            case GameState.DEPLOY:
                break;
            case GameState.TACTIC:
                if(ActiveShip.Owner != Player.ActivePlayer)
                    return;
                SetGameState(GameState.MODULE_SELECT);
                break;
            case GameState.MODULE_SELECT:
                if(ActiveShip.validTargets.Count > 0)
                    SetGameState(GameState.TARGET_SELECT);
                break;
            case GameState.TARGET_SELECT:
                SetGameState(GameState.SHOOTING);
                CommandManager.QueueCommand( new AttackCommand(ActiveShip, TargetShip) );
                commandManager.PlayCommands();
                break;
        }    
    }
    
    void OnMoveInput()
    {
        if(State == GameState.TACTIC)
        {
            // TODO enable movement action setup
            if(ActiveShip.canMove == false)
                return;
            SetGameState(GameState.MOVE_POINT);
            ActiveShip.SetState(ShipState.MOVING);
        }
    }
    
    void OnMouseMoveInput(Vector3 point, Quaternion rotation)
    {
        if(State != GameState.MOVE_POINT)
            return;
        if(ActiveShip.canMove)
        {
            MoveCommand moveCommand = new MoveCommand(ActiveShip.transform.position, point);
            CommandManager.QueueCommand(moveCommand);
            commandManager.PlayCommands();
            //ActiveShip.MoveOrder(point, rotation);
        }
    }
    
    void OnRotateInput()
    {
        
    }
    #endregion INPUT_RESPONSES

    #region  CYCLE_SELECTION
    void CycleActiveShip(int i)
    {
        ShipLogic nextShip = null;

        if(State == GameState.DEPLOY)
        {
            foreach(ShipLogic ship in allShips)
            {
                if(ship.state == ShipState.IDLE)
                {
                    continue;
                }else
                if(ship.state == ShipState.WAIT_DEPLOY || ship.state == ShipState.DEPLOY)
                    nextShip = ship;
            }
        }
        else
        if(State == GameState.TACTIC)
        {
            List<ShipLogic> elejibles = new List<ShipLogic>();
            foreach(ShipLogic s in allShips)
            {
                if(s.Owner == Player.ActivePlayer)
                {
                    if(s.CanBeActivated)
                        elejibles.Add(s);
                }     
            }

            // Ninguna nave del jugador puede actuar?
            if(elejibles.Count == 0)
            {
                Debug.LogWarning("El jugador actual no tiene naves capaces de actuar. Pasando turno.");
                NextTurn();
            }
            
            selectedShipIndex += i;
            if(selectedShipIndex < 0)
            {
                selectedShipIndex = allShips.Count-1;
            }else
            if(selectedShipIndex >= allShips.Count)
            {
                selectedShipIndex = 0;
            }

            //Debug.Log("Viendo numero " + selectedShipIndex);

            // LA NAVE SIGUIENTE NO PUEDE?
            if(!allShips[selectedShipIndex].CanBeActivated || allShips[selectedShipIndex].Owner != Player.ActivePlayer)
            {
                // Hay mas naves elejibles?
                if(elejibles.Count > 0)
                {
                    // dame la primera
                    //Debug.Log("Tomando nave por descarte");
                    nextShip = elejibles[0];
                    selectedShipIndex = allShips.IndexOf(nextShip);
                }
            }else
            {
                nextShip = allShips[selectedShipIndex];
            }

            // Algo salio mal, no hay nave disponible
            if(nextShip == null)
            {
                Debug.LogWarning("Nunca encontramos una nave disponible. Cancelando");
                return;
            }
        }
        SetActiveShip(nextShip);
    }

    void CycleSelectedModule(int i)
    {
        if(ActiveShip == null)
            return;

        if(ActiveShip.Slots.Weapons.Count <= 0)
        {
            Debug.LogWarning("No tiene armas");
            return;
        }
        
        if(ActiveShip.Slots.Modules.Count <= 0)
        {
            Debug.LogWarning("No tiene modulos genericos");
            return;
        }

        int totalModuleAmount = ActiveShip.Slots.Weapons.Count + ActiveShip.Slots.Modules.Count;
        if(totalModuleAmount <= 0) return;
        int nextModule = selectedModuleIndex + i;
        
        selectedModuleIndex += i;
        if(selectedModuleIndex >= totalModuleAmount)
        {
            selectedModuleIndex = 0;
        }
        else
        if(selectedModuleIndex < 0)
        {
             selectedModuleIndex = totalModuleAmount-1;
        }

        Debug.Log("Modulo " + selectedModuleIndex + " seleccionado");

        if(OnModuleSelected != null)
            OnModuleSelected(selectedModuleIndex);
    }
    
    void CycleTargetShip(int i)
    {
        selectedTargetIndex += i;
        if(selectedTargetIndex < 0)
        {
            selectedTargetIndex = ActiveShip.validTargets.Count-1;
        }else
        if(selectedTargetIndex >= ActiveShip.validTargets.Count)
        {
            selectedTargetIndex = 0;
        }
        for (int x = 0; x < allShips.Count; x++)
        {
            if(selectedTargetIndex == x)
                TargetShip = allShips[ ActiveShip.validTargets[selectedTargetIndex]]; 

        }

        TargetShip = allShips[ ActiveShip.validTargets[selectedTargetIndex]]; 

        DamageData dd = CalculatePotentialAttack(ActiveShip, TargetShip);

        if(OnShipTargeted != null)
            OnShipTargeted(dd);
    }

    #endregion CYCLE_SELECTION

    void HoldAction()
    {
        ActiveShip.GiveActionPoints(false);
        if(Player.ActivePlayer.CanActivateShips())
            CycleActiveShip(0);
        else
            NextTurn();
    }

    void OnActionEnded()
    {
        switch(State)
        {
            case GameState.MOVE_POINT:
                SetGameState(GameState.TACTIC);
                break;
        }
    }

    void SpawnAllShips()
    {
        foreach(Player p in players)
        {
            foreach(ShipData data in p.Fleet.shipsToDeploy)
            {
                GameObject go = Instantiate(shipBasePrefab);
                if (p == player_A)
                {
                    go.transform.rotation = Quaternion.Euler(0,90,0);
                }
                else
                {
                    go.transform.rotation = Quaternion.Euler(0, -90, 0);
                }
                go.transform.SetParent(entityContainer);
                ShipLogic logic = go.GetComponent<ShipLogic>();
                allShips.Add(logic);
                logic.state = ShipState.WAIT_DEPLOY;
                logic.LoadData(data, p); 
                go.SetActive(false);
            }
            p.Fleet.shipsToDeploy.Clear();
        }
    }

    public DamageData CalculateAttackData()
    {
        ShipLogic attacker = ActiveShip;
        ShipLogic defender = TargetShip;
        attacker.canAttack = false;

        DamageData dd = new DamageData(attacker, defender, ActiveWeapon);
        return dd;
    }

    // TODO: implementar el UI_AimScreen
    public DamageData CalculatePotentialAttack(ShipLogic attacker, ShipLogic defender)
    {
        DamageData dd = new DamageData(attacker, defender, ActiveWeapon);
        return dd;
    }

    public void NextTurn()
    {
        if(Player.ActivePlayer == player_A)
        {
            Player.ActivePlayer = player_B;
        }else
        {
            Player.ActivePlayer = player_A;
        }

        if(State == GameState.TACTIC)
        {
            currentRound++;
            Player.ActivePlayer.BeginBattleTurn();
        }

        if(State == GameState.DEPLOY)
            CycleActiveShip(0);
        if(State == GameState.TACTIC)
            CycleActiveShip(0);  

        if(OnTurnPassed != null)
            OnTurnPassed();
    }

    public void SetGameState(GameState newState)
    {
        GameState previousState = State;
        State = newState;

        switch (State){
            case GameState.TACTIC:
                CycleActiveShip(0);
                break;
            case GameState.DEPLOY:

                break;
            case GameState.MODULE_SELECT:
                CycleSelectedModule(0);
                break;
            case GameState.TARGET_SELECT:
                CycleTargetShip(0);
                break;
        }
              
        if(OnChangeState != null)
            OnChangeState(State);
    }

    public void SetDeployArea(bool estado)
    {
        if (State != GameState.DEPLOY)
            return;
        canDeploy = estado;
        ActiveShip.gameObject.SetActive(estado);
    }

    public void SetWait(float t)
    {
        SetGameState(GameState.WAIT);
        StartCoroutine("WaitRoutine", t);
    }

    IEnumerator WaitRoutine(float t)
    {
        Debug.Log("Esperando..." + t + "s");
        yield return new WaitForSeconds(t);
        CommandManager.CompleteCurrentCommand();
        SetGameState(GameState.TACTIC);
        yield return null;
    }
}
