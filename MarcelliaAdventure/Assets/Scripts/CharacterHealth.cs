using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CharacterHealth : MonoBehaviour
{
    [SerializeField] float characterHealth = 20f;
    [SerializeField] float currentHealth;
    [SerializeField] float spellDamage = 5f;
    Rigidbody rb;
    bool isAlive = true;
    Animator anim;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        currentHealth = characterHealth;
    }

    

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Spell")
        {
            Destroy(other.gameObject);
            currentHealth -= spellDamage;
            if (currentHealth <= 0)
            {
                Debug.Log("öldü");
                CharacterDeath();
            }
        }
    }

    void CharacterDeath()
    {
        currentHealth = characterHealth;
        isAlive = false;
        Vector3 noMove = new Vector3(0f,0f,0f);
        rb.velocity = noMove;
        //death animation
    }

    public bool Alive
    {
        get
        {
            return isAlive;
        }
    }

}
