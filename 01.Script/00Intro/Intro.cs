using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Intro : MonoBehaviour
{
    public ParticleSystem glow;
    public Button btnTouchSound;
    private bool readyToStart;
    private void Start()
    {
        //glow.Stop();
        StartCoroutine(StartDelay());
        btnTouchSound.onClick.AddListener(() => BtnTouchSound());
    }
    private void Update()
    {
        if (readyToStart)
        {

            if (Input.GetMouseButtonDown(0))
            {
                StartCoroutine(SceneLoadDelay());
            }
        }
    }

    private void BtnTouchSound()
    {
        ProjectManager.instance.touchSound.Play();
    }

    IEnumerator StartDelay()
    {
        yield return new WaitForSeconds(4f);
        readyToStart = true;
        //glow.Play();
    }
    IEnumerator SceneLoadDelay()
    {
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(1);
    }
}
