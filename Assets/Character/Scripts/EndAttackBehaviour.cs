using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndAttackBehaviour : StateMachineBehaviour {
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        animator.SetLayerWeight(layerIndex, 0);
        CharacterAttack characterAttack = animator.GetComponent<CharacterAttack>();
        characterAttack.EndAttack();
    }
}
