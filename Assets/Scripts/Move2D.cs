using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// based on https://www.youtube.com/watch?v=L6Q6VHueWnU
public class Move2D : MonoBehaviour
{
    public float horizontalSpeed = 5f;
    public float jumpForce = 8f;
    public bool isGrounded = true;
    public bool isAtEnd = false;
    public float horizontalAxis = 0f;
    private bool jumpPressed = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Jump();
        var hInput = new Vector3(horizontalAxis, 0f);
        transform.position += hInput * Time.fixedDeltaTime * horizontalSpeed;
    }

    void Update()
    {
        horizontalAxis = Input.GetAxis("Horizontal");
        GetComponent<SpriteRenderer>().flipX = (horizontalAxis < 0);
        if (Input.GetButtonDown("Jump"))
        {
            jumpPressed = true;
        }
    }

    void Jump()
    {
        if (jumpPressed && isGrounded)
        {
            gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
        }
        jumpPressed = false;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Finish")
        {
            isAtEnd = true;
            StartCoroutine(LoadSceneAfterTransition());
        }
    }

    private IEnumerator LoadSceneAfterTransition()
    {
        //show animate out animation
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
