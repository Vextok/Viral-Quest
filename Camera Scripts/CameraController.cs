using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    Transform target;
    Vector3 velocity = Vector3.zero;
    [Range(0, 1)]
    public float smoothTime;
    public Vector2 xlimit;
    public Vector2 ylimit;
    public Vector3 positionOffset;

    public void Awake()
    {
        // Target will be the player's transform variables
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void LateUpdate()
    {
        // The target position will then be offset by an amount
        Vector3 targetPosition = target.position + positionOffset;
        // The new x and y position will be within the limits of the level
        targetPosition = new Vector3(Mathf.Clamp(targetPosition.x, xlimit.x, xlimit.y), Mathf.Clamp(targetPosition.y, ylimit.x, ylimit.y), -10);
        // The position of the camera will gradually change to the player's position
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }
}
