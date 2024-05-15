using UnityEngine;

namespace Platforms
{
    public class Pickup : MonoBehaviour
    {
        public delegate void PlayerEnteredPickupHandler();
    
        // Sent when a pickup has been triggered. The pickup is destroyed after this is event.
        public static event PlayerEnteredPickupHandler OnPlayerEnteredPickup;

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
            OnPlayerEnteredPickup?.Invoke();
        
            // Destroy the pickup and remove it from the game.
            Destroy(gameObject);
        }
    } 
}
