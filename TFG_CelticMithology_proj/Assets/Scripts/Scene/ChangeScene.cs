using UnityEngine.SceneManagement;
using UnityEngine;

public class ChangeScene : MonoBehaviour { 

    public void Change_Scene(string scene)
    {
        SceneManager.LoadScene(scene, LoadSceneMode.Single);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

}
