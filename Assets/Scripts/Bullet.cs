using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour
{
    public static int totalInScreen;
    public float speed = 3f;
    [SerializeField] protected float killRadius = .01f;
    [SerializeField][Range(.5f, 10f)] protected float lifeTime = 1f;
    [Space]
    [SerializeField] protected AudioClip impactClip;
    [SerializeField] protected AudioClip spawnClip;
    [SerializeField] protected GameObject impactVFX;
    [SerializeField] protected GameObject spawnVFX;
    [SerializeField] bool missileMove;
    [SerializeField] float rotateSpeed = 10f;
    protected Vector3 origen;
    protected Vector3 destino;
    Vector3 moveDirection;
    protected Rigidbody rb;
    float targetDist;
    bool isHit;

    #region EVENTS
    public delegate void EmptyVoidDelegate(int total);
    public static event EmptyVoidDelegate OnBulletDeleted;
    #endregion EVENTS


    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        totalInScreen++;
    }

    public virtual void Setup(Vector3 _origen, Vector3 _destino, bool _isHit)
    {
        origen = _origen;
        destino = _destino;
        isHit = _isHit;
        targetDist = Vector3.Distance(origen, destino);
        moveDirection = destino - origen;

        if(missileMove)
        {
           //transform.rotation =  Quaternion.Euler(origen - transform.position);
            transform.rotation = Quaternion.Euler(Random.Range(0,360f), Random.Range(0, 360f), Random.Range(0, 360f));

        }

        Impulsar();
        Invoke("Delete", lifeTime);
        if(spawnVFX)
            Instantiate(spawnVFX, transform.position, Quaternion.Euler(moveDirection));
        if(spawnClip)
            AudioManager.instance.PlayOneShot(spawnClip, true);
    }

    void Impulsar()
    {
        rb.AddForce((destino - origen) * speed, ForceMode.Impulse); 
    }

    private void FixedUpdate()
    {
        if (!missileMove)
            return;
        rb.velocity = transform.forward * speed;
    }

    void Update()
    {
        

        if (missileMove)
        {
            var rotation = Quaternion.LookRotation(destino - transform.position);
            var lerpRotation = Quaternion.RotateTowards(transform.rotation, rotation, rotateSpeed * Time.deltaTime);
            rb.MoveRotation(lerpRotation);
            //Quaternion targetRot = transform.LookAt(destino);
            //Quaternion targetRot = Quaternion.Euler(transform.position - destino);
            //transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRot, Time.deltaTime * rotateSpeed);
        }
        
        if (!isHit) return;
        float dist = Vector3.Distance(transform.position, destino);
        if (dist <= killRadius)
        {
            Kill();
        }
    }

    protected void Kill()
    {
        if(impactVFX)
            Instantiate(impactVFX, transform.position, Quaternion.Euler(moveDirection));
        if (impactClip)
            AudioManager.instance.PlayOneShot(impactClip);
        Delete();
    }

    protected void Delete()
    {
        totalInScreen--;
        if (OnBulletDeleted != null)
            OnBulletDeleted(totalInScreen);
        Destroy(this.gameObject);
    }

}
