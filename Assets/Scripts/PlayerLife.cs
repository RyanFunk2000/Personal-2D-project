using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerLife : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    private PlayerMovement movement;
    private SpriteRenderer sprite;

    private float rotateMagnitude = 0f;
    private float rotateSign = 0f;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        movement = GetComponent<PlayerMovement>();
        sprite = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Blastzone"))
        {
            Die();
        }
    }

    private void Die()
    {
        rb.bodyType = RigidbodyType2D.Static;
        this.movement.enabled = false;
        anim.SetTrigger("death");

        if (gameObject.transform.position.y > 5)
        {
            rotateMagnitude = 90f;
            rotateSign = 1f;
            sprite.flipX = false;
        }
        else if (gameObject.transform.position.y < -8)
        {
            rotateMagnitude = 90f;
            rotateSign = -1f;
            sprite.flipX = false;
        }

        if (gameObject.transform.position.x > 12)
        {
            rotateMagnitude -= 45f;
        }
        else if (gameObject.transform.position.x < -12)
        {
            rotateMagnitude += 45f;
        }

        transform.Rotate(Vector3.forward * rotateMagnitude * rotateSign);
    }

    private void Respawn()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
