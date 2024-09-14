using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    [HideInInspector] public Transform camTarget;
    public Transform backWall;

    private GameManager gm;

    // Start is called before the first frame update
    void Start()
    {
        gm = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
            gm.TriggerGameWon();

        if (transform.position.x > 10)
            gm.StartGame();

    }

    private void FixedUpdate()
    {
        if (camTarget)
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(camTarget.position.x + 4, 0, -10), 0.1f);

            if (transform.position.x - 15 > backWall.position.x)
                backWall.position = new Vector3(transform.position.x - 15, 0, 0);
        }
    }
}
