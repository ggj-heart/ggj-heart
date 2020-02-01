using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grounded : MonoBehaviour
{
    private GameObject player;
    private Vector3 offsetFromPlayer;

    // Start is called before the first frame update
    void Start()
    {
        player = gameObject.transform.parent.gameObject;
        offsetFromPlayer = transform.position - player.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = player.transform.position + offsetFromPlayer;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Ground")
        {
            player.GetComponent<Move2D>().isGrounded = true;
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.tag == "Ground")
        {
            player.GetComponent<Move2D>().isGrounded = false;
        }
    }
}
