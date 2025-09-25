using System.Collections;
using UnityEngine;

public class EnemyCombat : MonoBehaviour
{
  [Header("Combat Settings")]
  public int damage = 10;
  public float attackCooldown = 1f;
  private float nextAttackTime = 0f;

  [Header("Attack Range")]
  public Transform attackPoint;
  public float attackRange = 3f;
  [Range(0, 360)] public float attackAngle = 180f;
  public LayerMask playerLayer;

  private Animator anim;
  private EnemyController controller;

  void Start()
  {
    anim = GetComponent<Animator>();
    controller = GetComponent<EnemyController>();
  }

  public bool CanAttackPlayer()
  {
    Collider2D[] hitPlayers = Physics2D.OverlapCircleAll(
        attackPoint.position,
        attackRange - 0.1f,
        playerLayer
    );

    foreach (Collider2D player in hitPlayers)
    {
      Vector2 forward = (transform.localScale.x > 0) ? Vector2.right : Vector2.left;
      Vector2 dirToTarget = (player.transform.position - attackPoint.position).normalized;

      float angle = Vector2.Angle(forward, dirToTarget);

      if (angle <= attackAngle / 2f)
      {
        return true;
      }
    }
    return false;
  }

  public void AttemptAttack()
  {
    if (Time.time >= nextAttackTime)
    {
      anim.SetTrigger("Attack");
      nextAttackTime = Time.time + attackCooldown;
    }
  }

  public void DoDamageWithDelay(float delay)
  {
    StartCoroutine(DoDamageDelayed(delay));
  }

  private IEnumerator DoDamageDelayed(float delay)
  {
    yield return new WaitForSeconds(delay);

    Vector2 forward = (transform.localScale.x > 0) ? Vector2.right : Vector2.left;
    Collider2D[] hits = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, playerLayer);

    foreach (Collider2D player in hits)
    {
      Vector2 dirToTarget = (player.transform.position - attackPoint.position).normalized;

      float angle = Vector2.Angle(forward, dirToTarget);

      if (angle <= attackAngle / 2f)
      {
        Debug.Log("Enemy trúng Player!");
        player.GetComponent<PlayerHealth>()?.TakeDamage(damage);
      }
    }
  }

  void OnDrawGizmosSelected()
  {
    if (attackPoint == null) return;

    Gizmos.color = Color.red;
    Gizmos.DrawWireSphere(attackPoint.position, attackRange);

    // Vẽ hướng hình quạt dựa theo localScale
    Vector2 forward = (transform.localScale.x > 0) ? Vector2.right : Vector2.left;
    float halfAngle = attackAngle / 2f;

    Quaternion leftRot = Quaternion.Euler(0, 0, -halfAngle);
    Quaternion rightRot = Quaternion.Euler(0, 0, halfAngle);

    Vector3 leftDir = leftRot * forward * attackRange;
    Vector3 rightDir = rightRot * forward * attackRange;

    Gizmos.color = Color.yellow;
    Gizmos.DrawLine(attackPoint.position, attackPoint.position + leftDir);
    Gizmos.DrawLine(attackPoint.position, attackPoint.position + rightDir);
  }
}
