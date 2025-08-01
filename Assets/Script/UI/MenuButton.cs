using UnityEngine;
using UnityEngine.SceneManagement;
public class MenuButton : MonoBehaviour
{
    private void Awake()
    {
        if (Application.isEditor == false)
        {
            Debug.unityLogger.logEnabled = false;
        }
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }    
}
