using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack3D : MonoBehaviour
{
    public enum ObjectType
    {
        Player,
        Enemy
    }
    public ObjectType objectType;
    public float damage = 10f;
    public float range = 100f;
    public Vector3 offset = new Vector3(0, 0, 0);
    bool isAttacking = false;
    public GameObject attackPoint;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(isAttacking)
        {
            //use overlap sphere to detect enemies in range based on the offset and the attackPoint

            Vector3 attackPointPos = attackPoint.transform.TransformPoint(offset);
            Collider[] hitEnemies = Physics.OverlapSphere(attackPointPos, range);

            // Collider[] hitEnemies = Physics.OverlapSphere(transform.position + offset, range);
            foreach (Collider enemy in hitEnemies)
            {
                if(objectType == ObjectType.Player)
                {
                    if(enemy.gameObject.tag == "Enemy")
                    {
                        print("We hit " + enemy.name);
                        Health health = enemy.GetComponent<Health>();
                        if(health != null)
                        {
                            if(health.GetHealth() > 0)
                            {
                                health.TakeDamage(damage);
                            }
                            else
                            {
                                health.Death();
                            }
                            isAttacking = false;
                        }
                    }
                }
                else if(objectType == ObjectType.Enemy)
                {
                    if(enemy.gameObject.tag == "Player")
                    {
                        print("We hit " + enemy.name);
                        Health health = enemy.GetComponent<Health>();
                        enemySimpleAI enemyAI = enemy.GetComponent<enemySimpleAI>();
                        if(health != null)
                        {
                            health.TakeDamage(damage);
                            if(health.GetHealth() > 0)
                            {  
                                enemyAI.Hurt();
                            }
                            else
                            {
                                //enemyAI.Die();
                                health.Death();
                            }
                            isAttacking = false;
                        }
                    }
                }
            }
        }
    }
    //use overlap sphere for attacking. 
    //This can be used for melee attacks for enemies and the player

    public void Attack()
    {
        isAttacking = true;
    }
    public void StopAttack()
    {
        isAttacking = false;
    }

    public bool GetIsAttacking()
    {
        return isAttacking;
    }

    //use gizmos to draw a sphere to show the range of the attack
    void OnDrawGizmos()
    {
        if(isAttacking)
        {
            Gizmos.color = Color.red;
        }
        else
        {
            Gizmos.color = Color.green;
        }
        Vector3 attackPointPos = attackPoint.transform.TransformPoint(offset);
        Gizmos.DrawWireSphere(attackPointPos, range);
    }
    
}