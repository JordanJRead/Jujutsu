using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimHandsPunch : StateMachineBehaviour
{
    public GameObject HitboxPrefab;
    public float HitboxLifespan;
    public float HitboxStunDuration;
    public float HitboxDelay;

    private Timer hitboxTimer = new Timer();

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        hitboxTimer.ResetTime(HitboxDelay);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (hitboxTimer.Update(Time.deltaTime))
        {
            GameObject hitboxObject = Instantiate(HitboxPrefab, animator.gameObject.transform.Find("PunchHitboxLocation"));
            hitboxObject.transform.localPosition = Vector3.zero;
            Hitbox hitbox = hitboxObject.GetComponent<Hitbox>();
            hitbox.Instantiate(HitboxLifespan, HitboxStunDuration, animator.gameObject);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
