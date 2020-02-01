using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

[Serializable]
public struct RepairableTransform
{
    public RepairableTransform(Vector3 position, Quaternion rotation, Transform transform = null)
    {
        this.position = position;
        this.rotation = rotation;
        this.transform = transform;
    }

    public Transform transform;
    public Vector3 position;
    public Quaternion rotation;
}

[ExecuteInEditMode]
[RequireComponent(typeof(Repairable))]
public class RepairableEditor : MonoBehaviour
{
    [SerializeField] private bool savePositions;

    [SerializeField] private List<RepairableTransform> childTransforms;

    private void Update()
    {
        if (savePositions)
        {
            SavePositions();
            savePositions = false;
        }
    }

    private void SavePositions()
    {
        var transforms = GetComponentsInChildren<Transform>().Skip(1).ToList();

        childTransforms = new List<RepairableTransform>();
        foreach (Transform trans in transforms)
        {
            childTransforms.Add(new RepairableTransform(trans.position, trans.rotation, trans));
        }
    }

    public List<RepairableTransform> GetChildFixedTransforms() => childTransforms;
}