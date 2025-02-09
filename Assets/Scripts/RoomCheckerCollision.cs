/******************************************************************
 *    Author: Mitchell Young
 *    Contributors:
 *    Date Created: 2/8/25
 *    Description: Detects room checker collision for game
 *    manager script.
 *******************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomCheckerCollision : MonoBehaviour
{
    public bool nextRoom = false;
    public bool atEndOfArea = false;
    public Vector2 startAreaPos;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            nextRoom = true;
        }

        if (collision.gameObject.CompareTag("EndAreaMarker"))
        {
            atEndOfArea = true;
            startAreaPos.x = collision.gameObject.transform.position.x;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            nextRoom = false;
        }

        if (collision.gameObject.CompareTag("EndAreaMarker"))
        {
            atEndOfArea = false;
        }
    }
}
