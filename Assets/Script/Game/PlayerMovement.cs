using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    [Header("Input")]

    [SerializeField] private float _inputRememberTime;

    [Header("Horizontal Movement")]

    [SerializeField] private KeyCode _movementLeftKey;
    [SerializeField] private KeyCode _movementRightKey;
    private int _inputMovement;
    private bool _canMove = true;

    [Space(16)]

    [SerializeField] private float _movementSpeed;
    private Rigidbody2D _rb;

    [Header("Jump")]

    [SerializeField] private KeyCode _jumpKey;
    private float _inputJump;

    [Space(16)]

    [SerializeField] private float _jumpHeight;

    [Space(16)]

    [SerializeField] private Vector2 _groundcheckOffset;
    [SerializeField] private Vector2 _groundcheckBox;
    [SerializeField] private LayerMask _groundcheckLayer;

    [Header("Wall Slide")]

    [SerializeField] private float _wallslideGravityScale;
    [SerializeField] private Vector2 _wallslideOffset;
    [SerializeField] private Vector2 _wallslideBox;
    [SerializeField] private LayerMask _wallslideLayer;

    [Space(16)]

    [SerializeField] private Vector2 _walljumpVector;
    [SerializeField] private float _wallJumpRegainControlTime;

    [Header("Cache")]

    private Vector2 _velocityCache;
    private WaitForSeconds _wallJumpRegainControlWait;
    private Collider2D _col;

    // Access

    public Collider2D Collider { get { return _col; } }

    private void Awake() {
        _rb = GetComponent<Rigidbody2D>();
        _col = GetComponent<Collider2D>();
        _wallJumpRegainControlWait = new WaitForSeconds(_wallJumpRegainControlTime);

        _walljumpVector = _walljumpVector.normalized;
    }

    private void Update() {
        DecreseInputTimer();
        GetInput();
        if (_canMove) {
            _velocityCache = _rb.velocity;
            Jump();
            WallJump();
            _rb.velocity = _velocityCache;
        }
    }

    private void FixedUpdate() {
        if (_canMove) {
            _velocityCache = _rb.velocity;
            HorizontalMovement();
            WallSlide();
            _rb.velocity = _velocityCache;
        }
    }

    private void DecreseInputTimer() {
        if (_inputJump > 0) _inputJump -= Time.deltaTime;
    }

    private void GetInput() {
        // Horizontal Movement
        _inputMovement = (Input.GetKey(_movementLeftKey) ? -1 : 0) +
                         (Input.GetKey(_movementRightKey) ? 1 : 0);

        // Jump Trigger
        if (Input.GetKeyDown(_jumpKey)) _inputJump = _inputRememberTime;
    }

    private void HorizontalMovement() {
        _velocityCache[0] = _inputMovement * _movementSpeed;
    }

    private void Jump() {
        if (_inputJump > 0 && IsGrounded()) {
            _inputJump = 0;

            _velocityCache[1] = Mathf.Sqrt(2 * -Physics2D.gravity.y * _jumpHeight);
        }
    }

    private bool IsGrounded() {
        return Physics2D.OverlapBox((Vector2)transform.position + _groundcheckOffset, _groundcheckBox, 0, _groundcheckLayer);
    }

    private void WallSlide() {
        _rb.gravityScale = IsSlidingWall(_inputMovement) ? _wallslideGravityScale : 1;
    }

    private void WallJump() {
        if (_inputJump > 0 && IsSlidingWall(_inputMovement)) {
            _inputJump = 0;
            StartCoroutine(WallJumpRoutine());
        }
    }

    private IEnumerator WallJumpRoutine() {
        _rb.gravityScale = 1;
        _canMove = false;

        _walljumpVector[0] = Mathf.Abs(_walljumpVector[0]) * (_inputMovement < 0 ? 1 : -1);
        _rb.velocity = _walljumpVector * Mathf.Sqrt(2 * -Physics2D.gravity.y * _jumpHeight); // Maybe specific force

        yield return _wallJumpRegainControlWait;

        _canMove = true;
    }

    private bool IsSlidingWall(int direction) {
        if (direction == 0 || _rb.velocity.y > 0) return false;

        return Physics2D.OverlapBox((Vector2)transform.position + (direction > 0 ? _wallslideOffset : -_wallslideOffset), _wallslideBox, 0, _wallslideLayer);
    }

#if UNITY_EDITOR
    [Header("Debug")]

    [SerializeField] private bool _showDebugGizmos;

    private void OnDrawGizmosSelected() {
        if (_showDebugGizmos) {
            Gizmos.color = IsGrounded() ? Color.green : Color.red;
            Gizmos.DrawWireCube((Vector2)transform.position + _groundcheckOffset, _groundcheckBox);

            Gizmos.color = IsSlidingWall(_inputMovement) ? Color.green : Color.red;
            if (_inputMovement != 0) Gizmos.DrawWireCube((Vector2)transform.position + (_inputMovement > 0 ? _wallslideOffset : -_wallslideOffset), _wallslideBox);
        }
    }
#endif

}
