using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class TimeManager : MonoBehaviour
{
    // Start is called before the first frame update
    private int time = 0;
    [SerializeField] private AudioSource song;
    [SerializeField] private AudioSource transition;
    [SerializeField] private AudioClip day;
    [SerializeField] private AudioClip night;
    [SerializeField] private AudioClip chicken;
    [SerializeField] private AudioClip wolf;
    void Start()
    {
        StartCoroutine(CallFunctionEveryFiveSeconds());

    }

    IEnumerator CallFunctionEveryFiveSeconds()
    {
        while (true)
        {
            yield return new WaitForSeconds(5f);
            PassHour();
        }
    }

    void PassHour()
    {
        time++;
        if (time > 23)
        {
            time = 0;
        }
        int timeIn12HourFormat = time % 12;
        if (timeIn12HourFormat == 0)
        {
            timeIn12HourFormat = 12;
        }
        bool am = time < 12 ? true : false;
        string amPm = am ? "AM" : "PM";
        Debug.Log("Time: " + timeIn12HourFormat + " " + amPm);
        if (timeIn12HourFormat == 6)
        {
            //AudioClip songVariant = am ? day : night;
            //song.clip = songVariant;
            AudioClip transitionVariant = am ? chicken : wolf;
            transition.clip = transitionVariant;
            transition.Play();
            string audioLog = am ? "quiquiriqui" : "auuuuuuu";
            Debug.Log(audioLog);
            Debug.Log("Playing: " + transitionVariant.name);
        }


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
