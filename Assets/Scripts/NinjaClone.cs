using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NinjaClone : MonoBehaviour
{
    public int value = 1;

    private NinjaCollection ninjaCollection;

    private void Start()
    {
        ninjaCollection = FindObjectOfType<NinjaCollection>();
    }

    private void Update()
    {
        if (transform.position.y < -10)
        {
            ninjaCollection.DestroyClone(transform);
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Spikes"))
        {
            ninjaCollection.DestroyClone(transform);
        }
    }
}
