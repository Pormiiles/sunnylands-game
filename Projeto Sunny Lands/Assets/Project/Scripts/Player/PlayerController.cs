using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Animator playerAnim;
    public Rigidbody2D playerRigidBody;
    public SpriteRenderer playerSR;

    public float playerSpeed;
    public Transform groundCheck;
    public bool isPlayerTouchingTheGround; // Variavel responsável por verificar se o Player está tocando o chão

    public float touchRun = 0.0f; // Movimentação do Player no eixo Horizontal 
    public bool playerFacingRight = true;
    public bool playerVulnerable;
    public int playerTotalLife = 3;

    public bool isJumping = false; // Verifica se o Player está pulando na cena
    public int countOfJumps = 0; // Quantidade de pulos que o Player deu
    public int maxJumps = 2; // Número de 2 pulos máximos
    public float jumpForce; // Força do impulso do pulo

    private GameController gameController;
    public AudioSource audioSource;
    public AudioClip playerJumpSound;
    public AudioClip playerHitSound;
    public AudioClip deathEnemySound;

    // Start is called before the first frame update
    void Start()
    {
        // Inicializando variáveis com os componentes do GameObject 
        playerAnim = GetComponent<Animator>();
        playerRigidBody = GetComponent<Rigidbody2D>();
        playerSR = GetComponent<SpriteRenderer>();
        gameController = FindObjectOfType<GameController>();
    }

    // Update is called once per frame
    void Update()
    {
        // Verifica se o Player toca o chão pela Layer do Collider do chão
        isPlayerTouchingTheGround = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));
        playerAnim.SetBool("isGrounded", isPlayerTouchingTheGround); // Passa o valor para a variável isGrounded do Animator

        // Inputs Padrões da Unity  - Edit -> Project Settings -> Input Manager
        touchRun = Input.GetAxisRaw("Horizontal");
        
        if(Input.GetButtonDown("Jump"))
        {
            isJumping = true;
        }

        SetAnimations();
    }

    private void FixedUpdate() 
    {
        PlayerMoveAction(touchRun);

        if(isJumping)
        {
            PlayerJumpAction();
        }
    }

    #region AnimationsAndActions

    void SetAnimations() // Método principal que faz a chamada das animações 
    {
        playerAnim.SetBool("isWalking", playerRigidBody.velocity.x != 0 && isPlayerTouchingTheGround);
        playerAnim.SetBool("isJumping", !isPlayerTouchingTheGround);
    }

    void FlipPlayerSprite() // Método que "vira" o sprite do personagem horizontalmente (para onde está olhando)
    {
        playerFacingRight = !playerFacingRight;
        Vector3 playerScale = transform.localScale;
        playerScale *= -1;

        transform.localScale = new Vector3(playerScale.x, transform.localScale.y, transform.localScale.z);
    }

    void PlayerMoveAction(float horizontalMovement) // Método de movimentação (Walk) do Player
    {


        playerRigidBody.velocity = new Vector2(horizontalMovement * playerSpeed, playerRigidBody.velocity.y);

        if (horizontalMovement < 0 && playerFacingRight || (horizontalMovement > 0 && !playerFacingRight))
        {
            FlipPlayerSprite();
        }
    }

    void PlayerJumpAction() // Método de pulo (Jump) do Player
    {
        if(isPlayerTouchingTheGround)
        {
            countOfJumps = 0;
        }

        if(isPlayerTouchingTheGround || countOfJumps < maxJumps)
        {
            audioSource.PlayOneShot(playerJumpSound, 0.3f);
            playerRigidBody.velocity = new Vector2(0f, jumpForce);
            isPlayerTouchingTheGround = false;
            countOfJumps++;
        }

        isJumping = false;
    }

    #endregion

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Collectable")
        {
            Destroy(collision.gameObject);
            gameController.ScorePoints(1); // Ganha 1 ponto quando coleta a cenoura 
        } 

        if(collision.gameObject.tag == "Enemy")
        {
            Animator enemyAnimator = collision.gameObject.GetComponent<Animator>();
            
            enemyAnimator.SetBool("isDead", true);
            GameController.instance.PlaySFX(deathEnemySound, 0.5f);
            playerRigidBody.velocity = new Vector2(0f, jumpForce);
            Destroy(collision.gameObject, 0.4f);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            PlayerHit();
        }
    }

    void PlayerHit()
    {
        if(!playerVulnerable) // O player não pode tomar mais dano se estiver invulneravel (levou hit)
        {
            playerVulnerable = true;

            playerTotalLife--;
            GameController.instance.PlaySFX(playerHitSound, 0.5f);
            StartCoroutine(HitEffect());
            GameController.instance.UpdateLifeBarSprite(playerTotalLife);
        }
    }

    IEnumerator HitEffect() // Efeito de Hit (Player piscando durante um curto periodo)
    {
        for(float i = 0; i < 0.5; i += 0.1f)
        {
            playerSR.enabled = false;
            yield return new WaitForSeconds(0.1f);
            playerSR.enabled = true;
            yield return new WaitForSeconds(0.1f);
        }

        playerVulnerable = false; // Player volta ao estado normal (vulneravel a sofrer hit de novo)
    }
}
