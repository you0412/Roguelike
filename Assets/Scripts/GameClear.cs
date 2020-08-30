using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameClear : MonoBehaviour
{
    int score;
    // Start is called before the first frame update
    void Start()
    {
        score = GameManager.instance.playerMoney + (GameManager.instance.playerHp * 100);
        GameObject scoreObj = GameObject.Find("Score");
        scoreObj.GetComponent<Text>().text = "Score: " + score.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            GameManager.instance.score += score;
            GameManager.instance.PlayerReset();
            SceneManager.LoadScene("Lobby");
        }
    }
}
