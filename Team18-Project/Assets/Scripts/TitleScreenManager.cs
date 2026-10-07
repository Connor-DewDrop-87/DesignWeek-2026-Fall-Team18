using UnityEngine;
using UnityEngine.SceneManagement;
public class TitleScreenManager : MonoBehaviour
{
    public GameObject instructions;
    public void StartGame()
    {
        SceneManager.LoadScene("BattleSceneCyclops");
    }
    public void BringUpInstructions()
    {

    }
}
