using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicTerrainEnemyLogic : MonoBehaviour
{
    public string playerTag;
    public float moveSpeed, actionTime, reactTime;
    public float detectRadius;
    private GameObject target;
    private Rigidbody2D rb;
    private Animator anim;
    private float startX;
    private bool performingAction, goRight, reacting;

    private void Awake() {
        target = GameObject.FindGameObjectWithTag(playerTag);
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        startX = transform.position.x;
    }

    private void Update() {
        movementLogic();
        if(performingAction) {
            if(goRight) {
                moveRight();
            } else {
                moveLeft();
            }
        }
    }

    private void LateUpdate() {
        updateAnimations();
    }

    private void movementLogic() {
        if(Mathf.Abs(transform.position.x  - target.transform.position.x) <= detectRadius && 
            Mathf.Abs(transform.position.y - target.transform.position.y) <= detectRadius / 4) {
            if(performingAction) {
                rb.velocity = new Vector2(0, rb.velocity.y);
                performingAction = false;
                react();
            }
            if(!reacting) {
                if(transform.position.x <= target.transform.position.x) {
                    moveRight();
                } else {
                    moveLeft();
                }
            }
        } else {
            if(!performingAction) {
                rb.velocity = new Vector2(0, rb.velocity.y);
                int action = Random.Range(0,2);
                switch(action) {
                    case 0:
                        StartCoroutine(CoWaitAction());
                        if(Mathf.Abs(transform.position.x - startX) > detectRadius) {
                            goRight = true;
                        } else {
                            goRight = false;
                        }
                        break;
                    case 1:
                        StartCoroutine(CoWaitAction());
                        if(Mathf.Abs(transform.position.x - startX) > detectRadius) {
                            goRight = true;
                        } else {
                            goRight = false;
                        }
                        break;
                    case 2:
                        StartCoroutine(CoWaitAction());
                        break;
                }
            }
        }
    }

    private void moveRight() {
        rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
        transform.localScale = new Vector3(1,1,1);
    }

    private void moveLeft() {
        rb.velocity = new Vector2(-moveSpeed, rb.velocity.y);
        transform.localScale = new Vector3(-1,1,1);
    }

    private IEnumerator CoWaitAction() {
        performingAction = true;
        yield return new WaitForSeconds(actionTime);
        performingAction = false;
    }

    private void react() {
        for(int i = 0; i < anim.parameterCount; i++) {
            if(anim.parameters[i].name == "react") {
                anim.SetTrigger("react");
                reacting = true;
                StartCoroutine(CoReact());
            }
        }
    }

    private IEnumerator CoReact() {
        if(transform.position.x > target.transform.position.x) {
            transform.localScale = new Vector3(-1,1,1);
        } else {
            transform.localScale = new Vector3(1,1,1);
        }
        yield return new WaitForSeconds(reactTime);
        reacting = false;
    }

    private void updateAnimations() {
        anim.SetBool("moving", rb.velocity.x != 0);
    }
}
