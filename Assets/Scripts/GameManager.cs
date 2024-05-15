using UnityEngine;

/// <summary>
/// A singleton which persists through scene changes.
/// </summary>
public class GameManager : MonoBehaviour
{
    // Singleton instance.
    private static GameManager _instance;
    public static GameManager Instance => _instance;

    public float forwardMoveSpeed;
    float _originalMoveSpeed;
    public float sidewaysMoveSpeed;

    public bool spawnProtection;

    public int spawnedPlatforms = 0;
    public int spawnedPlatformsWhenBossActivated = 0;

    public bool playerHasShield = false;
    public bool playerHasPickup = false;
    public bool bossActive = false;
    public bool bossSpawned = false;
    public bool bossKilled = false;

    public int score = 0;
    public int scoreGoal = 500;

    public string desiredFruit;

    public Vector3 playerPosition = Vector3.zero;

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
            return;
        }
        
        _instance = this;
        
        // Make sure this singleton doesn't get destroyed.
        DontDestroyOnLoad(gameObject);

        // The the movement speed that the game begins with.
        _originalMoveSpeed = forwardMoveSpeed;
    }

    public void ResetMovementSpeed()
    {
        forwardMoveSpeed = _originalMoveSpeed;
    }
}
