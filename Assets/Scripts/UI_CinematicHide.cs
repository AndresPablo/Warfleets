using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class UI_CinematicHide : MonoBehaviour
{
    CanvasGroup grupo;
    Canvas canvas;
    
    void Start()
    {
        GameManager.OnChangeState += CheckState;
        canvas = GetComponent<Canvas>();
        grupo = GetComponent<CanvasGroup>();
    }

    void CheckState(GameState state)
    {
        if(state == GameState.SHOOTING || state == GameState.WAIT)
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
        }
    }
}
