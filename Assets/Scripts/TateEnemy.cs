using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TateEnemy : Enemy
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
        posX = 0;
        posY = 1;
        lifeGauge = GameObject.Find("LifeGauge");
        hp = 2;
        power = 1;
        money = 10;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

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
            posY *= -1;
        }
    }

}
