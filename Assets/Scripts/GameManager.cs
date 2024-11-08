

using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public Transform player;
    public Transform boss;

    bool isVictory = false;
    public GameObject readObj;
    public TextMeshProUGUI GameCount;
    public TextMeshProUGUI Clear;
    public TextMeshProUGUI GameOver;

    public float delaycount = 5;

    public GameObject Chooseobj;

    

    void Start(){
        // PauseGame();
        
    }

    IEnumerator StartDelay(float delay){
       float countdown = delaycount;

        while (countdown > 0)
        {
            GameCount.text = Mathf.Ceil(countdown).ToString(); // 카운트다운 텍스트 표시
            yield return new WaitForSecondsRealtime(1f); // 실제 시간으로 1초 대기
            countdown--; // 카운트다운 감소
        }

        GameCount.text = "Start!"; // 시작 메시지 표시
        yield return new WaitForSecondsRealtime(1f); // 1초 더 대기 후 메시지 지우기

        GameCount.gameObject.SetActive(false); // 카운트다운 텍스트 비활성화
        ResumGame(); // 게임 재개
    }

    void Update()
    {
        // if (Input.GetKeyDown("space"))
        // {
        //     GameStart();
        // }
        
        var Bosshp = boss.GetComponent<BossController>().Hp;
        var Playerhp = player.GetComponent<PlayerController>().Hp;

        if(Bosshp <= 0 && !isVictory){
            Cursor.visible = true;
            Cursor.lockState =CursorLockMode.None;
            var ani = player.GetComponent<Animator>();
            ani.SetTrigger("Victory");
            Clear.gameObject.SetActive(true);
            Chooseobj.SetActive(true);
            Clear.text = "Game Clear";
            isVictory = true;
        }
        if(Playerhp <= 0){
            Cursor.visible = true;
            Cursor.lockState =CursorLockMode.None;
            GameOver.gameObject.SetActive(true);
            Chooseobj.SetActive(true);
            GameOver.text = "You Die";
        }
    }
   
    public void GameStart(){
        readObj.gameObject.SetActive(false);
        GameCount.gameObject.SetActive(true);
        StartCoroutine(StartDelay(delaycount));
    }
    void PauseGame(){
        Time.timeScale  =  0f;
    }

    void ResumGame(){
        Time.timeScale  =  1f;
    }

    public void AaginPressed(){
        SceneManager.LoadScene("SampleScene");
    }

   public void QuitPressed(){
        Application.Quit();
   }
    
}
