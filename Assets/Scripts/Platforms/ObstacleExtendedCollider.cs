using UnityEngine;

namespace Platforms
{
    public class ObstacleExtendedCollider : MonoBehaviour
    {
        public delegate void PlayerEnteredExtendedColliderHandler();

        public static event PlayerEnteredExtendedColliderHandler OnPlayerEnteredCollider;
        
        private void OnTriggerEnter(Collider other)
        {
            // Make sure our local player triggered this.
            if (other.CompareTag("Player") == false)
                return;
            
            // Broadcast that the player hit an obstacle.
            OnPlayerEnteredCollider?.Invoke();
        }
    } 
}
