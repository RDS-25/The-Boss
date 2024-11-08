
using UnityEngine;

public class AniStateCheck : StateMachineBehaviour
{



    public override void OnStateExit(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex) {
        animator.GetComponent<PlayerController>().Currentstate = PlayerController.State.Idle;
        
    }
 
}
