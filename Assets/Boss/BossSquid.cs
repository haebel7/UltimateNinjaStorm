using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UI;

public enum BossSquidState
{
    Idle,
    Move,
    HomeTarget,
    ResetPosition,
    Attacking,
    Dead
}
public class BossSquid : MonoBehaviour
{
    private float health = 100;
    private NinjaCollection ninjaCollection;
    public BossSquidState currentState = BossSquidState.Move;
    public Transform target;

    private bool moovingUp = true;

    private Vector2 BasePosition;

    private Animator anim;


    public float randomInterval;

    public List<GameObject> inRange = new List<GameObject>();

    private Collider2D triggerZone;

    [SerializeField] private UnityEngine.UI.Slider healthBar;
    [SerializeField] private float speed = 1;
    [SerializeField] private float matchSpeed = 5;
    [SerializeField] private float chargeSpeed = 1;
    [SerializeField] private float force = 1;
    [SerializeField] private float closeDistance = 5;
    [SerializeField] private float farDistance = 15;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "NInja(Clone)" || other.gameObject.name == "NInja")
        {
            inRange.Add(other.gameObject);
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if ((other.gameObject.name == "NInja(Clone)" || other.gameObject.name == "NInja") && inRange.Contains(other.gameObject))
        {
            inRange.Remove(other.gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Bullet" && currentState != BossSquidState.Attacking)
        {
            health -= 10;
            healthBar.value = health;
            anim.SetTrigger("TakeDamage");
            if (health <= 0)
            {
                currentState = BossSquidState.Dead;
            }
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        triggerZone = GetComponentInChildren<Collider2D>();
        BasePosition = transform.position;
        randomInterval = Random.Range(3, 10);
        ninjaCollection = GameObject.FindObjectOfType<NinjaCollection>();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (target == null)
        {
            target = ninjaCollection.GetRandomNinja();
        }
        if (currentState != BossSquidState.HomeTarget)
        {
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(target.transform.position.x + farDistance, BasePosition.y), 0.01f * matchSpeed);
        }
        switch (currentState)
        {
            case BossSquidState.Idle:
                break;
            case BossSquidState.Move:
                if (moovingUp)
                {
                    transform.position = new Vector2(transform.position.x, transform.position.y + 0.01f * speed);
                    if (transform.position.y >= BasePosition.y + 1)
                    {
                        moovingUp = false;
                    }
                }
                else
                {
                    transform.position = new Vector2(transform.position.x, transform.position.y - 0.01f * speed);
                    if (transform.position.y <= BasePosition.y - 1)
                    {
                        moovingUp = true;
                    }
                }
                print(Time.time + " - " + randomInterval);
                if (Time.time > randomInterval)
                {
                    currentState = BossSquidState.HomeTarget;
                }
                break;
            case BossSquidState.HomeTarget:
                transform.position = Vector2.MoveTowards(transform.position, target.transform.position + new Vector3(0, 1, 0), 0.01f * chargeSpeed);
                print(Vector2.Distance(transform.position, target.transform.position));
                if (Vector2.Distance(transform.position, target.transform.position) <= closeDistance)
                {
                    currentState = BossSquidState.Attacking;
                    StartCoroutine(Attack());
                }
                break;
            case BossSquidState.ResetPosition:
                transform.position = Vector2.MoveTowards(transform.position, new Vector2(target.transform.position.x + farDistance, BasePosition.y), 0.01f * speed);
                if (Vector2.Distance(transform.position, target.transform.position) >= farDistance)
                {
                    currentState = BossSquidState.Move;
                }
                break;
            case BossSquidState.Attacking:
                break;
            case BossSquidState.Dead:
                break;

        }

    }
    public IEnumerator Attack()
    {
        anim.SetTrigger("Attack");
        foreach (var ninja in inRange)
        {
            ninja.GetComponent<Rigidbody2D>().AddForce((target.transform.position - transform.position).normalized * 2 * (Vector2.right * force), ForceMode2D.Impulse);
        }
        yield return new WaitForSeconds(0.5f);
        currentState = BossSquidState.Idle;
        yield return new WaitForSeconds(4f);
        target = ninjaCollection.GetRandomNinja();
        currentState = BossSquidState.ResetPosition;
        randomInterval = Time.time + Random.Range(3, 10);
    }

}
