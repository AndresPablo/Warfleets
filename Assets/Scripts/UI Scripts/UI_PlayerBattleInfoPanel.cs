using UnityEngine.UI;
using TMPro;
using UnityEngine;
using System;

namespace Warfleets.UI{
public class UI_PlayerBattleInfoPanel : MonoBehaviour
{
    [SerializeField] Player myPlayer;
    public TextMeshProUGUI playerName_Label;
    public TextMeshProUGUI factionPoints_Label;
    public TextMeshProUGUI myTurn_Label;
    [Space]
    public GameObject scoreBar;
    //public Image scoreFill;
    //public TextMeshProUGUI scoreLabel;
    
    void Start()
    {
        GameManager.OnTurnPassed += Refresh;
    }

    private void Refresh()
    {
        if(Player.ActivePlayer == myPlayer)
        {
            playerName_Label.text = myPlayer.name;
            factionPoints_Label.text = myPlayer.factionData + " // " + myPlayer.Fleet.TotalPointCost + " pts";
            myTurn_Label.text = "> ACTIVO";
        }else
        {
            myTurn_Label.text = "";
        }
    }

    public void Toogle(bool state)
    {
        gameObject.SetActive(state);
    }
}}
