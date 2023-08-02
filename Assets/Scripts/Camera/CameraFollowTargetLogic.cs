using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowTargetLogic : MonoBehaviour
{
    public string targetTag;
    public float smothness;
    public Vector2 minPos;
    public Vector2 maxPos;
    private GameObject target;
    private float heightDiff;

    private void Awake() {
        target = GameObject.FindGameObjectWithTag(targetTag);
        heightDiff = 3;
    }

    private void Update() {
        if(transform.position.x != target.transform.position.x || transform.position.y != transform.position.y + heightDiff) {
            transform.position = new Vector3(
                Mathf.Lerp(transform.position.x, target.transform.position.x, smothness),
                Mathf.Lerp(transform.position.y, target.transform.position.y + heightDiff, smothness), -10);
        }

        if(transform.position.x < minPos.x) transform.position = new Vector3(minPos.x, transform.position.y, transform.position.z);
        if(transform.position.x > maxPos.x) transform.position = new Vector3(maxPos.x, transform.position.y, transform.position.z);
        if(transform.position.y < minPos.y) transform.position = new Vector3(transform.position.x, minPos.y, transform.position.z);
        if(transform.position.y > maxPos.y) transform.position = new Vector3(transform.position.x, maxPos.y, transform.position.z);
    }
}
