using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityStandardAssets.CrossPlatformInput;

public class Player : MonoBehaviour
{

    // Config
    [SerializeField] float runSpeed = 5f;
    [SerializeField] float jumpSpeed = 5f;
    [SerializeField] float climbSpeed = 5f;
    [SerializeField] Vector2 deathKick = new Vector2(10f, 5f);
    [SerializeField] float LevelLoadDelay = 5f;
    [SerializeField] Sprite[] playerSprites;
    [SerializeField] GameObject wand;
    [SerializeField] GameObject magicShot;
    [SerializeField] Transform wandSpawn;
    [SerializeField] Transform groundCheckCollider;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] LayerMask climbLayer;
    GameObject wand2;
    float gravityScaleAtStart;
    const float groundCheckRadius = 0.2f;
    //int playerHits = 3;

    // Ladderiin keskitt‰minen TESTILADDER
    public GameObject ladder;

    // State
    bool isAlive = true;
    bool isShooting = false;
    bool isGrounded = false;
    bool isClimbing;
    bool canShoot = true;
    //bool isRunning = false;
    //bool isJumping = false;

    // Cached component references
    Rigidbody2D myRigidBody;
    Animator myAnimator;
    Collider2D myCollider2D;
    SpriteRenderer mySpriteRenderer;
    // Tilemap tilemap;

    // Message then methods
    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>(); // Haetaan rigidbody komponentti
        myAnimator = GetComponent<Animator>(); // Haetaan animator komponentti
        myCollider2D = GetComponent<Collider2D>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();
       // tilemap = GetComponent<Tilemap>();
        gravityScaleAtStart = myRigidBody.gravityScale; // Haetaan RigidBody komponentista Gravity Scale
    }

    // Update is called once per frame
    void Update()
    {
        if (!isAlive) { return;  } // Jos pelaaja kuollut, ei menn‰ t‰ss‰ metodissa eteenp‰in

        Run();
        FlipSprite();
        Jump();
        ClimbLadder();
        //CenterClimbingPlayer();
        Hit();
        Shoot();
        WandPositioning();
        GroundCheck();

        // Set the yVelocity in the animator (for jumping animation)
        myAnimator.SetFloat("yVelocity", myRigidBody.velocity.y);
    }

    private void Run()
    {
        // Jos inventory on auki, ei pysty liikkumaan
        if (FindObjectOfType<InventorySystem>().isOpen)
        {
            return;
        }

        float controlThrow = CrossPlatformInputManager.GetAxis("Horizontal"); // Haetaan horisontaalinen liike Inputmanagerista ; value is between -1 to +1 ; GetAxis sen takia, jos halutaan liikkua eri muodossa, esim hitaammin ja nopeammin esim joystickill‰ (eli ei laiteta t‰h‰n n‰pp‰imen painallusta)
        Vector2 playerVelocity = new Vector2(controlThrow * runSpeed, myRigidBody.velocity.y); // playerVelocity eli playerin sijainti on horisontaalinen liike * liikkumisnopeus ; myRigidBody.velocity.y hakee sen hetkisen sijainnin y-akselilla
        myRigidBody.velocity = playerVelocity; // Sijainniksi haetaan pelaajan uusi sijainti - new Vector2 siis asettaa suunnnan ja velocity siirt‰‰ pelaajan suuntaa kohti

        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidBody.velocity.x) > Mathf.Epsilon; // t‰m‰ on true, jos pelaaja liikkuu
        myAnimator.SetBool("Walking", playerHasHorizontalSpeed); // jos playerHasHorizontalSpeed on true, Walking-animaatio asetetaan trueksi
    }

    private void Jump()
    {
        if (!myCollider2D.IsTouchingLayers(LayerMask.GetMask("Ground"))) // Haetaan luotu layer (Ground) ja jos pelaaja ei osu siihen, return, mut jos osuu, niin sit hypp‰‰
            // t‰ll‰ siis estet‰‰n, et pelaaja ei voi hyp‰t‰ ilmassa
        {
            return;
        }

        if (CrossPlatformInputManager.GetButtonDown("Jump")) // Haetaan Input Managerista toiminto "Jump". Sinne on m‰‰r‰tty n‰pp‰in toiminnolle
        {
            Vector2 jumpVelocityToAdd = new Vector2(0f, jumpSpeed);
            myRigidBody.velocity += jumpVelocityToAdd;
           // myAnimator.SetBool("Jumping", true); // t‰t‰ ei kai tarvii koska se on GroundCheck-metodissa?
        }
    }
    /*
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy" || collision.tag == "Hazards")
        {
            isAlive = false;
            myAnimator.SetTrigger("Dying"); // inspectorissa luotu Die-animaatiolle ehto: trigger "Dying", jonka t‰m‰ laukaisee
            GetComponent<Rigidbody2D>().velocity = deathKick; // T‰‰ ei toimi, koska rigidbody muutetaan staticiks kuoleman j‰lkeen
            StartCoroutine(BodyStatic()); // Muutetaan rigidbody static 0.3s kuluttua, jotta pelaaja ehtii tippuun maahan ja deathkick toimii
            StartCoroutine(ProcessDeath()); // Prosessoidaan pelaajan kuolema xx ajan kuluttua, jotta kuolemis-animaatio ehtii pyˆri‰
        }
    } */
    
    private void Hit()
    {
        if (myCollider2D.IsTouchingLayers(LayerMask.GetMask("Enemy"))) 
        {
            
            if (!FindObjectOfType<EnemyMovement>().CheckIfEnemyDead()) // Jos osuma enemyyn ja enemy on kuollut, ei menn‰ t‰ss‰ metodissa eteenp‰in
            {
                return;
            }
            /*
            else
            {
                if (playerHits > 1)
                {
                    Damage();
                    Debug.Log("Osuma");
                }
                else
                {
                    SoDead();
                    Debug.Log("kuolin:(");
                }
            } */

            SoDead();
        } 

        if (myCollider2D.IsTouchingLayers(LayerMask.GetMask("Hazards"))) // Luotu Tilemap Grid "Hazards", johon lis‰tty Tilemap Collider (IsTrigger), johon lis‰t‰‰n kaikki kuolemaan johtavat jutut (esim. lava)
        {
            SoDead();
        }
    }

    /*
    private void Damage()
    {
        Physics.IgnoreLayerCollision(10, 12, true);
        myRigidBody.velocity = -transform.localScale.x * deathKick; // -transform.localScale, jotta pelaaja lent‰‰ aina taaksep‰in
        myAnimator.SetBool("Damage", true);
        playerHits--;
        StartCoroutine(DamageDone());
        Debug.Log("true");
    }

    IEnumerator DamageDone()
    {
        yield return new WaitForSecondsRealtime(1f);
        myAnimator.SetBool("Damage", false);
        Physics.IgnoreLayerCollision(10, 12, false);
        Debug.Log("false");
    } */

    private void SoDead()
    {
        isAlive = false;
        myAnimator.enabled = true; // jos just kiipe‰m‰ss‰ tikkaita 
        myRigidBody.gravityScale = gravityScaleAtStart; // jos just kiipe‰m‰ss‰ tikkaita
        myAnimator.SetTrigger("Dying"); // inspectorissa luotu Die-animaatiolle ehto: trigger "Dying", jonka t‰m‰ laukaisee
        myRigidBody.velocity = -transform.localScale.x * deathKick; // -transform.localScale, jotta pelaaja lent‰‰ aina taaksep‰in
        //playerHits = 3;
        StartCoroutine(BodyStatic()); // Muutetaan rigidbody static 0.3s kuluttua, jotta pelaaja ehtii tippuun maahan ja deathkick toimii
        StartCoroutine(ProcessDeath()); // Prosessoidaan pelaajan kuolema xx ajan kuluttua, jotta kuolemis-animaatio ehtii pyˆri‰
    }

    IEnumerator BodyStatic()
    {
        yield return new WaitForSecondsRealtime(0.3f);
        myRigidBody.bodyType = RigidbodyType2D.Static; // T‰m‰ est‰‰, ett‰ pelaaja ei liiku en‰‰ kuoleman j‰lkeen
        myCollider2D.enabled = false;
    }

    IEnumerator ProcessDeath()
    {
        yield return new WaitForSecondsRealtime(LevelLoadDelay);
        FindObjectOfType<GameSession>().ProcessPlayerDeath(); // Haetaan GameSession -scriptist‰ metodi, joka k‰sittelee pelaajan kuoleman
    }

    private void ClimbLadder()
    {
        if (!myCollider2D.IsTouchingLayers(LayerMask.GetMask("Climbing")))
        {
            if (!isShooting)
            {
                myAnimator.enabled = true; // jotta dash-sprite menee p‰‰lle
            }
            isClimbing = false;
            myAnimator.SetBool("Climbing", false); // T‰m‰ est‰‰ kiipeilyanimaation p‰‰lle j‰‰misen
            myRigidBody.gravityScale = gravityScaleAtStart; // painovoima se tavallinen, kun ei kosketa tikkaisiin
            return;
        }

        isClimbing = true;
        float controlThrow = CrossPlatformInputManager.GetAxis("Vertical"); // Haetaan vertikaalinen  axis/liike, eli ylˆsp‰in painaminen
        Vector2 climbingVelocity = new Vector2(myRigidBody.velocity.x, controlThrow * climbSpeed); // Liikkuminen y-akselilla ykˆsp‰in
        myRigidBody.velocity = climbingVelocity;
        myRigidBody.gravityScale = 0f; // Asetetaan painovoimaksi 0, kun kiivet‰‰n tikkaita (ettei valuta alasp‰in)

        ClimbingAnimationOrSprite();
    }

    private void ClimbingAnimationOrSprite()
    {
        bool playerHasVerticalSpeed = Mathf.Abs(myRigidBody.velocity.y) > Mathf.Epsilon; // Jos pelaaja liikkuu ylˆs- tai alasp‰in
        myAnimator.SetBool("Climbing", playerHasVerticalSpeed); // haetaan kiipe‰mis-animaatio

        // Jos pelaaja ei ole liikkeess‰ tikkailla, haetaan kiipe‰mis-sprite
        if (!playerHasVerticalSpeed)
        {
            myAnimator.enabled = false;
            mySpriteRenderer.sprite = playerSprites[1];
        }
        else
        {
            myAnimator.enabled = true;
        }
    }

    /*
    private void CenterClimbingPlayer()
    {
        if (myCollider2D.IsTouchingLayers(LayerMask.GetMask("Climbing")))
        {

            Vector3Int tilePos = tilemap.WorldToCell(transform.position); // kun kosketaan "climbing" layeriin, se poisitio tulee t‰h‰n
            TileBase climbTile = tilemap.GetTile(tilePos); // tunnistaa, ett‰ kyseess‰ on climbing-tilemapin tile 
            


            float ladderXPos = ladder.transform.position.x;
            transform.position = new Vector3(ladderXPos, transform.position.y, 0);

            Debug.Log("Ladderin sijainti:" + ladderXPos + "pelaajan sijainti:" + transform.position);
        }
    }
    */

    private void Shoot()
    {
        if (isClimbing || PlayerPrefs.GetInt("WandPickedUp") != 1 || canShoot == false)
        {
            return;
        }

        if (CrossPlatformInputManager.GetButtonDown("Fire1"))
        {
            isShooting = true;
            myAnimator.enabled = false;
            mySpriteRenderer.sprite = playerSprites[2]; // dash-sprite p‰‰lle
            wand2 = Instantiate(wand, wandSpawn.position, transform.rotation); // wandi ilmestyy
            Instantiate(magicShot, wandSpawn.position, transform.rotation); // magic shot ilmestyy
            canShoot = false; // t‰‰ sen takia et ei pysty ampuun, ennen kun couroutine on menny (jotta wandi ei j‰‰ ilmaan leijuun jos r‰mpytt‰‰ ampumista)
            StartCoroutine(BackToNormal()); // sprite p‰‰ll‰ 0.5s, jonka j‰lkeen takas iddle
        }
    }

    IEnumerator BackToNormal()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        isShooting = false;
        myAnimator.enabled = true;
        canShoot = true;
        mySpriteRenderer.sprite = playerSprites[0];
        Destroy(wand2);
    }

    private void WandPositioning()
    {
        if (isShooting)
        {
            wand2.transform.position = wandSpawn.position; // liimataan wand playerin k‰teen 
            wand2.transform.localScale = transform.localScale; // K‰‰nnet‰‰n wand-sprite pelaajan suuntaan
            /*
            if (transform.localScale.x < 0)
            {
                wand2.transform.localScale = new Vector2(-1f, 1f); // K‰‰nnet‰‰n wand-sprite pelaajan suuntaan
            }
            */
        }
    }

    private void FlipSprite()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidBody.velocity.x) > Mathf.Epsilon; // playerHasHorizontalSpeed = true, jos pelaaja liikkuu x-akselilla
        // Mathf.Abs palauttaa luvun absoluuttisen arvon
        if (playerHasHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(myRigidBody.velocity.x), 1f); // Jos pelaaja liikkuu, k‰‰nnet‰‰n ukkeli x-akselilla liikkeen suuntaan (eli -1 tai 1)
        }
    }

    private void GroundCheck()
    {
        isGrounded = false;

        // Check if the GroundCheckObject is colliding with other 2D colliders that are in the "Ground" Layer
        // If yes (isGrounded true) else (isGrounded false)
        Collider2D[] groundColliders = Physics2D.OverlapCircleAll(groundCheckCollider.position, groundCheckRadius, groundLayer);
        Collider2D[] climbColliders = Physics2D.OverlapCircleAll(groundCheckCollider.position, groundCheckRadius, climbLayer);

        // Jos pelaaja koskee maata, kiipe‰‰ tai kuolee niin asetetaan isGrounded = true, jotta hyppyanimaatio ei j‰‰ p‰‰lle
        if (groundColliders.Length > 0 || climbColliders.Length > 0 || !isAlive)
        {
            isGrounded = true;
        }

        // As long as we are grounded the "Jumping" bool in the animator is disabled
        myAnimator.SetBool("Jumping", !isGrounded);
    }

}
