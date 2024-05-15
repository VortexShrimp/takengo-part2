using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;

public class BossController : MonoBehaviour
{
    public delegate void BossHitHandler(int maxHealth, int remainingHealth);
    public static BossHitHandler OnBossHit;

    [SerializeField] int maxHitPoints;
    int _hitPoints;
    [SerializeField] float distanceFromPlayer;
    [SerializeField] GameObject cartPrefab;

    bool _bossDoneMoving = true;
    bool _bossNeedsShot = true;

    void Start()
    {
        _hitPoints = maxHitPoints;
    }

    // Update is called once per frame
    void Update()
    {
        var pos = transform.position;

        // Z distance between boss and player.
        float deltaZ = pos.z - GameManager.Instance.playerPosition.z;

        if (deltaZ > distanceFromPlayer)
        {
            // Move the boss towards the player.
            transform.Translate(new Vector3(0, 0, -(GameManager.Instance.forwardMoveSpeed * Time.deltaTime)));
        }
        else
        {  
            if (_bossDoneMoving)
            {
                _bossDoneMoving = false;

                StartCoroutine(MoveBoss());
            }

            if (_bossNeedsShot)
            {
                var cartPos = new Vector3(pos.x, pos.y - 3f, pos.z - 2f);
                Instantiate(cartPrefab, cartPos, Quaternion.identity);

                _bossNeedsShot = false;

                float randTimeBetweenShots = Random.Range(0.5f, 2f);
                StartCoroutine(WaitForNextShot(randTimeBetweenShots));
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Make sure boss is hit by an egg.
        if (!other.CompareTag("Egg"))
            return;

        Debug.Log("Egg hit Boss.");

        Destroy(other.gameObject);

        _hitPoints -= 10;

        OnBossHit?.Invoke(maxHitPoints, _hitPoints);

        if (_hitPoints <= 0)
        {
            GameManager.Instance.bossKilled = true;

            SceneManager.LoadSceneAsync("Game Over", LoadSceneMode.Single);
        }
    }

    IEnumerator MoveBoss()
    {
        Vector3 start = transform.position;
        Vector3 end = start + (Vector3.left * 5f);

        float timeElapsedSeconds = 0;

        while (timeElapsedSeconds < 2f)
        {
            transform.position = Vector3.Lerp(start, end, timeElapsedSeconds / 2f);
            timeElapsedSeconds += Time.deltaTime;
            yield return null;
        }

        start = transform.position;
        end = start + (Vector3.right * 10f);

        timeElapsedSeconds = 0;

        while (timeElapsedSeconds < 4f)
        {
            transform.position = Vector3.Lerp(start, end, timeElapsedSeconds / 4f);
            timeElapsedSeconds += Time.deltaTime;
            yield return null;
        }

        start = transform.position;
        end = start + (Vector3.left * 5f);

        timeElapsedSeconds = 0;

        while (timeElapsedSeconds < 2f)
        {
            transform.position = Vector3.Lerp(start, end, timeElapsedSeconds / 2f);
            timeElapsedSeconds += Time.deltaTime;
            yield return null;
        }

        _bossDoneMoving = true;
    }

    IEnumerator WaitForNextShot(float timeSeconds)
    {
        float timeElapsedSeconds = 0;

        while (timeElapsedSeconds < timeSeconds)
        {
            timeElapsedSeconds += Time.deltaTime;
            yield return null;
        }

        _bossNeedsShot = true;
    }
}
