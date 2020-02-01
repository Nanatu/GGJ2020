using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GenericEnemy : MonoBehaviour
{

    List<GameObject> currentCollisions = new List<GameObject>();
    private Rigidbody2D rb;

    public float damageAmount = 10f;    // default value
    public float health = 100f;        // default value
    public float maxHealth = 100f;      // default value
    public float moveRate = 10f;        // default value
    public float maxRange = 10f;        // default value
    public float minRange = 1f;         // default value
    private float distanceToPlayer = 1000f;
    public float backupRate = 0.5f;
    public float attackRate = 2f;
    public float backupDistance = 1f;
    public float attackDistance = 4f;
    private bool attacking = false;
    public float attackCooldown = 3f;
    private Vector3 attackDirection;

    private GameObject Player;
    public Animator anim;

    // Start is called before the first frame update
    public void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        health = maxHealth;
    }

    // Update is called once per frame
    public void Update()
    {
        distanceToPlayer = Vector2.Distance(transform.position, Player.transform.position);
        moveToEnemy();

    }

    public void TakeDamage(int damage)
    {
        health = health - damage;
        if (health <= 0)
        {
            handleDeath();
        }
    }

    private void handleDeath()
    {
        Destroy(this);
    }

    private void moveToEnemy()
    {
        if (distanceToPlayer < maxRange && distanceToPlayer > minRange)
        {   
            if (!attacking)
            {
                transform.position = Vector2.MoveTowards(transform.position, Player.transform.position, moveRate * Time.deltaTime);
            }
            
        }
        if (distanceToPlayer < minRange)
        {
            if (!attacking)
            {
                attack();
            }
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player" && !currentCollisions.Contains(col.gameObject) && attacking)
            {
                currentCollisions.Add(col.gameObject);
                col.gameObject.SendMessage("TakeDamage", damageAmount);
                Debug.Log("Adding" + col.gameObject.tag + "to List");
            }
        
    }

    void ResetCollisionList()
    {
        Debug.Log("Clearing Collision List");
        currentCollisions.Clear();
    }


    public void attack()
    {
        attacking = true;
        attackDirection = (Player.transform.position - transform.position);
        attackDirection.Normalize();
        
        Vector3 attackTarget = transform.position + attackDistance * attackDirection;
        Vector3 backupTarget = transform.position - backupDistance * attackDirection;
        StartCoroutine(WindUpAttack(this.gameObject,backupTarget,attackTarget, backupRate, attackRate));
    }

    //IEnumerator attackCooldownTime()
    //{
    //    yield return new WaitForSeconds(attackCooldown);
    //    attacking = false;
    //}

    //public IEnumerator MoveOverSeconds(GameObject objectToMove, Vector3 end, float seconds)
    //{
    //    float elapsedTime = 0;
    //    Vector3 startingPos = objectToMove.transform.position;
    //    while (elapsedTime < seconds)
    //    {
    //        objectToMove.transform.position = Vector3.Lerp(startingPos, end, (elapsedTime / seconds));
    //        elapsedTime += Time.deltaTime;
    //        yield return new WaitForEndOfFrame();
    //    }
    //    objectToMove.transform.position = end;
    //}

    //public IEnumerator MoveOverSpeed(GameObject objectToMove, Vector3 end, float speed)
    //{
    //    // speed should be 1 unit per second
    //    while (objectToMove.transform.position != end)
    //    {
    //        objectToMove.transform.position = Vector3.MoveTowards(objectToMove.transform.position, end, speed * Time.deltaTime);
    //        yield return new WaitForEndOfFrame();
    //    }
    //}

    public IEnumerator WindUpAttack(GameObject objectToMove, Vector3 backup, Vector3 target, float backupSpeed, float attackSpeed)
    {
        // speed should be 1 unit per second
        while (objectToMove.transform.position != backup)
        {
            objectToMove.transform.position = Vector3.MoveTowards(objectToMove.transform.position, backup, backupSpeed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }

        // speed should be 1 unit per second
        while (objectToMove.transform.position != target)
        {
            objectToMove.transform.position = Vector3.MoveTowards(objectToMove.transform.position, target, attackSpeed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSeconds(attackCooldown);
        attacking = false;
        ResetCollisionList();
    }


}
