using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WarriorMovementLogic
{
    public float moveSpeed;
    public float jumpForce;
    public float jumpTime;
    public SpriteRenderer sr;
    public Rigidbody2D rb;
    public Transform groundDetector;
    public LayerMask groundLayer;

    private float currentAxis;
    private bool grounded;
    private float jumpTimer;

    private void Awake() {
        currentAxis = 0;
    }

    public void checkMovement(PlayerState playerState) {
        if(playerState == PlayerState.ATTACKING) {
            rb.velocity = Vector2.zero;
        } else {
            currentAxis = Input.GetAxisRaw("Horizontal");
            if(currentAxis != 0) {
                rb.velocity = new Vector2(moveSpeed * currentAxis, rb.velocity.y);
                sr.transform.localScale = (currentAxis > 0) ? new Vector3(1, 1, 1) : new Vector3(-1, 1, 1);
            } else {
                rb.velocity = new Vector2(Mathf.Lerp(rb.velocity.x, 0, 0.7f), rb.velocity.y);
            }
        }
    }

    public void checkJump(PlayerState playerState) {
        grounded = Physics2D.OverlapCircle(groundDetector.transform.position, 0.1f, groundLayer);
        if(Input.GetKey(KeyCode.Space) && playerState != PlayerState.ATTACKING && playerState != PlayerState.COMBAT) {
            if(grounded) {
                jumpTimer = jumpTime;
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            }else {
                if(jumpTimer > 0) {
                    jumpTimer -= Time.deltaTime;
                    rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                }
            }
        }

        if(Input.GetKeyUp(KeyCode.Space)) {
            jumpTimer = 0;
        }
    }

    public float getCurrentAxis() { return currentAxis;}
    public bool getGrounded() {return grounded;}
}
