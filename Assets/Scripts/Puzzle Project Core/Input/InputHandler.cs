using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.Serialization;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
public class InputHandler : MonoBehaviour
{
    [SerializeField] float m_swipeThreshold = 100f;
    [SerializeField] float m_swipeTimeOut = 1f;
    public UnityEvent<Vector2> OnTap;
    public UnityEvent<Vector2, Vector2> OnSwipe;
    void OnEnable()
    {
        EnhancedTouchSupport.Enable();
        Touch.onFingerUp += OnTouchEnded;
    }
    void OnDisable()
    {
        Touch.onFingerUp -= OnTouchEnded;
        EnhancedTouchSupport.Disable();
    }

    void OnTouchEnded(Finger finger)
    {
        if(Camera.main is null) return;
        Vector2 worldStart = Camera.main.ScreenToWorldPoint(finger.currentTouch.startScreenPosition);
        Vector2 worldEnd = Camera.main.ScreenToWorldPoint(finger.currentTouch.screenPosition);
        if (finger.currentTouch.isTap)
        {
            OnTap?.Invoke(worldEnd);
            return;
        }
        if(Vector2.Distance(worldStart, worldEnd) < m_swipeThreshold) return;
        OnSwipe?.Invoke(worldStart, worldEnd);
        
    }
}
