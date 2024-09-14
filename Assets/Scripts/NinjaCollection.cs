using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class NinjaCollection : MonoBehaviour
{
    [SerializeField] private GameObject ninjaClone;
    [SerializeField] private Sprite cloneSpriteSingle;
    [SerializeField] private Sprite cloneSpriteDouble;
    [SerializeField] private Sprite cloneSpriteQuad;
    [SerializeField] private NinjaCounter ninjaCounter;

    [SerializeField] private CameraScript cam;

    private List<Transform> clones = new List<Transform>();
    private GameManager gm;

    public Transform GetRandomNinja()
    {
        return clones[Random.Range(0, clones.Count)];
    }

    void OnEnable()
    {
        ninjaCounter.ninjaCount = 0;
    }

    // Start is called before the first frame update
    void Start()
    {
        gm = FindObjectOfType<GameManager>();
        GameObject newClone = Instantiate(ninjaClone, transform.position, transform.rotation);
        newClone.transform.parent = transform;
        clones.Add(newClone.transform);
        ninjaCounter.ninjaCount++;
        SetCamTarget();
    }

    // Update is called once per frame
    void Update()
    {
        if (gm.GetIsGameOver())
            return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (ninjaCounter.ninjaCount > 1000000000)
            {
                print(ninjaCounter.ninjaCount);
                return;
            }

            if (/*ninjaCounter.ninjaCount > 900*/clones.Count > 100)
            {
                foreach (Transform clone in clones)
                {
                    if (!clone.GetComponent<NinjaMovement>().CheckIfGrounded())
                        continue;

                    ninjaCounter.ninjaCount += clone.GetComponent<NinjaClone>().value;
                    clone.GetComponent<NinjaClone>().value *= 2;
                }
                print(ninjaCounter.ninjaCount);
                //print("Clone count: " + clones.Count);
                return;
            }

            List<Transform> newClones = new List<Transform>();
            foreach (Transform clone in clones)
            {
                if (!clone.GetComponent<NinjaMovement>().CheckIfGrounded())
                    continue;

                if (ninjaCounter.ninjaCount < 20
                    || (ninjaCounter.ninjaCount < 120 && Random.value < 0.3)
                    || Random.value < 0.1
                    || clone.GetComponent<NinjaClone>().value >= 4)
                {
                    newClones.Add(CloneNinja(clone));
                }
                else
                {
                    if (clone.GetComponent<NinjaClone>().value == 1)
                    {
                        //clone.GetComponent<SpriteRenderer>().sprite = cloneSpriteDouble;
                        clone.GetComponent<Animator>().Play("Animation_Idle_Basic");
                        clone.GetComponent<NinjaClone>().value += 1;
                        ninjaCounter.ninjaCount += 1;
                    }
                    else
                    {
                        //clone.GetComponent<SpriteRenderer>().sprite = cloneSpriteQuad;
                        clone.GetComponent<Animator>().Play("Animation_Idle_Basic");
                        clone.GetComponent<NinjaClone>().value += 2;
                        ninjaCounter.ninjaCount += 2;
                    }
                }
            }
            clones.AddRange(newClones);

            SetCamTarget();

            print(ninjaCounter.ninjaCount);
        }
    }

    private Transform CloneNinja(Transform clone)
    {
        GameObject newClone = Instantiate(ninjaClone, clone.position, transform.rotation);
        newClone.transform.parent = transform;
        newClone.GetComponent<Rigidbody2D>().velocity = clone.GetComponent<Rigidbody2D>().velocity;
        newClone.transform.position += new Vector3(0.5f, 0, 0);
        newClone.GetComponent<NinjaMovement>().CheckIfGrounded();
        newClone.GetComponent<NinjaMovement>().Jump();

        //newClone.GetComponent<SpriteRenderer>().sprite = clone.GetComponent<SpriteRenderer>().sprite;
        newClone.GetComponent<Animator>().Play("Animation_Idle_Basic");
        int newCloneValue = newClone.GetComponent<NinjaClone>().value = clone.GetComponent<NinjaClone>().value;
        ninjaCounter.ninjaCount += newCloneValue;

        return newClone.transform;
    }

    public void DestroyClone(Transform clone)
    {
        ninjaCounter.ninjaCount -= clone.GetComponent<NinjaClone>().value;

        clones.Remove(clone);
        Destroy(clone.gameObject);

        SetCamTarget();

        if (clones.Count <= 0)
        {
            gm.TriggerGameOver();
        }
    }

    private void SetCamTarget()
    {
        if (clones.Count <= 0)
            return;

        clones.Sort((a, b) => a.position.x.CompareTo(b.position.x));
        cam.camTarget = clones[clones.Count / 2];
    }
}
