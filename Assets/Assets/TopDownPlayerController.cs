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

    private PlayerCombat combat; // Tham chiếu combat

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        controls = new Controls();
        controls.Player.SetCallbacks(this);
        anim = GetComponent<Animator>();
        combat = GetComponent<PlayerCombat>();
    }

    void OnEnable() => controls.Player.Enable();
    void OnDisable() => controls.Player.Disable();

    void FixedUpdate()
    {
        // Di chuyển
        rb.MovePosition(rb.position + moveInput * moveSpeed * Time.fixedDeltaTime);

        // Animation Idle/Walk
        bool walking = moveInput.sqrMagnitude > 0.01f;
        anim.SetBool("isWalking", walking);

        // Flip hướng
        if (moveInput.x > 0.01f)
            transform.localScale = new Vector3(1, 1, 1);
        else if (moveInput.x < -0.01f)
            transform.localScale = new Vector3(-1, 1, 1);
    }

    // --- Input System Callbacks ---
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
            combat.AttemptAttack();
        // else if (context.canceled)
        //     combat.StopAttack();
    }
}
