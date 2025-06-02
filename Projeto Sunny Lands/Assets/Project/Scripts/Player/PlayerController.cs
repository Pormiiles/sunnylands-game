using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Animator playerAnim;
    public Rigidbody2D playerRigidBody;

    public float playerSpeed;
    public Transform groundCheck;
    public bool isPlayerTouchingTheGround; // Variavel respons�vel por verificar se o Player est� tocando o ch�o

    public float touchRun = 0.0f; // Movimenta��o do Player no eixo Horizontal 

    public bool playerFacingRight = true;

    public bool isJumping = false; // Verifica se o Player est� pulando na cena
    public int countOfJumps = 0; // Quantidade de pulos que o Player deu
    public int maxJumps = 2; // N�mero de 2 pulos m�ximos
    public float jumpForce; // For�a do impulso do pulo

    // Start is called before the first frame update
    void Start()
    {
        playerAnim = GetComponent<Animator>();
        playerRigidBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // Verifica se o Player toca o ch�o pela Layer do Collider do ch�o
        isPlayerTouchingTheGround = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));
        playerAnim.SetBool("isGrounded", isPlayerTouchingTheGround); // Passa o valor para a vari�vel isGrounded do Animator

        // Inputs Padr�es da Unity  - Edit -> Project Settings -> Input Manager

        touchRun = Input.GetAxisRaw("Horizontal");
        
        if(Input.GetButtonDown("Jump"))
        {
            isJumping = true;
        }

        SetAnimations();
    }

    void PlayerMove(float horizontalMovement) // M�todo de movimenta��o (Walk) do Player
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
