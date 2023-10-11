using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    [Header("Parameters")]

    [SerializeField] private float _shootVelocity;
    [SerializeField] private float _shootDeactivateTime;
    private bool _isActive;

    [Header("Cache")]

    private Rigidbody2D _rb;
    private SpriteRenderer _sr;
    private WaitForSeconds _shootDeactivateWait;

    // Access

    public bool IsActive { get { return _isActive; } }

    private void Awake() {
        _rb = GetComponent<Rigidbody2D>();
        _sr = GetComponent<SpriteRenderer>();
        _shootDeactivateWait = new WaitForSeconds(_shootDeactivateTime);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Finish")) {
            _isActive = false;
            _rb.simulated = false;
            _sr.enabled = false;
        }
    }

    public void Shoot(Vector3 shootPos, Vector2 direction) {
        _isActive = true;
        _rb.simulated = true;
        _sr.enabled = true;
        transform.position = shootPos;
        _rb.velocity = direction.normalized * _shootVelocity;

        StartCoroutine(Deactivate());
    }

    private IEnumerator Deactivate() {
        yield return _shootDeactivateWait;

        _isActive = false;
        _rb.simulated = false;
        _sr.enabled = false;
    }

}
