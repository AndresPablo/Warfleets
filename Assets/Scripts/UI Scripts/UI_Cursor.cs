using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class UI_Cursor : MonoBehaviour
{
    Camera cam;
    GameState state;
    [SerializeField] UserInput uInput;
    [SerializeField] Canvas canvas;
    [SerializeField] LineRenderer line;
    [Space]
    [SerializeField] Image sizeRing_img;
    [SerializeField] Image sizeFill_img;
    [SerializeField] Image moveSpot_Image;
    //[SerializeField] Image moveArrow_Image;
    [SerializeField] Image occupied_Image;
    Vector3 originalScale;
    bool deployMode;
    [SerializeField]bool canDeploy;


    void Start()
    {
        cam = Camera.main;   
        GameManager.OnChangeState += SetState;
        GameManager.OnShipSelected += SelectShip;
        GameManager.OnChangeState += ChangePointerState;
        UserInput.OnMoveClick += HideWaypointCursor;
        UserInput.OnCursorSpaceFree += DisplayOccupiedSpace;
        originalScale = transform.localScale;
    }

    void SelectShip(ShipLogic shipLogic)
    {
        Color newColor = sizeRing_img.color;
        newColor.a = .5f;
        sizeRing_img.color = newColor;        
        sizeRing_img.rectTransform.localScale = (Vector3.one * shipLogic.Size);
    }

    void ChangePointerState(GameState state)
    {
        if(state == GameState.MOVE_POINT || state == GameState.DEPLOY)
        {
            ToogleCursor(true);
        }
        else
        {
            ToogleCursor(false);
        }
    }

    void HandleDeploy()
    {
        if(canDeploy)
        {
            DisplayPosition();
        }
    }


    void HideWaypointCursor(Vector3 pos, Quaternion q)
    {
        ToogleCursor(false);
        line.enabled = false;
    }

    void HandleMovement()
    {
        if(!GameManager.ActiveShip.canMove)
            return;
        if(uInput.DrawLine && !uInput.PointOutOfRange)
        {
            line.SetPosition(0, GameManager.ActiveShip.transform.position);
            line.SetPosition(1, uInput.MouseHitPosition);
        }
        line.enabled = canvas.enabled= !uInput.PointOutOfRange;
        
    }

    void Update()
    {
        line.enabled = uInput.DrawLine;
        
        if(state == GameState.DEPLOY)
            HandleDeploy();
        else
        if(state == GameState.MOVE_POINT){
            HandleMovement();
            DisplayPosition();
        }    
    }

    void DisplayOccupiedSpace(bool state)
    {
        occupied_Image.enabled = !state;
        moveSpot_Image.enabled = state;
        if(state == false)
            sizeFill_img.color = Color.white;
        else
            sizeFill_img.color = Color.black;
    }

    void DisplayPosition()
    {
        transform.position = uInput.MouseHitPosition;
        if(deployMode)
        {
            if (GameManager.ActiveShip)
                transform.rotation = GameManager.ActiveShip.transform.rotation * uInput.CursorRotation;
        }
        else transform.rotation =  uInput.CursorRotation;
    }

    void SetState(GameState _state)
    {
        state = _state;

        if(state == GameState.MOVE_POINT)
        {
            ToogleCursor(true);
            moveSpot_Image.enabled = true;
            sizeFill_img.enabled = false;
        }
        else
        if(state == GameState.DEPLOY)
        {
            ToogleCursor(true);
            occupied_Image.enabled = false;
            moveSpot_Image.enabled = false;
            sizeFill_img.enabled = true;
            line.enabled = false;
        }
    }

    void ToogleCursor()
    {
        ToogleCursor(!canvas.enabled);
    }

    void ToogleCursor(bool state)
    {
        line.enabled = state;
        canvas.enabled = state;
    }
}
