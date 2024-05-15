using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;
using Platforms;

namespace UserInterface
{
    /// <summary>
    /// Controls the UI in the "Main Game" scene.
    /// </summary>
    public class MainGameController : MonoBehaviour
    {
        public delegate void ScoreGoalReachedHandler();
        public static ScoreGoalReachedHandler OnScoreGoalReached;

        [Tooltip("The TextMesh object you want to hold the score.")]
        [SerializeField] TextMeshProUGUI scoreTextMesh;
        [SerializeField] TextMeshProUGUI desiredFruitTextMesh;
        [SerializeField] TextMeshProUGUI bossHealthText;

        [SerializeField] string[] fruitNames;
    
        private void OnEnable()
        {
            Pickup.OnPlayerEnteredPickup += OnPlayerEnteredPickup;
            ObstacleNormalCollider.OnPlayerEnteredCollider += OnPlayerEnteredObstacleNormalCollider;
            ObstacleExtendedCollider.OnPlayerEnteredCollider += OnPlayerEnteredObstacleExtendedCollider;
            PickupFruit.OnPlayerPickupFruit += OnPlayerPickupFruit;
            BossController.OnBossHit += OnBossHit;
            Platform.OnBossSpawn += OnBossSpawn;
        }

        private void OnDisable()
        {
            Pickup.OnPlayerEnteredPickup -= OnPlayerEnteredPickup;
            ObstacleNormalCollider.OnPlayerEnteredCollider -= OnPlayerEnteredObstacleNormalCollider;
            ObstacleExtendedCollider.OnPlayerEnteredCollider -= OnPlayerEnteredObstacleExtendedCollider;
            PickupFruit.OnPlayerPickupFruit -= OnPlayerPickupFruit;
            BossController.OnBossHit -= OnBossHit;
            Platform.OnBossSpawn -= OnBossSpawn;
        }

        private void Start()
        {
            GameManager.Instance.bossActive = false;
            GameManager.Instance.bossSpawned = false;
            GameManager.Instance.bossKilled = false;

            // Initialize score to zero.
            GameManager.Instance.score = 0;
            UpdateScoreText();

            int randomFruitName = Random.Range(0, fruitNames.Length);
            GameManager.Instance.desiredFruit = fruitNames[randomFruitName];

            desiredFruitTextMesh.text = $"Desired Fruit: {GameManager.Instance.desiredFruit}";

            bossHealthText.text = "";
        }

        private void OnPlayerEnteredObstacleNormalCollider()
        {
            // Player is invincible.
            if (GameManager.Instance.playerHasShield == true)
                return;

            // Player hit an obstacle, show them the game over scene.
            SceneManager.LoadSceneAsync("Game Over", LoadSceneMode.Single);
        }

        private void OnPlayerEnteredObstacleExtendedCollider()
        {
            if (GameManager.Instance.bossActive)
                return;

            // Player performed a near miss.
            GameManager.Instance.score += 10;
            ClampPlayerScore();
            UpdateScoreText();
        }
    
        private void OnPlayerEnteredPickup()
        {
            if (GameManager.Instance.bossActive)
                return;

            // Add some score for pickups.
            GameManager.Instance.score += 5;
            ClampPlayerScore();
            UpdateScoreText();
        }

        private void OnPlayerPickupFruit(string fruitName)
        {
            if (GameManager.Instance.bossActive)
                return;

            if (Equals(fruitName, GameManager.Instance.desiredFruit))
                GameManager.Instance.score += 50;
            else
                GameManager.Instance.score -= 50;

            ClampPlayerScore();
            UpdateScoreText();
        }

        private void OnBossSpawn()
        {
            bossHealthText.text = $"Johannes is coming!";
        }

        private void OnBossHit(int maxHitPoints, int remaining)
        {
            bossHealthText.text = $"Johannes health remaining ({remaining} / {maxHitPoints})";
        }

        // Helper function for setting the player's score.
        private void UpdateScoreText()
        {
            scoreTextMesh.text = $"Score: {GameManager.Instance.score} / {GameManager.Instance.scoreGoal}"; 
        }

        private void ClampPlayerScore()
        {
            if (GameManager.Instance.score < 0)
            {
                // Ensure score does not fall below 0;
                GameManager.Instance.score = 0;
            }
            else if (GameManager.Instance.score >= GameManager.Instance.scoreGoal)
            {
                GameManager.Instance.score = GameManager.Instance.scoreGoal;

                // Player reached the score goal. Broadcast this event.
                OnScoreGoalReached?.Invoke();

                GameManager.Instance.bossActive = true;
                GameManager.Instance.spawnedPlatformsWhenBossActivated = GameManager.Instance.spawnedPlatforms;

                desiredFruitTextMesh.text = "Kill Johannes the Manager!";
            }
        }
    }
 
}
