using UnityEngine;

public class MainMenuUI : MonoBehaviour
{
    private Animator anim;
    [SerializeField] private GameObject worldBackground;
    private void OnEnable()
    {
        anim = GetComponent<Animator>();
        GameManager.OnGameState += AnimationState;
    }

    private void OnDisable()
    {
        GameManager.OnGameState -= AnimationState;
    }

    private void AnimationState(bool status)
    {
        worldBackground.SetActive(!status);
        anim.SetTrigger(status ? "Hide" : "Show");
    }
}