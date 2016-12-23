using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class StartingLineMultiplayer2 : MonoBehaviour {

    //Variables
    NetworkCar carscript;
    public bool passedAllCheckpoints = false;

    //Gui Variables
    public Text lapText;
    public Text bestTimeText;

    public GameObject worseLap;
    public Text worseLapText;
    public Image worseLapImage;
    public GameObject betterLap;
    public Text betterLapText;
    public Image betterLapImage;

    public Text FinalLapText; 

    //Wait functions for GUI which disappears later

    IEnumerator WaitFiveFinal()
    {
        yield return new WaitForSeconds(5f);
        FinalLapText.enabled = false;
    }
    IEnumerator WaitFiveRed()
    {
        yield return new WaitForSeconds(5f);
        worseLapImage.enabled = false;
        worseLapText.enabled = false;
    }

    IEnumerator WaitFiveGreen()
    {
        yield return new WaitForSeconds(5f);
        betterLapImage.enabled = false;
        betterLapText.enabled = false;
 
    }

    //Initiate variables

	void Start () {
        //Find UI elements at start
        lapText = GameObject.Find("LapText").GetComponent<Text>();
        bestTimeText = GameObject.Find("BestTimeText").GetComponent<Text>();
        worseLap = GameObject.Find("WorseLap");

        worseLapImage = worseLap.GetComponent<Image>();
        worseLapText = worseLap.GetComponentInChildren<Text>();
        betterLap = GameObject.Find("BetterLap");
        betterLapImage = betterLap.GetComponent<Image>();
        betterLapText = betterLap.GetComponentInChildren<Text>();

        FinalLapText = GameObject.Find("FinalLapText").GetComponent<Text>();

        //Set better and worse laps as invisible
        worseLapImage.enabled = false;
        worseLapText.enabled = false;
        betterLapImage.enabled = false;
        betterLapText.enabled = false;

        FinalLapText.enabled = false;

         

	}
	

	
    
    void  OnTriggerEnter(Collider col)
    {
        if(col.tag == "Player")
        {
            passedAllCheckpoints = true;
            carscript =  col.GetComponent<NetworkCar>();
            Debug.Log(carscript);
            //Check if the car has passed from all checkpoints
            Debug.Log(carscript.currentLap);
            for(int i = 0; i<=6; i++)
            {
                if(carscript.fairRace[i] != true)
                {
                    passedAllCheckpoints = false;
                }
            }
            if (passedAllCheckpoints == true)
            {
                //Finish race
                if (carscript.currentLap == 3 && !carscript.raceFinished)
                {
                    carscript.speed = 0;
                    carscript.racing = false;
                    carscript.raceFinished = true;
                    carscript.raceWon = true;
                }

                //Add a lap if the car has passed from all checkpoints
                carscript.currentLap = carscript.currentLap + 1;
                lapText.text = "Lap: " + carscript.currentLap;
                //Change the besttime GUI text if the car did a better lap
                if (carscript.lapTime < carscript.bestTime || carscript.bestTime == 0)
                {
                    //If it's not the start of the race show green arrow and better time
                    if (carscript.bestTime != 0)
                    {
                        betterLapImage.enabled = true;
                        betterLapText.text = (carscript.lapTime - carscript.bestTime).ToString("F3");
                        betterLapText.enabled = true;
                        StartCoroutine(WaitFiveGreen());
                    }


                    //Set the best time as this laptime.
                    carscript.bestTime = carscript.lapTime;
                    bestTimeText.text =    carscript.bestTime.ToString("F3");
                    
                }
                else if(carscript.lapTime >= carscript.bestTime && carscript.bestTime != 0)
                {
                    //if it's not the start of the race show red arrow and worse time
                    worseLapImage.enabled = true;
                    worseLapText.text = (carscript.lapTime - carscript.bestTime).ToString("F3");
                    worseLapText.enabled = true;
                    StartCoroutine(WaitFiveRed());

                }
                //Show final lap if it's race mode

                //Debug.Log(SceneManager.GetActiveScene().name);
                if (carscript.currentLap == 3 && SceneManager.GetActiveScene().name == "Physics_Test_scene")
                {
                    Debug.Log("Physics");
                    FinalLapText.enabled = true;
                    StartCoroutine(WaitFiveFinal());
                }

                carscript.lapTime = 0;
            }

            passedAllCheckpoints = false;
            //Reset array
            for(int i = 0; i<=6; i++)
            {
                carscript.fairRace[i] = false;
            }
        }
    }

    
}
