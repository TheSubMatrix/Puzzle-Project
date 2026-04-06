using UnityEngine;
public interface ISceneTransitionManager
{
    public bool IsTransitioning { get; }
    public void TransitionToScene(string sceneName);
}
