using System.Collections;
using System.Collections.Generic;
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

    [SerializeField] private float _jumpForce; //

    private void Awake() {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Update() {
        GetInput();
    }

    private void FixedUpdate() {
        HorizontalMovement();
        Jump();
    }

    // Remember teach cooler way
    private void GetInput() {
        // Horizontal Movement
        int i = 0;
        if(Input.GetKey(_movementLeftKey)) i--;
        if(Input.GetKey(_movementRightKey)) i++;
        _inputMovement = i;

        if (Input.GetKeyDown(_jumpKey)) _inputJump = true;
    }

    private void HorizontalMovement() {
        _rb.velocity = new Vector2(_inputMovement * _movementSpeed, _rb.velocity.y);
    }

    // Teach Jump based on height
    private void Jump() {
        if (_inputJump) {
            _inputJump = false;

            //_rb.AddForce(new Vector2(0, _jumpForce));//
            _rb.velocity = new Vector2(_rb.velocity.x, _jumpForce);
        }
    }

}
