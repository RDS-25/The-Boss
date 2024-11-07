using UnityEngine;

public class BossAniStateCheck : StateMachineBehaviour
{

    public override void OnStateExit(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        animator.GetComponent<BossController>().reStartNumber();
        
        animator.GetComponent<BossController>().nav.isStopped= false;
        if (animator.GetComponent<BossController>().RandomNumber <= 100)
        {
            animator.GetComponent<BossController>().Move();
        }
        else if (animator.GetComponent<BossController>().RandomNumber > 101)
        {
            animator.GetComponent<BossController>().Attack();
        }
        
    }
}
