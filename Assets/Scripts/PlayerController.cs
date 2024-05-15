using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] GameObject eggProjectilePrefab;

    // We use this to move the player.
    private Rigidbody _rb;

    void OnEnable()
    {
        PickupRocket.OnPlayerPickupRocket += OnPlayerPickupRocket;
        PickupShield.OnPlayerPickupShield += OnPlayerPickupShield;
    }

    void OnDisable()
    {
        PickupRocket.OnPlayerPickupRocket -= OnPlayerPickupRocket;
        PickupShield.OnPlayerPickupShield -= OnPlayerPickupShield;
    }

    private void Awake()
    {
        _rb = gameObject.GetComponent<Rigidbody>();
    }

    void Start()
    {
        GameManager.Instance.ResetMovementSpeed();
        GameManager.Instance.playerHasShield = false;

        GameManager.Instance.playerHasPickup = false;
    }
    
    private void FixedUpdate()
    {
        // Get our X movement input from Unity.
        var horizontalInput = Input.GetAxis("Horizontal");
        
        // Move the player based on moveSpeed and horizontal input.
        _rb.MovePosition(_rb.position + new Vector3(horizontalInput * GameManager.Instance.sidewaysMoveSpeed * Time.fixedDeltaTime, 0, 0));

        // Store the new pos in GameManager to access it later.
        GameManager.Instance.playerPosition = _rb.position;
    }

    private void Update()
    {
        if (!GameManager.Instance.bossSpawned)
            return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            var offset = new Vector3(2.3f, 4f, -1.15f);
            Instantiate(eggProjectilePrefab, transform.position + offset, Quaternion.identity);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Boss Cart"))
            return;

        SceneManager.LoadSceneAsync("Game Over", LoadSceneMode.Single);
    }

    void OnPlayerPickupRocket()
    {
        GameManager.Instance.forwardMoveSpeed = 40;
        StartCoroutine(WaitForRocketExpiry(5f));

        GameManager.Instance.playerHasPickup = true;
    }

    void OnPlayerPickupShield()
    {
        GameManager.Instance.playerHasShield = true;
        StartCoroutine(WaitForShieldExpiry(10f));

        GameManager.Instance.playerHasPickup = true;
    }

    IEnumerator WaitForRocketExpiry(float timeSeconds)
    {
        float elapsedSeconds = 0;

        while (elapsedSeconds < timeSeconds)
        {
            elapsedSeconds += Time.deltaTime;
            yield return null;
        }

        // Restore the player's movement speed.
        GameManager.Instance.ResetMovementSpeed();
        GameManager.Instance.playerHasPickup = false;
    }

    IEnumerator WaitForShieldExpiry(float timeSeconds)
    {
        float elapsedSeconds = 0;

        while (elapsedSeconds < timeSeconds)
        {
            elapsedSeconds += Time.deltaTime;
            yield return null;
        }

        GameManager.Instance.playerHasShield = false;
        GameManager.Instance.playerHasPickup = false;
    }
}
