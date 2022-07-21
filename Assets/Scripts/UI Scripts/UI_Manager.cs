using UnityEngine.UI;
using TMPro;
using UnityEngine;
namespace Warfleets.UI{
public class UI_Manager : MonoBehaviour
{
    [SerializeField] UI_BattleScreen battleScreen;
    [SerializeField] UI_PrepScreen prepScreen;
    [SerializeField] UI_DeployScreen deployScreen;


    void Start()
    {
        //GameManager.OnGameStart += 
        GameManager.OnChangeState += SetupDeployScreen;
        GameManager.OnChangeState += SetupBattleScreen;
        battleScreen.Toogle(false);
        prepScreen.Toogle(false);
    }

    void SetupPrepScreen()
    {
        prepScreen.Toogle(true);
    }

    void SetupBattleScreen(GameState state)
    {
        if(state != GameState.TACTIC)
            return;
        battleScreen.Toogle(true);
    }

    void SetupDeployScreen(GameState state)
    {
        if(state != GameState.DEPLOY)
            return;
        battleScreen.Toogle(false);
        deployScreen.Hide();
    }
}}