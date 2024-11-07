using UnityEngine;

public class CheckAttack : StateMachineBehaviour
{
    [SerializeField] string triggerName;
    public override void OnStateExit(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex) {
        animator.ResetTrigger(triggerName);
        animator.SetInteger("chargeCount",-1);
        animator.GetComponent<PlayerController>().isAttack= false;
        Debug.Log("어택 종료");
        
    }
}
