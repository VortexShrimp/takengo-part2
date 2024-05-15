using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupShield : MonoBehaviour
{
    public delegate void PlayerPickupShieldHandler();
    public static event PlayerPickupShieldHandler OnPlayerPickupShield;

    private void Update()
    {
        // Move the pickup towards the player.
        transform.Translate(new Vector3(0, 0, -(GameManager.Instance.forwardMoveSpeed * Time.deltaTime)));
    }
        
    private void OnTriggerEnter(Collider other)
    {
        // Make sure our player has entered the trigger.
        if (other.CompareTag("Player") == false)
            return;
        
        // Broadcast the event to any listeners.
        OnPlayerPickupShield?.Invoke();
        
        // Destroy the pickup and remove it from the game.
        Destroy(gameObject);
    }
}
