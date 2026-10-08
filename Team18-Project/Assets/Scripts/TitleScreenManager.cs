using UnityEngine;
using UnityEngine.SceneManagement;
public class TitleScreenManager : MonoBehaviour
{
    public GameObject instructionsPage;
    public void StartGame()
    {
        SceneManager.LoadScene("BattleSceneCyclops");
    }
    public void StartHardGame()
    {
        SceneManager.LoadScene("HardMode");
    }
    public void BringUpInstructions()
    {
        instructionsPage.SetActive(true);
    }
    public void GoAwayInstructions()
    {
        instructionsPage.SetActive(false);
    }
}
