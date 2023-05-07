using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{   
    public GameObject StartManu;
    public GameObject MainManu;
    public GameObject ShopManu;

    public void OnClickBottonToStart()
    {
        StartManu.SetActive(false);
        MainManu.SetActive(true);
    }

    public void OnClickBottonToOpenShop()
    {
        MainManu.SetActive(false);
        ShopManu.SetActive(true);
    }

    public void OnClickBottonToExitShop()
    {
        ShopManu.SetActive(false);
        MainManu.SetActive(true);
    }
}
