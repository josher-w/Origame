using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crate : MonoBehaviour
{
    private bool isPulling;
    public bool IsPulling
    {
        get { return isPulling; }
    } 

    [SerializeField] private PhysicsMaterial2D crateMat;
    [SerializeField] private float gravityScale = 1;
    [SerializeField] private float mass = 5;

    [SerializeField] private float offset;
    [SerializeField] private Vector2 groundCheckSize;
    [SerializeField] private LayerMask groundLayer;

    private Vector2 startPos;

    [SerializeField] private Rigidbody2D rb;

    [SerializeField] private float respawnTime;

    [SerializeField] private AudioSource audioSource;

    private void Start()
    {
        startPos = transform.position;
    }

    private void FixedUpdate()
    {
        rb = GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            rb.gravityScale = gravityScale;
            rb.mass = mass;
            rb.sharedMaterial = crateMat;
        }

        if (isPulling && !Physics2D.OverlapBox(transform.position + Vector3.down * offset, groundCheckSize, 0f, groundLayer))
        {
            Debug.Log("bonjour");
            StopPull();
        }
    }

    public void StartPull(Transform holdPos, Vector2 offset)
    {
        transform.SetParent(holdPos);
        transform.localPosition = offset;
        transform.localEulerAngles = Vector3.zero;
        Destroy(rb);

        isPulling = true;
    }

    public void StopPull()
    {
        transform.SetParent(null);
        rb = gameObject.AddComponent<Rigidbody2D>();

        isPulling = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Kill"))
        {

            rb.gravityScale = 0;
            Invoke("Respawn", respawnTime);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Object Kill"))
        {
            Respawn();
        }
    }

    private void Respawn()
    {
        rb.velocity = Vector2.zero;
        transform.eulerAngles = Vector3.zero;
        transform.position = startPos;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position + Vector3.down * offset, groundCheckSize);
    }
}
