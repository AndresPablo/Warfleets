using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;


public class CameraHandler : MonoBehaviour
{
    bool follow;
    [Header("Game State Cams")]
    [SerializeField] CinemachineVirtualCamera deployVCam;
    [SerializeField] CinemachineVirtualCamera aimVCam;
    [SerializeField] CinemachineVirtualCamera moveVCam;
    [Header("Attack Cams")]
    [SerializeField] CinemachineVirtualCamera atkCam_1;
    CinemachineVirtualCamera activeVCam;

    void Start()
    {
        GameManager.OnShipSelected += SetFollowTarget;
        GameManager.OnShipTargeted += SetLookAtWeaponTarget;
        AttackDirector.OnShootingBegins += SetShootingCamera;
        SwitchCamera(deployVCam);
    }

    void SetLookAtWeaponTarget(DamageData damageData)
    {
        SwitchCamera(aimVCam);
        Transform shipTransform = damageData.defender.ModelTrans;
        //Transform modelTransform;
        aimVCam.LookAt = shipTransform;
        aimVCam.Follow = GameManager.ActiveShip.transform;
    }

    void SetFollowTarget(ShipLogic shipLogic)
    {
        if(GameManager.State == GameState.DEPLOY)
        {
            Transform shipTransform = shipLogic.transform;
            //Transform modelTransform;
        }else
        if(GameManager.State == GameState.TACTIC)
        {
            Transform shipTransform = shipLogic.transform;
            //Transform modelTransform;
            moveVCam.LookAt = shipTransform;
            moveVCam.Follow = null;
            SwitchCamera(moveVCam);
        }

    }

    void SetShootingCamera(DamageData dd)
    {
        // TODO: randomize entre muchas camaras de ataque
        SwitchCamera(atkCam_1);
        atkCam_1.LookAt = dd.defender.ModelTrans;
        atkCam_1.Follow = dd.defender.transform;
    }

    void SwitchCamera(CinemachineVirtualCamera newActiveVcam)
    {
        deployVCam.enabled = false;
        aimVCam.enabled = false;
        moveVCam.enabled = false;
        atkCam_1.enabled = false;
        newActiveVcam.enabled = true;
        activeVCam = newActiveVcam;
    }

    public void LookAtAttackerShip(ShipLogic attacker, ShipLogic defender)
    {
        SwitchCamera(aimVCam);
        activeVCam.LookAt = attacker.ModelTrans;
        activeVCam.LookAt = defender.ModelTrans;
    }
}
