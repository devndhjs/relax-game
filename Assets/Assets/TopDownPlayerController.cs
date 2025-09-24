using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class TopDownPlayerController : MonoBehaviour, Controls.IPlayerActions
{
    public float moveSpeed = 5f;

    private Rigidbody2D rb;
    private Controls controls;
    private Vector2 moveInput;
    private Animator anim;
    private bool holdingAttack = false;
    public GameObject attackHitbox;
    private Collider2D hitboxCollider;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        controls = new Controls();
        controls.Player.SetCallbacks(this);
        anim = GetComponent<Animator>();
        hitboxCollider = attackHitbox.GetComponent<Collider2D>();
        hitboxCollider.enabled = false;
    }

    void OnEnable()
    {
        controls.Player.Enable();
    }

    void OnDisable()
    {
        controls.Player.Disable();
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + moveInput * moveSpeed * Time.fixedDeltaTime);

        // Animation Idle/Walk
        bool walking = moveInput.sqrMagnitude > 0.01f;
        anim.SetBool("isWalking", walking);

        if (moveInput.x > 0.01f)
        {
            transform.localScale = new Vector3(1, 1, 1); // quay phải
        }
        else if (moveInput.x < -0.01f)
        {
            transform.localScale = new Vector3(-1, 1, 1); // quay trái
        }
    }

    // --- Callback từ Controls.IPlayerActions ---
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            holdingAttack = true;
            anim.SetTrigger("isAttacking");
        }
        else if (context.canceled)
        {
            holdingAttack = false;
        }
    }

    public void TryContinueAttack()
    {
        if (holdingAttack)
        {
            anim.SetTrigger("isAttacking");
        }
    }
    public void EnableHitbox()
    {
        hitboxCollider.enabled = true;
    }
    public void DisableHitbox()
    {
        hitboxCollider.enabled = false;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("Trúng enemy: " + other.name);
            // Gọi hàm trừ máu enemy ở đây
            // other.GetComponent<EnemyHealth>()?.TakeDamage(10);
        }
    }
}
