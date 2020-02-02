using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class FireHydrant : MonoBehaviour, IRewindable
{
    [SerializeField] private Sprite fixedSprite;

    [SerializeField] private ParticleSystem particle;

    [SerializeField] private Collider2D waterColl;

    private SpriteRenderer rend;

    private void Awake()
    {
        rend = GetComponent<SpriteRenderer>();
    }

    public void StartRewind()
    {
        particle.Stop(true);
        rend.sprite = fixedSprite;
        waterColl.enabled = false;
    }
}