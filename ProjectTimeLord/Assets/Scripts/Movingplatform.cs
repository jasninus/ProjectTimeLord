using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movingplatform : TimeScaled
{
    [SerializeField] private TransformRewind transformRewind;
    [SerializeField] private Transform platformOrigin;
    private Vector3 platform;
    private Vector3 originalPos;
    private float tolerance = 0.5f;
    private int count = 0;
    public float travelDist;
    public int direction;

    private int moveLeft(float timeScale, float travelDist, Vector3 originalPos)
    {
        if (originalPos.x - travelDist < platform.x && count == 0)
        {
            platform.x -= timeScale;
            transform.position = platform;
            if (platform.x - tolerance <= originalPos.x - travelDist)
            {
                return count++;
            }
        }
        return 0;
    }

    private int moveRight(float timeScale, float travelDist, Vector3 originalPos)
    {
        if (originalPos.x + travelDist > platform.x && count == 1)
        {
            platform.x += timeScale;
            transform.position = platform;
            if (platform.x + tolerance >= originalPos.x + travelDist)
            {
                return count--;
            }
        }
        return 1;
    }

    private int moveUp(float timeScale, float travelDist, Vector3 originalPos)
    {
        if (originalPos.y + travelDist > platform.y && count == 0)
        {
            platform.y += timeScale;
            transform.position = platform;
            if (platform.y + tolerance >= originalPos.y + travelDist)
            {
                return count++;
            }
        }
        return 0;
    }

    private int moveDown(float timeScale, float travelDist, Vector3 originalPos)
    {
        if (originalPos.y - travelDist < platform.y && count == 1)
        {
            platform.y -= timeScale;
            transform.position = platform;
            if (platform.y - tolerance <= originalPos.y - travelDist)
            {
                return count--;
            }
        }
        return 1;
    }

    private int moveDiagRight(float timeScale, float travelDist, Vector3 originalPos, int specficiDirectionY)
    {
        if (originalPos.y + travelDist > platform.y && count == 0)
        {
            if (specficiDirectionY == 0)
            {
                platform.y += timeScale;
                platform.x += timeScale;
            }
            if (specficiDirectionY == 1)
            {
                platform.y -= timeScale;
                platform.x += timeScale;
            }
            transform.position = platform;
            if (platform.x + tolerance >= originalPos.x + travelDist)
            {
                return count++;
            }
        }
        return 0;
    }

    private int moveDiagLeft(float timeScale, float travelDist, Vector3 originalPos, int specficiDirectionY)
    {
        if (originalPos.y - travelDist < platform.y && count == 1)
        {
            if (specficiDirectionY == 0)
            {
                platform.y -= timeScale;
                platform.x -= timeScale;
            }
            if (specficiDirectionY == 1)
            {
                platform.y += timeScale;
                platform.x -= timeScale;
            }
            transform.position = platform;
            if (platform.x - tolerance <= originalPos.x - travelDist)
            {
                return count--;
            }
        }
        return 1;
    }

    private void moveLeftRight(float timeScale, float travelDist, Vector3 originalPos)
    {
        moveLeft(timeScale, travelDist, originalPos);
        moveRight(timeScale, travelDist, originalPos);
    }

    private void moveUpDown(float timeScale, float travelDist, Vector3 originalPos)
    {
        moveUp(timeScale, travelDist, originalPos);
        moveDown(timeScale, travelDist, originalPos);
    }

    private void moveDiagBotLeftTopRight(float timeScale, float travelDist, Vector3 originalPos, int specficiDirectionX, int specficiDirectionY)
    {
        moveDiagLeft(timeScale, travelDist, originalPos, specficiDirectionY);
        moveDiagRight(timeScale, travelDist, originalPos, specficiDirectionX);
    }

    private void moveDiagBotRightTopLeft(float timeScale, float travelDist, Vector3 originalPos, int specficiDirectionX, int specficiDirectionY)
    {
        moveDiagLeft(timeScale, travelDist, originalPos, specficiDirectionY);
        moveDiagRight(timeScale, travelDist, originalPos, specficiDirectionX);
    }

    protected override void Start()
    {
        base.Start();
        platform = platformOrigin.position;
        originalPos = platform;
    }

    public override void TimeScaledFixedUpdate(float timeScale)
    {
        platform = platformOrigin.position;
        if (!transformRewind.IsRewinding)
        {
            if (direction == 0)
            {
                moveLeftRight(timeScale, travelDist, originalPos);
            }
            if (direction == 1)
            {
                moveUpDown(timeScale, travelDist, originalPos);
            }
            if (direction == 3)
            {
                moveDiagBotLeftTopRight(timeScale, travelDist, originalPos, 0, 0);
            }
            if (direction == 4)
            {
                moveDiagBotRightTopLeft(timeScale, travelDist, originalPos, 1, 1);
            }
        }
    }

    public override void TimeScaledUpdate(float timeScale)
    {
    }

    // for diagonal ones use input
}