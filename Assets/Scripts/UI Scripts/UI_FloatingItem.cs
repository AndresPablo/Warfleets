using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_FloatingItem : MonoBehaviour
{
    
    [SerializeField] TextMeshProUGUI label;
    [SerializeField] Vector3 speedVector;
    public float lifeTime = 2f;

    void Start()
    {
        Destroy(this.gameObject, lifeTime);
    }

    public void LoadInfo(string info)
    {
        label.text = info;
    }

    void Update()
    {
        transform.position += (speedVector*Time.deltaTime);
    }
}
