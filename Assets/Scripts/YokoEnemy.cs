using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class YokoEnemy : Enemy
{
    public override void MoveEnemy()
    {
        if (ready)
        {
            animator.SetTrigger("move");
            AttemptMove(posX, posY);
            ready = false;
        }
        else
        {
            ready = true;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        posX = 1;
        posY = 0;
        lifeGauge = GameObject.Find("LifeGauge");
        hp = 2;
        power = 1;
        money = 10;
        animator = GetComponent<Animator>();
    }

    protected override void OnCantMove(GameObject hitComponent)
    {
        //if (hitComponent.tag == "Player" && endPos != GameManager.instance.playerPos ||
        //    hitComponent.tag == "Enemy" && !hitComponent.GetComponent<Enemy>().castPosList.Contains(endPos) &&
        //    !hitComponent.GetComponent<Enemy>().castPosList.Contains(strPos) &&
        //    hitComponent.GetComponent<Enemy>().strPos != endPos)
        //{
        //    Debug.Log("特殊Move");
        //    StartCoroutine(Movement(endPos));
        //}
        if (hitComponent.tag == "Money")
        {
            StartCoroutine(Movement(endPos));
        }
        else
        {
            posX *= -1;
            transform.localScale = new Vector3(posX, 1, 1);
        }
    }
}
