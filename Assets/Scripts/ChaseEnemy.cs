using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ChaseEnemy : Enemy
{
    public GameObject player;

    public override void MoveEnemy()
    {
        if (posX != 0)
        {
            transform.localScale = new Vector3(posX * -1, 1, 1);
        }
        if (ready)
        {
            animator.SetTrigger("move");
            if (GameManager.instance.playerPos == new Vector2(strPos.x + 1, strPos.y) ||
                GameManager.instance.playerPos == new Vector2(strPos.x - 1, strPos.y) ||
                GameManager.instance.playerPos == new Vector2(strPos.x, strPos.y + 1) ||
                GameManager.instance.playerPos == new Vector2(strPos.x, strPos.y - 1)
                )
            {
                //boxCollider.offset = new Vector2(0, 0);
                AttackPlayer(GameObject.Find("Player"));
                ready = false;
            }
            else
            {
                //boxCollider.offset = new Vector2(0, 0);
                AttemptMove(posX, posY);
                ready = false;
            }
        }
        else
        {
            animator.SetTrigger("ready");
            ready = true;
        }
    }

    public (float x, float y) Distance()
    {
        player = GameObject.Find("Player");
        Vector2 playerPos = GameManager.instance.playerPos;
        Vector2 enemyPos = transform.position;

        // プレイヤーへの距離を求める
        float dx = playerPos.x - enemyPos.x;
        float dy = playerPos.y - enemyPos.y;

        if (Mathf.Abs(dx) > Mathf.Abs(dy))
        {
            // X方向への距離の方が遠いのでそっちに進む
            if (dx < 0)
            {
                // 左
                return (-1, 0);
            }
            else
            {
                // 右
                return (1, 0);
            }
        }
        else
        {
            // Y方向へ進む
            if (dy < 0)
            {
                // 下
                return (0, -1);
            }

            else
            {
                // 上
                return (0, 1);
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        lifeGauge = GameObject.Find("LifeGauge");
        hp = 3;
        power = 1;
        boxCollider = GetComponent<BoxCollider2D>();
        money = 30;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public override void MoveCast()
    {
        castPosList = new List<Vector2>();
        (posX, posY) = Distance();
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
                     hitObj.transform.gameObject.tag == "Item")
            {
                castPos = strPos;
            }
            // 移動先に別の敵がいた場合の処理
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
}
