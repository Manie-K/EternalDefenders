using UnityEngine;

namespace EternalDefenders
{
    public class PlaySoundEnter : StateMachineBehaviour
    {
        [SerializeField] SoundType soundType;
        [SerializeField] int soundIndex = 0;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.GetComponentInParent<AudioHelper>().PlaySound(soundType, soundIndex);
        }
    }
}
