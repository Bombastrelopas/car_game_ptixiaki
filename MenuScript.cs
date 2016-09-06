using UnityEngine;
using System.Collections;

public class MenuScript : MonoBehaviour {

    public GameObject LoadingScreen;
    public GameObject cam;
    public GameObject mainmenu;
    public GameObject helpMenu;
    public float fraction;
    public Transform mainMenuTransform;
    public Transform helpMenuTransform;

	
    public void Start()
    {
        mainmenu = gameObject.gameObject;
        mainMenuTransform = GameObject.Find("MainMenuPosition").transform;
        helpMenuTransform = GameObject.Find("HelpMenuPosition").transform;

    }

    public void LevelSelect(int level)
    {
        LoadingScreen.SetActive(true);
        Application.LoadLevel(level);
        
    }

    public void CloseGame()
    {
        Application.Quit();
    }

    public void Update()
    {
        if (fraction <= 1)
        {
            fraction += Time.deltaTime * 0.2f;
        }

    }

    public void GoToHelp()
    {
        StartCoroutine(HelpMenu());

    }
    //Close mainmenu and move camera to help menu
    IEnumerator HelpMenu()
    {
        helpMenu.SetActive(true);
        cam = GameObject.Find("Main Camera");
        fraction = 0;
        while (fraction < 1)
        {
            cam.transform.position = Vector3.Slerp(cam.transform.position, helpMenuTransform.position, fraction * Time.deltaTime);
            cam.transform.rotation = Quaternion.Slerp(cam.transform.rotation, helpMenuTransform.rotation, fraction * Time.deltaTime);
            yield return null;
     
        }
   }
    public void GoBackToMaain()
    {
        StartCoroutine(ReturnToMain());
    }

    IEnumerator ReturnToMain()
    {
        fraction = 0;
        while (fraction < 1)
        {
            helpMenu.SetActive(false);
            cam.transform.position = Vector3.Slerp(cam.transform.position, mainMenuTransform.position, fraction * Time.deltaTime);
            cam.transform.rotation = Quaternion.Slerp(cam.transform.rotation, mainMenuTransform.rotation, fraction * Time.deltaTime);
            yield return null;
        }
    }
	
	
}
