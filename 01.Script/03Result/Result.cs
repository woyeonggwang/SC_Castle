using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Result : MonoBehaviour
{
    public static bool result;
    public GameObject[] resultObj;

    public AnimationClip[] successClip;
    public AnimationClip[] failClip;

    private float successDelay;
    private float failDelay;

    private void Start()
    {
        successDelay = successClip[0].length + successClip[1].length;
        failDelay = failClip[0].length + failClip[1].length;
        switch (result)
        {
            case true:
                resultObj[0].SetActive(true);
                resultObj[1].SetActive(false);
                break;
            case false:
                resultObj[0].SetActive(false);
                resultObj[1].SetActive(true);
                break;
        }
        StartCoroutine(DelayLoad());
    }

    private void Update()
    {
        
    }

    IEnumerator DelayLoad()
    {
        switch (result)
        {
            case true:
                yield return new WaitForSeconds(successDelay);
                SceneManager.LoadScene(0);
                break;
            case false:
                yield return new WaitForSeconds(failDelay);
                SceneManager.LoadScene(0);
                break;
        }
    }


}
