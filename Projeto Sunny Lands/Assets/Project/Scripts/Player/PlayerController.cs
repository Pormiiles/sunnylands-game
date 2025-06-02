using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Animator playerAnim;
    public Rigidbody2D playerRigidBody;

    public float playerSpeed;
    public Transform groundCheck;
    public bool isPlayerTouchingTheGround; // Variavel responsável por verificar se o Player está tocando o chão

    public float touchRun = 0.0f; // Movimentação do Player no eixo Horizontal 

    public bool playerFacingRight = true;

    public bool isJumping = false; // Verifica se o Player está pulando na cena
    public int countOfJumps = 0; // Quantidade de pulos que o Player deu
    public int maxJumps = 2; // Número de 2 pulos máximos
    public float jumpForce; // Força do impulso do pulo

    // Start is called before the first frame update
    void Start()
    {
        playerAnim = GetComponent<Animator>();
        playerRigidBody = GetComponent<Rigidbody2D>();
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

    void PlayerMove(float horizontalMovement) // Método de movimentação (Walk) do Player
    { 


        playerRigidBody.velocity = new Vector2(horizontalMovement * playerSpeed, playerRigidBody.velocity.y);

        if (horizontalMovement < 0 && playerFacingRight || (horizontalMovement > 0 && !playerFacingRight))
        {
            FlipPlayerSprite();
        }    
    }

    private void FixedUpdate() 
    {
        PlayerMove(touchRun);

        if(isJumping)
        {
            JumpAnimation();
        }
    }

    void FlipPlayerSprite()
    {
        playerFacingRight = !playerFacingRight;
        Vector3 playerScale = transform.localScale;
        playerScale *= -1; 

        transform.localScale = new Vector3(playerScale.x, transform.localScale.y, transform.localScale.z);
    }

    #region AnimationsActions

    void SetAnimations()
    {
        playerAnim.SetBool("isWalking", playerRigidBody.velocity.x != 0 && isPlayerTouchingTheGround);
        playerAnim.SetBool("isJumping", !isPlayerTouchingTheGround);
    }

    void JumpAnimation()
    {
        if(isPlayerTouchingTheGround)
        {
            playerRigidBody.AddForce(new Vector2(0f, jumpForce));
            isPlayerTouchingTheGround = false;
        }

        isJumping = false;
    }

    #endregion
}
