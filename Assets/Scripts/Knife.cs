using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knife : Item
{

    // Start is called before the first frame update
    void Awake()
    {
        itemName = "Knife";
        itemPower = 1;
        itemRange = new int[] { 1, 1 };
    }

    // Update is called once per frame
    void Update()
    {

    }
}
