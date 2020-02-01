using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.Events;

public class PressurePad : MonoBehaviour
{
    public enum ActivationType
    {
        ItemCount, ItemMass
    }

    public PlatformCatcher platformCatcher;
    public ActivationType activationType;
    public int requiredCount;
    public float requiredMass;
    public Sprite deactivatedBoxSprite;
    public Sprite activatedBoxSprite;
    public SpriteRenderer[] boxes;
    public UnityEvent OnPressed;
    public UnityEvent OnReleased;

    protected bool eventFired;

    static int DELAYEDFRAME_COUNT = 2;
    protected int activationFrameCount = 0;
    protected bool previousWasPressed = false;

    private void FixedUpdate()
    {
        if (activationType == ActivationType.ItemCount)
        {
            if (platformCatcher.CaughtObjectCount >= requiredCount)
            {
                if (!previousWasPressed)
                {
                    previousWasPressed = true;
                    activationFrameCount = 1;
                }
                else
                    activationFrameCount += 1;

                if (activationFrameCount > DELAYEDFRAME_COUNT && !eventFired)
                {
                    OnPressed.Invoke();
                    eventFired = true;
                }
            }
            else
            {
                if (previousWasPressed)
                {
                    previousWasPressed = false;
                    activationFrameCount = 1;
                }
                else
                    activationFrameCount += 1;

                if (activationFrameCount > DELAYEDFRAME_COUNT && eventFired)
                {
                    OnReleased.Invoke();
                    eventFired = false;
                }
            }
        }
        else
        {
            if (platformCatcher.CaughtObjectsMass >= requiredMass)
            {
                if (!previousWasPressed)
                {
                    previousWasPressed = true;
                    activationFrameCount = 1;
                }
                else
                    activationFrameCount += 1;

                if (activationFrameCount > DELAYEDFRAME_COUNT && !eventFired)
                {
                    OnPressed.Invoke();
                    eventFired = true;
                }
            }
            else
            {
                if (previousWasPressed)
                {
                    previousWasPressed = false;
                    activationFrameCount = 1;
                }
                else
                    activationFrameCount += 1;

                

                if (activationFrameCount > DELAYEDFRAME_COUNT && eventFired)
                {
                    OnReleased.Invoke();
                    eventFired = false;
                }
            }
        }

        for (int i = 0; i < boxes.Length; i++)
        {
            boxes[i].sprite = platformCatcher.HasCaughtObject(boxes[i].gameObject) ? activatedBoxSprite : deactivatedBoxSprite;
        }
    }
}
