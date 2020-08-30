using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Enemy : Actor
{
    public bool ready;
    public int delay;
    public GameObject lifeGauge;
    // 移動後のポジションをいれる変数
    public Vector2 castPos;
    // 移動の距離
    public float posX;
    public float posY;
    public List<Vector2> castPosList;

    //public int Hp
    //{
    //    get
    //    {
    //        return hp;
    //    }

    //    set
    //    {
    //        hp = value;
    //        if (hp <= 0)
    //        {
    //            Vector3 pos = new Vector3(Mathf.Floor(transform.position.x), Mathf.Floor(transform.position.y), 0);
    //            GameObject prefab = (GameObject)Resources.Load("MoneyPrefab");
    //            GameObject mon = Instantiate(prefab, pos, Quaternion.identity);
    //            mon.GetComponent<Money>().SetMoney(money);
    //            Destroy(gameObject);
    //        }
    //    }
    //}

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }
    protected override void OnCantMove(GameObject hitComponent)
    {
        //if (hitComponent.gameObject.tag == "Player" && endPos != GameManager.instance.playerPos ||
        //    hitComponent.tag == "Enemy" && !hitComponent.GetComponent<Enemy>().castPosList.Contains(endPos) &&
        //    !hitComponent.GetComponent<Enemy>().castPosList.Contains(strPos) &&
        //    hitComponent.GetComponent<Enemy>().strPos != endPos)
        //{
        //    Debug.Log("特殊Move");
        //    StartCoroutine(Movement(endPos));
        //}
        //else if (hitComponent.tag == "Enemy" && hitComponent.GetComponent<Enemy>().boxCollider.offset != new Vector2(0, 0) ||
        //    hitComponent.tag == "Boss" && hitComponent.GetComponent<Enemy>().boxCollider.offset != new Vector2(0, 0))
        //{
        //    StartCoroutine(Movement(endPos));

        //}
        if (hitComponent.gameObject.tag == "Player")
        {
            AttackPlayer(hitComponent);
        }
        else if (hitComponent.gameObject.tag == "Money")
        {
            StartCoroutine(Movement(endPos));
        }
    }

    public virtual void AttackPlayer(GameObject hitComponent)
    {
        if (hitComponent.tag == "Player")
        {
            Player player = hitComponent.GetComponent<Player>();
            player.TakeDamage(power);
        }
    }

    protected override void AttemptMove(float x, float y)
    {
        endPos = strPos + new Vector2(x, y);
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        // 重複する要素がある場合、その要素を抜き出す
        var duplicates = GameManager.instance.enemyPos.GroupBy(pos => pos).Where(pos => pos.Count() > 1)
           .Select(group => group.Key).ToList();

        // 自分の移動後の位置にプレイヤーがいたら攻撃
        if (endPos == GameManager.instance.playerPos)
        {
            AttackPlayer(GameObject.Find("Player"));
        }
        // すべての敵の移動後の中で重複していたポジションが自分の移動後と同じだった場合
        else if (duplicates.Count != 0 && duplicates.Contains(endPos))
        {
            OnCantMove(GameObject.FindGameObjectWithTag("Wall"));
        }
        else
        {
            boxCollider.enabled = false;
            RaycastHit2D hitObj = Physics2D.Linecast(strPos, endPos);
            boxCollider.enabled = true;

            if (hitObj.transform == null)
            {
                StartCoroutine(Movement(endPos));
            }
            // 目の前に別の敵がいる
            else if (hitObj.transform.gameObject.tag == "Player" && endPos != GameManager.instance.playerPos ||
 hitObj.transform.gameObject.tag == "Enemy" && !hitObj.transform.gameObject.GetComponent<Enemy>().castPosList.Contains(endPos) &&
 !hitObj.transform.gameObject.GetComponent<Enemy>().castPosList.Contains(strPos) &&
 hitObj.transform.gameObject.GetComponent<Enemy>().strPos != endPos)
            {
                StartCoroutine(Movement(endPos));
            }
            else
            {
                OnCantMove(hitObj.transform.gameObject);
            }
        }
    }
    public abstract void MoveEnemy();
    public virtual void MoveCast()
    {
        castPosList = new List<Vector2>();
        boxCollider = GetComponent<BoxCollider2D>();
        strPos = transform.position;

        if (ready)
        {
            // 移動先に何もない（移動できる）かどうか
            castPos = strPos + new Vector2(posX, posY);

            boxCollider.enabled = false;
            RaycastHit2D hitObj = Physics2D.Linecast(strPos, castPos);
            boxCollider.enabled = true;
            if (hitObj.transform == null)
            {

            }
            // 移動できない場合は現在地をキャスト
            else if (hitObj.transform.gameObject.tag == "Wall" ||
                     hitObj.transform.gameObject.tag == "Item" ||
                     hitObj.transform.gameObject.tag == "Weapon")
            {
                castPos = strPos;
            }
            else if (hitObj.transform.gameObject.tag == "Enemy")
            {
                castPosList.Add(strPos);
                GameManager.instance.enemyPos.Add(strPos);
            }
            else if (hitObj.transform.gameObject.tag == "Player")
            {
                castPosList.Add(strPos);
                GameManager.instance.enemyPos.Add(strPos);
            }

        }
        else
        {
            castPos = strPos;
        }
        castPosList.Add(castPos);
        GameManager.instance.enemyPos.Add(castPos);
    }
    public override void TakeDamage(int damage)
    {
        hp -= damage;
        lifeGauge.GetComponent<LifeGauge>().SetEnemyLife(this.gameObject);
        if (hp <= 0)
        {
            power = 0;
            Vector3 pos = new Vector3(Mathf.Floor(transform.position.x), Mathf.Floor(transform.position.y), 0);
            GameObject prefab = (GameObject)Resources.Load("MoneyPrefab");
            GameObject mon = Instantiate(prefab, pos, Quaternion.identity);
            mon.GetComponent<Money>().SetMoney(money);
            Destroy(gameObject);
        }
    }
}
