using UnityEngine;
using System.Collections.Generic;

public class UserInput : MonoBehaviour
{
    [SerializeField] LayerMask tableLayer, shipLayer;
    [SerializeField] bool waypointCursorMode;
    [SerializeField] bool allowInput = true;
    public List<ShipLogic> overlayedShips = new List<ShipLogic>();
    public Vector3 waypointExtraRotation;
    public bool moveMode;

    #region EVENTS
    public delegate void EmptyVoidDelegate();
    public delegate void EmptyIntDelegate(int amount);
    public static event EmptyVoidDelegate OnFireInput;
    public static event EmptyVoidDelegate OnMoveInput; 
    public static event EmptyVoidDelegate OnCancelInput; 
    public static event EmptyIntDelegate OnArrowInput; 
  
    public delegate void BoolDelegate(bool state);
    public delegate void ClickPositionDelegate(Vector3 cursorPosition);
    public delegate void MouseInShipSpaceDelegate(ShipLogic otherShip);
    public delegate void WaypointDelegate(Vector3 cursorPosition, Quaternion rotation);
    public delegate void RotationDelegate(Quaternion rotation);
    public static event BoolDelegate OnCursorSpaceFree;
    public static event MouseInShipSpaceDelegate OnCursorEnterShipSpace;
    public static event MouseInShipSpaceDelegate OnCursorExitShipSpace;
    public static event WaypointDelegate OnDeployClick;
    public static event WaypointDelegate OnMoveClick;
    #endregion EVENTOS

    public bool ValidMovePoint  { get; private set; }
    public bool PointOutOfRange  { get; private set; }
    public bool DrawLine  { get; private set; }
    public bool SpaceOccupied  { get; private set; }
    public Vector3 MouseHitPosition { get; private set; }
    public Quaternion CursorRotation { get; private set; }


    void Start()
    {
        GameManager.OnChangeState += CheckState;
    }

    void HandleMousePosition()
    {
        if(!waypointCursorMode) return;

        Vector3 mousePos = Input.mousePosition;
        Ray mouseRay = Camera.main.ScreenPointToRay(mousePos);
        RaycastHit hit = new RaycastHit();

        if(Physics.Raycast(mouseRay, out hit, Mathf.Infinity, tableLayer))
        {
            if(moveMode)
            {
                Transform shipTrans =GameManager.ActiveShip.transform;
                // Si esta a a distancia
                float dist = Vector3.Distance(hit.point, shipTrans.position);
                float maxDist = GameManager.ActiveShip.MaxMoveValue;
                if(dist <= maxDist)
                {
                    // Si el punto esta dentro del arco
                    Vector3 v1 = shipTrans.right.normalized;
                    Vector3 v2 = (-shipTrans.right).normalized;
                    float angleArea = Vector3.Angle(v1, v2);
                    if (Vector3.Angle(v1, hit.point) < angleArea && Vector3.Angle(v2,hit.point) < angleArea)
                    {
                        //Debug.Log(angleArea+ " /" + Vector3.Angle(v1, hit.point) + " "  + Vector3.Angle(v2,hit.point));

                        // Ver si el espacio esta ocupado por otra nave
                        PointOutOfRange = false; 
                        ValidMovePoint = true;
                        MouseHitPosition = hit.point; 
                        CheckOccupiedSpace();
                    }else
                        PointOutOfRange = true; 
                }else
                        PointOutOfRange = true; 
            }
        }

        if(Input.GetMouseButtonDown(0) && !PointOutOfRange && ValidMovePoint)
        {
                moveMode = false;
                waypointCursorMode = false;
                DrawLine = false; 
            if(OnMoveClick != null)
                OnMoveClick(MouseHitPosition, CursorRotation);
        }
    }

    void HandleWaypointRotation()
    {
        Vector2 magnitude = Input.mouseScrollDelta;
        Vector3 newRotation;
        if(magnitude.magnitude != 0)
        {
            float newAngle = waypointExtraRotation.y + 10;
            if(newAngle > 360)
                newAngle = 0;
            newRotation = new Vector3(
                0,
                newAngle,
                0
            );
            waypointExtraRotation = newRotation;
        }

        Vector3 shipPos = GameManager.ActiveShip.transform.position;
        Vector3 direction = MouseHitPosition - shipPos;

        Quaternion newQuat= Quaternion.LookRotation(-direction); 
        //newQuat.Set(0, newQuat.y + waypointExtraRotation.y, 0, 1); 
        //if(waypointExtraRotation.y != 0) newQuat.Set(0, newQuat.y + waypointExtraRotation.y, 0, 1); 
            //newQuat = Quaternion.Euler(newQuat.x, newQuat.y+ waypointExtraRotation.y, newQuat.z);
        CursorRotation = newQuat;
    }

