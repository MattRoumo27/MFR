using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float speed;
    public float changeTime = 3.0f;

    Rigidbody2D rb;
    float timer;
    protected int direction = -1;

    protected Animator animator;

    #region Start
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        timer = changeTime;
        animator = GetComponent<Animator>();
    }
    #endregion

    #region Update
    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;

        if (timer < 0)
        {
            direction = -direction;
            timer = changeTime;
        }

        CustomEnemyBehavior();
    }
    #endregion

    #region FixedUpdate
    void FixedUpdate()
    {
        Movement();
    }
    #endregion

    #region OnCollisionEnter2D
    void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerController player = collision.gameObject.GetComponent<PlayerController>();

        if (player != null)
            player.ChangeHealth(-1);
    }
    #endregion

    #region CustomEnemyBehavior
    protected virtual void CustomEnemyBehavior()
    {

    }
    #endregion

    #region Movement
    protected virtual void Movement()
    {
        Vector2 position = rb.position;
        position.x = position.x + Time.deltaTime * speed * direction;
        animator.SetFloat("LookX", direction);
        animator.SetFloat("Speed", position.magnitude);

        rb.MovePosition(position);
    }
    #endregion
}
