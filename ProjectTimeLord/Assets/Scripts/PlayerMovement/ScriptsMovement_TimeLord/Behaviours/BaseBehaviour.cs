using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseBehaviour : MonoBehaviour {

   
    public MonoBehaviour[] disableScripts;
    protected FaceDirection faceDirection;
    protected Rigidbody2D body2d;
    protected Collision collisionState;

    public float AbsVelocityX
    {
        get { return Mathf.Abs(body2d.velocity.x); }
    }

    public float AbsVelocityY
    {
        get { return Mathf.Abs(body2d.velocity.y); }
    }

    public float VelocityX
    {
        get { return body2d.velocity.x; }
    }

    public float VelocityY
    {
        get { return body2d.velocity.y; }
    }



    // Use this for initialization
    protected virtual void Awake()
    {
        body2d = GetComponent<Rigidbody2D>();
        collisionState = GetComponent<Collision>();
        faceDirection = GetComponent<FaceDirection>();
    }
    protected virtual void ToggleScripts(bool value)
    {
       foreach(var script in disableScripts )
        {
            script.enabled = value;
        }
    }

}
