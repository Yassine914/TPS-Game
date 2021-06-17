using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float speed = 6f;
    [SerializeField] private float sprintSpeed = 12f;
    [SerializeField] private float crouchSpeed = 2f;
    [SerializeField] private float jumpForce = 5f;
    private bool _isCrouching;
    private bool _isSprinting;
    private bool _isGrounded;
    private Rigidbody _rb;
    
    private void Start()
    {
        _isGrounded = false;
        _rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        Move();
        Crouch();
        Sprint();
        Jump();
    }

    private void Move()
    {
        var horizontalInput = Input.GetAxisRaw("Horizontal");
        var verticalInput = Input.GetAxisRaw("Vertical");
        float allSpeed;
        
        if (_isCrouching)
            allSpeed = crouchSpeed;
        else if (_isSprinting)
            allSpeed = sprintSpeed;
        else
            allSpeed = speed;
        
        transform.Translate(horizontalInput * allSpeed * Time.deltaTime, 0, verticalInput * allSpeed * Time.deltaTime);
    }

    private void Sprint()
    {
        _isSprinting = Input.GetKey(KeyCode.LeftShift);
    }

    private void Crouch()
    {
        if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.C))
        {
            _isCrouching = true;
            
            var toCrouchScale = new Vector3(1, 0.6f, 1);
            transform.localScale = toCrouchScale;
        }
        else
        {
            _isCrouching = false;
            
            var fromCrouchScale = new Vector3(1, 1, 1);
            transform.localScale = fromCrouchScale;
        }
    }

    private void OnCollisionStay()
    {
        _isGrounded = true;
    }

    private void OnCollisionExit()
    {
        _isGrounded = false;
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && _isGrounded && !_isCrouching)
        {
            _rb.AddForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse);
            _isGrounded = false;
        }
    }
}
