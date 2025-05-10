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
    [SerializeField] private GameObject Lights;
    [SerializeField] private Material sky;
    [SerializeField] private Material days;
    [SerializeField] private Material nights;



    void Start()
    {
        song.Stop();
        StartCoroutine(CallFunctionEveryFiveSeconds());
        song.clip = night;
        song.Play();
    }


    void RotateLights()
    {
        float rotationSpeed = 360 / 120f; 
        float rotationAmount = rotationSpeed * Time.deltaTime;
        Lights.transform.Rotate(rotationAmount, 0, 0);
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
            StartCoroutine(FadeOutSong(am));
            AudioClip transitionVariant = am ? chicken : wolf;
            transition.clip = transitionVariant;
            transition.Play();
            string audioLog = am ? "quiquiriqui" : "auuuuuuu";
            Debug.Log(audioLog);
            Debug.Log("Playing: " + transitionVariant.name);
            //lerp the volume of Song down to 0 through 5 seconds
            
        }


        //sky.Lerp(days, nights, 5f);

    }


    IEnumerator FadeOutSong(bool am)
    {
        float initialVolume = song.volume;
        float totalTime = 3;
        float currentTime = 0;
        while (song.volume > 0.1f)
        {
            currentTime += Time.deltaTime;
            song.volume = Mathf.Lerp(1, 0, currentTime / totalTime);
            yield return null;
        }
        song.Stop();
        AudioClip songVariant = am ? day : night;
        song.clip = songVariant;
        song.Play();
        song.volume = 1;
    }

    // Update is called once per frame
    void Update()
    {
        RotateLights();
    }
}
