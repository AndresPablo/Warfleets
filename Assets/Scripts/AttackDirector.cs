using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackDirector : MonoBehaviour
{
    #region SINGLETON
    public static AttackDirector instance;
    private void Awake()
    {
        instance = this;
    }
    #endregion SINGLETON

    public delegate void EmptyDamageDelegate(DamageData dd);
    public static event EmptyDamageDelegate OnShootingBegins;
    public static event EmptyDamageDelegate OnShootingEnds;

    public void PlayShooting(DamageData dd)
    {
        StartCoroutine("ShootRoutine", dd);
    }

    IEnumerator ShootRoutine(DamageData damageData)
    {
        WeaponModuleData wData = GameManager.instance.ActiveWeapon.wData;
        List<Vector3> firePoints = new List<Vector3>(); 
        List<Vector3> targetPoints = new List<Vector3>();

        if (OnShootingBegins != null)
            OnShootingBegins(damageData);

        for (int j = 0; j < wData.gunsAmount; j++)
        {
            firePoints.Add( GetRandomPointInCircle(damageData.attacker.ModelTrans.position, wData.originSpread));
            //targetPoints.Add( GetRandomPointInCircle(dd.defender.ModelTrans.position, data.targetSpread));
        }

        for (int i = 0; i < wData.volleyAmount; i++)
        {
            foreach(Vector3 fPoint in firePoints)
            {
                GameObject bulletPrefab = wData.bulletPrefab;
                GameObject bulletGO = Instantiate(bulletPrefab);    // TODO: spawnear en Entidades

                Vector3 origin = fPoint;
                bulletGO.transform.position = origin;
                Vector3 tOffset;
                Vector3 destino = GetRandomPointInCircle(damageData.defender.ModelTrans.position, wData.targetSpread) ;
                Bullet script = bulletGO.GetComponent<Bullet>();
                
                script.Setup(origin, destino, damageData.isHit);
                yield return new WaitForSeconds(wData.fireRate);
            }
        }

        while (Bullet.totalInScreen > 0)
        {
            yield return new WaitForSeconds(1f);
        }

        CommandManager.CompleteCurrentCommand();
        yield return null;
    }

    Vector3 GetRandomPointInCircle(Vector3 center, float size)
    {   
        Vector3 point = center + Random.insideUnitSphere * size;
        return point;
    }
}
