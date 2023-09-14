using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update

    //UI에 넣을 이미지 컴퍼넌트 만들기
    public List<Image> currentColor = new List<Image>();
    public List<Image> targetColor = new List<Image>();
    //처음 3초만 이미지 보여주게끔.
    public float displayTime = 2f;

    //색깔 목록
    public List<Color> colorList; // 순서대로 아이템을 먹을 색깔 목록
    private int currentIndex = 0; // 현재 아이템 순서의 인덱스

    //알맞은 순서로 아이템을 먹었을 때 나는 사운드
    public AudioSource Sound;
    public AudioClip fail;
    public AudioClip success;
    public AudioClip Finish;

    //배경음악
    //public AudioManager audioManager;

    //게임 끝날 때 UI
    public GameObject gameClear;
    //Scene Change
    public int stage;
    public bool finalStage;


    void Start() //첫 프레임에 실행
    {
        //사운드 객체 가져오기
        //audioManager.PlayBackgroundMusic();
        Sound = GetComponent<AudioSource>();


        //색깔 추가
        HideCurrentImages();
        colorList.Add(Color.red);
        colorList.Add(Color.yellow);
        colorList.Add(Color.blue);
        // 아이템 순서를 섞기 위해 리스트를 랜덤하게 섞습니다.
        ShuffleColorList();
        // 아이템 순서를 출력합니다.
        PrintColorList();
    }


    // 아이템 순서를 랜덤하게 섞는 함수
    void ShuffleColorList()
    {
        for (int i = 0; i < colorList.Count; i++)
        {
            Color temp = colorList[i];
            int randomIndex = Random.Range(i, colorList.Count);
            colorList[i] = colorList[randomIndex];
            colorList[randomIndex] = temp;
            
        }
    }

    // 아이템 순서를 출력하는 함수
    void PrintColorList()
    {
        for (int i = 0; i < colorList.Count; i++)
        {
            targetColor[i].color = colorList[i]; //현재 
        }
        //코루틴 실행하여 3초만 이미지 나타나게 설정.
        StartCoroutine(HideImageAfterDelay(displayTime));
    }
    IEnumerator HideImageAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay); // delay 시간(3초)만큼 대기합니다.

        HideTargetImages(); // Traget이미지를 비활성화하면서 동시에
        ShowCurrentImages();// Current이미지 활성화.
    }
    void HideTargetImages() // Traget이미지를 비활성화함수
    {
        foreach (Image image in targetColor)
        {
            image.enabled = false; 
        }
    }
    void HideCurrentImages() // Current이미지를 비활성화함수
    {
        foreach (Image image in currentColor)
        {
            image.enabled = false; 
        }
    }
    void ShowCurrentImages() //Current이미지 활성함수
    {
        foreach (Image image in currentColor)
        {
            image.enabled = true;
        }
    }
    //플레이어가 아이템을 먹을 때 호출되는 함수
    public void EatItem(GameObject obj)
    {
        //현재 아이템과 일치하는지 확인하고 다음 아이템으로 진행합니다.
        Color color = obj.GetComponent<Renderer>().material.color;

        if (color == targetColor[currentIndex].color)
        {
            Debug.Log("정답! 다음 아이템으로 진행합니다.");
            Sound.clip = success;
            Sound.Play();
            currentColor[currentIndex].color = targetColor[currentIndex].color;
            currentIndex++;
            Destroy(obj);

            // 모든 아이템을 다 먹었을 때 게임 클리어 처리를 할 수도 있습니다.
            if (currentIndex == colorList.Count)
            {
                Debug.Log("게임 클리어!");
                Sound.clip = Finish;
                Sound.Play();
                gameClear.SetActive(true);
                // 게임 클리어 처리 추가
                StartCoroutine(NextStage(1.5f));
                
            }
        }
        else
        {
            Debug.Log("틀렸습니다!");
            Sound.clip = fail;
            Sound.Play();
            // 게임 오버 처리 추가
        }
    }
    IEnumerator NextStage(float delay)
    {
        yield return new WaitForSeconds(delay); // delay 시간(3초)만큼 대기합니다.

        if (!finalStage)
        {
            SceneManager.LoadScene(stage + 1);
        }
        else
        {
            ExitGame();
        }

    }
    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif

    }

    //public void NextStage()
    //{
    //    if (itemCount == totalCount)
    //    {
    //        if (!finalStage)
    //        {
    //           SceneManager.LoadScene(stage + 1);
    //        }
    //        else
    //        {
    //            ExitGame();
    //        }
    //    }
    //    else
    //    {
    //        SceneManager.LoadScene(stage);
    //    }
    //}
    //public void ExitGame()
    //{
    //    #if UNITY_EDITOR
    //        UnityEditor.EditorApplication.isPlaying = false;
    //    #else
    //        Application.Quit();
    //    #endif

    //}
}