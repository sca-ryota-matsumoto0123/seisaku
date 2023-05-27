using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyControll : MonoBehaviour
{

    [SerializeField]
    Animator animator = null;
    [SerializeField]
    NavMeshAgent navmeshAgent = null;
    [SerializeField]
    Transform target = null;
    [SerializeField]
    CapsuleCollider capsuleCollider = null;
    [SerializeField, Min(0)]
    int maxHp = 3;
    [SerializeField]
    float deadWaitTime = 3;

    [SerializeField]
    float chaseDistnce = 5;
    [SerializeField]
    Collider attackCollider = null;
    [SerializeField]
    int attackPower = 10;
    [SerializeField]
    float attackTime = 0.5f;
    [SerializeField]
    float attackInterval = 2;
    [SerializeField]
    float attackDistance = 2;

    readonly int SpeedHash = Animator.StringToHash("Speed");
    readonly int AttackHash = Animator.StringToHash("Attack");
    readonly int DeadHash = Animator.StringToHash("Dead");

    bool isDead = false;
    int hp = 0;
    Transform thisTransform;

    bool isAttacking = false;
    Transform player;
    Transform defaultTarget;
    WaitForSeconds attackWait;
    WaitForSeconds attackIntervalWait;

    GameManager gameManager;

    public int Hp
    {
        set
        {
            hp = Mathf.Clamp(value , 0, maxHp);
        }
        get
        {
            return hp;
        }
    }

    public Transform Target
    {
        set
        {
            target = value;
        }
        get
        {
            return target;
        }
    }

    GameManager GameManager
    {
        get
        {
            if(gameManager == null)
            {
                gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
            }

            return gameManager;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        thisTransform = transform;

        defaultTarget = target;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        attackWait = new WaitForSeconds(attackTime);
        attackIntervalWait = new WaitForSeconds(attackInterval);


        InitEnemy();
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead)
        {
            return;
        }
        
        CheckDistance();
        Move();
        UpdateAnimator();
    }

    void InitEnemy()
    {
        Hp = maxHp;
    }


    public void Damage(int value)
    {
        if (value <= 0)
        {
            return;
        }

        Hp -= value;

        if (Hp <= 0)
        {
            Dead();
        }
    }

    void Dead()
    {
        isDead = true;
        capsuleCollider.enabled = false;
        animator.SetBool(DeadHash, true);

        StopAttack();
        navmeshAgent.isStopped = true;

        GameManager.Count++;

        StartCoroutine(nameof(DeadTimer));
    }

    void Move()
    {
        navmeshAgent.SetDestination(target.position);
    }

    void UpdateAnimator()
    {
        animator.SetFloat(SpeedHash, navmeshAgent.desiredVelocity.magnitude);
    }
     
    void CheckDistance()
    {
        float diff = (player.position - thisTransform.position).sqrMagnitude;

        if(diff < attackDistance * attackDistance)
        {
            if (!isAttacking)
            {
                StartCoroutine(nameof(Attack));
            }
        }
        else if(diff < chaseDistnce * chaseDistnce)
        {
            target = player;
        }
        else
        {
            target = defaultTarget;
        }
    }

    IEnumerator DeadTimer()
    {
        yield return new WaitForSeconds(deadWaitTime);

        Destroy(gameObject);
    }

    IEnumerator Attack()
    {
        isAttacking = true;
        animator.SetTrigger(AttackHash);
        attackCollider.enabled = true;

        yield return attackWait;

        attackCollider.enabled = false;

        yield return attackIntervalWait;

        isAttacking = false;
    }

    void StopAttack()
    {
        StopCoroutine(nameof(Attack));

        attackCollider.enabled = false;

        isAttacking = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("ÚG");
            BulletScript bullet = other.gameObject.GetComponent<BulletScript>();

            if(bullet != null)
            {
                bullet.MinHp -= attackPower;
            }
        }
    }
}
