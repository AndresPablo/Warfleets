using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ShipVisual : MonoBehaviour
{
    [SerializeField] ShipLogic logic;
    [SerializeField]LineRenderer baseLine;
    [SerializeField]Sprite ringSprite;
    [SerializeField]Sprite circleSprite;
    [SerializeField]Image sizeImage;
    [SerializeField]Image moveArc_img;
    [Space]
    public Color activeColor = Color.green;
    public Color idleColor = Color.white;
    public Color targetColor = Color.red;
    public Color enemyColor = Color.red;
    [Space]
    [SerializeField] UI_LineRendCircle areaRadius_Circle;
    [SerializeField] GameObject weaponRange_CirclePrefab;
    [SerializeField] Transform weaponRangeContainer;
    [SerializeField] UI_LineRendCircle[] weaponRanges;
    [Space]
    [SerializeField] GameObject HPCell_prefab;
    [SerializeField] Transform HP_container;
    [SerializeField] Canvas localCanvas;
    [SerializeField] CanvasGroup localCanvas_group;
    [SerializeField] TextMeshProUGUI shipName_label;
    [SerializeField] TextMeshProUGUI hitChance_label;
    [SerializeField] Image hitChance_img;
    bool isActiveShip;

    public Color FactionColor {get {return logic.Faction.color_1; }}

    
    void Start()
    {
        ShipLifeHandler.OnDamageToShip += UpdateHullBar;
    }

    public void SetupLocalCanvasInfo(ShipLogic logic)
    {
        UpdateHullBar(logic.CurrentHealthPoints);

        shipName_label.text = logic.mName;
        hitChance_label.enabled = false;
        hitChance_img.enabled = false;
    }

    public void SetAreaSize(float size)
    {
        sizeImage.rectTransform.localScale = (Vector3.one * size);
        moveArc_img.rectTransform.localScale = (Vector3.one * size);
    }

    public void SetWeaponRange(float size)
    {
        //weaponRangeImage.rectTransform.localScale += (Vector3.one * size);
    }

    public void SetMoveRangeCircle(float radius)
    {
        //weaponRangeImage.rectTransform.localScale += (Vector3.one * size);
    }

    public void SetAsActive(bool state)
    {
        isActiveShip = state;
        baseLine.startColor = FactionColor;
        baseLine.endColor = FactionColor;

        if(logic.CanBeActivated)
        {
            areaRadius_Circle.SetColor(FactionColor);
            sizeImage.color = FactionColor;
        }else
        {
            sizeImage.color = idleColor;
            areaRadius_Circle.SetColor(idleColor);
        }

        if(isActiveShip)
        {
            sizeImage.sprite = circleSprite;
            moveArc_img.enabled = logic.canMove;
        }else
        {
            moveArc_img.enabled = false;
            sizeImage.sprite = ringSprite;
        }
    }

    void UpdateHullBar(int newHP)
    {
        localCanvas.transform.forward = Camera.main.transform.forward;
        ClearHullBar();
        for (int i = 0; i < newHP; i++)
        {
            GameObject hp_GO = Instantiate(HPCell_prefab, HP_container);
        }
    }

    void ClearHullBar()
    {
        foreach(Transform child in HP_container)
        {
            Destroy(child.gameObject);
        }
    }

    public void ClearAsTarget()
    {
        hitChance_img.enabled = false;
        hitChance_label.enabled = false;
    }

    public void MarkAsTarget(float hitChance)
    {
        hitChance_img.enabled = true;
        hitChance_label.enabled = true;
        hitChance_label.text = (hitChance*100).ToString("") + "%";
    }

    void Update()
    {
        localCanvas.transform.forward = Camera.main.transform.forward;
    }
}
