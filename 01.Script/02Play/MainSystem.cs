using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainSystem : MonoBehaviour
{
    public int stageIndex;
    public Button[] btnAsSeek;
    public Button[] btnGsSeek;
    public Button[] btnInfoAs;
    public Button[] btnInfoGs;
    public Image[] checkAs; //fill amount image
    public Image[] checkGs; //fill amount image
    public Image fadeImage; //색이 칠해지는 메테리얼을 가진 이미지
    public Image timerGauge;
    private int selectedInfoIndex;
    private int infoGSNarCount;
    private int infoASNarCount;
    public Texture2D[] fadeTexture; //색이 칠해지는 텍스쳐
    public GameObject success;
    public GameObject fail;
    public GameObject[] infoAs;
    public GameObject[] infoGs;
    public GameObject gsBtnParent;
    public GameObject asBtnParent;
    public GameObject asObj;
    public GameObject gsObj;
    public AnimationClip[] successClip;
    public AnimationClip[] failClip;
    public ParticleSystem succesParticle;
    public ParticleSystem failParticle;
    public Button btnX;
    public Button[] touchSoundButton;
    public AudioSource narrationGsAudioSource;
    public AudioSource narrationAsAudioSource;
    public AudioSource infoNarSource;
    public AudioSource scoreDoneSound;

    private int gsNarIndexCount; //gs 인덱스 나레이션이 재생될때마다 1씩 올려줌
    private int asNarIndexCount;
    
    public AudioClip[] gsNarClip;
    public AudioClip[] asNarClip;
    private int[] gsNarIndex;
    private int[] asNarIndex;
    public AudioClip[] gsInfoNarClip;
    public AudioClip[] asInfoNarClip;
    public AudioSource succesSound;
    public AudioSource failSound;
    private int gsNarRanNum;
    private int asNarRanNum;
    private int timeCheck;
    

    private float successDelay;
    private float failDelay;


    private bool[] checkBoolGs=new bool[5]; //동그라미 표시 bool값
    private bool[] checkBoolAs=new bool[7]; //동그라미 표시 bool값
    private bool[] fadeBool; //페이드 bool값
    private float[] textureVal;

    private int countAmount; //이미지 fill amount를 위한 카운트
    private int scoreAs;
    private int scoreGs;
    private int gsCount; //gs score가 다 찼을때 코루틴 시작
    private int asCount; //as score가 다 찼을
    private int fadeIdx; //fade를 위한 인덱스
    private int checkIdx;//fill amount 인덱스
    private int fadeCount; //fade를 위한 카운트
    private int resultCount;
    private float timer;
    private bool animDone; //타이머 시작하기 전 애니메이션 끝났는지 체크해주는 bool값
    private int animDoneCount; //animdone 카운트해주는 int


    private bool nonOver;


    private void Start()
    {
        gsNarIndex = new int[gsNarClip.Length];
        asNarIndex=new int[asNarClip.Length];
        int ranGSLength = gsNarIndex.Length;
        int[] ranGSArr = Enumerable.Range(0, ranGSLength).ToArray();
        for (int i = 0; i < ranGSLength; ++i)
        {
            int ranIdx = Random.Range(i, ranGSLength);
            int tmp = ranGSArr[ranIdx];
            ranGSArr[ranIdx] = ranGSArr[i];
            ranGSArr[i] = tmp;
            gsNarIndex[i] = ranGSArr[i];
        }
        int ranASLength = asNarIndex.Length;
        int[] ranASArr = Enumerable.Range(0, ranASLength).ToArray();
        for (int i = 0; i < ranASLength; ++i)
        {
            int ranIdx = Random.Range(i, ranASLength);
            int tmp = ranASArr[ranIdx];
            ranASArr[ranIdx] = ranASArr[i];
            ranASArr[i] = tmp;
            asNarIndex[i] = ranASArr[i];
        }
        
        infoGSNarCount = 2;
        infoASNarCount = 2;
        failParticle.Stop();
        succesParticle.Stop();
        animDone = false;
        StartCoroutine(AnimDonePlay());
        timerGauge.fillAmount = 1;
        successDelay = successClip[0].length + successClip[1].length;
        failDelay = failClip[0].length + failClip[1].length;
        resultCount = 0;
        for(int i=0; i < checkBoolGs.Length; i++)
        {
            checkBoolGs[i] = false;
        }
        for(int i=0; i < checkBoolAs.Length; i++)
        {
            checkBoolAs[i] = false;
        }
        gsCount = 2;
        asCount = 2;
        fadeCount = 2;
        textureVal = new float[8];
        for(int i=0; i < textureVal.Length; i++)
        {
            textureVal[i] = 0;
        }
        countAmount = 2;
        scoreGs = 0;
        scoreAs = 0;
        timer = 70;
        nonOver = true;
        for(int i=0; i < btnInfoAs.Length; i++)
        {
            checkAs[i].fillAmount = 0;
            btnInfoAs[i].enabled = true;
            infoAs[i].SetActive(false);
        }
        for(int i=0; i<btnInfoGs.Length; i++)
        {
            checkGs[i].fillAmount = 0;
            infoGs[i].SetActive(false);
            btnInfoGs[i].enabled = true;
        }
        stageIndex = 0;
        for(int i=0; i < btnAsSeek.Length; i++)
        {
            int idx = i;
            btnAsSeek[idx].onClick.AddListener(() => BtnAsSeek(idx));
        }
        for (int i = 0; i < btnGsSeek.Length; i++)
        {
            int idx = i;
            btnGsSeek[idx].onClick.AddListener(() => BtnGsSeek(idx));
        }
        for (int i = 0; i < btnInfoAs.Length; i++)
        {
            int idx = i;
            btnInfoAs[idx].onClick.AddListener(() => BtnInfoAs(idx));
        }
        for (int i = 0; i < btnInfoGs.Length; i++)
        {
            int idx = i;
            btnInfoGs[idx].onClick.AddListener(() => BtnInfoGs(idx));
        }
        btnX.onClick.AddListener(() => BtnTouchSoundX());
        for(int i=0; i<touchSoundButton.Length; i++)
        {
            int idx = i;
            touchSoundButton[idx].onClick.AddListener(() => BtnTouchSound());
        }
    }

    private void Update()
    {
        
        countAmount++;
        if (countAmount == 1)
        {
            StartCoroutine(FillAmount(checkIdx));
        }

        //fadeCount++;
        //if (fadeCount == 1)
        //{
        //    StartCoroutine(FadeTexturePlay());
        //}
        
        switch (stageIndex)
        {
            case 0:
                if (animDone)
                {
                    timerGauge.fillAmount -= 0.015f * Time.deltaTime;
                    if (timer < 0 || timer == 0)
                    {
                        nonOver = false;
                        stageIndex = 3;
                    }
                    else
                    {
                        timer -= Time.deltaTime;
                    }
                }
                else
                {
                    timerGauge.fillAmount += 0.5f * Time.deltaTime;
                }
                infoGSNarCount++;
                if (infoGSNarCount == 1)
                {
                    StartCoroutine(InfoGSNar(selectedInfoIndex));
                }
                gsBtnParent.SetActive(true);
                asBtnParent.SetActive(false);
                for (int i = 0; i < checkBoolGs.Length; i++)
                {
                    if (scoreGs == 5)
                    {
                        textureVal[7] += 0.1f * Time.deltaTime;
                    }
                    else
                    {

                        if (checkBoolGs[i])
                        {
                            if (textureVal[i] < 1)
                            {
                                textureVal[i] += 1f * Time.deltaTime;
                            }
                        }
                    }
                }
                gsObj.SetActive(true);
                asObj.SetActive(false);
                fadeImage.material.SetTexture("_ClipImg0", fadeTexture[0]);
                fadeImage.material.SetTexture("_ClipImg1", fadeTexture[1]);
                fadeImage.material.SetVector("_ClipImg0Controller", new Vector4(textureVal[0], textureVal[1], textureVal[2], textureVal[3]));
                fadeImage.material.SetVector("_ClipImg1Controller", new Vector4(textureVal[4], textureVal[5], textureVal[6], textureVal[7]));

                gsCount++;
                if (gsCount == 1)
                {
                    StartCoroutine(GsCompletePlay());

                }
                break;
            case 1:
                if (animDone)
                {
                    timerGauge.fillAmount -= 0.015f * Time.deltaTime;
                    if (timer < 0 || timer == 0)
                    {
                        nonOver = false;
                        stageIndex = 3;
                    }
                    else
                    {
                        timer -= Time.deltaTime;
                    }
                    for (int i = 0; i < checkBoolAs.Length; i++)
                    {
                        if (scoreAs == 7)
                        {
                            textureVal[7] += 0.1f * Time.deltaTime;
                        }
                        else
                        {

                            if (checkBoolAs[i])
                            {
                                if (textureVal[i] < 1)
                                {
                                    textureVal[i] += 1f * Time.deltaTime;
                                }
                            }
                        }
                    }
                }
                else
                {
                    timerGauge.fillAmount += 0.5f * Time.deltaTime;
                }
                infoASNarCount++;
                if (infoASNarCount == 1)
                {
                    StartCoroutine(InfoASNar(selectedInfoIndex));
                }
                gsBtnParent.SetActive(false);
                asBtnParent.SetActive(true);
                fadeImage.material.SetTexture("_ClipImg0", fadeTexture[2]);
                fadeImage.material.SetTexture("_ClipImg1", fadeTexture[3]);
                fadeImage.material.SetVector("_ClipImg0Controller", new Vector4(textureVal[0], textureVal[1], textureVal[2], textureVal[3]));
                fadeImage.material.SetVector("_ClipImg1Controller", new Vector4(textureVal[4], textureVal[5], textureVal[6], textureVal[7]));
                gsObj.SetActive(false);
                asObj.SetActive(true);
                asCount++;
                if (asCount == 1)
                {
                    StartCoroutine(AsCompletePlay());
                }

                break;
            case 2:
                resultCount++;
                if (resultCount == 1)
                {
                    StartCoroutine(SuccessPlay());
                }
                break;
            case 3:
                resultCount++;
                if (resultCount == 1)
                {
                    StartCoroutine(FailPlay());
                }
                break;
        }
    }
    //숨은그림 버튼
    private void BtnAsSeek(int idx)
    {

        if (nonOver)
        {
            fadeCount = 0;
            countAmount = 0;
            checkIdx = idx;
            checkAs[idx].raycastTarget = true;
            for (int i = 0; i < infoAs.Length; i++)
            {
                infoAs[i].SetActive(false);
            }
        }
        if (!checkBoolAs[idx])
        {

            ProjectManager.instance.oTouchSound.Play();
            scoreAs++;
        }
        if (scoreAs == 7)
        {
            fadeIdx = 7;
            asCount = 0;
        }
        else
        {
            fadeIdx = idx;
        }
        btnAsSeek[idx].gameObject.SetActive(false);
    }
    //숨은그림 버튼
    private void BtnGsSeek(int idx)
    {
        if (nonOver)
        {
            fadeCount = 0;
            countAmount = 0;
            checkIdx = idx;
            checkGs[idx].raycastTarget = true;
            for(int i=0; i < infoGs.Length; i++)
            {
                infoGs[i].SetActive(false);
            }
        }
        if (!checkBoolGs[idx])
        {

            ProjectManager.instance.oTouchSound.Play();
            scoreGs++;
        }
        
        if (scoreGs == 5)
        {
            gsCount = 0;
        }
        else
        {
            fadeIdx = idx;
        }
        btnGsSeek[idx].gameObject.SetActive(false);
    }

    //상단 힌트 버튼
    private void BtnInfoAs(int idx)
    {
        if (nonOver)
        {
            ProjectManager.instance.touchSound.Play();
            for(int i=0; i < infoAs.Length; i++)
            {
                if (i == idx)
                {
                    infoASNarCount = 0;
                    selectedInfoIndex = i;
                    infoAs[idx].SetActive(true);
                }
                else
                {
                    infoAs[i].SetActive(false);
                }
            }
        }
    }
    //상단 힌트 버튼
    private void BtnInfoGs(int idx)
    {
        if (nonOver)
        {
            ProjectManager.instance.touchSound.Play();
            for (int i = 0; i < infoGs.Length; i++)
            {
                if (i == idx)
                {
                    selectedInfoIndex = i;
                    infoGSNarCount = 0;
                    infoGs[idx].SetActive(true);
                    
                }
                else
                {
                    infoGs[i].SetActive(false);
                }
            }
            
        }
    }

    private void BtnTouchSound()
    {
        ProjectManager.instance.touchSound.Play();
    }

    private void BtnTouchSoundX()
    {
        ProjectManager.instance.xTouchSound.Play();
    }

    //IEnumerator FadeTexturePlay()
    //{
    //    for(float i=0; i<1.1f; i += 0.01f)
    //    {
    //        textureVal[fadeIdx] = i;
    //        yield return new WaitForSeconds(0.01f);
    //    }
    //}

    IEnumerator GsCompletePlay()
    {
        timer = 70;
        animDone = false;
        timerGauge.fillAmount = 1;
        scoreDoneSound.Play();
        yield return new WaitForSeconds(4f);
        stageIndex = 1;
        for(int i=0; i < textureVal.Length; i++)
        {
            textureVal[i] = 0;
        }
        yield return new WaitForSeconds(2f);
        animDone = true;
        StartCoroutine(ASNar(0));
        narrationGsAudioSource.gameObject.SetActive(false);

    }
    IEnumerator AsCompletePlay()
    {
        timer = 70;
        scoreDoneSound.Play();
        narrationAsAudioSource.gameObject.SetActive(false);
        yield return new WaitForSeconds(6f);
        stageIndex = 2;
        


    }

    IEnumerator FillAmount(int idx)
    {

        if (stageIndex == 0)
        {
            if (!checkBoolGs[idx])
            {

                for (float i = 0; i <=1.1f; i+= 0.05f)
                {
                    checkGs[idx].fillAmount = i;
                    yield return new WaitForSeconds(0.01f);
                }
                yield return new WaitForSeconds(0.2f);
                checkBoolGs[idx] = true;
            }

        }
        if (stageIndex == 1)
        {
            if (!checkBoolAs[idx])
            {

                for (float i = 0; i <= 1.1f; i += 0.05f)
                {
                    checkAs[idx].fillAmount = i;
                    yield return new WaitForSeconds(0.01f);
                }
                yield return new WaitForSeconds(0.2f);
                checkBoolAs[idx] = true;
            }
        }
        
    }

    IEnumerator SuccessPlay()
    {
        succesSound.Play();
        success.SetActive(true);
        yield return new WaitForSeconds(successClip[0].length);
        succesParticle.Play();
        yield return new WaitForSeconds(successClip[1].length);
        SceneManager.LoadScene(0);
    }

    IEnumerator FailPlay()
    {
        narrationGsAudioSource.gameObject.SetActive(false);
        narrationAsAudioSource.gameObject.SetActive(false);
        failSound.Play();
        fail.SetActive(true);
        yield return new WaitForSeconds(failClip[0].length);
        failParticle.Play();
        yield return new WaitForSeconds(failClip[1].length);
        SceneManager.LoadScene(0);
    }
    IEnumerator AnimDonePlay()
    {
        yield return new WaitForSeconds(2f);
        animDone = true;
        StartCoroutine(GSNar(gsNarIndex[0]));
    }
    IEnumerator ASNar(int ran)
    {
        if (asNarIndexCount >= asNarClip.Length)
        {
            asNarIndexCount = 0;
        }
        else
        {
            asNarIndexCount++;
        }
        asNarIndexCount++;
        yield return new WaitForSeconds(3f);
        narrationAsAudioSource.PlayOneShot(asNarClip[ran]);
        yield return new WaitForSeconds(asNarClip[ran].length);
        StartCoroutine(ASNar(asNarIndex[asNarIndexCount]));
    }
    IEnumerator GSNar(int ran)
    {
        if (gsNarIndexCount >= gsNarClip.Length)
        {
            gsNarIndexCount = 0;
        }
        else
        {
            gsNarIndexCount++;
        }
        yield return new WaitForSeconds(3f);
        narrationGsAudioSource.PlayOneShot(gsNarClip[ran]);
        yield return new WaitForSeconds(gsNarClip[ran].length);
        StartCoroutine(GSNar(gsNarIndex[gsNarIndexCount]));
        
    }
    IEnumerator InfoGSNar(int idx)
    {
       
        for(float i=1; i>-0.1f; i -= 0.1f)
        {

            narrationGsAudioSource.volume = i;
            yield return new WaitForSeconds(0.1f);
        }
        
        yield return new WaitForSeconds(0f);

        infoNarSource.PlayOneShot(gsInfoNarClip[idx]);
        yield return new WaitForSeconds(gsInfoNarClip[idx].length);
       
        yield return new WaitForSeconds(0.5f);
        
        for (float i = 0; i < 1f; i += 0.1f)
        {

            narrationGsAudioSource.volume = i;
            yield return new WaitForSeconds(0.1f);
        }
    }

    IEnumerator InfoASNar(int idx)
    {


        infoNarSource.PlayOneShot(asInfoNarClip[idx]);
        for (float i = 1; i > -0.1f; i -= 0.1f)
        {

            narrationAsAudioSource.volume = i;
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(0f);
        yield return new WaitForSeconds(asInfoNarClip[idx].length);
        yield return new WaitForSeconds(0.5f);
        for (float i = 0; i < 1f; i += 0.1f)
        {

            narrationAsAudioSource.volume = i;
            yield return new WaitForSeconds(0.1f);
        }

    }

}
