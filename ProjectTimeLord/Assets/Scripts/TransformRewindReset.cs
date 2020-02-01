using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformRewindReset : TransformRewind
{
    private List<TransformTimePoint> resetTimePoints = new List<TransformTimePoint>();

    [SerializeField] private bool reset;

    [SerializeField] private byte skipFrames;
    private byte saveFrameCounter;

    private void Update()
    {
        if (reset)
        {
            reset = false;
            RewindReset();
        }
    }

    private void RewindReset()
    {
        StartRewind();
        StartCoroutine(RewindingReset());
    }

    private IEnumerator RewindingReset()
    {
        //yield return new WaitUntil(() => );
        yield return null;
    }

    protected override void RemoveOldestTimePoint()
    {
        if (++saveFrameCounter % skipFrames == 0)
        {
            resetTimePoints.Add(timePoints[0]);
            saveFrameCounter = 0;
        }

        base.RemoveOldestTimePoint();
    }
}