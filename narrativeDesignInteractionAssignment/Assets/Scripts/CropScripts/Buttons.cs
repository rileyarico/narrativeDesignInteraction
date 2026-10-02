using UnityEngine;
using UnityEngine.SceneManagement;

public class Buttons : MonoBehaviour
{
    public void NextScene(string sceneName)
    {
        PlayerMovement player = FindFirstObjectByType<PlayerMovement>();
        if (player != null) { player.UnlockCursor(); }
        SceneManager.LoadScene(sceneName);
    }

    public void ExitPauseMenu()
    {
        RequestManager rq = FindFirstObjectByType<RequestManager>();
        rq.ManagePauseMenu();
    }
}

