using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    //variables
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float attackRate = 1f;
    public bool isStopped = false;
    private Vector2 movement;
    private bool canAttack = true;

    //components
    [SerializeField] GameObject attackAnimation;
    private Rigidbody2D rigidbody;
    [SerializeField] private Combat combat;
    private Animator animator;

    //methods
    public void SetStop(bool active)
    {
        isStopped = active;
    }
    private IEnumerator AttackCulldown()
    {
        canAttack = false;
        yield return new WaitForSeconds(attackRate);
        canAttack = true;
    }

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    //inputs
    private void Update()
    {
        if (!isStopped)
        {
            movement.x = Input.GetAxis("Horizontal");
            movement.y = Input.GetAxis("Vertical");

            if (Input.GetMouseButtonDown(0) && canAttack)
            {
                combat.Hit();
                Instantiate(attackAnimation, combat.AttackPoint.position, combat.AttackPoint.rotation);
                StartCoroutine(AttackCulldown());
            }
        }
    }

    //movement
    private void FixedUpdate()
    {
        if (!isStopped)
        {
            if (movement.x < 0 && transform.localScale.x == -1)
                transform.localScale = new Vector3(1, 1, 1);
            if (movement.x > 0 && transform.localScale.x == 1)
                transform.localScale = new Vector3(-1, 1, 1);

            rigidbody.velocity = movement * moveSpeed;
            animator.SetBool("Run", true);
        }
        if (movement == Vector2.zero)
            animator.SetBool("Run", false);
    }
}