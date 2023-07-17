using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WizardController : MonoBehaviour
{
    private NavMeshAgent agent;
    [SerializeField] Transform target;
    [SerializeField] float enemyHealth = 10;
    [SerializeField] float currentEnemyHealth;
    [SerializeField] float damageDealt = 5f;
    Animator anim;
    [SerializeField] Transform spellPoint;
    [SerializeField] float limitDistance;
    [SerializeField] GameObject spellPrefab;
    [SerializeField] float spellSpeed = 3f;
    [SerializeField] float stopLimit = 1f;
    float distance;
    bool enemyAlive = true;
    bool isAttacking;
    float attackCount = 0;
    CharacterHealth characterHealth;
    bool characterAlive;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        currentEnemyHealth = enemyHealth;
        anim = GetComponent<Animator>();
        characterHealth = FindObjectOfType<CharacterHealth>();
    }

    // Update is called once per frame
    void Update()
    {
        distance = Vector3.Distance(transform.position, target.position);
        characterAlive = characterHealth.Alive;

        if (!characterAlive || !enemyAlive) return;
        EnemyMovement();
        EnemyAttack();
    }

    void EnemyMovement()
    {
        agent.speed = 1f;
        agent.stoppingDistance = stopLimit;
        if (Mathf.Abs(distance) < limitDistance && Mathf.Abs(distance) > stopLimit)
        {
            isAttacking = false;
            anim.SetBool("isWalk", true);
            agent.SetDestination(target.position);
            Debug.Log("yürü");
        }
        else
        {
            agent.SetDestination(transform.position); // Stop following by setting the destination to current position
            anim.SetBool("isWalk", false);
            Debug.Log("dur");
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.tag == "Bullet")
        {
            currentEnemyHealth -= damageDealt;
            if (currentEnemyHealth <= 0) EnemyDeath();  
        }
    }

    void EnemyAttack()
    {
        
        if (Mathf.Abs(distance) <= 5f)
        {
            
            attackCount += Time.deltaTime;
            if(attackCount > 2) 
            {
                
                Debug.Log("vur");
                var spell = Instantiate(spellPrefab, spellPoint.position, Quaternion.identity);
                var rb = spell.AddComponent<Rigidbody>();
                rb.useGravity = false;

                Vector3 fireDirection = (new Vector3(target.transform.position.x, target.transform.position.y + 0.7f, target.transform.position.z) - spellPoint.position).normalized;
                rb.AddForce(fireDirection * spellSpeed, ForceMode.Impulse);
                
                Destroy(spell, 2f);
                attackCount = 0;
            }
            
        }
    }
    void EnemyDeath()
    {
        enemyAlive = false;
        anim.SetBool("isDead", true);
        Debug.Log("Enemy is dead");
        Destroy(gameObject, 4.6f);
        //ölüm animasyon vb.
        
    }

}
