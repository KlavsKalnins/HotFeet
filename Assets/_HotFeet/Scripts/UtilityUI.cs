using UnityEngine;
using UnityEngine.SceneManagement;

public class UtilityUI : MonoBehaviour
{
    public void ReloadScene() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
}