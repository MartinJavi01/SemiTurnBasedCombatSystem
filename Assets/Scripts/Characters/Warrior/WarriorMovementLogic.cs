using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WarriorMovementLogic
{
    public float moveSpeed;
    public float jumpForce;
    public float jumpTime;
    public List<GameObject> localScaleObjects;
    public Rigidbody2D rb;
    public Transform groundDetector, wallDetector;
    public LayerMask groundLayer;

    private float currentAxis;
    private bool grounded, onWall, blockKeyboard;
    private float jumpTimer;

    private void Awake() {
        currentAxis = 0;
    }

    public void checkMovement(PlayerState playerState) {
        if(blockKeyboard) return;
        
        if(playerState == PlayerState.ATTACKING) {
            rb.velocity = Vector2.zero;
        } else {
            currentAxis = Input.GetAxisRaw("Horizontal");
            if(currentAxis != 0) {
                rb.velocity = new Vector2(moveSpeed * currentAxis, rb.velocity.y);
                foreach(GameObject g in localScaleObjects) {
                    g.transform.localScale = (currentAxis > 0) ? new Vector3(1, 1, 1) : new Vector3(-1, 1, 1);
                }
            } else {
                if(rb.velocity.y < 2) rb.velocity = new Vector2(Mathf.Lerp(rb.velocity.x, 0, 0.7f), rb.velocity.y);
            }
        }
    }

    public void checkJump(PlayerState playerState) {
        grounded = Physics2D.OverlapCircle(groundDetector.transform.position, 0.1f, groundLayer);
        if(Input.GetKey(KeyCode.Space) && playerState != PlayerState.ATTACKING && playerState != PlayerState.COMBAT) {
            if(grounded && jumpTimer <= 0) {
                jumpTimer = jumpTime;
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            }else if(!grounded && jumpTimer > 0){
                jumpTimer -= Time.deltaTime;
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            }
        }

        if(Input.GetKeyUp(KeyCode.Space)) {
            jumpTimer = 0;
        }
    }

    public void checkWallSlideLogic(GameObject refObj) {
        if(!grounded) {
            onWall = Physics2D.OverlapCircle(wallDetector.transform.position, 0.1f, groundLayer);
            if(onWall) {
                if(rb.velocity.y > 2) rb.velocity = new Vector2(rb.velocity.x, 2);
                if(rb.velocity.y < -2) rb.velocity = new Vector2(rb.velocity.x, -2);
                if(Input.GetKeyDown(KeyCode.Space)) {
                    onWall = false;
                    blockKeyboard = true;
                    rb.velocity = new Vector2(jumpForce * -refObj.transform.localScale.x, jumpForce * 1.5f);
                    foreach(GameObject g in localScaleObjects) {
                        g.transform.localScale = (rb.velocity.x > 0) ? new Vector3(1, 1, 1) : new Vector3(-1, 1, 1);
                    }
                }
            }
        } else {
            onWall = false;
        }
    }

    public float getCurrentAxis() { return currentAxis;}
    public bool getGrounded() {return grounded;}
    public bool getOnWall() {return onWall;}
    public bool getBlockKeyboard() {return blockKeyboard;}
    public void setBlockKeyBoard(bool b) { blockKeyboard = b;}
}
