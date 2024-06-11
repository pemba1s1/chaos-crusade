using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public Animator animator;
    private GameObject player;
    public float speed = 1f;

    public Transform m_hitBox;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;

    private float distance;
    private Vector3 originalScale;
    private float attackRate = 0.5f;
    private float nextAttackTime = 0;
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        originalScale = transform.localScale;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<HeroKnight>().gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null || player.GetComponent<HeroKnight>().currentHealth <= 0) {
            return;
        }
        Vector3 targetPosition = player.transform.position;
        distance = Vector2.Distance(transform.position, targetPosition);
        if (distance < 1.5f) {
            attack();
        } else {
            animator.SetInteger("state", 1);

            Vector2 direction = (player.transform.position - transform.position).normalized;
            Vector3 newPosition = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
            newPosition.z = transform.position.z;
            transform.position = newPosition;
            if (direction.x > 0)
            {
                transform.localScale = new Vector3(Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
            }
            else if (direction.x < 0)
            {
                transform.localScale = new Vector3(-Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
            }
        }
    }

    public void takeDamage(int damage) {
        currentHealth -= damage;
        animator.SetTrigger("Hit");
        if (currentHealth <= 0) {
            die();
        }
    }

    void attack() {
        animator.SetInteger("state", 0);
        if ( nextAttackTime == 0)
        {
            nextAttackTime = Time.time + 1f / attackRate;
        }
        if (Time.time < nextAttackTime)
        {
            animator.SetBool("isAttacking", false);
            return;
        }

        animator.SetBool("isAttacking", true);
        
        nextAttackTime = Time.time + 1f / attackRate;
    }

    void findEnemyAndAttack()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(m_hitBox.position, attackRange, enemyLayers);
        foreach (Collider2D enemy in hitEnemies)
        {
            Debug.Log("We hit " + enemy.name);
            if (!player.activeSelf) return;

            player.GetComponent<HeroKnight>().takeDamage(10);
        }
    }

    void die() {
        Debug.Log("Enemy died!");
        animator.SetBool("isDead", true);
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;
        Destroy(gameObject, 5f);
    }

    private void OnDrawGizmosSelected()
    {
        if (m_hitBox == null)
            return;

        Gizmos.DrawWireSphere(m_hitBox.position, attackRange);
    }
}
