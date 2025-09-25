using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
  public int maxHealth = 100;
  private int currentHealth;

  private Animator anim;

  void Start()
  {
    currentHealth = maxHealth;
    anim = GetComponent<Animator>();
  }

  public void TakeDamage(int damage)
  {
    currentHealth -= damage;
    Debug.Log("Player HP: " + currentHealth);

    if (currentHealth <= 0)
      Die();
  }

  void Die()
  {
    anim.SetTrigger("isDead");
    Debug.Log("Player Dead!");
    // Disable controls hoặc respawn
    Destroy(gameObject, 1f);
  }
}
