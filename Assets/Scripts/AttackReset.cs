using UnityEngine;

public class AttackReset : StateMachineBehaviour
{
    [SerializeField] string triggerName;

    public override void OnStateExit(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex) {
        animator.ResetTrigger(triggerName);
        animator.GetComponent<PlayerController>().Currentstate = PlayerController.State.Idle;
    }
 
}
