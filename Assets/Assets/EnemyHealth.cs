using System.Collections;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
  public int maxHealth = 50;
  private int currentHealth;
  private Animator anim;

  public bool IsDead { get; private set; } = false;
  private EnemyController enemyController;
  private Rigidbody2D rb;

  [Header("Knockback Settings")]
  public float knockbackDistance = 1f;
  public float knockbackSpeed = 10f;
  public float stunDuration = 0.5f;
  [HideInInspector] public bool isKnockback = false;


  void Start()
  {
    currentHealth = maxHealth;
    anim = GetComponent<Animator>();
    enemyController = GetComponent<EnemyController>();
    rb = GetComponent<Rigidbody2D>();
  }

  [System.Obsolete]
  public void TakeDamage(int damage, Transform attacker = null)
  {
    if (IsDead) return;

    enemyController.TriggerAggro();

    currentHealth -= damage;
    Debug.Log("Enemy trúng đòn, còn " + currentHealth + " HP");
    anim.SetTrigger("Hurt");

    if (attacker != null && !isKnockback)
    {
      StartCoroutine(KnockbackRoutine(attacker.position));
    }

    if (currentHealth <= 0)
    {
      Die();
    }
  }

  [System.Obsolete]
  private IEnumerator KnockbackRoutine(Vector3 attackerPos)
  {
    isKnockback = true;

    // Chặn enemy di chuyển trong khi knockback
    if (enemyController != null) enemyController.enabled = false;

    Vector2 dir = (transform.position - attackerPos).normalized;
    rb.velocity = dir * knockbackSpeed;  // đẩy ngay

    yield return new WaitForSeconds(knockbackDistance / knockbackSpeed);

    rb.velocity = Vector2.zero; // dừng lại

    // Giữ choáng thêm 0.5s
    yield return new WaitForSeconds(stunDuration);

    // Bật lại nếu còn sống
    if (!IsDead && enemyController != null)
    {
      enemyController.enabled = true;
    }

    isKnockback = false;
  }

  private void Die()
  {
    IsDead = true;
    Debug.Log("Enemy chết!");
    anim.SetTrigger("Die");

    GetComponent<EnemyController>().enabled = false;
    GetComponent<EnemyCombat>().enabled = false;
    this.enabled = false;
  }
}
