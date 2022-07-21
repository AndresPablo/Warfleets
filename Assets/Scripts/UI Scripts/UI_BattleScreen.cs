using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

namespace Warfleets.UI{
public class UI_BattleScreen : MonoBehaviour
{
    [SerializeField]Canvas canvas;
    [SerializeField] GameObject controlsBox;
    [SerializeField] GameObject weaponsBox;
    [Header("CONTROLS")]
    [SerializeField] GameObject moveBox;
    [SerializeField] GameObject doneBox;
    [SerializeField] GameObject attackBox;
    [SerializeField] GameObject cycleBox;
    [Header("SHIP")]
    [SerializeField] TextMeshProUGUI activeShipName_Label;
    [SerializeField] TextMeshProUGUI activeShipSub_Label;
    [SerializeField] TextMeshProUGUI sHull_Label;
    [SerializeField] TextMeshProUGUI sArmor_Label;
    [SerializeField] TextMeshProUGUI sEvasion_Label;
    [SerializeField] TextMeshProUGUI sMove_Label;
    [Header("ACTIVE PLAYER")]
    [SerializeField] UI_PlayerBattleInfoPanel playerA_panel;
    [SerializeField] UI_PlayerBattleInfoPanel playerB_panel;
    [Header("WEAPONS")]
    [SerializeField] TextMeshProUGUI selectedWeapon_Label;
    [SerializeField] TextMeshProUGUI weaponHitChance_Label;
    [SerializeField] TextMeshProUGUI weaponSpecial_Label;
    [SerializeField] TextMeshProUGUI weaponDamage_Label;
    [SerializeField] Transform weaponPanelsContainer;
    [SerializeField] GameObject weaponUIPrefab;
    [SerializeField] GameObject activeModUIPrefab;
    [SerializeField] GameObject passiveModUIPrefab;
    List<UI_WeaponItem> weaponBoxes = new List<UI_WeaponItem>();
    

    void Start()
    {
        canvas = GetComponent<Canvas>();
        GameManager.OnShipSelected += UpdateActiveShipInfo;
        GameManager.OnTurnPassed += UpdateTurnInfo;
        GameManager.OnModuleSelected += UpdateWeaponsInfo;
        GameManager.OnShipTargeted += UpdateTargetInfo;
        ShipMovement.OnShipStartsMoving += HideUI;
        ShipMovement.OnShipStopsMoving += ShipStops;
    }

    private void UpdateTargetInfo(DamageData dd)
    {
        weaponsBox.SetActive(false);
        controlsBox.SetActive(false);
        playerA_panel.Toogle(false);
        playerB_panel.Toogle(false);
    }

    private void UpdateWeaponsInfo(int y)
    {
        weaponsBox.SetActive(true);
        controlsBox.SetActive(false);
        playerA_panel.Toogle(false);
        playerB_panel.Toogle(false);
        ClearWeaponList();
        
        WeaponSlot weapon = GameManager.ActiveShip.Slots.Weapons[y];
        for (int i = 0; i < GameManager.ActiveShip.Weapons.Length; i++)
        {
            WeaponSlot w = GameManager.ActiveShip.Weapons[i];
            GameObject go = Instantiate(weaponUIPrefab, weaponPanelsContainer);
            UI_WeaponItem w_UI = go.GetComponent<UI_WeaponItem>();
            if(w.data.icon)
                w_UI.icon.sprite = w.data.icon;
            w_UI.label.text = i.ToString("0");
            weaponBoxes.Add(w_UI);         

            if(i == GameManager.instance.selectedModuleIndex)
            {
                selectedWeapon_Label.text = ">> "+ w.data.name + " <<";
                weaponHitChance_Label.text = w.wData.accuracyValue*100 +"%";
                weaponDamage_Label.text = "[" + w.wData.damage +"]";
                weaponSpecial_Label.text = "...";
                w_UI.Select();
            }
            else
            {
                w_UI.Unselect();
            }
        }
    }

    void ShipMoves()
    {

    }

    void HideUI()
    {
        canvas.enabled = false;
    }

    void ShipStops()
    {        
        canvas.enabled = true;
        if(!GameManager.ActiveShip.canMove)
        {
            moveBox.SetActive(false);
            cycleBox.SetActive(false);
        }
    }

    void UpdateTurnInfo()
    {
        weaponsBox.SetActive(false);
        controlsBox.SetActive(true);
        playerA_panel.Toogle(true);
        playerB_panel.Toogle(true);
    }

    void UpdateActiveShipInfo(ShipLogic logic)
    {
        weaponsBox.SetActive(false);
        controlsBox.SetActive(true);
        activeShipName_Label.text = logic.mName;
        activeShipSub_Label.text = logic.Type + " // " + logic.pointValue +" pts";
        sHull_Label.text = logic.CurrentHealthPoints + "";
        sEvasion_Label.text = logic.evasionValue + "";
        sMove_Label.text = (int)logic.MaxMoveValue + "";
        sArmor_Label.text = (int)logic.armorValue + "";

        moveBox.SetActive(logic.canMove);
        if(logic.canAttack || logic.canMove)
            cycleBox.SetActive(true);
        else
            cycleBox.SetActive(false);
        attackBox.SetActive(logic.canAttack);

        if(logic.Owner != Player.ActivePlayer)
        {
            weaponsBox.SetActive(false);
            controlsBox.SetActive(false);
            return;
        }
    }

    void ClearWeaponList()
    {
        foreach(Transform child in weaponPanelsContainer)
        {
            Destroy(child.gameObject);
        }
        weaponBoxes.Clear();
    }
    
    public void Toogle(bool state)
    {
        canvas.enabled = state;
    }
}
}