using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEditor;
using UnityEngine.UI;

public class gameManager : MonoBehaviour
{
    public static gameManager Instance;
    public float enemyType;
    /*
    1 = deputy enemy 
    2 = ranger enemy 
    3 = cactus enemy 
    4 = banker boss
    5 = sheriff boss
    */


    [SerializeField] GameObject Character; //this is used to check the varibale damage is true
    [SerializeField] PlayerScript playerScript;

    [SerializeField] GameObject sceneControllerObject;
    [SerializeField] SceneController sceneController;

    //bleedStuff
    [SerializeField] GameObject BleedPanel;
    private float bleedTimer;
    public int bleedDamage = 2;


    //health stuff
    public int HealthCurrent;
    public int HealthMax = 100;
    [SerializeField] PlayerHealth healthBarScript;
    [SerializeField] GameObject healthBarObject;



    //objective text and stuff
    [SerializeField] TMP_Text objectiveText;
    public int objectiveNumber;
    [SerializeField] GameObject objectWithPro;
    private string[] objectiveNames = { "Beat Cactus Tutorial (optional) or continue to saloon", "Find and rob the bank!", "Find a way out of the town",
    "You now hate the train go kill it", "Escape with horses"};




    //Weapon variables 
    public int Ammo;
    public int gunType; // public variable made to inform other scripts that the player has this gun equipped
    /*
     0 = revolver 
     1 = rifle
     2 = shotgun
     */


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        else
        {
            Destroy(gameObject);
        }

    }
    // Start is called before the first frame update
    void Start()
    {
        gunType = 0; //character starts with revolver
        Ammo = 6;
        healthBarObject = GameObject.FindWithTag("healthBar");
        healthBarScript = healthBarObject.GetComponent<PlayerHealth>();
        HealthCurrent = HealthMax;
        healthBarScript.SetMaxHealth(HealthMax);
    }

    // Update is called once per frame
    void Update()
    {
        //this is used to initialize certain variables when the player is in
        if (SceneManager.GetActiveScene().name == "Train Station" || SceneManager.GetActiveScene().name
    == "Bank Interrior" || SceneManager.GetActiveScene().name == "Saloon" || SceneManager.GetActiveScene().name == "SampleScene" || 
    SceneManager.GetActiveScene().name == "Barn-stable") //checking if the scene is playabl
        {
            if (playerScript)
            {
                
            }
            else
            {
                Character = GameObject.FindWithTag("Player");
                playerScript = Character.GetComponent<PlayerScript>();
                healthBarObject = GameObject.FindWithTag("healthBar");
                healthBarScript = healthBarObject.GetComponent<PlayerHealth>();
                sceneControllerObject = GameObject.FindWithTag("sceneManager");
                sceneController = sceneControllerObject.GetComponent<SceneController>();
                BleedPanel = GameObject.Find("Canvas/bleedPanel");
            }
            
            if (playerScript != null)
            {
                Bleed();


            }
            else
            {
                Debug.Log("Cannot locate the player script");
            }

            if (HealthCurrent <= 0)
            {
                Death();
            }
            else
            {
                healthBarScript.SetHealth(HealthCurrent); //updating the healthBar
            }

        }
        else
        {
            Debug.Log("There is no player in this scene so I'm not loading"); //cannot find the proper script that identifies the player
        }

        
        /*
        if (objectiveNumber == 0)
        {
            objectiveText.text = objectiveNames[0];
        }
        */

    }
    /*
    public void Damage() //damage function with a parameter for varying damages
    {
        if (playerScript.damageType == 1)
        {
            HealthCurrent = HealthCurrent - regularShot;
            playerScript.damageType = 0;
        }
        if (playerScript.damageType == 2)
        {
            HealthCurrent = 0;
            playerScript.damageType = 0;
        }
        if (playerScript.damageType == 3)
        {
            HealthCurrent = HealthCurrent - miniShot;
            playerScript.damageType = 0;
        }

    }
    */

    public void Damage(int damageAmount)
    {
        HealthCurrent = HealthCurrent - damageAmount;
    }

    public void Bleed()
    {
        if (playerScript.bleed == true)
        {
            BleedPanel.SetActive(true);
            bleedTimer += Time.deltaTime;
            if(bleedTimer >= 5)
            {
                HealthCurrent = HealthCurrent - bleedDamage;
                bleedTimer = 0;
            }
        }
        else
        {
            bleedTimer = 0;
            BleedPanel.SetActive(false);
        }
            
    }

    public void Death()
    {
        sceneController.lose();
        //reset for next possible play through
        HealthCurrent = HealthMax;
        gunType = 0;
        healthBarScript.SetMaxHealth(HealthMax);
    }
}
