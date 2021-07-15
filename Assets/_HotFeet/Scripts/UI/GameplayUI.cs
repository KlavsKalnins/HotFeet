using UnityEngine;

public class GameplayUI : MonoBehaviour
{
    private Animator anim;
    private void OnEnable()
    {
        anim = GetComponent<Animator>();
        GameManager.OnGameState += AnimationState;
    }

    private void OnDisable() => GameManager.OnGameState -= AnimationState;

    private void AnimationState(bool status) => anim.SetTrigger(status ? "Show" : "Hide");
}
