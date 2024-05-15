using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Platforms
{
    /// <summary>
    /// Manages the creation and deletion of new platforms.
    /// </summary>
    public class PlatformSpawner : MonoBehaviour
    {
        [Tooltip("The selection of random platforms to choose from.")]
        [SerializeField] GameObject[] platformPrefabs;
        
        [Tooltip("The amount of platforms you want to initially spawn.")]
        [SerializeField] int initialPlatformCount;
        [SerializeField] float spawnProtectionSeconds;
        
        // The last spawned platform.
        private Platform _lastPlatform;

        private void OnEnable()
        {
            Platform.OnPlayerLeftPlatform += OnPlayerLeftPlatform;
        }

        private void OnDisable()
        {
            Platform.OnPlayerLeftPlatform -= OnPlayerLeftPlatform;
        }

        private void Start()
        {
            // Reset this everytime platform spawner starts.
            GameManager.Instance.spawnedPlatforms = 0;
            GameManager.Instance.spawnedPlatforms -= initialPlatformCount;

            // Stop the game from spawning obstacles right in the beginning.
            GameManager.Instance.spawnProtection = true;
            StartCoroutine(SpawnProtectionTimer(spawnProtectionSeconds));

            _lastPlatform = null;
            
           // Spawn the initial amount of platforms desired. 
            for (var i = 0; i < initialPlatformCount; ++i)
                SpawnPlatform();
        }

        // Spawns a platform at _lastPlatform's connector.
        private void SpawnPlatform()
        {
            // To randomly select isles to spawn.
            var randomIndex = Random.Range(0, platformPrefabs.Length);

            // Spawn a new platform at the previous connector.
            var newPlatform = Instantiate(platformPrefabs[randomIndex],
                // Last platform will be null at the very beginning.
                _lastPlatform == null ? Vector3.zero : _lastPlatform.ConnectorPos,
                Quaternion.identity);

            // Save the last platform created.
            _lastPlatform = newPlatform.GetComponent<Platform>();

            GameManager.Instance.spawnedPlatforms += 1;
        }

        private void OnPlayerLeftPlatform(Platform platform)
        {
            // Destroy the platform that we just left with a second delay.
            Destroy(platform.gameObject, 2f);
            
            // Spawn a new one at the last platform's connector.
            SpawnPlatform();
        }

        IEnumerator SpawnProtectionTimer(float timeSeconds)
        {
            float elapsedSeconds = 0;

            while (elapsedSeconds < timeSeconds)
            {
                elapsedSeconds += Time.deltaTime;
                yield return null;
            }

            GameManager.Instance.spawnProtection = false;
        }
    } 
}
