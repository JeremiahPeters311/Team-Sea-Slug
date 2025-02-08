/******************************************************************
 *    Author: Mitchell Young
 *    Contributors:
 *    Date Created: 2/8/25
 *    Description: Detects teleport reticle collision for player
 *    controller script.
 *******************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportCollision : MonoBehaviour
{
    public bool canTeleport = true;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        canTeleport = false;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        canTeleport = true;
    }
}
