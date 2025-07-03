
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ProjectManager : MonoBehaviour
{
    public static ProjectManager instance;
    public ParticleSystem particle;
    public AudioSource touchSound;
    public AudioSource xTouchSound;
    public AudioSource oTouchSound;
    
    public AudioSource main;
    private float mainVol;

    public float distance = 5f;

    private float delayTime;


    private void Awake()
    {
        SettingsImport();
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }


    public void Update()
    {
        delayTime += Time.deltaTime;
        int index = SceneManager.GetActiveScene().buildIndex;
        if (delayTime > 300)
        {
            SceneManager.LoadScene(0);
            delayTime = 0;
        }
        if (Input.GetMouseButtonDown(0))
        {
            delayTime = 0;
            Vector3 pos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, distance));
            if (particle != null)
            {
                particle.transform.position = pos;
                particle.Play();
            }
            //if (touchSound != null)
            //{
            //    //touchSound.Play();
            //}
        }
        main.volume = mainVol;
        if (Input.GetKeyDown(KeyCode.I))
        {
            SettingsImport();
        }
    }
    private void SettingsImport()
    {
        string filePath = Environment.CurrentDirectory + "\\settings.txt";
        if (System.IO.File.Exists(filePath))
        {
            string[] config = System.IO.File.ReadAllLines(filePath);
            for (int i = 0; i < config.Length; i++)
            {
                string[] arr = config[i].Split(' ');
                if (arr[0].ToLower() == "main")
                {
                    mainVol = float.Parse(arr[1]);
                }
            }
        }
        else
        {
            string[] config = new string[]
            {
                "main 0.2",
            };

            System.IO.File.Create(filePath).Close();
            System.IO.File.WriteAllLines(filePath, config);

#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

    }
}