    void HandleDeployPositionRotation()
    {
        Vector3 mousePos = Input.mousePosition;
        Ray mouseRay = Camera.main.ScreenPointToRay(mousePos);
        RaycastHit hit = new RaycastHit();

        if(Physics.Raycast(mouseRay, out hit, Mathf.Infinity, tableLayer))
        {
            MouseHitPosition = hit.point + (Vector3.up * .01f);

            if(GameManager.ActiveShip == null)
            {
                Debug.LogWarning("No active ship!");
                return;
            }

             if(GameManager.ActiveShip.state == ShipState.DEPLOY)
            {
                GameManager.ActiveShip.transform.position = MouseHitPosition;

                Vector2 magnitude = Input.mouseScrollDelta;
                if(magnitude.magnitude != 0)
                {
                    Vector3 newRotation = new Vector3(
                        0,
                        waypointExtraRotation.y + (magnitude.y *10),
                        0
                    );
                    waypointExtraRotation = newRotation;
                    CursorRotation = Quaternion.Euler(waypointExtraRotation);
                    GameManager.ActiveShip.transform.rotation = CursorRotation;
                }
            }

            if(Input.GetMouseButtonUp(0)){

                if(OnDeployClick != null)
                    OnDeployClick(MouseHitPosition, CursorRotation );
            }
        }
    }

    void Update()
    {
        if(!allowInput)
            return;

        HandleMousePosition();

        if(GameManager.State == GameState.MOVE_POINT)
            HandleWaypointRotation();
        if(GameManager.State == GameState.DEPLOY)
            HandleDeployPositionRotation();
            
        // ATACAR?
        if(Input.GetButtonDown("Accion"))
        {
            if(OnFireInput != null)
                OnFireInput();  
        }

        // MOVER?
        if(Input.GetButtonDown("Mover"))
        {
            if(OnMoveInput != null)
                OnMoveInput();  
        }

        // ATRAS
        if(Input.GetButtonDown("Atras"))
        {
            if(OnCancelInput != null)
                OnCancelInput();
        }

        if(Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if(OnArrowInput != null)
                OnArrowInput(-1);
        }

        if(Input.GetKeyDown(KeyCode.RightArrow))
        {            
            if(OnArrowInput != null)
                OnArrowInput(+1);
        }

    }

    void CheckState(GameState gameState)
    {
        if(gameState == GameState.DEPLOY)
        {
            moveMode = false;
            waypointCursorMode = false;
            DrawLine = false;
        }else
        if(gameState == GameState.MOVE_POINT)
        {
            moveMode = true;
            waypointCursorMode = true;
            DrawLine = true; 
        }else
        {
            moveMode = false;
            waypointCursorMode = false;
            DrawLine = false; 
        }
    }

    void CheckOccupiedSpace()
    {
        ShipLogic activeShip = GameManager.ActiveShip;

        foreach(ShipLogic s in GameManager.instance.allShips)
        {
            if(Vector3.Distance(s.transform.position, MouseHitPosition) < (s.Size/2 + activeShip.Size/2) )
            {
                if(s != GameManager.ActiveShip)
                {
                    if(!overlayedShips.Contains(s))
                        overlayedShips.Add(s);
                    if(OnCursorEnterShipSpace != null)
                        OnCursorEnterShipSpace(s);
                    if(OnCursorSpaceFree != null)
                        OnCursorSpaceFree(false);
                }
            }else
            {
                if(overlayedShips.Contains(s))
                {
                    overlayedShips.Remove(s);
                    if(OnCursorExitShipSpace != null)
                        OnCursorExitShipSpace(s);
                }
            }
        }

        if(overlayedShips.Count <= 0)
        {
            SpaceOccupied = false;
            if(OnCursorSpaceFree != null)
                OnCursorSpaceFree(true);
        }else
        {
            ValidMovePoint = false;
            SpaceOccupied = true;
        }
    } 

}
