using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Image fill;

    [SerializeField] private Transform followTarget;

    private RectTransform rectTransform;

    private Vector3 offset;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void Start()
    {
        offset = rectTransform.position - Camera.main.WorldToScreenPoint(followTarget.position);
    }

    private void FixedUpdate()
    {
        rectTransform.position = Camera.main.WorldToScreenPoint(followTarget.position) + offset;
    }

    public void SetFill(float amount)
    {
        fill.fillAmount = amount;
    }
}