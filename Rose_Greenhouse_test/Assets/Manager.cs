using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{   
    public Text ClicksTotalText;
    public Text RoselevelText;
    public Text RoseUpgradeCostText;
    public float TotalClicks;
    
    public float minimumClicksToUnlockRoseUpgrade;
    public float level = 1;
    
    
    public Button button;
    public GameObject lampOBJ;
    public GameObject ClockOBJ;
    public GameObject Fower_PotOBJ;
    public Sprite Roselv20;
    public Sprite Roselv60;
    public Sprite Roselv180;
    public Sprite Roselv540;
    public GameObject AutoClickbutton;
    public GameObject skillActive1;
    public GameObject skillActive1Unlocked;
    public GameObject skillActive2;
    public GameObject skillActive2Unlocked;

    public float minimumClicksToUnlock;
    public float minimumClicksToUpgrade;
    
    private bool hasUpgradeAutoClick;
    public float autoClicksPerSecond;
    
    public float lampUnlock;
    private bool lamp;
    public float ClockUnlock;
    private bool Clock;

    public void AddClicks()
    {
        TotalClicks += level;
        ClicksTotalText.text = TotalClicks.ToString("0");
    }
    
     public void RoseUpgrade()
    {
        if(TotalClicks >= minimumClicksToUnlockRoseUpgrade)
        {
            TotalClicks -= minimumClicksToUnlockRoseUpgrade;
            level += 1;
            RoseUpgradeCostText.text = minimumClicksToUnlockRoseUpgrade.ToString("0");
            RoselevelText.text = level.ToString("0");
            ClicksTotalText.text = TotalClicks.ToString("0");
            
    
        if(level == 20)
        {
            minimumClicksToUnlockRoseUpgrade += 100;
            button.image.sprite = Roselv20;
        }
        if(level == 60)
        {
            minimumClicksToUnlockRoseUpgrade += 1000;
            button.image.sprite = Roselv60;
        }
        if(level == 180)
        {
            minimumClicksToUnlockRoseUpgrade += 10000;
            button.image.sprite = Roselv180;
        }   
        if(level == 540)
        {
            minimumClicksToUnlockRoseUpgrade += 100000;
            button.image.sprite = Roselv540;
        }
        }
    }
    
    public void AutoClickUnlock()
    {
        if(!hasUpgradeAutoClick && TotalClicks >= minimumClicksToUnlock)
        {
            TotalClicks -= minimumClicksToUnlock;
            hasUpgradeAutoClick = true;
        }   
    }

    public void AutoClickUpgrade()
    {
        if(hasUpgradeAutoClick && TotalClicks >= minimumClicksToUpgrade)
        {
            TotalClicks -= minimumClicksToUpgrade;
            autoClicksPerSecond += 1;
            ClicksTotalText.text = TotalClicks.ToString("0");
        }
    }

    public void lampToUnlock()
    {
        if(!lamp && TotalClicks >= lampUnlock)
        {
            TotalClicks -= lampUnlock;
            lamp = true;
        }
    }

    public void ClockToUnlock()
    {
        if(!Clock && TotalClicks >= ClockUnlock)
        {
            TotalClicks -= ClockUnlock;
            Clock = true;
            
        }
    }

    private void Update()
    {
        if(hasUpgradeAutoClick)
        {
            Fower_PotOBJ.SetActive(true);
            AutoClickbutton.SetActive(true);
            TotalClicks += autoClicksPerSecond * Time.deltaTime;
            ClicksTotalText.text = TotalClicks.ToString("0");
        } 

        if(lamp)
        {   
            lampOBJ.SetActive(true);
            skillActive1.SetActive(true);
            skillActive1Unlocked.SetActive(true);
            ClicksTotalText.text = TotalClicks.ToString("0");
        } 
        
        if(Clock)
        {   
            ClockOBJ.SetActive(true);
            skillActive2.SetActive(true);
            skillActive2Unlocked.SetActive(true);
            ClicksTotalText.text = TotalClicks.ToString("0");
        } 
    }
}
