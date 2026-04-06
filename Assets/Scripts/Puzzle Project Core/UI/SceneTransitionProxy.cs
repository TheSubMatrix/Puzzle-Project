using MatrixUtils.DependencyInjection;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionProxy : MonoBehaviour
{
    [Inject] ISceneTransitionManager m_sceneTransitionManager;
    public void TransitionToScene(string sceneName) => m_sceneTransitionManager.TransitionToScene(sceneName);
    public void RestartScene() => m_sceneTransitionManager.TransitionToScene(SceneManager.GetActiveScene().name);
}
