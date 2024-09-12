using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NinjaCollection : MonoBehaviour
{
    [SerializeField] private GameObject ninjaClone;
    [SerializeField] private Sprite cloneSpriteSingle;
    [SerializeField] private Sprite cloneSpriteDouble;
    [SerializeField] private Sprite cloneSpriteQuad;
    [SerializeField] private NinjaCounter ninjaCounter;

    private List<Transform> clones = new List<Transform>();

    void OnEnable()
    {
        ninjaCounter.ninjaCount = 0;
    }

    // Start is called before the first frame update
    void Start()
    {
        GameObject newClone = Instantiate(ninjaClone, transform.position, transform.rotation);
        newClone.transform.parent = transform;
        clones.Add(newClone.transform);
        ninjaCounter.ninjaCount++;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            DestroyClone(clones[Random.Range(0, clones.Count)]);
            print(ninjaCounter.ninjaCount);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (ninjaCounter.ninjaCount > 900)
            {
                ninjaCounter.ninjaCount = ninjaCounter.ninjaCount * 2;
                // TODO: Add code that doubles the value of each clone (accessed in their future ninjaClone script)
                print(ninjaCounter.ninjaCount);
                return;
            }

            List<Transform> newClones = new List<Transform>();
            foreach (Transform clone in clones)
            {
                if (ninjaCounter.ninjaCount < 20
                    || (ninjaCounter.ninjaCount < 120 && Random.value < 0.3)
                    || Random.value < 0.1
                    || clone.GetComponent<SpriteRenderer>().sprite == cloneSpriteQuad)
                {
                    GameObject newClone = Instantiate(ninjaClone, clone.position, transform.rotation);
                    newClone.transform.parent = transform;
                    newClones.Add(newClone.transform);
                    newClone.GetComponent<Rigidbody2D>().velocity = clone.GetComponent<Rigidbody2D>().velocity;
                    newClone.transform.position += new Vector3(0.5f, 0, 0);
                    newClone.GetComponent<NinjaMovement>().CheckIfGrounded();
                    newClone.GetComponent<NinjaMovement>().Jump();

                    var spriteRenderer = newClone.GetComponent<SpriteRenderer>();
                    spriteRenderer.sprite = clone.GetComponent<SpriteRenderer>().sprite;
                    if (spriteRenderer.sprite == cloneSpriteSingle)
                    {
                        ninjaCounter.ninjaCount += 1;
                    }
                    else if (spriteRenderer.sprite == cloneSpriteDouble)
                    {
                        ninjaCounter.ninjaCount += 2;
                    }
                    else
                    {
                        ninjaCounter.ninjaCount += 4;
                    }
                }
                else
                {
                    var spriteRenderer = clone.GetComponent<SpriteRenderer>();
                    if (spriteRenderer.sprite == cloneSpriteSingle)
                    {
                        spriteRenderer.sprite = cloneSpriteDouble;
                        ninjaCounter.ninjaCount += 1;
                    }
                    else
                    {
                        spriteRenderer.sprite = cloneSpriteQuad;
                        ninjaCounter.ninjaCount += 2;
                    }
                }
            }
            clones.AddRange(newClones);

            print(ninjaCounter.ninjaCount);
        }
    }

    public void DestroyClone(Transform clone)
    {
        var spriteRenderer = clone.GetComponent<SpriteRenderer>();
        if (spriteRenderer.sprite == cloneSpriteSingle)
        {
            ninjaCounter.ninjaCount -= 1;
        }
        else if (spriteRenderer.sprite == cloneSpriteDouble)
        {
            ninjaCounter.ninjaCount -= 2;
        }
        else
        {
            ninjaCounter.ninjaCount -= 4;
        }

        clones.Remove(clone);
        Destroy(clone.gameObject);
    }
}
