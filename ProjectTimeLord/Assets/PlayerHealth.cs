using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : Damageable
{
    [SerializeField] private HealthBar healthBar;

    public override void TakeDamage(Damager damager, bool ignoreInvincible = false)
    {
        base.TakeDamage(damager, ignoreInvincible);
        healthBar.SetFill(currentHealth / (float)startingHealth);
    }
}