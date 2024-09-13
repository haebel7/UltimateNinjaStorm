using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public float scrollSpeed;

    private GameManager gm;

    // Start is called before the first frame update
    void Start()
    {
        gm = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        // TODO: Remove this when player can actually die
        if (Input.GetKeyDown(KeyCode.R))
            gm.TriggerGameOver();

        if (transform.position.x > 10)
            gm.StartGame();
    }
}
