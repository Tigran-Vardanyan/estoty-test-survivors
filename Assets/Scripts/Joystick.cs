using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Joystick : MonoBehaviour
{
    public RectTransform joystickBackground;
    public RectTransform joystickHandle;
    public CanvasGroup canvasGroup;

    private Vector2 inputVector;
    private bool isDragging;

    public float Horizontal => inputVector.x;
    public float Vertical => inputVector.y;

    void Start()
    {
        // Initially hide the joystick
        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }

    void Update()
    {
        // Detect mouse down and up using the new Input System
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            OnPointerDown(Mouse.current.position.ReadValue());
        }

        if (Mouse.current.leftButton.isPressed)
        {
            OnDrag(Mouse.current.position.ReadValue());
        }

        if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            OnPointerUp();
        }
    }

    private void OnPointerDown(Vector2 pointerPosition)
    {
        // Make the joystick visible when touched
        canvasGroup.alpha = 1f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;

        // Move the joystick background to the touch position
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform)transform.parent, pointerPosition, null, out localPoint);
        joystickBackground.anchoredPosition = localPoint;

        isDragging = true;
    }

    private void OnDrag(Vector2 pointerPosition)
    {
        if (!isDragging) return;

        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(joystickBackground, pointerPosition, null, out localPoint);

        inputVector = new Vector2(localPoint.x / joystickBackground.sizeDelta.x * 2, localPoint.y / joystickBackground.sizeDelta.y * 2);
        inputVector = (inputVector.magnitude > 1.0f) ? inputVector.normalized : inputVector;

        joystickHandle.anchoredPosition = new Vector2(inputVector.x * (joystickBackground.sizeDelta.x / 2), inputVector.y * (joystickBackground.sizeDelta.y / 2));
    }

    private void OnPointerUp()
    {
        // Hide the joystick when the touch is released
        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        inputVector = Vector2.zero;
        joystickHandle.anchoredPosition = Vector2.zero;
        isDragging = false;
    }
}