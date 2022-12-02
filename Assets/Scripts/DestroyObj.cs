using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class DestroyObj : MonoBehaviour
{
    public float deleteTime = 3.0f;

    // Use this for initialization
    public GameManager gm;

    void Start()
    {
        gm=FindObjectOfType<GameManager>();
        Destroy(gameObject, deleteTime);

        if(Time.time > deleteTime)
        {
             gm.DownScore();
        }

        
    }
    

    // Update is called once per frame
void Update()
    {
    
    }
}