using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongSword : Item
{

    // Start is called before the first frame update
    void Start()
    {
        itemName = "LongSword";
        itemPower = 1;
        itemRange = new int[] { 1, 2 };
    }

    // Update is called once per frame
    void Update()
    {

    }
}
