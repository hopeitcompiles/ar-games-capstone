using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SplashScreen : MonoBehaviour
{
    [SerializeField]
    Slider progress;

    private const string mainSceneName="GameAR";

    void Start()
    {
        StartCoroutine(LoadMainScene());
    }

    private IEnumerator LoadMainScene()
    {
        yield return new WaitForSeconds(0.01f);
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(mainSceneName); // Carga de forma as�ncrona la escena principal
        asyncLoad.allowSceneActivation = false; // Evita que la escena se active autom�ticamente

        while (!asyncLoad.isDone)
        {
            progress.value = asyncLoad.progress;

            if (asyncLoad.progress >= 0.9f)
            {
                asyncLoad.allowSceneActivation = true; // Activa la escena principal cuando la carga est� casi completa
            }

            yield return null;
        }
    }
}
