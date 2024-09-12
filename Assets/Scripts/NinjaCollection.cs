using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NinjaCollection : MonoBehaviour
{
    public GameObject ninjaClone;

    private List<Transform> clones = new List<Transform>();

    // Start is called before the first frame update
    void Start()
    {
        GameObject newClone = Instantiate(ninjaClone, transform.position, transform.rotation);
        clones.Add(newClone.transform);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            Instantiate(ninjaClone, transform.position, transform.rotation);
    }
}
