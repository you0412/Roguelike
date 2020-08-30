using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Money : MonoBehaviour
{
    public int amount;
    // Start is called before the first frame update

    internal void SetMoney(int money)
    {
        amount = money;
    }
}
