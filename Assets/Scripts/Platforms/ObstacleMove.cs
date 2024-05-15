using UnityEngine;

namespace Platforms
{
    public class ObstacleMove : MonoBehaviour
    {
        private void Update()
        {
            // Move the obstacle towards the player.
            transform.Translate(new Vector3(0, 0, -(GameManager.Instance.forwardMoveSpeed * Time.deltaTime))); 
        }
    } 
}

