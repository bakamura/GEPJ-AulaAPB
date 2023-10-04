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
        if (_shootCooldownCurrent < 0) {
            _shootCooldownCurrent = _shootCooldown;
            GameObject shot = Instantiate(_shootPrefab, (Vector2)transform.position + _shootSpawnOffset, new Quaternion(0, 0, 0, 0));
            shot.GetComponent<Rigidbody2D>().velocity = ((Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - ((Vector2)transform.position + _shootSpawnOffset)).normalized * 6.66f;
        }
    }

    private void DecreaseCooldownTimer() {
        if (_shootInput > 0) _shootCooldownCurrent -= Time.deltaTime;
    }

}
