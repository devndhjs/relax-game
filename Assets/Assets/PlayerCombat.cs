using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
  [Header("Combat Settings")]
  public int damage = 10;
  public float attackCooldown = 0.5f;
  private float nextAttackTime = 0f;

  [Header("Attack Range")]
  public Transform attackPoint;
  public float attackRange = 2f;
  [Range(0, 360)] public float attackAngle = 90f;
  public LayerMask enemyLayer;

  private Animator anim;

  void Start()
  {
    anim = GetComponent<Animator>();
  }

  // Gọi từ input
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

    Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(
        attackPoint.position,
        attackRange,
        enemyLayer
    );

    HashSet<EnemyHealth> damaged = new HashSet<EnemyHealth>();

    foreach (Collider2D enemy in hitEnemies)
    {
      EnemyHealth eh = enemy.GetComponent<EnemyHealth>();
      if (eh == null || damaged.Contains(eh)) continue;

      Vector2 dirToTarget = (enemy.transform.position - attackPoint.position).normalized;
      Vector2 forward = GetFacingDirection();

      float angle = Vector2.Angle(forward, dirToTarget);

      if (angle <= attackAngle / 2f)
      {
        Debug.Log("Player trúng Enemy sau delay: " + enemy.name);
        eh.TakeDamage(damage, transform);
        damaged.Add(eh); // đảm bảo không bị gọi lần 2
      }
    }
  }


  // Hướng mặt player dựa vào scale
  private Vector2 GetFacingDirection()
  {
    return transform.localScale.x > 0 ? Vector2.right : Vector2.left;
  }

  void OnDrawGizmosSelected()
  {
    if (attackPoint == null) return;

    // vòng tròn range
    Gizmos.color = Color.green;
    Gizmos.DrawWireSphere(attackPoint.position, attackRange);

    // vẽ quạt
    Vector3 forward = Vector3.right;
    float halfAngle = attackAngle / 2f;

    Quaternion leftRot = Quaternion.Euler(0, 0, -halfAngle);
    Quaternion rightRot = Quaternion.Euler(0, 0, halfAngle);

    Vector3 leftDir = leftRot * forward * attackRange;
    Vector3 rightDir = rightRot * forward * attackRange;

    Gizmos.color = Color.cyan;
    Gizmos.DrawLine(attackPoint.position, attackPoint.position + leftDir);
    Gizmos.DrawLine(attackPoint.position, attackPoint.position + rightDir);
  }
}
