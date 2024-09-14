using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Canon : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private bool isRotating;
    [SerializeField] private float speed = 20;
    [SerializeField] private float force = 20;
    [SerializeField] private float targetAngle = 40;

    private Transform shootingPos;
    // Start is called before the first frame update
    void Start()
    {
        shootingPos = transform.GetChild(0);

    }

    // Update is called once per frame
    void Update()
    {
        if (isRotating)
        {
            // rotate up and down
            float angle = Mathf.PingPong(Time.time * speed, targetAngle);
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "NInja(Clone)" || other.gameObject.name == "NInja")
        {
            Destroy(other.gameObject);
            GameObject bullet = Instantiate(bulletPrefab, shootingPos.position, Quaternion.identity);
            bullet.GetComponent<Rigidbody2D>().velocity = transform.right * force;
        }
    }
}
