using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Animator playerAnim;
    public Rigidbody2D playerRigidBody;

    public float playerSpeed;
    public Transform groundCheck;
    public bool isPlayerTouchingTheGround;

    public float touchRun = 0.0f; // Movimentação do Player no eixo Horizontal 

    public bool playerFacingRight = true;

    // Start is called before the first frame update
    void Start()
    {
        playerAnim = GetComponent<Animator>();
        playerRigidBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        touchRun = Input.GetAxisRaw("Horizontal");
        MoveAnimation();
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
    }

    void FlipPlayerSprite()
    {
        playerFacingRight = !playerFacingRight;
        Vector3 playerScale = transform.localScale;
        playerScale *= -1; 

        transform.localScale = new Vector3(playerScale.x, transform.localScale.y, transform.localScale.z);
    }

    #region AnimationsActions

    void MoveAnimation()
    {
        playerAnim.SetBool("isWalking", playerRigidBody.velocity.x != 0);
    }

    #endregion
}
