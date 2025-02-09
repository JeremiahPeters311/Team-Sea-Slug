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
    public bool atEndOfArea = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("EndAreaMarker"))
        {
            atEndOfArea = true;
        }
    }
}
