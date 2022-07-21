using UnityEngine;

public class SelfDestroy : MonoBehaviour
{
    [SerializeField] float time = 1f;

    void Start()
    {
        Invoke("DestroyMe", time);
    }

    void DestroyMe()
    {
        Destroy(gameObject);
    }
}
