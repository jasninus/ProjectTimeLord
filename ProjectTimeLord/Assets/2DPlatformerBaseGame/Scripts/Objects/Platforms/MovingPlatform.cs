using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[SelectionBase]
[RequireComponent(typeof(Rigidbody2D))]
public class MovingPlatform : MonoBehaviour {
    
	public enum MovingPlatformType
    {
        BackForth,
        Loop,
        Once,
        TriggerLoop
    }

    public PlatformCatcher platformCatcher;
    public float speed = 1.0f;
    public MovingPlatformType platformType;
    
    public string triggerTag;
    public bool startMovingOnlyWhenVisible;
    public bool isMovingAtStart = false;

    [HideInInspector]
    public Vector3[] localNodes = new Vector3[1];

    public float[] waitTimes = new float[1];

    protected Vector3[] worldNode;
    public Vector3[] WorldNode { get { return worldNode; } }

    protected int current = 0;
    protected int next = 0;
    protected int dir = 1;

    protected float waitTime = -1.0f;

    new protected Rigidbody2D rigidbody;
    protected Vector2 velocity;

    protected bool started = false;
    protected bool veryFirstStart = false;
    

    public Vector2 Velocity
    {
        get { return velocity; }
    }

    //we always have at least a node which is the local position
    private void Reset()
    {
        localNodes[0] = Vector3.zero;
        waitTimes[0] = 0;

        rigidbody = GetComponent<Rigidbody2D>();
        rigidbody.isKinematic = true;

        if (platformCatcher == null)
            platformCatcher = GetComponent<PlatformCatcher>();
    }

    private void OnEnable()
    {
        EventManager.StartListening("TriggerPlatformStart", TriggerPlatformStart);
        EventManager.StartListening("TriggerPlatformStop", TriggerPlatformStop);
    }

    private void OnDisable()
    {
        EventManager.StopListening("TriggerPlatformStart", TriggerPlatformStart);
        EventManager.StopListening("TriggerPlatformStop", TriggerPlatformStop);
    }

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        rigidbody.isKinematic = true;

        if (platformCatcher == null)
            platformCatcher = GetComponent<PlatformCatcher>();

        //Allow to make platform only move when they became visible
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        for (int i = 0; i < renderers.Length; ++i)
        {
            var b = renderers[i].gameObject.AddComponent<VisibleBubbleUp>();
            b.objectBecameVisible = BecameVisible;
        }

        //we make point in the path being defined in local space so game designer can move the platform & path together
        //but as the platform will move during gameplay, that would also move the node. So we convert the local nodes
        // (only used at edit time) to world position (only use at runtime)
        worldNode = new Vector3[localNodes.Length];
        for (int i = 0; i < worldNode.Length; ++i)
            worldNode[i] = transform.TransformPoint(localNodes[i]);

        Init();
    }

    protected void Init()
    {
        current = 0;
        dir = 1;
        next = localNodes.Length > 1 ? 1 : 0;

        waitTime = waitTimes[0];

        veryFirstStart = false;
        if (isMovingAtStart)
        {
            started = !startMovingOnlyWhenVisible;
            veryFirstStart = true;
        }
        else
            started = false;
    }

    private void FixedUpdate()
    {
        if (!started)
            return;

        //no need to update we have a single node in the path
        if (current == next)
            return;

        if (waitTime > 0)
        {
            waitTime -= Time.deltaTime;
            return;
        }

        float distanceToGo = speed * Time.deltaTime;

        while (distanceToGo > 0)
        {
            Vector2 direction = worldNode[next] - transform.position;

            float dist = distanceToGo;
            if (direction.sqrMagnitude < dist * dist)
            {
                //we have to go farther than our current goal point, so we set the distance to the remaining distance
                //then we change the current & next indexe
                dist = direction.magnitude;

                current = next;

                waitTime = waitTimes[current];

                if (dir > 0)
                {
                    next += 1;
                    if (next >= worldNode.Length)
                    {
                        //we reach the end
                        switch (platformType)
                        {
                            case MovingPlatformType.BackForth:
                                next = worldNode.Length - 2;
                                dir = -1;
                                break;
                            case MovingPlatformType.Loop:
                                next = 0;
                                break;
                            case MovingPlatformType.Once:
                                next -= 1;
                                started = false;
                                break;
                            case MovingPlatformType.TriggerLoop:
                                next = 0;
                                break;
                        }
                    }

                    if (platformType == MovingPlatformType.TriggerLoop)
                    {
                        started = false;
                        EventManager.TriggerEvent("TriggerSwitchReset", triggerTag);
                        return;
                    }
                }
                else
                { //reached the beginning again
                    next -= 1;
                    if (next < 0)
                    {
                        switch(platformType)
                        {
                            case MovingPlatformType.BackForth:
                                next = 1;
                                dir = 1;
                                break;
                            case MovingPlatformType.Loop:
                                next = worldNode.Length - 1;
                                break;
                            case MovingPlatformType.Once:
                                next += 1;
                                StopMoving();
                                break;
                            case MovingPlatformType.TriggerLoop:
                                next = worldNode.Length - 1;
                                break;
                        }
                    }
                }
            }

            velocity = direction.normalized * dist;
            rigidbody.MovePosition(rigidbody.position + velocity);
            platformCatcher.MoveCaughtObjects(velocity);
            //We remove the distance we moved. That way if we didn't had enough distance to the next goal, we will do a new loop to finish
            //the remaining distance we have to cover this frame toward the new goal
            distanceToGo -= dist;

            // we have some wait time set, that mean we reach a point where we have to wait. So no need to continue to move the platform, early exit.
            if (waitTime > 0.001f)
                break;
        }
    }

    public void TriggerPlatformStart(string tag)
    {
        if (tag == triggerTag)
            started = true;
    }

    public void TriggerPlatformStop(string tag)
    {
        if (tag == triggerTag)
            started = false;
    }

    public void StartMoving()
    {
        started = true;
    }

    public void StopMoving()
    {
        started = false;
    }

    public void ResetPlatform()
    {
        transform.position = worldNode[0];
        Init();
    }

    private void BecameVisible(VisibleBubbleUp obj)
    {
        if (veryFirstStart && isMovingAtStart)
        {
            started = true;
            veryFirstStart = false;
        }
    }
}
