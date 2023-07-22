using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState {
    IDLE, RUNNING, JUMPING, ATTACKING, COMBAT
}

public class WarriorBaseLogic : MonoBehaviour {

    public WarriorMovementLogic warriorMovementLogic;
    public Animator anim;

    private PlayerState playerState;

    private void Awake() {
        playerState = PlayerState.IDLE;
    }

    private void Update() {
        if(playerState != PlayerState.COMBAT) {
            checkMovementLogic();
            checkAttackLogic();
        }
    }

    private void LateUpdate() {
        updateAnimations();
    }

    private void checkMovementLogic() {
        warriorMovementLogic.checkMovement(playerState);
        warriorMovementLogic.checkJump(playerState);
        if(playerState != PlayerState.ATTACKING) {
            if(warriorMovementLogic.getGrounded()) playerState = PlayerState.JUMPING;
            playerState = (warriorMovementLogic.getCurrentAxis() != 0) ? PlayerState.RUNNING : PlayerState.IDLE;
        }
    }

    private void checkAttackLogic() {
        if(Input.GetKeyDown(KeyCode.J) && canAttack()) {
            playerState = PlayerState.ATTACKING;
            StartCoroutine(CoFinishAttack());
        }
    }
 
    private IEnumerator CoFinishAttack() {
        anim.SetTrigger("attack");
        yield return new WaitForSeconds(0.6f);
        playerState = PlayerState.IDLE;
    }

    private void updateAnimations() {
        anim.SetBool("moving", warriorMovementLogic.getCurrentAxis() != 0);
        anim.SetBool("grounded", warriorMovementLogic.getGrounded());
    }

    private bool canAttack() {
        return (playerState != PlayerState.ATTACKING && 
                playerState != PlayerState.JUMPING);
    }
}
