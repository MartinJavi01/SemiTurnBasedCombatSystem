using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState {
    IDLE, RUNNING, JUMPING
}

public class WarriorBaseLogic : MonoBehaviour {

    public WarriorMovementLogic warriorMovementLogic;
    public Animator anim;

    private PlayerState playerState;

    private void Awake() {
        playerState = PlayerState.IDLE;
    }

    private void Update() {
        checkMovementLogic();
    }

    private void LateUpdate() {
        updateAnimations();
    }

    private void checkMovementLogic() {
        warriorMovementLogic.checkMovement();
        playerState = (warriorMovementLogic.getCurrentAxis() != 0) ? PlayerState.RUNNING : PlayerState.IDLE;

        warriorMovementLogic.checkJump();
    }

    private void updateAnimations() {
        anim.SetBool("moving", warriorMovementLogic.getCurrentAxis() != 0);
        anim.SetBool("grounded", warriorMovementLogic.getGrounded());
    }
}
