using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipLogic : MonoBehaviour
{
    public string mName;
    public ShipClass Type;
    public ShipState state;
    public int pointValue;
    public int evasionValue = 1 ;
    public int PowerPoints = 1;
    public ShipSlots Slots;
    //public Weapon[] weapons;
    [SerializeField] Transform modelContainer;
    [SerializeField] SphereCollider selectCollider;
    [SerializeField] ShipVisual visual;
    [SerializeField] ShipLifeHandler Health;
    [SerializeField] ShipMovement Movement;
    public List<int> validTargets;
    public bool canMove;
    public bool canAttack;
    ShipData data;


    #region PROPIEDADES
    public Player Owner { get; private set;}
    public Transform ModelTrans {get { return modelContainer;}}
    public int CurrentHealthPoints {get { return Health.currentHealth;}}
    public int MaxHealthPoints {get { return Health.maxHealth;}}
    public float MaxMoveValue {get { return Movement.moveRange;}}
    public int armorValue {get { return Health.armor;}}
    public FactionData Faction {get { return Owner.factionData;}}
    public float Size {
        get{
            if(Type == ShipClass.Squadron || Type == ShipClass.Frigate)
                return 1f;
            else return 2f;
        }
    }
    public bool CanBeActivated {
        get{
            if(!canMove && !canAttack)
                return false;
            if(Health.IsDestroyed)
                return false;
            return true;
        }
    }
    public WeaponSlot[] Weapons { get {return Slots.Weapons.ToArray();}}
    public ShipLifeHandler LifeHandler {get {return Health;}}
    #endregion PROPIEDADES


    void Start()
    {
        visual = GetComponent<ShipVisual>();
        Health = GetComponent<ShipLifeHandler>();
        Movement = GetComponent<ShipMovement>();
        visual = GetComponent<ShipVisual>();
        GameManager.OnShipSelected += OnSelected;
        Movement.OnShipStopped += SetMoveMode;
        ShipLifeHandler.OnAnyShipDestroyed += OnAnyShipDestroyed;
    }

    private void OnAnyShipDestroyed()
    {
        CalculateTargetsInRange(transform.position);
    }

    public void LoadData(ShipData _data, Player _owner)
    {
        if(_data == null | _owner == null)
        {
            Debug.LogError("No hay datos ni dieño");
            return;
        }

        data = _data;
        Owner = _owner;

        mName = name = data.names[Random.Range(0, data.names.Length)];
        Type = data.shipClass;
        pointValue = data.PointValue;

        Slots.LoadAllData(data.modulosData, data.weaponsData);
        Movement.moveRange = data.moveRange;

        Health.LoadData(this, data);
        evasionValue = data.evasionPoints;
        

        selectCollider.radius *= data.Size/2;
        visual.SetAreaSize(data.Size/2);
        visual.SetupLocalCanvasInfo(this);

        foreach(Transform childModel in modelContainer)
        {
            Destroy(childModel.gameObject);
        }

        GameObject modelGO = Instantiate(data.model_Prefab, modelContainer.transform.position, transform.rotation);
        modelGO.transform.SetParent(modelContainer);
    }

    void OnSelected(ShipLogic _logic)
    {
        if(_logic == this)
        {
            ClearAsTarget();
            // Esta nave fue seleccionada
            visual.SetWeaponRange(Slots.GetLongestWeaponRange());
            visual.SetAsActive(true);
            if(Owner == Player.ActivePlayer)
                CalculateTargetsInRange(transform.position);
        }else
        {
            visual.SetAsActive(false);
        }
    }

    public void SetMoveMode()
    {
        if(state == ShipState.MOVING)
            SetState(ShipState.IDLE);
        else
        if(state == ShipState.IDLE)
            SetState(ShipState.MOVING);
    }

    public void MoveOrder(Vector3 _originPos, Vector3 _targetPos)
    {
        if(GameManager.ActiveShip != this)
            return;
        
        Movement.MoveFromTo(_originPos, _targetPos);
        CalculateTargetsInRange(_targetPos);
        canMove = false;
    }

    public void SetAsTarget(float _hitChance)
    {
        visual.MarkAsTarget(_hitChance);
    }

    public void ClearAsTarget()
    {
        visual.ClearAsTarget();
    }

    void CalculateTargetsInRange(Vector3 origin)
    {
        validTargets.Clear();
        ShipLogic otherShip;
        for (int i = 0; i < GameManager.instance.allShips.Count; i++)
        {
            otherShip = GameManager.instance.allShips[i];
            if(otherShip == this || otherShip.Owner == Player.ActivePlayer)
            {
                  // Ignora la misma nave y sus aliados
                otherShip.ClearAsTarget();
                continue; 
            }

            float distance = Vector3.Distance(origin, otherShip.transform.position);
            bool otherCanBeTargeted = false;
            //float bestHitChance;
            float hitChance = .1f;
            foreach(WeaponSlot w in Slots.Weapons)
            {
                if (otherShip.state == ShipState.DEAD)
                {
                    otherCanBeTargeted = false;
                    continue;
                }

                if (distance <= w.range)
                {
                    if(!validTargets.Contains(i))
                    {
                        validTargets.Add(i);
                        hitChance = w.accuracy - (otherShip.evasionValue/10);
                        if(hitChance < .1f) hitChance = .1f;
                        otherCanBeTargeted = true;
                    }else
                    continue;
                }                
            }
            if(otherCanBeTargeted)
            {
                otherShip.SetAsTarget(hitChance);
            }
        }
    }


    public void SetState(ShipState _state)
    {
        state = _state;

        switch(state)
        {
            case ShipState.WAIT_DEPLOY:
                gameObject.SetActive(true);
                break;
            case ShipState.DEPLOY:
                gameObject.SetActive(true);
                break;
            case ShipState.MOVING:
                break;
            case ShipState.IDLE:
                break;
            case ShipState.AIM:
                break;
            case ShipState.FIRING:
                break;
            case ShipState.DEAD:
                if (data.SFX.destroyed)
                    AudioManager.instance.PlayOneShot(data.SFX.destroyed);
                break;

        }
    }

    public void GiveActionPoints(bool state)
    {
        canMove = state;
        canAttack = state;
        Movement.hasMoved = false;
    }
}
