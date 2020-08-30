using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class LifeGauge : MonoBehaviour
{
    public GameObject[] life;
    public GameObject[] maxLife;
    public GameObject player;
    public GameObject canvas;
    public GameObject enemy;
    public GameObject[] enemyChildren;
    int count;
    // Start is called before the first frame update
    void Awake()
    {
        life = GameObject.FindGameObjectsWithTag("PlayerLife");
        foreach (var item in life)
        {
            item.gameObject.GetComponent<Image>().enabled = false;
        }
        maxLife = GameObject.FindGameObjectsWithTag("MaxLife");
        foreach (var item in maxLife)
        {
            item.gameObject.GetComponent<Image>().enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
    public void SetLife(int hp, int maxHp)
    {
        //Life用のCanvasをセット
        canvas = GameObject.FindGameObjectWithTag("Canvas");
        life = GameObject.FindGameObjectsWithTag("PlayerLife");
        maxLife = GameObject.FindGameObjectsWithTag("MaxLife");
        foreach (var item in maxLife)
        {
            item.gameObject.GetComponent<Image>().enabled = false;
        }
        foreach (var item in life)
        {
            item.gameObject.GetComponent<Image>().enabled = false;
        }
        for (int i = 0; i < maxHp; i++)
        {
            canvas.transform.GetChild(i).gameObject.GetComponent<Image>().enabled = true;
            //maxLife[i].gameObject.GetComponent<Image>().enabled = true;
        }
        for (int i = 0; i < hp; i++)
        {
            canvas.transform.GetChild(i + 10).gameObject.GetComponent<Image>().enabled = true;
            //life[i].gameObject.GetComponent<Image>().enabled = true;
        }
    }
    public void SetEnemyLife(GameObject enemy)
    {
        GameObject child = enemy.transform.Find("Canvas/LifeCanvas").gameObject;

        int count = child.transform.childCount;
        enemyChildren = new GameObject[count];
        for (int i = 0; i < count; i++)
        {
            enemyChildren[i] = child.transform.GetChild(i).gameObject;
        }
        foreach (var item in enemyChildren)
        {
            item.gameObject.GetComponent<Image>().enabled = false;
        }
        for (int i = 0; i < enemy.GetComponent<Enemy>().hp; i++)
        {
            enemyChildren[i].gameObject.GetComponent<Image>().enabled = true;
        }
    }
}
