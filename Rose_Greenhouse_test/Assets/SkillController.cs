using UnityEngine;
using UnityEngine.UI;
using System.Collections;
 
public class SkillController : MonoBehaviour {
    [SerializeField] Manager manager;
    private float TotalClicks;
    public float skillDuration1 = 1f; 
    public float skillCooldown1 = 60f; 
    public float skillDuration2 = 10f;   
    public float skillCooldown2 = 180f; 
    public Button skillButton1;
    public Button skillButton2; 
    public Button UpgradeButtonNotActive;
    private float currentCooldown1 = 0f;
    private float currentCooldown2 = 0f; 
    private bool skillActive1 = false;
    private bool skillActive2 = false; 
    public GameObject PenroseParticle;
 
    void Start() {
        skillButton1.onClick.AddListener(ActivateSkill1);
        skillButton2.onClick.AddListener(ActivateSkill2);
    }
 
    void Update() {
        
        if (skillActive1) {
            skillDuration1 -= Time.deltaTime;
            
            if (skillDuration1 <= 0) {
                skillActive1 = false;
                skillDuration1 = 1f;
                currentCooldown1 = skillCooldown1;
                skillButton1.interactable = false;
                
            
            }
        }
        else if (currentCooldown1 > 0) {
            currentCooldown1 -= Time.deltaTime;
            if (currentCooldown1 <= 0) {
                
                currentCooldown1 = 0;
                skillButton1.interactable = true;

            }
        }


        if (skillActive2) {
            skillDuration2 -= Time.deltaTime;
            
            if (skillDuration2 <= 0) {
                skillActive2 = false;
                skillDuration2 = 10f;
                currentCooldown2 = skillCooldown2;
                skillButton2.interactable = false;
                UpgradeButtonNotActive.interactable = true;
                manager.level -= 50;
                manager.RoselevelText.text = manager.level.ToString("0");
                
            }
        }
        else if (currentCooldown2 > 0) {
            currentCooldown2 -= Time.deltaTime;
            if (currentCooldown2 <= 0) {
                currentCooldown2 = 0;
                skillButton2.interactable = true;
                PenroseParticle.SetActive(false);

            }
        }
    }
 
    void ActivateSkill1() {
        if (!skillActive1 && currentCooldown1 <= 0) {
            skillActive1 = true;
            skillButton1.interactable = false;

            manager.TotalClicks += manager.TotalClicks*1;
            manager.ClicksTotalText.text = manager.TotalClicks.ToString("0");
            
        }
    }

    void ActivateSkill2() {
        if (!skillActive2 && currentCooldown2 <= 0) {
            skillActive2 = true;
            skillButton2.interactable = false;

            manager.level += 50 ;
            manager.RoselevelText.text = manager.level.ToString("0");
            UpgradeButtonNotActive.interactable = false;
            PenroseParticle.SetActive(true);
            

        }
    }
}


