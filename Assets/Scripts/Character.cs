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

    bool faceRight = true;

	[SerializeField] public AudioClip[] footSteps;
    [SerializeField] AudioSource footStep;
	public float stepTiming = 0.5f;
	public float stepTimer = 0;

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
        rb.linearVelocity = moveDir * speed;
        if (rb.linearVelocity.x > 0 || rb.linearVelocity.y > 0)
        {
            stepTimer += Time.deltaTime;
            if (stepTimer >= stepTiming) 
            { 
                if (footSteps.Length > 0)
                {
                    footStep.clip = footSteps[UnityEngine.Random.Range(0, footSteps.Length)];
                    footStep.Play();
                    stepTimer = 0;
                }
            }
        }
        if (moveDir.x > 0 && !faceRight) Flip();
        if (moveDir.x < 0 && faceRight) Flip();

        animator.SetFloat("Speed", Mathf.Abs(moveDir.x));


    }

    private void Flip()
    {
        faceRight = !faceRight;
        spriteRenderer.flipX = !faceRight;
    }

    public float GetSpeed()
    {
        return speed;
    }
    public void UpdateSpeed(float Speed)
    {
        speed = Speed;
    }


}

