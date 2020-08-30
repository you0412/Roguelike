using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : Actor
{
    public GameObject gameManager;
    public GameObject lifeGauge;
    public GameObject equip;
    public GameObject drop;
    public GameObject hitObjs;
    // Start is called before the first frame update
    public int[] range;
    public string equipName;
    public int maxHp;
    public bool canMove;

    //public int Hp
    //{
    //    get
    //    {
    //        return hp;
    //    }

    //    set
    //    {
    //        hp = value;
    //    }
    //}

    void Start()
    {
        maxHp = GameManager.instance.playerMaxHp;
        hp = GameManager.instance.playerHp;
        money = GameManager.instance.playerMoney;
        Pick(GameManager.instance.playerEquip);
        lifeGauge.GetComponent<LifeGauge>().SetLife(hp, maxHp);
        //range = new int[] { 1, 1 };
        //power = 1;
        //equip = transform.Find("Knife").gameObject;
        //range = equip.GetComponent<Item>().itemRange;
        //power = equip.GetComponent<Item>().itemPower;
        animator = GetComponent<Animator>();
        gameManager = GameManager.instance.gameObject;
        GameManager.instance.player = gameObject;
        GameObject moneyObj = GameObject.Find("Money");
        moneyObj.GetComponent<Text>().text = money.ToString();
        canMove = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.currentGameState == GameState.KeyInput)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                AttemptMove(-1, 0);
                canMove = false;

            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                AttemptMove(1, 0);
                canMove = false;

            }
            else if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                AttemptMove(0, 1);
                canMove = false;

            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                AttemptMove(0, -1);
                canMove = false;
            }
            canMove = true;
        }
    }

    protected override void AttemptMove(float x, float y)
    {
        if (x != 0)
        {
            transform.localScale = new Vector3(x, 1, 1);
        }
        strPos = transform.position;
        endPos = strPos + new Vector2(x, y);
        if (gameManager.GetComponent<GameManager>().currentGameState == GameState.KeyInput)
        {
            rb = GetComponent<Rigidbody2D>();
            boxCollider = GetComponent<BoxCollider2D>();

            // 自身のColliderを読んでしまうので一時的に無効化
            boxCollider.enabled = false;
            RaycastHit2D hitObj = Physics2D.Linecast(strPos, endPos);
            // 縦に伸びるパターン
            if (range[0] > 1)
            {
                Vector2 widePos1;
                Vector2 widePos2;
                if (x != 0)
                {
                    widePos1 = strPos + new Vector2(x, 1);
                    widePos2 = strPos + new Vector2(x, -1);
                }
                else
                {
                    widePos1 = strPos + new Vector2(1, y);
                    widePos2 = strPos + new Vector2(-1, y);
                }
                RaycastHit2D hitObj1 = Physics2D.Linecast(widePos1, widePos1);
                RaycastHit2D hitObj2 = Physics2D.Linecast(widePos2, widePos2);

                boxCollider.enabled = true;
                // 斜め前に敵がいる場合
                if (hitObj1.transform != null && hitObj1.transform.gameObject.tag == "Enemy" ||
                    hitObj1.transform != null && hitObj1.transform.gameObject.tag == "Boss" ||
                    hitObj2.transform != null && hitObj2.transform.gameObject.tag == "Enemy" ||
                    hitObj2.transform != null && hitObj2.transform.gameObject.tag == "Boss")
                {
                    if (hitObj1.transform != null && hitObj1.transform.gameObject.tag == "Enemy" ||
                        hitObj1.transform != null && hitObj1.transform.gameObject.tag == "Boss")
                    {
                        AttackEnemy(hitObj1.transform.gameObject);
                    }
                    if (hitObj2.transform != null && hitObj2.transform.gameObject.tag == "Enemy" ||
                        hitObj2.transform != null && hitObj2.transform.gameObject.tag == "Boss")
                    {
                        AttackEnemy(hitObj2.transform.gameObject);
                    }
                    if (hitObj.transform != null && hitObj.transform.gameObject.tag == "Enemy")
                    {
                        AttackEnemy(hitObj.transform.gameObject);
                    }
                }
                else
                {
                    if (hitObj.transform == null)
                    {
                        // 移動できた時に現在地を保存
                        GameManager.instance.playerPos = endPos;
                        StartCoroutine(Movement(endPos));
                        if (drop != null)
                        {
                            // 移動した後にアイテムドロップ
                            Drop(drop);
                        }
                    }
                    else
                    {
                        OnCantMove(hitObj.transform.gameObject);
                    }
                }
                gameManager.GetComponent<GameManager>().SetCurrentState(GameState.PlayerTurn);
            }
            // 横に伸びるパターン
            else if (range[1] > 1)
            {
                Vector2 rangePos = strPos + new Vector2(x * range[1], y * range[1]);
                // 攻撃範囲内の当たり判定を調べる
                RaycastHit2D[] hit2Ds = Physics2D.LinecastAll(strPos, rangePos);

                boxCollider.enabled = true;
                // 2マス想定
                // 2マス以内に何かがある場合
                if (hit2Ds.Length > 1)
                {
                    // 2マス目が敵じゃない場合はいつも通りの処理
                    if (hit2Ds[1].transform.gameObject.tag != "Enemy" && hit2Ds[1].transform.gameObject.tag != "Boss")
                    {
                        if (hitObj.transform == null)
                        {
                            // 移動できた時に現在地を保存
                            GameManager.instance.playerPos = endPos;
                            StartCoroutine(Movement(endPos));
                            if (drop != null)
                            {
                                // 移動した後にアイテムドロップ
                                Drop(drop);
                            }
                        }
                        else
                        {
                            OnCantMove(hitObj.transform.gameObject);
                        }
                    }
                    else
                    {
                        // どれかが敵なら攻撃してターン終了
                        foreach (var item in hit2Ds)
                        {
                            if (item.transform.gameObject.tag == "Enemy" || item.transform.gameObject.tag == "Boss")
                            {
                                AttackEnemy(item.transform.gameObject);
                            }
                        }
                    }
                    gameManager.GetComponent<GameManager>().SetCurrentState(GameState.PlayerTurn);
                }
                else
                {
                    // 2マス目のみに敵がいた場合
                    if (hit2Ds.Length != 0 && hit2Ds[0].transform.gameObject.tag == "Enemy" || hit2Ds.Length != 0 && hit2Ds[0].transform.gameObject.tag == "Boss")
                    {
                        AttackEnemy(hit2Ds[0].transform.gameObject);
                    }
                    // 近くに敵がいないということなのでそれ以外の処理
                    else
                    {
                        if (hitObj.transform == null)
                        {
                            // 移動できた時に現在地を保存
                            GameManager.instance.playerPos = endPos;
                            StartCoroutine(Movement(endPos));
                            if (drop != null)
                            {
                                // 移動した後にアイテムドロップ
                                Drop(drop);
                            }
                        }
                        else if (hitObj.transform.gameObject.tag != "Enemy")
                        {
                            OnCantMove(hitObj.transform.gameObject);
                        }
                    }
                    gameManager.GetComponent<GameManager>().SetCurrentState(GameState.PlayerTurn);

                }
            }
            else
            {
                boxCollider.enabled = true;

                if (hitObj.transform == null)
                {
                    GameManager.instance.playerPos = endPos;
                    StartCoroutine(Movement(endPos));
                    if (drop != null)
                    {
                        // 移動した後にアイテムドロップ
                        Drop(drop);
                    }
                }
                else
                {
                    OnCantMove(hitObj.transform.gameObject);
                }
                gameManager.GetComponent<GameManager>().SetCurrentState(GameState.PlayerTurn);
            }
        }
    }
    protected override void OnCantMove(GameObject hitComponent)
    {
        if (hitComponent.tag == "Enemy")
        {
            if (hitComponent.GetComponent<Enemy>().ready)
            {
                if (!hitComponent.GetComponent<Enemy>().castPosList.Contains(endPos) && !hitComponent.GetComponent<Enemy>().castPosList.Contains(strPos))
                {
                    GameManager.instance.playerPos = endPos;
                    StartCoroutine(Movement(endPos));
                }
                else
                {
                    GameManager.instance.playerPos = strPos;
                    AttackEnemy(hitComponent);
                }
            }
            else
            {
                GameManager.instance.playerPos = strPos;
                AttackEnemy(hitComponent);
            }
        }
        else if (hitComponent.tag == "Food")
        {
            GameManager.instance.playerPos = endPos;
            StartCoroutine(Movement(hitComponent.transform.position));
            if (drop != null)
            {
                // 移動した後にアイテムドロップ
                Drop(drop);
            }
        }
        else if (hitComponent.tag == "Money")
        {
            GameManager.instance.playerPos = endPos;
            StartCoroutine(Movement(hitComponent.transform.position));
            if (drop != null)
            {
                // 移動した後にアイテムドロップ
                Drop(drop);
            }
        }
        else if (hitComponent.tag == "Heart")
        {
            GameManager.instance.playerPos = endPos;
            StartCoroutine(Movement(endPos));
            if (drop != null)
            {
                // 移動した後にアイテムドロップ
                Drop(drop);
            }
        }

        else if (hitComponent.tag == "Weapon")
        {
            GameManager.instance.playerPos = endPos;
            StartCoroutine(Movement(hitComponent.transform.position));
            if (drop != null)
            {
                // 移動した後にアイテムドロップ
                Drop(drop);
            }
            // すでに装備してる武器を落とす用の変数に格納
            drop = equip;
            Pick(hitComponent);
        }
        else if (hitComponent.tag == "Exit")
        {
            lifeGauge.GetComponent<LifeGauge>().SetLife(hp, maxHp);
            equip.transform.parent = GameManager.instance.transform;
            GameManager.instance.SetPlayer(maxHp, hp, money, equip);
            GameManager.instance.playerPos = endPos;
            StartCoroutine(Movement(endPos));
        }
    }

    private void AttackEnemy(GameObject hitComponent)
    {
        animator.SetTrigger("attack");
        endPos = strPos;
        Enemy enemy = hitComponent.GetComponent<Enemy>();
        enemy.TakeDamage(power);
        //if (enemy.Hp == 0)
        //{
        //    Debug.Log("敵破壊");
        //    //Destroy(hitComponent);
        //    // デスが間に合わないので攻撃力を０に
        //    hitComponent.GetComponent<Enemy>().power = 0;
        //}
    }

    public override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Food")
        {
            if (hp + 2 > maxHp)
            {
                hp = maxHp;
            }
            else
            {
                hp += 2;
            }
            lifeGauge.GetComponent<LifeGauge>().SetLife(hp, maxHp);
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.tag == "Heart")
        {
            //maxHp++;
            //hp++;
            //lifeGauge.GetComponent<LifeGauge>().SetLife(hp, maxHp);
            //Destroy(collision.gameObject);
        }
        if (collision.gameObject.tag == "Money")
        {
            money += collision.GetComponent<Money>().amount;
            Destroy(collision.gameObject);
            GameObject moneyObj = GameObject.Find("Money");
            moneyObj.GetComponent<Text>().text = money.ToString();
        }
    }
    void Pick(GameObject weapon)
    {
        weapon.transform.position = transform.position;
        // 装備
        equip = weapon;
        // プレイヤーの子にする
        weapon.transform.parent = transform;
        weapon.gameObject.SetActive(false);
        power = weapon.GetComponent<Item>().itemPower;
        range = weapon.GetComponent<Item>().itemRange;
    }
    void Drop(GameObject weapon)
    {
        // ドロップしたのでnull
        drop = null;
        // 移動後に落とすのでもともといた場所を指定
        weapon.transform.position = strPos;
        weapon.transform.parent = null;
        weapon.gameObject.SetActive(true);
    }

    public override void TakeDamage(int damage)
    {
        hp -= damage;
        lifeGauge.GetComponent<LifeGauge>().SetLife(hp, maxHp);
        if (hp <= 0)
        {
            hp = 0;
            GameManager.instance.SetPlayer(maxHp, hp, money);
            SceneManager.LoadScene("GameOver");
        }
    }
    public void TakeItem()
    {
        lifeGauge.GetComponent<LifeGauge>().SetLife(hp, maxHp);
    }
}
