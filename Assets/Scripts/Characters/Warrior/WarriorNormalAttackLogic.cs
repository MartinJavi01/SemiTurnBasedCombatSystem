using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorNormalAttackLogic : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Enemy")) {
            //enter combat and damage enemy
        }
    }
}
