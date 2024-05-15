using UnityEngine;

namespace Platforms
{
    public class ObstacleNormalCollider : MonoBehaviour
    {
        public delegate void PlayerEnteredNormalColliderHandler();

        public static event PlayerEnteredNormalColliderHandler OnPlayerEnteredCollider;
        
        private void OnTriggerEnter(Collider other)
        {
            // Make sure our local player triggered this.
            if (other.CompareTag("Player") == false)
                return;
            
            // Broadcast that the player hit an obstacle.
            OnPlayerEnteredCollider?.Invoke();

            // Always destroy the obstacle.
            Destroy(gameObject.transform.parent.gameObject);
        }
    } 
}
