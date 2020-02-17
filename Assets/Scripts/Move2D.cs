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
    private int touchDpadFingerId = -1;
    private Vector2 touchDpadStartPosition;
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
        // handle input from keyboard or gamepad
        horizontalAxis = Input.GetAxis("Horizontal");
        if (Input.GetButtonDown("Jump"))
        {
            jumpPressed = true;
        }

        // handle touch input for mobile devices (left side is virtual d-pad, right side is jump)
        if (Input.touchCount > 0)
        {
            foreach (var touch in Input.touches)
            {
                if ((touch.position.x < 0 && touchDpadFingerId == -1) ||
                    touch.fingerId == touchDpadFingerId)
                {
                    switch (touch.phase)
                    {
                        case TouchPhase.Began:
                            touchDpadFingerId = touch.fingerId;
                            touchDpadStartPosition = touch.position;
                            horizontalAxis = 0f;
                            break;
                        case TouchPhase.Ended:
                        case TouchPhase.Canceled:
                            touchDpadFingerId = -1;
                            touchDpadStartPosition = Vector2.zero;
                            horizontalAxis = 0f;
                            break;
                        case TouchPhase.Moved:
                        case TouchPhase.Stationary:
                            var x = (touch.position - touchDpadStartPosition).x / Screen.width * 10f;
                            if (Mathf.Abs(x) < 0.1f) {
                                x = 0f;
                            }
                            horizontalAxis = Mathf.Clamp(x, -1f, 1f);
                            break;
                    }
                    continue;
                }

                if (touch.position.x > 0 && touch.phase == TouchPhase.Began)
                {
                    jumpPressed = true;
                }
            }
        }

        // update direction of player character
        GetComponent<SpriteRenderer>().flipX = (horizontalAxis < 0);
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
