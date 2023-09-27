using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    [Header("Horizontal Movement")]

    [SerializeField] private KeyCode _movementLeftKey;
    [SerializeField] private KeyCode _movementRightKey;
    private int _inputMovement;

    [Space(16)]

    [SerializeField] private float _movementSpeed;
    private Rigidbody2D _rb;

    [Header("Jump")]

    [SerializeField] private KeyCode _jumpKey;
    private bool _inputJump = false;

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

    [Header("Cache")]

    private Vector2 _walljumpVector = Vector2.one.normalized;

    private void Awake() {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Update() {
        GetInput();
    }

    private void FixedUpdate() {
        HorizontalMovement();
        Jump();
        WallSlide();
        WallJump();
    }

    private void GetInput() {
        // Horizontal Movement
        _inputMovement = (Input.GetKey(_movementLeftKey) ? -1 : 0) + 
                         (Input.GetKey(_movementRightKey) ? 1 : 0);

        // Jump Trigger
        if (Input.GetKeyDown(_jumpKey)) _inputJump = true;
    }

    private void HorizontalMovement() {
        _rb.velocity = new Vector2(_inputMovement * _movementSpeed, _rb.velocity.y);
    }

    private void Jump() {
        if (_inputJump && IsGrounded()) {
            _inputJump = false;

            _rb.velocity = new Vector2(_rb.velocity.x, Mathf.Sqrt(2 * -Physics2D.gravity.y * _jumpHeight));
        }
    }

    private bool IsGrounded() {
        return Physics2D.OverlapBox((Vector2) transform.position + _groundcheckOffset, _groundcheckBox, 0, _groundcheckLayer);
    }

    private void WallSlide() {
        _rb.gravityScale = IsSlidingWall(_inputMovement) ? _wallslideGravityScale : 1;
    }

    private void WallJump() {
        if (_inputJump && IsSlidingWall(_inputMovement)) {
            _inputJump = false;

            _rb.velocity = (_inputMovement > 0 ? -_walljumpVector : _walljumpVector) * _jumpHeight; // Maybe specific force
        }
    }

    private bool IsSlidingWall(int direction) {
        if (direction == 0 || _rb.velocity.y > 0) return false;

        return Physics2D.OverlapBox((Vector2)transform.position + (direction > 0 ? _wallslideOffset : -_wallslideOffset), _wallslideBox, 0, _wallslideLayer);
    }

#if UNITY_EDITOR
    [Header("Debug")]

    [SerializeField] private bool _showDebugGizmos;

    private void OnDrawGizmosSelected() {
        Gizmos.color = IsGrounded() ? Color.green : Color.red;
        Gizmos.DrawWireCube((Vector2)transform.position + _groundcheckOffset, _groundcheckBox);

        Gizmos.color = IsSlidingWall(_inputMovement) ? Color.green : Color.red;
        if(_inputMovement != 0) Gizmos.DrawWireCube((Vector2)transform.position + (_inputMovement > 0 ? _wallslideOffset : -_wallslideOffset), _wallslideBox);
    }
#endif

}
