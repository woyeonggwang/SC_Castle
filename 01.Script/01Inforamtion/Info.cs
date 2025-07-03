using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class Info : MonoBehaviour
{
    public VideoPlayer video;
    public VideoClip begin;
    public VideoClip loop;
    public GameObject[] info;
    public GameObject text;
    public AudioSource[] infoNar;

    private int index;
    private int countNar;
    private int countVideo;
    private int countThird;
    private bool ready;
    private int countLoadScene;

    private void Start()
    {
        countNar = 2;
        video.GetComponent<RawImage>().color = new Color(1, 1, 1, 0);
        video.Stop();
        index = 0;
        ready = false;
        countVideo = 0;
        StartCoroutine(DelayTouch());
    }

    private void Update()
    {
        countNar++;
        if (countNar == 1)
        {
            for (int i = 0; i < infoNar.Length; i++)
            {
                if (i == index-1)
                {
                    //infoNar[i].Play();
                }
                else
                {
                    //infoNar[i].Stop();
                }
            }
        }
        if (ready)
        {

            if (Input.GetMouseButtonDown(0))
            {
                ProjectManager.instance.touchSound.Play();
                if (index < 2)
                {

                    index++;
                    countNar = 0;
                }
                else
                {
                    countLoadScene++;
                    if (countLoadScene == 1)
                    {
                        StartCoroutine(SceneLoadDelay());
                    }
                    
                    index = 2;
                }
            }
        }
        
        if (index == 1)
        {
            for (int i = 0; i < infoNar.Length; i++)
            {
                if (i == index)
                {
                    //infoNar[i].Play();
                }
                else
                {
                    //infoNar[i].Stop();
                }
            }
            countVideo++;
            if (countVideo == 1)
            {
                StartCoroutine(VideoPlay());
            }
        }
        if (index == 2)
        {
            info[2].SetActive(true);
            info[1].SetActive(false);
        }
        //for(int i=0; i < info.Length; i++)
        //{
        //    if (i == index)
        //    {
        //        info[i].SetActive(true);
        //    }
        //    else
        //    {
        //        info[i].SetActive(false);
        //    }
        //}
    }

    IEnumerator DelayTouch()
    {
        yield return new WaitForSeconds(1.5f);
        ready = true;
        yield return new WaitForSeconds(1f);
        infoNar[0].Play();
    }


    IEnumerator SceneLoadDelay()
    {
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(2);
    }
    IEnumerator VideoPlay()
    {
        text.SetActive(false);
        info[1].SetActive(true);
        video.clip = begin;
        video.Play();
        yield return new WaitForSeconds(0.2f);
        video.GetComponent<RawImage>().color = new Color(1, 1, 1, 1);
        yield return new WaitForSeconds(0.05f);
        info[0].SetActive(false);
        yield return new WaitForSeconds((float)begin.length);
        video.clip = loop;
        video.Play();
        video.isLooping = true;
    }
}
