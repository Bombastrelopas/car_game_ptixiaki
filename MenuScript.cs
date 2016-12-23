using UnityEngine;
using System.Collections;

public class MenuScript : MonoBehaviour {

    public GameObject LoadingScreen;
    public GameObject cam;
    public GameObject mainmenu;
    public GameObject helpMenu;
    public GameObject multiMenu;
    public float fraction;
    public Transform mainMenuTransform;
    public Transform helpMenuTransform;
    public Transform multiplayerMenuTransform;

	
    public void Start()
    {
        mainMenuTransform = GameObject.Find("MainMenuPosition").transform;
        helpMenuTransform = GameObject.Find("HelpMenuPosition").transform;
        multiplayerMenuTransform = GameObject.Find("MultiplayerMenuPosition").transform;
        cam = GameObject.Find("Main Camera");

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

    public void GoToMulti()
    {
        StartCoroutine(MultiMenu());
    }


    IEnumerator MultiMenu()
    {
        mainmenu.SetActive(false);
        fraction = 0;
        while (fraction < 1)
        {
            cam.transform.position = Vector3.Slerp(cam.transform.position, multiplayerMenuTransform.position, fraction * Time.deltaTime);
            cam.transform.rotation = Quaternion.Slerp(cam.transform.rotation, multiplayerMenuTransform.rotation, fraction * Time.deltaTime);
            yield return null;
     
        }
        multiMenu.SetActive(true);
 

    }
    //Close mainmenu and move camera to help menu
    IEnumerator HelpMenu()
    {
        mainmenu.SetActive(false);
        fraction = 0;
        while (fraction < 1)
        {
            cam.transform.position = Vector3.Slerp(cam.transform.position, helpMenuTransform.position, fraction * Time.deltaTime);
            cam.transform.rotation = Quaternion.Slerp(cam.transform.rotation, helpMenuTransform.rotation, fraction * Time.deltaTime);
            yield return null;
     
        }
        helpMenu.SetActive(true);
   }
    public void GoBackToMaain()
    {
        StartCoroutine(ReturnToMain());
    }

    public void GoBackToMainFromMulti()
    {
        StartCoroutine(ReturnToMainFromMulti());
    }

    IEnumerator ReturnToMainFromMulti()
    {
        multiMenu.SetActive(false);
        fraction = 0;
        while (fraction < 1)
        {
            helpMenu.SetActive(false);
            cam.transform.position = Vector3.Slerp(cam.transform.position, mainMenuTransform.position, fraction * Time.deltaTime);
            cam.transform.rotation = Quaternion.Slerp(cam.transform.rotation, mainMenuTransform.rotation, fraction * Time.deltaTime);
            yield return null;
        }
        mainmenu.SetActive(true);
    }
	
    IEnumerator ReturnToMain()
    {
        helpMenu.SetActive(false);
        fraction = 0;
        while (fraction < 1)
        {
            helpMenu.SetActive(false);
            cam.transform.position = Vector3.Slerp(cam.transform.position, mainMenuTransform.position, fraction * Time.deltaTime);
            cam.transform.rotation = Quaternion.Slerp(cam.transform.rotation, mainMenuTransform.rotation, fraction * Time.deltaTime);
            yield return null;
        }
        mainmenu.SetActive(true);
    }
	
	
}
