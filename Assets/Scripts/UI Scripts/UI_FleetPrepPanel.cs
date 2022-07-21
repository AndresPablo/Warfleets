using UnityEngine.UI;
using TMPro;
using UnityEngine;

namespace Warfleets.UI{
public class UI_FleetPrepPanel : MonoBehaviour
{
    public Player player;
    [Space]
    [SerializeField] GameObject factionButton_prefab;
    [SerializeField] Transform factionsGrid;
    [Space]
    public GameObject[] tabs;
    public GameObject factionTab;
    public GameObject fleetBuildTab;
    public GameObject quickPlayTab;
    public int activeTab;


    void Start()
    {
        FillFactionGrid();
    }

    public void SelectPlayerFaction(Player _player, FactionData _data)
    {
        player.SelectFaction(_data);
    }

    void ClearFactionGrid()
    {
        foreach(Transform child in factionsGrid)
        {
            Destroy(child.gameObject);
        }
    }

    void FillFactionGrid()
    {
        ClearFactionGrid();
        foreach(FactionData data in  GameManager.instance.factions)
        {
            GameObject go = Instantiate(factionButton_prefab, factionsGrid);
            UI_FactionSelectButton buttonScript = go.GetComponent<UI_FactionSelectButton>();
            buttonScript.SetInfo(player, data);
            buttonScript.button.onClick.AddListener(() => SelectPlayerFaction(player, data));
        }
    }
}
}
