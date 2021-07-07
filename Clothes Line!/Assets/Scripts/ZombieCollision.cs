using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieCollision : MonoBehaviour
{
    Animator animator;
    ZombieAI zombieAI;
    bool hasDied = false;
    // Start is called before the first frame update
    void Start()
    {
        zombieAI = GetComponent<ZombieAI>();
        animator = GetComponent<Animator>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void ZombieDeath()
    {
        if (zombieAI != null && hasDied == false)
        {
            zombieAI.Die();
            hasDied = true;
        }
        if (animator != null)
        {
        }
    }
}
