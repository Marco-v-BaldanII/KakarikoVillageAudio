using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseButton : MonoBehaviour
{
    public GameObject pausePannel;
    public void Pause()
    {
        if(pausePannel.gameObject.activeSelf == true)
        {
            pausePannel.SetActive(false);
        }
        else
        {
            pausePannel.SetActive(true);
        }
    }
}
