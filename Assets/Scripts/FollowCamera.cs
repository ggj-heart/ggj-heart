using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// based on https://learn.unity.com/tutorial/movement-basics?projectId=5c514956edbc2a002069467c#5c7f8528edbc2a002053b711
public class FollowCamera : MonoBehaviour
{
    public GameObject player;
    private Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        offset = transform.position - player.transform.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (!player.GetComponent<Move2D>().isAtEnd)
        {
            transform.position = new Vector3(player.transform.position.x + offset.x, transform.position.y, transform.position.z);
        }
    }
}
