using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Boss : Enemy
{
    public int count = 0;
    public GameObject player;
    public Vector2 castPosA;
    public Vector2 castPosB;
    public Vector2 castPosC;
    public Vector2 castPosD;
    public Vector2 endPosA;
    public Vector2 endPosB;
    public Vector2 castPosTempA;
    public Vector2 castPosTempB;

    public override void MoveEnemy()
    {
        if (posX != 0)
        {
            transform.localScale = new Vector3(posX * 2 * -1, 2, 1);
        }
        if (ready)
        {
            animator.SetTrigger("move");
            //Distance();

            if (GameManager.instance.playerPos == new Vector2(Mathf.Floor(strPos.x), Mathf.Ceil(strPos.y) + 1) ||
                GameManager.instance.playerPos == new Vector2(Mathf.Ceil(strPos.x), Mathf.Ceil(strPos.y) + 1) ||
                GameManager.instance.playerPos == new Vector2(Mathf.Ceil(strPos.x) + 1, Mathf.Ceil(strPos.y)) ||
                GameManager.instance.playerPos == new Vector2(Mathf.Ceil(strPos.x) + 1, Mathf.Floor(strPos.y)) ||
                GameManager.instance.playerPos == new Vector2(Mathf.Ceil(strPos.x), Mathf.Floor(strPos.y) - 1) ||
                GameManager.instance.playerPos == new Vector2(Mathf.Floor(strPos.x), Mathf.Floor(strPos.y) - 1) ||
                GameManager.instance.playerPos == new Vector2(Mathf.Floor(strPos.x) - 1, Mathf.Floor(strPos.y)) ||
                GameManager.instance.playerPos == new Vector2(Mathf.Floor(strPos.x) - 1, Mathf.Ceil(strPos.y)))
            {
                AttackPlayer(GameObject.Find("Player"));
                ready = false;
            }
            else
            {
                boxCollider.offset = new Vector2(0, 0);
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
    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
    }
    // Start is called before the first frame update
    void Start()
    {
        lifeGauge = GameObject.Find("LifeGauge");
        hp = 5;
        power = 3;
        money = 50;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

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
    public override void MoveCast()
    {
        castPosList = new List<Vector2>();
        (posX, posY) = Distance();
        strPos = transform.position;
        // 現在のボスの右上左上左下右下を求める
        // 右上
        castPosA = new Vector2(Mathf.Ceil(strPos.x), Mathf.Ceil(strPos.y));
        // 右下
        castPosB = new Vector2(Mathf.Ceil(strPos.x), Mathf.Floor(strPos.y));
        // 左上
        castPosC = new Vector2(Mathf.Floor(strPos.x), Mathf.Ceil(strPos.y));
        // 左下
        castPosD = new Vector2(Mathf.Floor(strPos.x), Mathf.Floor(strPos.y));
        castPosList.Add(castPosA);
        castPosList.Add(castPosB);
        castPosList.Add(castPosC);
        castPosList.Add(castPosD);
        GameManager.instance.enemyPos.Add(castPosA);
        GameManager.instance.enemyPos.Add(castPosB);
        GameManager.instance.enemyPos.Add(castPosC);
        GameManager.instance.enemyPos.Add(castPosD);
        if (ready)
        {
            if (posX == 1)
            {
                // 右上
                castPosTempA = new Vector2(Mathf.Ceil(strPos.x + 1), Mathf.Ceil(strPos.y));
                // 右下
                castPosTempB = new Vector2(Mathf.Ceil(strPos.x + 1), Mathf.Floor(strPos.y));
            }
            else if (posX == -1)
            {
                // 左上
                castPosTempA = new Vector2(Mathf.Floor(strPos.x - 1), Mathf.Ceil(strPos.y));
                // 左下
                castPosTempB = new Vector2(Mathf.Floor(strPos.x - 1), Mathf.Floor(strPos.y));
            }
            else if (posY == 1)
            {
                // 左上
                castPosTempA = new Vector2(Mathf.Floor(strPos.x), Mathf.Ceil(strPos.y + 1));
                // 右上
                castPosTempB = new Vector2(Mathf.Ceil(strPos.x), Mathf.Ceil(strPos.y + 1));
            }
            else if (posY == -1)
            {
                // 左下
                castPosTempA = new Vector2(Mathf.Floor(strPos.x), Mathf.Floor(strPos.y - 1));
                // 右下
                castPosTempB = new Vector2(Mathf.Ceil(strPos.x), Mathf.Floor(strPos.y - 1));
            }
            // 移動先に何もない（移動できる）かどうか
            boxCollider.enabled = false;
            RaycastHit2D[] hitObjs = Physics2D.LinecastAll(castPosTempA, castPosTempB);
            //RaycastHit2D[] hitObjsA = Physics2D.LinecastAll(castPosA, castPosB);
            //RaycastHit2D[] hitObjsB = Physics2D.LinecastAll(castPosC, castPosD);

            boxCollider.enabled = true;

            if (hitObjs.Length == 0 ||
                hitObjs.Length == 1 && hitObjs[0].transform.gameObject.tag == "Player")
            {
                castPosList.Add(castPosTempA);
                castPosList.Add(castPosTempB);
                GameManager.instance.enemyPos.Add(castPosTempA);
                GameManager.instance.enemyPos.Add(castPosTempB);
            }
            //else if (hitObjsB.Length > 0)
            //{
            //    for (int i = 0; i < hitObjsB.Length; i++)
            //    {
            //        if (hitObjsB[i].transform.gameObject.tag == "Wall" ||
            //            hitObjsB[i].transform.gameObject.tag == "Item")
            //        {
            //            // 右上
            //            castPosA = new Vector2(Mathf.Ceil(strPos.x), Mathf.Ceil(strPos.y));
            //            // 右下
            //            castPosB = new Vector2(Mathf.Ceil(strPos.x), Mathf.Floor(strPos.y));
            //            // 左上
            //            castPosC = new Vector2(Mathf.Floor(strPos.x), Mathf.Ceil(strPos.y));
            //            // 左下
            //            castPosD = new Vector2(Mathf.Floor(strPos.x), Mathf.Floor(strPos.y));
            //        }
            //    }
            //}
        }
    }

    protected override void AttemptMove(float x, float y)
    {
        // 方向に応じた2つのポジションを調べればよい
        if (x == 1)
        {
            // 右上
            endPosA = new Vector2(Mathf.Ceil(strPos.x + 1), Mathf.Ceil(strPos.y));
            // 右下
            endPosB = new Vector2(Mathf.Ceil(strPos.x + 1), Mathf.Floor(strPos.y));
        }
        else if (x == -1)
        {
            // 左上
            endPosA = new Vector2(Mathf.Floor(strPos.x - 1), Mathf.Ceil(strPos.y));
            // 左下
            endPosB = new Vector2(Mathf.Floor(strPos.x - 1), Mathf.Floor(strPos.y));
        }
        else if (y == 1)
        {
            // 左上
            endPosA = new Vector2(Mathf.Floor(strPos.x), Mathf.Ceil(strPos.y + 1));
            // 右上
            endPosB = new Vector2(Mathf.Ceil(strPos.x), Mathf.Ceil(strPos.y + 1));
        }
        else if (y == -1)
        {
            // 左下
            endPosA = new Vector2(Mathf.Floor(strPos.x), Mathf.Floor(strPos.y - 1));
            // 右下
            endPosB = new Vector2(Mathf.Ceil(strPos.x), Mathf.Floor(strPos.y - 1));
        }
        endPos = strPos + new Vector2(x, y);
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();

        // 重複する要素がある場合、その要素を抜き出す
        var duplicates = GameManager.instance.enemyPos.GroupBy(pos => pos).Where(pos => pos.Count() > 1)
           .Select(group => group.Key).ToList();
        // 自分の移動後の位置にプレイヤーがいたら攻撃
        if (endPosA == GameManager.instance.playerPos || endPosB == GameManager.instance.playerPos)
        {
            AttackPlayer(GameObject.Find("Player"));
        }
        // すべての敵の移動後の中で重複していたポジションが自分の移動後と同じだった場合
        else if (duplicates.Count != 0 && duplicates.Contains(endPosA) ||
            duplicates.Count != 0 && duplicates.Contains(endPosB))
        {
            OnCantMove(GameObject.FindGameObjectWithTag("Wall"));
        }
        // 目の前にプレイヤーがいなかった場合
        else
        {
            boxCollider.enabled = false;
            RaycastHit2D hitObj = Physics2D.Linecast(endPosA, endPosB);
            RaycastHit2D[] hitObjs = Physics2D.LinecastAll(endPosA, endPosB);
            boxCollider.enabled = true;
            // 目の前に何もない
            if (hitObj.transform == null)
            {
                StartCoroutine(Movement(endPos));
            }
            // 目の前にお金だけがある
            else if (hitObjs.Length == 1 && hitObjs[0].transform.gameObject.tag == "Money" ||
                hitObjs.Length == 2 && hitObjs[0].transform.gameObject.tag == "Money" &&
                hitObjs[1].transform.gameObject.tag == "Money")
            {
                StartCoroutine(Movement(endPos));
            }
            // 目の前に別の敵がいる
            // hitObjsにしないとだめ
            else if (hitObjs.Length < 2 && hitObj.transform.gameObject.tag == "Player" && endPosA != GameManager.instance.playerPos && endPosB != GameManager.instance.playerPos ||
                hitObj.transform.gameObject.tag == "Enemy" && !hitObj.transform.gameObject.GetComponent<Enemy>().castPosList.Contains(endPosA) &&
                !hitObj.transform.gameObject.GetComponent<Enemy>().castPosList.Contains(endPosB) &&
           hitObj.transform.gameObject.GetComponent<Enemy>().strPos != endPosA &&
           hitObj.transform.gameObject.GetComponent<Enemy>().strPos != endPosB)
            {
                StartCoroutine(Movement(endPos));
            }
            // それ以外
            else
            {
                OnCantMove(hitObj.transform.gameObject);
            }
        }
    }

}
