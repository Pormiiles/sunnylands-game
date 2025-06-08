using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlugEnemyAI : MonoBehaviour
{
    public Transform enemy;
    public SpriteRenderer enemySprite;
    public Transform[] position;
    public float slugSpeed;
    public bool isRight;
    private int idTarget;

    private void Start()
    {
        enemySprite = enemy.gameObject.GetComponent<SpriteRenderer>();
        enemy.position = position[0].position;
        idTarget = 1;
    }

    private void Update()
    {
        if(enemy != null)
        {
            enemy.position = Vector3.MoveTowards(enemy.position, position[idTarget].position, slugSpeed * Time.deltaTime);

            if(enemy.position == position[idTarget].position)
            {
                idTarget += 1;

                if(idTarget == position.Length)
                {
                    idTarget = 0;
                }
            }

            if (position[idTarget].position.x < enemy.position.x && isRight == true)
            {
                FlipSlugSprite();
            } else if (position[idTarget].position.x > enemy.position.x && isRight == false)
            {
                FlipSlugSprite();
            }
        }
    }

    void FlipSlugSprite()
    {
        isRight = !isRight;
        enemySprite.flipX = !enemySprite.flipX;
    }
}
