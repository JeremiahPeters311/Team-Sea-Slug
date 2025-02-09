using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    [SerializeField]
    float _bulletSpeed = 4;
    readonly List<Vector2>  vectorDirections = new() { Vector2.up, Vector2.down, Vector2.left, Vector2.right};
    Vector3 travelDirection = Vector2.zero;

    private void FixedUpdate()
    {
        Move();
    }

    public void SetDirection(Vector2 playerMovementDirection, bool isPlayerFacingLeft) 
    {
        if (playerMovementDirection == Vector2.zero) 
        {
            travelDirection = isPlayerFacingLeft ? Vector3.left : Vector3.right;
            return;
        }
        travelDirection = playerMovementDirection;
    }

    private void Move() 
    {
        transform.position += _bulletSpeed * Time.deltaTime * travelDirection;
    }
}
