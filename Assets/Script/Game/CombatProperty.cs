using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatProperty : MonoBehaviour {

    [Header("Health")]

    [SerializeField] private int _healthMax;
    private int _healthCurrent;

    [Header("Cache")]

    private Rigidbody2D _rb;
    private SpriteRenderer _sr;

    private void Awake() {
        _rb = GetComponent<Rigidbody2D>();
        _sr = GetComponent<SpriteRenderer>();

        _healthCurrent = _healthMax;
    }

    public void TakeDamage(int damageTaken) {
        _healthCurrent -= damageTaken;
        if(_healthCurrent <= 0) Defeated();
    }

    private void Defeated() {
        Deactivate();
    }

    public void Deactivate() {
        gameObject.SetActive(false);
        _rb.isKinematic = true;
        _sr.enabled = false;
    }

}
