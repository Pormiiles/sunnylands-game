using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Animator playerAnim;
    private Rigidbody2D playerRigidBody;

    public float playerSpeed;
    public Transform groundCheck;
    public bool isPlayerTouchingTheGround;

    public float touchRun = 0.0f; // Movimentação do Player no eixo Horizontal 

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
    }

    void PlayerMove(float horizontalMovement) // Método de movimentação (Walk) do Player
    {
        playerRigidBody.velocity = new Vector2(horizontalMovement * playerSpeed, playerRigidBody.velocity.y);
    }

    private void FixedUpdate() 
    {
        PlayerMove(touchRun);
    }
}
