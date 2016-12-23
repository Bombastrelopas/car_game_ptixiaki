using UnityEngine;
using System.Collections;

public class Exhaust : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Hello");
            gameObject.SetActive(!gameObject.activeSelf);
            
            
        }
	}
}
