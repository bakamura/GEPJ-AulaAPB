using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour {

    [Header("Input")]

    [SerializeField] private float _inputRememberTime;

    [Header("Shoot")]

    [SerializeField] private KeyCode _shootKey;
    private float _shootInput;
    [SerializeField] private GameObject _shootPrefab;
    [SerializeField] private Vector2 _shootSpawnOffset;
    [SerializeField] private float _shootCooldown;
    private float _shootCooldownCurrent;

    private List<Bullet> _shootInstances = new List<Bullet>();

    [Header("Cache")]

    private bool _hasShootCache;

    private void Update() {
        DecreaseInputTimer();
        GetInput();

        DecreaseCooldownTimer();
        if (_shootInput > 0) {
            _shootInput = 0;
            Shoot();
        }
    }

    private void DecreaseInputTimer() {
        if (_shootInput > 0) _shootInput -= Time.deltaTime;
    }

    private void GetInput() {
        if (Input.GetKeyDown(_shootKey)) _shootInput = _inputRememberTime;
    }

    private void Shoot() {
        if (_shootCooldownCurrent <= 0) {
            _shootCooldownCurrent = _shootCooldown;

            _hasShootCache = false;
            if (_shootInstances.Count > 0) {
                foreach (Bullet bullet in _shootInstances) {
                    if (!bullet.IsActive) {
                        bullet.Shoot(transform.position + (Vector3)_shootSpawnOffset, (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - ((Vector2)transform.position + _shootSpawnOffset));
                        _hasShootCache = true;
                        break;
                    }
                }
            }
            if (!_hasShootCache) {
                _shootInstances.Add(Instantiate(_shootPrefab, (Vector2)transform.position + _shootSpawnOffset, new Quaternion(0, 0, 0, 0)).GetComponent<Bullet>());
                _shootInstances[_shootInstances.Count - 1].Shoot(transform.position + (Vector3)_shootSpawnOffset, (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - ((Vector2)transform.position + _shootSpawnOffset));
            }
        }
    }

    private void DecreaseCooldownTimer() {
        if (_shootCooldownCurrent > 0) _shootCooldownCurrent -= Time.deltaTime;
    }

}
