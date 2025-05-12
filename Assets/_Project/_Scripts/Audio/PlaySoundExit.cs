using UnityEngine;

namespace EternalDefenders
{
    public class PlaySoundExit : StateMachineBehaviour
    {
        [SerializeField] SoundType soundType;
        [SerializeField] int soundIndex = 0;

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.GetComponentInParent<AudioHelper>().PlaySound(soundType, soundIndex);
        }
    }
}
