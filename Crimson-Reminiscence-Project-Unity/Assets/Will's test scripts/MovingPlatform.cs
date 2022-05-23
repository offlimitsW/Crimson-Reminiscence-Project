using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public bool isActivated = false;

    public enum PlatformMovementType
    {
        Once,
        PingPong
    }
    public PlatformMovementType movementType;
    public float speed;
    //[Tooltip("If it's PingPong, how long still it stop before the next movement?")]
    //public float restTime;

    public Transform startPosition;
    public Transform endPosition;
    bool isReversed = false;

    public void Update()
    {
        if (isActivated) 
        {
            switch (movementType)
            {
                case PlatformMovementType.Once:
                    {
                        LoopOnce();
                        break;
                    }
                case PlatformMovementType.PingPong:
                    {
                        LoopPingPong();
                        break;
                    }
            }
        }    
    }

    void LoopOnce()
    {
        transform.position = Vector3.MoveTowards(transform.position, endPosition.position, speed * Time.deltaTime);
    }

    void LoopPingPong()
    {
        if (transform.position == endPosition.position) 
            isReversed = true;
        if (transform.position == startPosition.position)
            isReversed = false;
        if (!isReversed)
        {
            transform.position = Vector3.MoveTowards(transform.position, endPosition.position, speed * Time.deltaTime);
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, startPosition.position, speed * Time.deltaTime);
        }
    }
}
