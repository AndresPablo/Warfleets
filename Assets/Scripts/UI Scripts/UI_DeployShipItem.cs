using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class UI_DeployShipItem : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI shipName_label;
    [SerializeField]Image arrow;
    public ShipData data;

    void Start()
    {
        arrow.enabled = false;
    }

    public void LoadInfo(ShipData _data)
    {
        this.data = _data;
        shipName_label.text = data.name; // TODO
    }

    public void MarkAsDeployed()
    {
        shipName_label.text = shipName_label.text + "*";
    }
}
