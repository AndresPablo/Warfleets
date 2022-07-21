using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipMovement : MonoBehaviour
{
    public float moveRange = 1f;
    public bool hasRotated;
    public bool hasMoved;
    public float basicSteerAngle = 45f;
    public float remainingSteer = 45f;
    [Range(1,10f)]public float speed = 2f;
    Vector3 originPosition;
    Vector3 targetPosition;
    Quaternion newRotation;
    bool isMoving;

    #region  EVENTS
    public delegate void EmptyVoidDelegate();
    public static event EmptyVoidDelegate OnShipStartsMoving;
    public event EmptyVoidDelegate OnShipStopped;
    public static event EmptyVoidDelegate OnShipStopsMoving;
    #endregion EVENTS


    public void MoveFromTo(Vector3 _originPos, Vector3 _targetPos)
    {
        if(hasMoved)
            return;
        originPosition = _originPos;
        targetPosition = _targetPos;

        // Rotar
        transform.rotation =  Quaternion.LookRotation(targetPosition - originPosition, Vector3.up) ;

        StartCoroutine("MoveFromToAsync");

        if(OnShipStartsMoving != null)
            OnShipStartsMoving();
    }

    public void Rotate()
    {

    }

    IEnumerator MoveFromToAsync()
    {
        float elapsed = 0f;
        while(elapsed < 2f)
        {
            Vector3 newPos = Vector3.Lerp(originPosition, targetPosition, elapsed);
            transform.position = newPos;
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.position = targetPosition;

        if(OnShipStopsMoving != null)
            OnShipStopsMoving();
        if(OnShipStopped != null)
            OnShipStopped();
    }

    // TODO: deprecate
    void Update()
    {
        if(isMoving)
        {
            var step =  speed * Time.deltaTime; // calculate distance to move
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);
            transform.rotation = Quaternion.RotateTowards(transform.rotation,newRotation, 50f);
            if (Vector3.Distance(transform.position, targetPosition) < 0.001f)
            {
                if(transform.rotation == newRotation)
                {
                    isMoving = false;
                    if(OnShipStopsMoving != null)
                        OnShipStopsMoving();
                    if(OnShipStopped != null)
                        OnShipStopped();
                }
            }
        }
    }
}
