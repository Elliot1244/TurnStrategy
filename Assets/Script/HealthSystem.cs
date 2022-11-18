using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HealthSystem : MonoBehaviour
{
    public event EventHandler OnDead;
    public event EventHandler OnDamaged;

    [SerializeField] private int _health;
    private int _healthMax;

    private void Awake()
    {
        _healthMax = _health;
    }

    public void Damage(int damageAmount)
    {
        _health -= damageAmount;

        if (_health < 0)
        {
            _health = 0;
        }

        //A chaque fois que des dégats sont subis, on déclenche l'event
        OnDamaged?.Invoke(this, EventArgs.Empty);

        if(_health == 0)
        {
            Die();
        }

        Debug.Log(_health);
    }

    private void Die()
    {
        OnDead?.Invoke(this, EventArgs.Empty);
    }

    //Récupère la vie. le (float) est là pour pouvoir faire la division entre deux int sans que la convertion en int ne supprime les décimales.
    public float GetHealthNormalized()
    {
        return (float)_health / _healthMax;
    }

    public int Health => _health;
}
