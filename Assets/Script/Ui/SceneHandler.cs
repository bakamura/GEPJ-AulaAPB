using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneHandler : MonoBehaviour {

    public void GoToScene(int sceneId) {
        SceneManager.LoadScene(sceneId, LoadSceneMode.Additive);
        //AsyncOperation asyncLoadOperation = SceneManager.LoadSceneAsync(sceneId, LoadSceneMode.Additive);
        //AsyncOperation asyncUnloadOperation = SceneManager.UnloadSceneAsync(sceneId);
    }

}
