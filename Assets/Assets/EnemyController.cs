using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyController : MonoBehaviour
{
  public float moveSpeed = 3f;
  public float chaseRange = 5f;
  public Transform player;

  private Rigidbody2D rb;
  private Animator anim;

  private Vector2 movement;
  private Vector2 lastMoveDir = Vector2.right;
  private enum State { Idle, Chase, Attack, Dead }
  private State currentState = State.Idle;

  private EnemyCombat combat;
  private EnemyHealth health;

  private bool isAggro = false; // <--- Thêm cờ này

  void Start()
  {
    rb = GetComponent<Rigidbody2D>();
    anim = GetComponent<Animator>();
    combat = GetComponent<EnemyCombat>();
    health = GetComponent<EnemyHealth>();
  }

  void Update()
  {
    if (health.IsDead) return;

    float distance = Vector2.Distance(transform.position, player.position);

    // check state
    if (combat.CanAttackPlayer() && isAggro)
    {
      currentState = State.Attack;
    }
    else if (distance <= chaseRange && isAggro)
    {
      currentState = State.Chase;
    }
    else
    {
      currentState = State.Idle;
    }

    // xử lý theo state
    switch (currentState)
    {
      case State.Idle:
        anim.SetBool("isWalking", false);
        movement = Vector2.zero;
        break;

      case State.Chase:
        anim.SetBool("isWalking", true);
        movement = (player.position - transform.position).normalized;
        break;

      case State.Attack:
        anim.SetBool("isWalking", false);
        movement = Vector2.zero;
        combat.AttemptAttack();
        break;
    }
  }

  void FixedUpdate()
  {
    if (movement != Vector2.zero)
    {
      rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);

      lastMoveDir = movement.normalized;

      if (movement.x > 0.01f)
        transform.localScale = new Vector3(1, 1, 1);
      else if (movement.x < -0.01f)
        transform.localScale = new Vector3(-1, 1, 1);
    }
  }

  public Vector2 GetFacingDirection()
  {
    return lastMoveDir;
  }

  // Hàm này gọi khi bị đánh
  public void TriggerAggro()
  {
    isAggro = true;
  }
}
