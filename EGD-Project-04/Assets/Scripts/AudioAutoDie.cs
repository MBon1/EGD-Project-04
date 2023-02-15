using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioAutoDie : MonoBehaviour
{
    float timer;
    bool die = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (die)
        {
            timer -= Time.deltaTime;
            if (timer <= 0f) Destroy(gameObject);
        }
    }

    public void SetClip(AudioClip c)
    {
        GetComponent<AudioSource>().clip = c;
        GetComponent<AudioSource>().Play();
        timer = GetComponent<AudioSource>().clip.length;
        die = true;
    }
}
