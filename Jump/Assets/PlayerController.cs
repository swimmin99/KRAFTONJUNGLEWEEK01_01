using System.Collections;
using System.Collections.Generic;
using UnityEngine; 

public class PlayerController : MonoBehaviour
{

    public GameObject player;
    public Rigidbody2D playerRigid;
    public float playerFallGravityScale;
    public float playerMaxVelocity;
    public float playerMoveSpeed;
    public float playerMaxSpeed;
    public Vector2 jumpDirection;
    public bool isPlayerJump;
    public bool isPlayerMove;
    private bool isGrounded;
    public float jumpForce;
    public float jumpLimitTime;
    public int maxJumpStep;
    public float jumpTimer;

    public float minBackBounceX;
    public float maxBackBounceX;

    public float minBackBounceY;
    public float maxBackBounceY;

    public float maxBackBounceForce;
    public float minBackBounceForce;

    public ForceGameController forceGameController;

    public float minJumpValue;

    void Start()
    {
        player = gameObject;
        playerRigid = gameObject.GetComponent<Rigidbody2D>();
    }

    void PlayerMove()
    {
        if (isPlayerMove)
        {
            if (Input.GetKey(KeyCode.RightArrow))
            {
                if (playerRigid.velocity.magnitude < playerMaxSpeed)
                {
                    playerRigid.AddForce(Vector2.right * playerMoveSpeed);
                }
                
            }

            else if (Input.GetKey(KeyCode.LeftArrow))
            {
                if (playerRigid.velocity.magnitude < playerMaxSpeed)
                {
                    playerRigid.AddForce(-Vector2.right * playerMoveSpeed);
                }
            }
        }
        
    }

    void PlayerJump()
    {
        if (isPlayerJump)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                forceGameController.gameObject.SetActive(true);
                forceGameController.IncreaseSpeedGauge(jumpLimitTime * 1.5f);
                isGrounded = true;
            }

            if (Input.GetKey(KeyCode.Space))
            {
                if(isGrounded)
                {
                    if (jumpTimer < jumpLimitTime)
                    {
                        jumpTimer += Time.deltaTime;
                        Debug.Log(jumpTimer);
                    }
                }
            }

            if (Input.GetKeyUp(KeyCode.Space))
            {
                forceGameController.InitGauge();
                forceGameController.gameObject.SetActive(false);

                isGrounded = false;
                float interval = jumpLimitTime / (float)maxJumpStep;
                float jumpStep = 0;
                for (int i = 0; i < maxJumpStep; i++)
                {
                    if (jumpStep >= jumpTimer)
                    {
                        break;
                    }
                    jumpStep += interval;
                }
                Debug.Log(jumpStep);

                Vector2 jumpForceVector = jumpDirection.normalized * jumpStep * jumpForce;
                playerRigid.AddForce(jumpForceVector, ForceMode2D.Impulse);
                jumpTimer = 0;

            }
        }
        
    }

    void PlayerBounceBack()
    {
        float randomDirectionX = Random.Range(minBackBounceX, maxBackBounceX);
        float randomDirectionY = Random.Range(minBackBounceY, maxBackBounceY);
        Vector2 randomDirection = new Vector2(randomDirectionX, randomDirectionY);
        float randomJumpForce = Random.Range(minBackBounceForce, maxBackBounceForce);
        Debug.Log("nowForce : " + randomJumpForce);
        Vector2 jumpForceVector = randomDirection.normalized *randomJumpForce;

        playerRigid.AddForce(jumpForceVector, ForceMode2D.Impulse);
    }

    void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            PlayerBounceBack();
        }
    }

    void LimitPlayerVelocity()
    {
        if (playerRigid.velocity.magnitude > playerMaxVelocity)
        {
            playerRigid.velocity = playerRigid.velocity.normalized * playerMaxVelocity;
        }
    }

    void ControlPlayerGravityScale()
    {
        if (playerRigid.velocity.y < 0)
        {
            playerRigid.gravityScale = playerFallGravityScale;
        }
        else
        {
            
            playerRigid.gravityScale =1;
        }
    }

    void HideGaugeByVelocity()
    {

        if (playerRigid.velocity.y > minJumpValue)
        {
            forceGameController.InitGauge();
            forceGameController.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        LimitPlayerVelocity();
        PlayerJump();
        HideGaugeByVelocity();
       // ControlPlayerGravityScale();
    }

    void FixedUpdate()
    {
        PlayerMove();
    }
}
