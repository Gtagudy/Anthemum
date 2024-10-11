using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] Animator animator;

    [SerializeField] SpriteRenderer spriteRenderer;

    [SerializeField] float speed;
    [SerializeField] float groundDist;
    [SerializeField] LayerMask terrainLayer;

    [SerializeField] Rigidbody rb;

    [SerializeField] float sr;

    public float movementPoints = 50f;

    public int hp = 100;

    public int strength = 20;

    public string Name = "Unnamed";

    bool faceRight = true;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Vector3 castPos = transform.position;
        castPos.y += 1;
        if (Physics.Raycast(castPos, -transform.up, out hit, Mathf.Infinity, terrainLayer))
        {
            if (hit.collider != null)
            {
                Vector3 movePos = transform.position;
                movePos.y = hit.point.y + groundDist;
                transform.position = movePos;
            }
        }

        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        Vector3 moveDir = new Vector3(x, 0, y);
        rb.velocity = moveDir * speed;

        if (moveDir.x > 0 && !faceRight) Flip();
        if (moveDir.x < 0 && faceRight) Flip();

        animator.SetFloat("Speed", Mathf.Abs(moveDir.x));


    }

    private void Flip()
    {
        faceRight = !faceRight;
        spriteRenderer.flipX = !faceRight;
    }
}

