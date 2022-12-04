using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    // Config
    [SerializeField] float moveSpeed = 1f;
    [SerializeField] int enemyHealth = 3;

    // State
    bool isAlive = true;
    bool canMove = true;
    bool flipEnemy = true;

    // Cached component references
    Rigidbody2D myRigidBody;
    //Collider2D myCollider;
    Animator myAnimator;

    // Start is called before the first frame update
    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        //myCollider = GetComponent<Collider2D>();
        myAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isAlive) { return; } // Jos enemy kuollut, ei menn‰ t‰ss‰ metodissa eteenp‰in
        Walk();
    }

    public void Damage()
    {
        enemyHealth -= 1;
        myAnimator.SetBool("Damage", true);
        myRigidBody.bodyType = RigidbodyType2D.Static;
        canMove = false; // jotta ei yrit‰ liikkua 
        flipEnemy = false; // jotta ei k‰‰nny shotista

        if (enemyHealth <= 0)
        {
            EnemyDeath();
        }

        StartCoroutine(BackToNormal());
    }

    IEnumerator BackToNormal()
    {
        yield return new WaitForSecondsRealtime(1f);
        myAnimator.SetBool("Damage", false);
        myRigidBody.bodyType = RigidbodyType2D.Kinematic;
        canMove = true;
        flipEnemy = true;
    }

    void Walk()
    {
        if (canMove)
        {
            if (IsFacingRight()) // jos true, liikkuu oikeeseen suuntaan, muuten liikkuu "v‰‰r‰‰n" suuntaan (neg)
            {
                myRigidBody.velocity = new Vector2(moveSpeed, 0f);
            }
            else
            {
                myRigidBody.velocity = new Vector2(-moveSpeed, 0f);
            }
        }
    }

    bool IsFacingRight()
    {
        return transform.localScale.x > 0; // Jos enemy on oikein p‰in, palauttaa true ja vice versus
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Ground" && flipEnemy)
        {
            transform.localScale = new Vector2(-(Mathf.Sign(myRigidBody.velocity.x)), 1f); // enemy k‰‰ntyy toiseen suuntaan tˆrm‰tty‰‰n sein‰‰n (- -merkki k‰‰nt‰‰)
        }
    }

    public void EnemyDeath()
    {
        isAlive = false;
        myAnimator.SetTrigger("Dying");
        StartCoroutine(DestroyTheEnemy());
    }

    IEnumerator DestroyTheEnemy()
    {
        yield return new WaitForSecondsRealtime(3f);
        Destroy(gameObject);
    }

    public bool CheckIfEnemyDead()
    {
        return isAlive;
    }
}
