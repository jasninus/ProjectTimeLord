using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Damageable : MonoBehaviour, IDataPersister
{
    [Serializable]
    public class HealthEvent : UnityEvent<Damageable> { }

    [Serializable]
    public class DamageEvent : UnityEvent<Damager, Damageable> { }

    [Serializable]
    public class HealEvent : UnityEvent<int, Damageable> { }

    public int startingHealth = 5;
    public bool invulnerableAfterDamage = true;
    public float invulnerabilityDuration = 3f;
    public bool disableOnDeath = false;
    public Vector2 centerOffset = new Vector2(0f, 1f);
    public HealthEvent OnHealthSet;
    public HealEvent OnGainHealth;
    public DamageEvent OnTakeDamage;
    public DamageEvent OnDie;

    [HideInInspector]
    public DataSettings dataSettings;

    protected bool invulnerable;
    protected float invulnerabilityTimer;
    protected int currentHealth;
    protected Vector2 damageDirection;
    protected bool resetHealthOnSceneReload;

    public int CurrentHealth => currentHealth;

    private void OnEnable()
    {
        PersistentDataManager.RegisterPersister(this);
        currentHealth = startingHealth;

        OnHealthSet.Invoke(this);

        DisableInvulnerability();
    }

    private void OnDisable()
    {
        PersistentDataManager.UnregisterPersister(this);
    }

    private void Update()
    {
        if (invulnerable)
        {
            invulnerabilityTimer -= Time.deltaTime;

            if (invulnerabilityTimer <= 0f)
            {
                Debug.Log("Disabling invincibility");
                invulnerable = false;
            }
        }
    }

    public void EnableInvulnerability(bool ignoreTimer = false)
    {
        invulnerable = true;
        invulnerabilityTimer = ignoreTimer ? float.MaxValue : invulnerabilityDuration;
    }

    public void DisableInvulnerability()
    {
        Debug.Log("Disabling invincibility");
        invulnerable = false;
    }

    public Vector2 GetDamageDirection()
    {
        return damageDirection;
    }

    public virtual void TakeDamage(Damager damager, bool ignoreInvincible = false)
    {
        if ((invulnerable && !ignoreInvincible) || currentHealth <= 0)
        {
            return;
        }

        if (!invulnerable)
        {
            currentHealth -= damager.damage;
            OnHealthSet.Invoke(this);
            EnableInvulnerability();
        }

        damageDirection = transform.position + (Vector3)centerOffset - damager.transform.position;

        if (damager.forceRespawn)
            damageDirection = Vector3.zero;
        OnTakeDamage.Invoke(damager, this);

        if (currentHealth <= 0)
        {
            OnDie.Invoke(damager, this);
            resetHealthOnSceneReload = true;
            if (disableOnDeath)
            {
                gameObject.SetActive(false);
            }
        }
    }

    public void GainHealth(int amount)
    {
        currentHealth += amount;

        if (currentHealth > startingHealth)
        {
            currentHealth = startingHealth;
        }

        OnHealthSet.Invoke(this);

        OnGainHealth.Invoke(amount, this);
    }

    public void SetHealth(int amount)
    {
        currentHealth = amount;

        OnHealthSet.Invoke(this);
    }

    public DataSettings GetDataSettings()
    {
        return dataSettings;
    }

    public void SetDataSettings(string dataTag, DataSettings.PersistenceType persistenceType)
    {
        dataSettings.dataTag = dataTag;
        dataSettings.persistenceType = persistenceType;
    }

    public Data SaveData()
    {
        return new Data<int, bool>(CurrentHealth, resetHealthOnSceneReload);
    }

    public void LoadData(Data data)
    {
        Data<int, bool> healthData = (Data<int, bool>)data;
        currentHealth = healthData.value1 ? startingHealth : healthData.value0;
        OnHealthSet.Invoke(this);
    }
}