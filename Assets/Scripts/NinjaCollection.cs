using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NinjaCollection : MonoBehaviour
{
    [SerializeField] private GameObject ninjaClone;
    [SerializeField] private Sprite cloneSpriteSingle;
    [SerializeField] private Sprite cloneSpriteDouble;
    [SerializeField] private Sprite cloneSpriteQuad;
    [SerializeField] private float cloningForce = 5;

    private List<Transform> clones = new List<Transform>();

    // TODO: Replace with counter from scriptable object
    private int cloneCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        GameObject newClone = Instantiate(ninjaClone, transform.position, transform.rotation);
        clones.Add(newClone.transform);
        cloneCount++;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            DestroyClone(clones[Random.Range(0, clones.Count)]);
            print(cloneCount);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            List<Transform> newClones = new List<Transform>();
            foreach (Transform clone in clones)
            {
                if (cloneCount < 20
                    || (cloneCount < 120 && Random.value < 0.3)
                    || Random.value < 0.1
                    || clone.GetComponent<SpriteRenderer>().sprite == cloneSpriteQuad)
                {
                    GameObject newClone = Instantiate(ninjaClone, clone.position, transform.rotation);
                    newClones.Add(newClone.transform);
                    newClone.GetComponent<Rigidbody2D>().AddForce(new Vector2(-0.3f, 0.7f) * cloningForce, ForceMode2D.Impulse);
                    newClone.GetComponent<Rigidbody2D>().velocity = clone.GetComponent<Rigidbody2D>().velocity;
                    newClone.transform.position += new Vector3(0.5f,0,0);
                    newClone.GetComponent<NinjaMovement>().CheckIfGrounded();
                    newClone.GetComponent<NinjaMovement>().Jump();

                    var spriteRenderer = newClone.GetComponent<SpriteRenderer>();
                    spriteRenderer.sprite = clone.GetComponent<SpriteRenderer>().sprite;
                    if (spriteRenderer.sprite == cloneSpriteSingle)
                    {
                        cloneCount += 1;
                    }
                    else if (spriteRenderer.sprite == cloneSpriteDouble)
                    {
                        cloneCount += 2;
                    }
                    else
                    {
                        cloneCount += 4;
                    }
                }
                else
                {
                    var spriteRenderer = clone.GetComponent<SpriteRenderer>();
                    if (spriteRenderer.sprite == cloneSpriteSingle)
                    {
                        spriteRenderer.sprite = cloneSpriteDouble;
                        cloneCount += 1;
                    }
                    else
                    {
                        spriteRenderer.sprite = cloneSpriteQuad;
                        cloneCount += 2;
                    }
                }
            }
            clones.AddRange(newClones);

            print(cloneCount);
        }
    }

    public void DestroyClone(Transform clone)
    {
        var spriteRenderer = clone.GetComponent<SpriteRenderer>();
        if (spriteRenderer.sprite == cloneSpriteSingle)
        {
            cloneCount -= 1;
        }
        else if (spriteRenderer.sprite == cloneSpriteDouble)
        {
            cloneCount -= 2;
        }
        else
        {
            cloneCount -= 4;
        }

        clones.Remove(clone);
        Destroy(clone.gameObject);
    }
}
