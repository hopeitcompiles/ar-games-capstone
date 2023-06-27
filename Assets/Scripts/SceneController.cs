using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{


    public void LoadManoMotionScene()
    {
        StartCoroutine(LoadMainScene());
    }

    private IEnumerator LoadMainScene()
    {
        yield return new WaitForSeconds(0.01f);
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("ManoMotionSDKProFeatures"); // Carga de forma asíncrona la escena principal
        asyncLoad.allowSceneActivation = false; // Evita que la escena se active automáticamente

        while (!asyncLoad.isDone)
        {

            if (asyncLoad.progress >= 0.9f)
            {
                asyncLoad.allowSceneActivation = true; // Activa la escena principal cuando la carga esté casi completa
            }

            yield return null;
        }
    }
}
