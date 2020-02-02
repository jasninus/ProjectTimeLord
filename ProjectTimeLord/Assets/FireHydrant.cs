using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class FireHydrant : MonoBehaviour, IRewindable, IResettable
{
    [SerializeField] private Sprite fixedSprite;
    private Sprite brokenSprite;

    [SerializeField] private ParticleSystem particle;

    [SerializeField] private Collider2D waterColl;

    private SpriteRenderer rend;

    private void Awake()
    {
        rend = GetComponent<SpriteRenderer>();
        brokenSprite = rend.sprite;
    }

    public void StartRewind()
    {
        particle.Stop(true);
        rend.sprite = fixedSprite;
        waterColl.enabled = false;
    }

    public void ResetObject()
    {
        particle.Play(true);
        rend.sprite = brokenSprite;
        waterColl.enabled = true;
    }
}