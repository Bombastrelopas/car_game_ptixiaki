using UnityEngine;
using System.Collections;

public class MusicBox : MonoBehaviour {
    //Make array of all the songs
    public AudioSource[] songs;
 
    public bool nowPlaying = false;

	// Use this for initialization
	void Start () {
        songs = GameObject.Find("Music").GetComponentsInChildren<AudioSource>();
	}
	
    void Update()
    {
        //If previous song stopped start playing a next one in random
        if (!nowPlaying)
        {
            nowPlaying = true;
            StartCoroutine(playMusic(Random.Range(1,10 )));
        }
    }

    //Play the song
    IEnumerator playMusic(int musicTrack)
    {
        songs[musicTrack].Play();
        yield return new WaitForSeconds(songs[musicTrack].clip.length);
        nowPlaying = false;
    }
}
