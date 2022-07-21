using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;
using System;

public class UI_DeployScreen : MonoBehaviour
{
    [SerializeField]Canvas canvas;
    [SerializeField] GameObject deployItem_prefab;
    public UI_FleetDeployPanel panel_A;
    public UI_FleetDeployPanel panel_B;
    public Player player_A;
    public Player player_B;
    [SerializeField] Transform lista_A;
    [SerializeField] Transform lista_B;
    public int shipToDeploy;
    [SerializeField] TextMeshProUGUI activePlayerName;

    void Start()
    {
        GameManager.OnChangeState += CheckState;
        GameManager.OnTurnPassed += Refresh;
        GameManager.OnShipSelected += UpdateSelection;
    }

    private void Refresh()
    {
        if(GameManager.State == GameState.DEPLOY)
        {
            canvas.enabled = true;
            activePlayerName.text = Player.ActivePlayer + " " + Player.ActivePlayer.factionData.name;
        }
    }

    void UpdateSelection(ShipLogic shipScript)
    {
        if(GameManager.State != GameState.DEPLOY)
            return;
        Clear();
        Fill();
    }

    void CheckState(GameState state)
    {
        if(state == GameState.DEPLOY)
        {
            canvas.enabled = true;
            Fill();
            Refresh();
        }else
        {
            if(canvas.isActiveAndEnabled)
                Hide();
        }
    }

    public void Fill()
    {/*
        if(player_A.Fleet.shipsToDeploy.Count <= 0 && player_B.Fleet.shipsToDeploy.Count <= 0)
        {
            Debug.LogWarning("NO HAY NAVES EN NINGUNA FLOTA");
            canvas.enabled = false;
            return;
        }

        if(player_A.Fleet.shipsToDeploy.Count <= 0 || player_B.Fleet.shipsToDeploy.Count <= 0)
        {
            Debug.LogWarning("NO HAY NAVES EN LA FLOTA");
            canvas.enabled = false;
            return;
        }

        Clear();
        canvas.enabled = true;

        foreach(ShipData data in player_A.Fleet.shipsToDeploy)
        {
            GameObject go = Instantiate(deployItem_prefab, lista_A);
            go.GetComponent<UI_DeployShipItem>().LoadInfo(data);
        }

        foreach(ShipData data in player_B.Fleet.shipsToDeploy)
        {
            GameObject go = Instantiate(deployItem_prefab, lista_B);
            go.GetComponent<UI_DeployShipItem>().LoadInfo(data);
        }*/
    }

    public void Clear()
    {
        foreach(Transform child in lista_A)
        {
            Destroy(child.gameObject);
        }
        foreach(Transform child in lista_B)
        {
            Destroy(child.gameObject);
        }
    }

    public void Hide(){
        canvas.enabled = false;
    }
}
