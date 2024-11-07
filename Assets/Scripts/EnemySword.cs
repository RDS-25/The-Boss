using UnityEngine;

public class EnemySword : MonoBehaviour
{
    int Damage = 10;
  
    private void OnTriggerEnter(Collider other) {
        if(other.tag !="Player"){
          Debug.Log(other.name);
        }
        
        if(other.tag =="Player"){

            if(other.GetComponent<PlayerController>().isDodge){
                Debug.Log("데미지없음");
            }else{
                other.GetComponent<PlayerController>().Hp -= Damage;
            }
            transform.GetComponent<Collider>().enabled=false;
         
        }
    }
}
