using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Random = UnityEngine.Random;
using Vector3 = UnityEngine.Vector3;

namespace Platforms
{
    /// <summary>
    /// Represents a platform in the game.
    /// </summary>
    public class Platform : MonoBehaviour
    {
        public delegate void PlayerLeftPlatformHandler(Platform platform);
       
        // Called when a platform needs to be destroyed.
        public static event PlayerLeftPlatformHandler OnPlayerLeftPlatform;

        public delegate void BossSpawnHandler();
        public static BossSpawnHandler OnBossSpawn;

        [SerializeField] Transform connector;
        [SerializeField] int laneCount;
        [SerializeField] GameObject bossPrefab;
        [SerializeField] GameObject[] pickupPrefabs;
        [SerializeField] GameObject[] obstaclePrefabs;
        [SerializeField] GameObject[] fruitPickupPrefabs;

        // We need this to access the size of the platform.
        Renderer _floorRenderer;
        Rigidbody _floorRigidbody;

        // A reference to the pickup which this platform spawned.
        GameObject _pickup;
        GameObject _obstacle;
        GameObject _fruit;

        // Expose the platform connector's position so that the spawner can access it.
        public Vector3 ConnectorPos => connector.position;

        private void Awake()
        {
            var floor = gameObject.transform.GetChild(0).gameObject;
            
            _floorRenderer = floor.GetComponent<Renderer>();
            _floorRigidbody = floor.GetComponent<Rigidbody>();
        }

        private void Start()
        {
            // Player gets a certain immunity time where objects dont spawn.
            if (GameManager.Instance.spawnProtection)
                return;

            // Size and position of the floor geometry in the world.
            Vector3 floorSize = _floorRenderer.bounds.size;
            Vector3 floorPos = _floorRigidbody.position;

            // Don't spawn obstacles or pickups when boss is active.
            if (GameManager.Instance.bossActive)
            {
                // Let x platforms pass until boss is spawned.
                if (GameManager.Instance.spawnedPlatforms - GameManager.Instance.spawnedPlatformsWhenBossActivated == 3
                    && GameManager.Instance.bossSpawned == false)
                {
                    var bossPosition = new Vector3(
                        floorPos.x,
                        GameManager.Instance.playerPosition.y + 2f,
                        floorPos.z
                    );

                    Instantiate(bossPrefab, bossPosition, Quaternion.identity);

                    GameManager.Instance.bossSpawned = true;

                    OnBossSpawn?.Invoke();
                }

                return;
            }
            
            // The width of a single lane.
            float laneWidth = floorSize.x / (laneCount + 1);
            float laneStart = floorPos.x - (floorSize.x / 2f);

            int randomLane = Random.Range(1, laneCount + 1);
            float spawnX = laneStart + (laneWidth * randomLane);
            var spawnPos = new Vector3(spawnX, floorPos.y + 1f, floorPos.z);

            int randomObstacle = Random.Range(0, obstaclePrefabs.Length);
            _obstacle = Instantiate(obstaclePrefabs[randomObstacle], spawnPos, Quaternion.identity); 

            // Only spawn a pickup every x platforms.
            // And don't spawn pickups when player has one active.
            if (GameManager.Instance.spawnedPlatforms % 10 == 0 && GameManager.Instance.playerHasPickup == false)
            {
                int newRandomLane = Random.Range(1, laneCount + 1);
                while (newRandomLane == randomLane)
                    newRandomLane = Random.Range(1, laneCount + 1);

                spawnX = laneStart + (laneWidth * newRandomLane);
                spawnPos = new Vector3(spawnX, floorPos.y + 3.5f, floorPos.z);

                int randomPickup = Random.Range(0, pickupPrefabs.Length);
                _pickup = Instantiate(pickupPrefabs[randomPickup], spawnPos, Quaternion.identity);
            }
            // Spawn a fruit every x platforms, as long as a pickup hasn't already been spawned.
            else if (GameManager.Instance.spawnedPlatforms % 6 == 0)
            {
                int newRandomLane = Random.Range(1, laneCount + 1);
                while (newRandomLane == randomLane)
                    newRandomLane = Random.Range(1, laneCount + 1);

                spawnX = laneStart + (laneWidth * newRandomLane);
                spawnPos = new Vector3(spawnX, floorPos.y + 3.5f, floorPos.z);

                int randomPickup = Random.Range(0, fruitPickupPrefabs.Length);
                _fruit = Instantiate(fruitPickupPrefabs[randomPickup], spawnPos, Quaternion.identity);
            }
        }

        private void Update()
        {
            // Move the platform towards the player.
           transform.Translate(new Vector3(0, 0, -(GameManager.Instance.forwardMoveSpeed * Time.deltaTime)));
        }

        private void OnTriggerExit(Collider other)
        {
            // Make sure the player has left the trigger.
            if (other.CompareTag("Player") == false)
                return;
           
            // Fire an event to notify everyone.
            OnPlayerLeftPlatform?.Invoke(this);
            
            // Pickups are only destroyed when the player hits them.
            // By this point, if _pickup is not equal to null, the player missed it.
            if (_pickup != null)
                Destroy(_pickup);
            
            Destroy(_obstacle);
        }
    }
}
