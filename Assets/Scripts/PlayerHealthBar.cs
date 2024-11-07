using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{
    public float hp;
    public float maxhp;
    public Image hpGauge;
    public GameObject Ui;
    void Start()
    {
        Ui.SetActive(false);
        maxhp = transform.GetComponent<PlayerController>().maxhp;
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.timeScale ==1f){
            Ui.SetActive(true);
        }
        hp = transform.GetComponent<PlayerController>().Hp;
        hpGauge.fillAmount =hp/maxhp;
    }
}
