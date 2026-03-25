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
        if (finger.currentTouch.isTap)
        {
            OnTap?.Invoke(finger.currentTouch.startScreenPosition);
            return;
        }
        if(Vector2.Distance(finger.currentTouch.startScreenPosition, finger.currentTouch.screenPosition) < m_swipeThreshold) return;
        if (finger.currentTouch.time > m_swipeTimeOut) return;
        OnSwipe?.Invoke(finger.currentTouch.startScreenPosition, finger.currentTouch.screenPosition);
        
    }
}
