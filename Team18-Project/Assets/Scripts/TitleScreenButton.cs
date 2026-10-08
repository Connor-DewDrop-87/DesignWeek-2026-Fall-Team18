using UnityEngine;
using UnityEngine.SceneManagement;
public class TitleScreenButton : MonoBehaviour
{
    public void ResetButton()
    {
        SceneManager.LoadScene("TitleScreen");
    }
}
