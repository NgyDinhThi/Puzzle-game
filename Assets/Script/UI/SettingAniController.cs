using UnityEngine;

public class SettingAniController : MonoBehaviour
{
    public Animator animator;

    public void PlaySettingScreenAni(string TriggerName)
    {
        animator.SetTrigger(TriggerName);
    }    
}
