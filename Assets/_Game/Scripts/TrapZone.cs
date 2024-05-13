using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapZone : MonoBehaviour
{
    [SerializeField] GameObject trapObj;
    [SerializeField] Vector3 offset;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player") {
            Instantiate(trapObj, transform.position + offset, Quaternion.identity);
        }
    }
}
