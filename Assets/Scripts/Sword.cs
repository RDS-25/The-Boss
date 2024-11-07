using UnityEngine;

public class Sword : MonoBehaviour 
{
    int Damage = 10;
  
    private void OnTriggerEnter(Collider other) {    
        if(other.tag =="Enemy"){
            other.GetComponent<BossController>().Hp -= Damage;
         
        }
    }
}
