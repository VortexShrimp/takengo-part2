using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCartController : MonoBehaviour
{
    [SerializeField] float moveSpeed;

    void Start()
    {
       Destroy(gameObject, 2f);
    }

    void Update()
    {
        transform.Translate(new Vector3(0, 0, -moveSpeed * Time.deltaTime));
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Egg"))
            return;

        Debug.Log("Egg hit cart.");

        // Destroy eggs if they hit the boss carts.
        Destroy(other.gameObject);
    }
}
