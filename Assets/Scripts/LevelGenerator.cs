using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public Camera mainCam;

    public GameObject startSegment;
    public GameObject easySegments;
    public GameObject mediumSegments;
    public GameObject hardSegments;

    public int mediumStartingPoint = 200;
    public int hardStartingPoint = 400;
    public int bossPoint = 600;

    private List<GameObject> levelSegments = new List<GameObject>();
    private GameObject currentSegment;
    private GameObject lastSegment;

    private int segmentWidth = 40;
    private int nextSegmentPos = 0;

    private DifficultyLevel currentDifficulty = DifficultyLevel.Easy;

    // Start is called before the first frame update
    void Start()
    {
        currentSegment = Instantiate(startSegment, new Vector3(0, 0, 0), Quaternion.identity);
        foreach (Transform child in easySegments.transform)
        {
            levelSegments.Add(child.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (mainCam.transform.position.x > currentSegment.transform.position.x)
        {
            if (currentDifficulty < DifficultyLevel.Medium && mainCam.transform.position.x > mediumStartingPoint)
            {
                currentDifficulty = DifficultyLevel.Medium;
                foreach (Transform child in mediumSegments.transform)
                {
                    levelSegments.Add(child.gameObject);
                }
            }
            if (currentDifficulty < DifficultyLevel.Hard && mainCam.transform.position.x > hardStartingPoint)
            {
                currentDifficulty = DifficultyLevel.Hard;
                foreach (Transform child in hardSegments.transform)
                {
                    levelSegments.Add(child.gameObject);
                }
            }

            if (lastSegment)
                Destroy(lastSegment);
            lastSegment = currentSegment;
            nextSegmentPos += segmentWidth;

            currentSegment = Instantiate(levelSegments[Random.Range(0, levelSegments.Count)], new Vector3(nextSegmentPos, 0, 0), Quaternion.identity);
        }
    }
}

public enum DifficultyLevel
{
    Easy,
    Medium,
    Hard
}