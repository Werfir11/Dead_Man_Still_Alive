using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinManager : MonoBehaviour
{
    public int coinCount;
    public Text coinText;
    public GameObject door;
    public int maxCoins;


    void Update()
    {
        coinText.text = coinCount.ToString();

        if(coinCount == maxCoins)
        {
            door.SetActive(false);
        }
    }
}