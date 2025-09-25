using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
  public int maxHealth = 50;
  private int currentHealth;
  private Animator anim;

  public bool IsDead { get; private set; } = false;

  void Start()
  {
    currentHealth = maxHealth;
    anim = GetComponent<Animator>();
  }

  public void TakeDamage(int damage)
  {
    if (IsDead) return;

    currentHealth -= damage;
    Debug.Log("Enemy trúng đòn, còn " + currentHealth + " HP");
    anim.SetTrigger("Hurt");

    if (currentHealth <= 0)
    {
      Die();
    }
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
