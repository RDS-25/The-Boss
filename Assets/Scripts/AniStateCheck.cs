
using UnityEngine;

public class AniStateCheck : StateMachineBehaviour
{



    public override void OnStateExit(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex) {
        animator.GetComponent<PlayerController>().characterController.enabled= true;
        animator.GetComponent<PlayerController>().HitCoilder.enabled = true;
        animator.GetComponent<PlayerController>().isDodge = false;
        animator.GetComponent<PlayerController>().isHit = false;
    }
 
}
