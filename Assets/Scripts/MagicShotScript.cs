using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicShotScript : MonoBehaviour
{
    [SerializeField] float shotSpeed = 10f;
    float xSpeed;
    Rigidbody2D myRigidbody;
    Transform player;

    // Start is called before the first frame update
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        player = GameObject.FindWithTag("Player").transform; // Haetaan playerin Transform-komponentti
        xSpeed = player.transform.localScale.x * shotSpeed; // Shotin suunta on pelaajan suunta
    }

    // Update is called once per frame
    void Update()
    {
        myRigidbody.velocity = new Vector2(xSpeed, 0f);
        KillTheShot();
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            FindObjectOfType<EnemyMovement>().Damage();
        }
        Destroy(gameObject);
    }
    
    void KillTheShot()
    {
        float shotXPos = Mathf.Abs(myRigidbody.position.x);
        float playerXPos = Mathf.Abs(player.position.x);

        if (shotXPos >= playerXPos+20f || shotXPos <= playerXPos-20f) 
        {
            Destroy(gameObject);
        }
    }
}
