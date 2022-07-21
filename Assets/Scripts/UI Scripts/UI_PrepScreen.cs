using UnityEngine.UI;
using TMPro;
using UnityEngine;

namespace Warfleets.UI{
    
[RequireComponent(typeof(Canvas))]
public class UI_PrepScreen : MonoBehaviour
{
    [SerializeField]Canvas canvas;
    [SerializeField] UI_FleetPrepPanel fleetA;
    [SerializeField] UI_FleetPrepPanel fleetB;

    void Start()
    {
        canvas = GetComponent<Canvas>();
        GameManager.OnChangeState += Refresh;
        Player.OnFactionChosen += CheckFactionSelection;
    }

    void CheckFactionSelection(FactionData _data)
    {
        // Si ambos tienen una faccion, seguimos
        if(fleetA.player.factionData != null && fleetB.player.factionData)
        {
            GameManager.instance.Invoke("QuickPlayStart",.5f);
        }
    }

    public void Refresh(GameState state)
    {
        switch(state)
        {
            case GameState.PREP:
                canvas.enabled = true;
            break;
            case GameState.DEPLOY:
                canvas.enabled = false;
            break;            
            case GameState.TACTIC:
                canvas.enabled = false;
            break;
        }
    }

    public void ConfirmQuickPlay()
    {
    }

    public void Toogle(bool state)
    {
        canvas.enabled = state;
    }
}}