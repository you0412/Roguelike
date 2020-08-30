using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart : Item
{
    // Start is called before the first frame update
    void Start()
    {
        itemName = "Heart";
        itemPower = 1;
        isSale = false;
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject player = collision.gameObject;
        if (!isSale)
        {
            player.GetComponent<Player>().hp++;
            player.GetComponent<Player>().maxHp++;
            player.GetComponent<Player>().TakeItem();
            Destroy(gameObject);
        }
        else
        {

        }

    }
}
