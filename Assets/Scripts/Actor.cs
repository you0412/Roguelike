using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Actor : MonoBehaviour
{
    public Rigidbody2D rb;
    public BoxCollider2D boxCollider;
    private float MoveTime = 0.1f;
    private float InverseMoveTime;
    public int power;
    public int money;
    public int hp;
    public Vector2 strPos;
    public Vector2 endPos;
    public Animator animator;


    void Start()
    {
    }

    // 移動を試みる
    protected abstract void AttemptMove(float x, float y);

    // Unity公式のやつ
    protected IEnumerator Movement(Vector3 EndPosition)
    {
        float sqrRemainingDistance = (transform.position - EndPosition).sqrMagnitude;
        InverseMoveTime = 1f / MoveTime;
        while (sqrRemainingDistance > float.Epsilon)
        {
            Vector3 NewPosition = Vector3.MoveTowards(rb.position, EndPosition, InverseMoveTime * Time.deltaTime);
            rb.MovePosition(NewPosition);
            sqrRemainingDistance = (transform.position - EndPosition).sqrMagnitude;
            yield return null;
        }

    }
    protected abstract void OnCantMove(GameObject hitComponent);

    public virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Money")
        {
            money += collision.GetComponent<Money>().amount;
            Destroy(collision.gameObject);
        }
    }
    public abstract void TakeDamage(int damage);
}
