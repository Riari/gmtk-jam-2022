using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{  
    public AudioSource bgm;

    //public GameObject ActivePlayer;
    //public GameObject player1;
    //public GameObject player2;


    // Start is called before the first frame update
    void Start()
    {
      
    }

    void Awake()
    {
        Destroy(GameObject.FindWithTag("BackgroundMusic"));
        GameObject currentBGM = GameObject.FindGameObjectWithTag("DungeonMusic");
        if (currentBGM == null)
        {
            AudioSource spawned = Instantiate(bgm);
            spawned.Play();
            DontDestroyOnLoad(spawned);
        }

        GameObject[] objs = GameObject.FindGameObjectsWithTag("DungeonMusic");
        if (objs.Length > 1)
         Destroy(this.gameObject);
        
        
    }
}
