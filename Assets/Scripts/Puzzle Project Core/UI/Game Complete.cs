using MatrixUtils.Attributes;
using TMPro;
using UnityEngine;
[RequireComponent(typeof(CanvasGroup))]
public class GameComplete : MonoBehaviour
{
    CanvasGroup m_canvasGroup;
    void Awake() => m_canvasGroup = GetComponent<CanvasGroup>();
    [SerializeField, RequiredField] TMP_Text m_finalTimeText;
    public void UpdateFinalTimeText(float finalTime) => m_finalTimeText.SetText($"Final Time: {finalTime:0.00}");
    void DisplayCompletePanel() => StartCoroutine(m_canvasGroup)
}
