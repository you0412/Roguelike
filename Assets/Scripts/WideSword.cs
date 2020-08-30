using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WideSword : Item
{
    // Start is called before the first frame update
    void Start()
    {
        itemName = "WideSword";
        itemPower = 1;
        itemRange = new int[] { 3, 1 };
    }

    // Update is called once per frame
    void Update()
    {

    }
}
