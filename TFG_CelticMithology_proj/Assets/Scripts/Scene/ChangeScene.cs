using UnityEngine.SceneManagement;
using UnityEngine;

public class ChangeScene : MonoBehaviour {
    public bool menu = false;

    public void Change_Scene(string scene)
    {
        if (!menu)
            SceneManager.LoadScene(scene, LoadSceneMode.Single);
        else
        {
            int tutorial = PlayerPrefs.GetInt("playerTutorial");
            if (tutorial != 1)
            {
                SceneManager.LoadScene("TutorialScene", LoadSceneMode.Single);
            }
            else
            {
                SceneManager.LoadScene(scene, LoadSceneMode.Single);
            }
        }
    }

    public void ExitGame()
    {
        Application.Quit();
    }

}
