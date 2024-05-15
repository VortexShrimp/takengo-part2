using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggController : MonoBehaviour
{
    [SerializeField] float moveSpeed;

    Rigidbody _rb;

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
       Destroy(gameObject, 3f);
    }

    void FixedUpdate()
    {
        _rb.MovePosition(_rb.position + new Vector3(0, 0, moveSpeed * Time.fixedDeltaTime));
    }
}
