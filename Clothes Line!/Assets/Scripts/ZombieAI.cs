using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieAI : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;
    GameManager gameManager;
    PlayerController target;
    bool isDead = false;
    float destroyTimer = 0f;
    PlayerController player;
    Animator animator;
    [SerializeField] bool isCrawler = false;
    float crawlerDeathTimer = 30f;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        gameManager = FindObjectOfType<GameManager>();
        SetTarget();
    }

    // Update is called once per frame
    void Update()
    {
        if (isCrawler)
        {
            crawlerDeathTimer -= Time.deltaTime;
            if (crawlerDeathTimer <= 0f && isDead == false)
            {
                Die();
            }
        }
        if (isDead)
        {
            destroyTimer += Time.deltaTime;
            if (destroyTimer > 3f) Destroy(gameObject);
            return;
        }
        if (target == null)
        {
            SetTarget();
        }
        if (target != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, moveSpeed * Time.deltaTime);
            Vector3 targetDirection = target.transform.position - transform.position;
            if (targetDirection != Vector3.zero)
                transform.forward = Vector3.RotateTowards(transform.forward, targetDirection, 1800, 1800f);
            if (transform.position == target.transform.position)
            {
                if (animator != null)
                {
                    animator.SetBool("Feasting", true);
                }
            }
        }
    }

    void SetTarget()
    {
        if (gameManager.GetPlayersList().Count < 1)
        {
            return;
        }
        int playerToFollow = Random.Range(0, gameManager.GetPlayersList().Count);
        target = gameManager.GetPlayersList()[playerToFollow];
    }

    public void Die()
    {
        gameManager.ZombieDeath(this);
        animator.SetBool("Falling1", true);
        this.GetComponent<BoxCollider>().enabled = false;
        isDead = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        player = collision.transform.GetComponent<PlayerController>();
        if (player != null)
        {
            player.GameOver();
        }
    }

    public bool IsCrawler()
    {
        return isCrawler;
    }
}
