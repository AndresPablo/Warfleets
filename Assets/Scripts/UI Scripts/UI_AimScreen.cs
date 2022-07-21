using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

namespace Warfleets.UI{
    public class UI_AimScreen : MonoBehaviour
    {
        [SerializeField] Canvas canvas;
        [SerializeField] GameObject reticulaGO;
        [SerializeField] GameObject hitChanceGO;
        [SerializeField] GameObject damageGO;
        [Header("Attack Chance Box")]
        [SerializeField] TextMeshProUGUI totalChance_label;
        [SerializeField] TextMeshProUGUI baseChance_label;
        [SerializeField] TextMeshProUGUI evadeChance_label;
        [SerializeField] TextMeshProUGUI bonusChance_label;
        [Header("Damage Box")]
        [SerializeField] TextMeshProUGUI damageLabel;
        [Space]
        public Color highColor = Color.white;
        public Color standardColor = Color.white;
        public Color lowColor = Color.white;
        public Color badColor = Color.white;


        void Start()
        {
            canvas.enabled = false;
            GameManager.OnShipTargeted += UpdateTargetInfo;
            UserInput.OnCancelInput += Hide;
            GameManager.OnChangeState += EvaluarEstado;
        }

        private void EvaluarEstado(GameState state)
        {
            if(state != GameState.SHOOTING)
                return;
            else
            {
                Hide();
            }
        }

        private void UpdateTargetInfo(DamageData damageData)
        {
            canvas.enabled = true;
            // TODO: Fill info
            totalChance_label.text = damageData.TotalHitChance*100+"%";
            baseChance_label.text =  damageData.baseHitMod*100+"%";
            if(damageData.bonusHitMod != 0)
                bonusChance_label.text = damageData.bonusHitMod*10+"+"+"%";
            else bonusChance_label.text = "-";
            evadeChance_label.text = "-" + damageData.evadeHitMod*10+ "%";
            damageLabel.text = damageData.damage + " daño";
        }

        void Hide()
        {
            if(canvas.isActiveAndEnabled)
                Toogle(false);
        }

        public void Toogle(bool state)
        {
            canvas.enabled = state;
        }
    }
}