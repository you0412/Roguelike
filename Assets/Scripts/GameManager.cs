using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum GameState
{
    // プレイヤーのターン開始
    KeyInput,
    // プレイヤーのターン
    PlayerTurn,
    // 敵のターン開始
    EnemyBegin,
    // 敵のターン
    EnemyTurn,
    // ターン終了、全員終了したら最初に戻る
    TurnEnd
}

// ゲームの進行を管理する
public class GameManager : MonoBehaviour
{
    // GameManager.instance.○○○でアクセスできるようになる
    // 本当はカプセル化して読み取り専用にしたほうがいいらしい
    public static GameManager instance;
    // 現在のゲームの状態を格納する変数
    public GameState currentGameState;
    public Vector2 playerPos;
    public GameObject player;
    public GameObject[] enemyObj;
    public List<Vector2> enemyPos;
    public int playerHp;
    public int playerMaxHp;
    public int playerMoney;
    public GameObject playerEquip;
    public int score;
    void Awake()
    {

        // Awakeはインスタンス化された時に呼ばれる
        // 初期状態はKeyInput
        SetCurrentState(GameState.KeyInput);
        // シングルトン
        // インスタンスが一つしか作られないようにしてるみたい
        if (instance == null)
        {
            PlayerReset();
            // インスタンスがなかったらここの内容でインスタンス作成
            instance = this;
            // シーン切り替え時に破棄されないように
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // すでにあったら破棄
            Destroy(gameObject);
        }
        MoveCast();
    }


    // 内部、または外部からゲームステータスを変更する
    public void SetCurrentState(GameState state)
    {
        // 引数に状態を変更
        currentGameState = state;
        OnGameStateChanged(currentGameState);
    }

    // 状態遷移
    private void OnGameStateChanged(GameState state)
    {
        switch (state)
        {
            case GameState.KeyInput:
                break;
            case GameState.PlayerTurn:
                PlayerTurn();
                break;
            case GameState.EnemyBegin:
                SetCurrentState(GameState.EnemyTurn);
                break;
            case GameState.EnemyTurn:
                EnemyTurn();
                break;
            case GameState.TurnEnd:
                MoveCast();
                SetCurrentState(GameState.KeyInput);
                break;
            default:
                break;
        }
    }

    void MoveCast()
    {

        // GameObjectの配列にしてみる
        enemyPos = new List<Vector2>();
        enemyObj = GameObject.FindGameObjectsWithTag("Enemy");
        for (int i = 0; i < enemyObj.Length; i++)
        {
            enemyObj[i].GetComponent<Enemy>().MoveCast();
        }

    }

    void PlayerTurn()
    {
        SetCurrentState(GameState.EnemyBegin);
    }
    void EnemyTurn()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<Player>().canMove = true;
        GameObject[] enemyObj = GameObject.FindGameObjectsWithTag("Enemy");
        //EnemyObjの数だけEnemyにアタッチしている移動処理を実行
        for (int i = 0; i < enemyObj.Length; i++)
        {
            enemyObj[i].GetComponent<Enemy>().MoveEnemy();
        }
        SetCurrentState(GameState.TurnEnd);
    }
    public void SetPlayer(int maxHp, int hp, int money, GameObject equip)
    {
        playerMaxHp = maxHp;
        playerHp = hp;
        playerMoney = money;
        playerEquip = transform.Find(equip.name).gameObject;
        GameObject moneyObj = GameObject.Find("Money");
        moneyObj.GetComponent<Text>().text = money.ToString();
    }
    // GameOverの時
    public void SetPlayer(int maxHp, int hp, int money)
    {
        playerMaxHp = maxHp;
        playerHp = hp;
        playerMoney = money;
        GameObject moneyObj = GameObject.Find("Money");
        moneyObj.GetComponent<Text>().text = money.ToString();
    }
    public void PlayerReset()
    {
        playerMaxHp = 3;
        playerHp = 3;
        playerMoney = 0;
        Vector3 pos = new Vector3(Mathf.Floor(transform.position.x), Mathf.Floor(transform.position.y), 0);
        GameObject prefab = (GameObject)Resources.Load("Knife");
        GameObject knife = Instantiate(prefab, pos, Quaternion.identity);
        knife.transform.parent = transform;
        playerEquip = knife;
        GameObject scoreObj = GameObject.Find("Score");
        scoreObj.GetComponent<Text>().text = score.ToString();
    }
    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_STANDALONE
      UnityEngine.Application.Quit();
#endif
    }
    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            Quit();
        }

    }

}
