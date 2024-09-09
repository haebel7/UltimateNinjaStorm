using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public GameObject[] segments;
    private GameObject currentSegment;
    private GameObject lastSegment;
    public Camera mainCam;

    private int segmentWidth = 25;
    private int nextSegmentPos = 0;

    // Start is called before the first frame update
    void Start()
    {
        currentSegment = Instantiate(segments[Random.Range(0, segments.Length - 1)], new Vector3(0, 0, 0), Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        if (mainCam.transform.position.x > currentSegment.transform.position.x)
        {
            if (lastSegment)
                Destroy(lastSegment);
            lastSegment = currentSegment;
            nextSegmentPos += segmentWidth;
            currentSegment = Instantiate(segments[Random.Range(0, segments.Length - 1)], new Vector3(nextSegmentPos, 0, 0), Quaternion.identity);
        }
    }
}
