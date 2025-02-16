using UnityEngine;
using UnityEngine.UI;

public class ControlInputGUi : MonoBehaviour
{
    [SerializeField]
    CanvasGroup canvasGroup;

    public KeyCode keyCode;

    [Space, Range(0, 1)]
    public float regularAlpha = .5f;
    [Range(0, 1)]
    public float pressedAlpha = .8f;

    void Update()
    {
        bool pressed = Input.GetKey(keyCode);
        canvasGroup.alpha = pressed ? pressedAlpha : regularAlpha;
    }
}