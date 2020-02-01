using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(RepairableEditor))]
public class Repairable : MonoBehaviour, IRewindable, IResettable
{
    [SerializeField] private float repairTime;

    [SerializeField] private Transform repairedParent;

    [SerializeField] private List<Behaviour> activateOnFinishRepair = new List<Behaviour>();

    [SerializeField] private List<Rigidbody2D> rigidbodyActivateOnRepair = new List<Rigidbody2D>();

    [SerializeField] private AnimationCurve repairProgressCurve;

    private List<RepairableTransform> childTransforms = new List<RepairableTransform>();

    private bool repairing;

    private TransformTimePoint startTP;

    private void Awake()
    {
        startTP = new TransformTimePoint(transform.position, transform.rotation);
        childTransforms = GetComponent<RepairableEditor>().GetChildFixedTransforms();
    }

    public void StartRewind()
    {
        if (repairing)
        {
            return;
        }

        repairing = true;

        for (int i = 0; i < childTransforms.Count; i++)
        {
            Rigidbody2D rb = childTransforms[i].transform.GetComponent<Rigidbody2D>();
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
            rb.isKinematic = true;
            rb.velocity = Vector2.zero;
        }

        StartCoroutine(Repairing());
    }

    private IEnumerator Repairing()
    {
        List<RepairableTransform> brokenTransforms = SetupBrokenTransforms();

        float startTime = Time.time, endTime = Time.time + repairTime;

        while (endTime >= Time.time)
        {
            LerpChildren(brokenTransforms, startTime);
            yield return null;
        }

        FinishRepair();
    }

    private List<RepairableTransform> SetupBrokenTransforms()
    {
        List<RepairableTransform> brokenTransforms = new List<RepairableTransform>();

        foreach (RepairableTransform trans in childTransforms)
        {
            brokenTransforms.Add(new RepairableTransform(trans.transform.position, trans.transform.rotation));
        }

        return brokenTransforms;
    }

    private void LerpChildren(List<RepairableTransform> brokenTransforms, float startTime)
    {
        for (int i = 0; i < childTransforms.Count; i++)
        {
            childTransforms[i].transform.position = Vector3.Lerp(brokenTransforms[i].position, childTransforms[i].position,
                repairProgressCurve.Evaluate((Time.time - startTime) / repairTime));
            childTransforms[i].transform.rotation = Quaternion.Lerp(brokenTransforms[i].rotation,
                childTransforms[i].rotation, repairProgressCurve.Evaluate((Time.time - startTime) / repairTime));
        }
    }

    private void FinishRepair()
    {
        SnapPositionsDisableColliders();
        SetComponentsActive(true);
        SetRigidbodiesActive(true);
        CheckAssignRepairParent();
    }

    private void SnapPositionsDisableColliders()
    {
        for (int i = 0; i < childTransforms.Count; i++)
        {
            childTransforms[i].transform.GetComponent<Collider2D>().enabled = false;
            childTransforms[i].transform.position = childTransforms[i].position;
            childTransforms[i].transform.rotation = childTransforms[i].rotation;
        }
    }

    private void SetComponentsActive(bool value)
    {
        for (int i = 0; i < activateOnFinishRepair.Count; i++)
        {
            activateOnFinishRepair[i].enabled = value;
        }
    }

    private void SetRigidbodiesActive(bool value)
    {
        for (int i = 0; i < rigidbodyActivateOnRepair.Count; i++)
        {
            rigidbodyActivateOnRepair[i].isKinematic = !value;
            rigidbodyActivateOnRepair[i].constraints = value ? RigidbodyConstraints2D.None : RigidbodyConstraints2D.FreezeAll;
            rigidbodyActivateOnRepair[i].velocity = Vector2.zero;
        }
    }

    private void CheckAssignRepairParent()
    {
        if (repairedParent)
        {
            transform.parent = repairedParent;
        }
    }

    public void ResetObject()
    {
        SetRigidbodiesActive(false);
        SetComponentsActive(false);
        childTransforms[0].transform.GetComponent<TransformRewindReset>().onResetFinish += ResetRubbleRigidbodies;
    }

    private void ResetRubbleRigidbodies()
    {
        for (int i = 0; i < childTransforms.Count; i++)
        {
            childTransforms[i].transform.GetComponent<Collider2D>().enabled = true;
            Rigidbody2D rb = childTransforms[i].transform.GetComponent<Rigidbody2D>();
            rb.constraints = RigidbodyConstraints2D.None;
            rb.isKinematic = false;
            rb.velocity = Vector2.zero;
        }

        childTransforms[0].transform.GetComponent<TransformRewindReset>().onResetFinish -= ResetRubbleRigidbodies;
        repairing = false;

        ResetPositionRotation();
    }

    private void ResetPositionRotation()
    {
        Transform[] children = transform.GetComponentsInChildren<Transform>().Skip(1).ToArray();
        transform.DetachChildren();
        transform.position = startTP.position;
        transform.rotation = startTP.rotation;
        foreach (Transform child in children)
        {
            child.parent = transform;
        }
    }
}