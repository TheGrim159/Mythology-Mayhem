using UnityEngine;

public class MouseAI : MonoBehaviour
{
    [Header("Mouse Movement")]
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float runSpeed = 10f;

    [Header("Attack Activation")]
    [SerializeField] private BoxCollider2D soundTrigger;
    [SerializeField] private GameObject attackTarget;

    [Header("Mouse Animations")]
    [SerializeField] private Animator mouseAnim;
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] float idleDuration = 5f;
    private float idleTimer = 0f;
    private bool movingLeft;

    [Header("Mouse Waypoints")]
    [SerializeField] private Transform[] waypoints;
    [SerializeField] private int waypointIndex = 0;

    private Transform[] savedWaypoints;
    public Rigidbody2D rb2d;
    public bool dead = false;
    private bool attacking = false;
    private bool idle = false;
    private Vector2 currentPosition;
    private Vector2 previousPosition;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        savedWaypoints = waypoints;
        if (attackTarget == null) 
        {
            attackTarget = GameObject.FindWithTag("Player");
        }
        transform.position = waypoints[waypointIndex].transform.position;
        sr = GetComponent<SpriteRenderer>();
        movingLeft = sr.flipX;
        currentPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (!dead)
        {
            previousPosition = currentPosition;
            currentPosition = transform.position;
            if (!attacking && !idle)
            {
                MoveMouse();
            }
            if (!attacking && idle)
            {
                Idle();
            }
            AttackPlayer();
            Flip();
        }
    }

    void MoveMouse()
    {
        mouseAnim.SetBool("IsPatrolling", true);
        transform.position = Vector2.MoveTowards(transform.position, waypoints[waypointIndex].transform.position, walkSpeed * Time.deltaTime);
        if (transform.position.x == waypoints[waypointIndex].transform.position.x)
        {
            waypointIndex += 1;
            idle = true;
            mouseAnim.SetBool("IsPatrolling", false);
            idleTimer = Time.time + idleDuration;
        }
        if (waypointIndex >= waypoints.Length)
        {
            waypointIndex = 0;
        }
    }

    void Flip()
    {
        Vector2 currentDirection = (currentPosition-previousPosition).normalized;
        if (currentDirection.x < 0)
        {
            sr.flipX = true;
        }
        else if (currentDirection.x > 0)
        {
            sr.flipX = false;
        }
    }

    void AttackPlayer()
    {
        attacking = soundTrigger.IsTouching(attackTarget.GetComponent<Collider2D>());
        if (attacking)
        {
            Debug.Log("Attacking");
            transform.position = Vector2.MoveTowards(transform.position, attackTarget.transform.position, runSpeed * Time.deltaTime);
        }
        mouseAnim.SetBool("IsAttacking", attacking);
    }
    void Idle()
    {
        if (Time.time >= idleTimer)
        {
            idle = false;
        }
    }
}
