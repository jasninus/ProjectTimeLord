using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidbodyRewindReset : RigidbodyRewind
{
    private List<TransformTimePoint> resetTimePoints = new List<TransformTimePoint>();

    [SerializeField] private float delayPerFrameRewind, delayDecreasePerRewoundFrame;

    [SerializeField] private byte skipFrames;
    private byte saveFrameCounter;

    public Action onResetFinish;

    public void ResetObject()
    {
        StartRewind();
        StartCoroutine(RewindingReset());
    }

    private IEnumerator RewindingReset()
    {
        yield return new WaitUntil(() => !IsRewinding);

        float rewindDelay = delayPerFrameRewind;
        int tpCount = resetTimePoints.Count;

        for (int i = tpCount - 1; i >= 0; i--)
        {
            ApplyTimePoint(resetTimePoints[i]);
            yield return new WaitForSeconds(rewindDelay);
            rewindDelay -= delayDecreasePerRewoundFrame;
        }

        onResetFinish?.Invoke();
        resetTimePoints.Clear();
        timePoints.Clear();
    }

    protected override void RemoveOldestTimePoint()
    {
        if (++saveFrameCounter % skipFrames == 0)
        {
            resetTimePoints.Add(timePoints[0].transformTimePoint);
            saveFrameCounter = 0;
        }

        base.RemoveOldestTimePoint();
    }
}