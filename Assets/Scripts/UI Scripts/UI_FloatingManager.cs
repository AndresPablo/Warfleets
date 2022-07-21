using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

namespace Warfleets.UI{
public class UI_FloatingManager : MonoBehaviour
{
    [SerializeField] GameObject UIDamagePrefab;
    [SerializeField] GameObject UIEvadePrefab;
    [Space]
    [SerializeField] Canvas canvas;


    void Start()
    {
        ShipLifeHandler.OnShipDamaged += NaveDañada;
        DealDamage_Command.OnShipEvades += NaveEvade;
    }

    IEnumerator CrearTextoDamage(DamageData dd)
    {
        float tiempoEspera = GameManager.instance.timeBetweenActions;
        yield return new WaitForSeconds(tiempoEspera);

        string info = dd.damage + " daño";
        GameObject go = Instantiate(UIDamagePrefab, this.transform);
        go.GetComponent<UI_FloatingItem>().LoadInfo(info);
    }

    IEnumerator CreaTextoEvasion(Vector3 position)
    {
        float tiempoEspera = GameManager.instance.timeBetweenActions;
        yield return new WaitForSeconds(tiempoEspera);
        GameObject go = Instantiate(UIEvadePrefab, this.transform);
    }

    void NaveDañada(DamageData dd)
    {
        StartCoroutine(CrearTextoDamage(dd));
    }

    void NaveEvade(Vector3 position)
    {
        StartCoroutine(CreaTextoEvasion(position));
    }
}
}