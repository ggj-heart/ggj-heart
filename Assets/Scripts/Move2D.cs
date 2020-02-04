using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// based on https://www.youtube.com/watch?v=L6Q6VHueWnU
public class Move2D : MonoBehaviour
{
    public float horizontalSpeed = 10f;
    public float jumpForce = 16f;
    public SpriteRenderer sceneDivider;
    public bool isGrounded = true;
    public bool isAtEnd = false;
    public float horizontalAxis = 0f;
    private bool jumpPressed = false;
    private AudioSource jumpAudioSource;
    private AudioClip[] jumpClips;

    // Start is called before the first frame update
    void Start()
    {
        sceneDivider.enabled = false;
        jumpAudioSource = gameObject.AddComponent<AudioSource>();
        jumpClips = new[] {
            Resources.Load<AudioClip>("Audio/Jumping_Sound")
        };
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
            
            var jumpClip = jumpClips[Random.Range(0, jumpClips.Length)];
            jumpAudioSource.volume = Random.Range(0.2f, 0.8f);
            jumpAudioSource.PlayOneShot(jumpClip);
        }
        jumpPressed = false;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Finish")
        {
            isAtEnd = true;
            sceneDivider.color = new Color(1, 1, 1, 0);
            sceneDivider.enabled = true;
            StartCoroutine(LoadSceneAfterTransition());
        }
    }

    private IEnumerator LoadSceneAfterTransition()
    {
        yield return FadeOut();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    // fade out over 1 second
    IEnumerator FadeOut()
    {
        for (float i = 0; i <= 1f; i += Time.fixedDeltaTime)
        {
            var c = sceneDivider.color;
            sceneDivider.color = new Color(c.r, c.g, c.b, i);
            yield return new WaitForSeconds(Time.fixedDeltaTime);
        }
    }
}
