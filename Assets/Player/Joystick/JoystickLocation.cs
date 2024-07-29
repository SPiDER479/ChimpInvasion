using UnityEngine;
public class JoystickLocation : MonoBehaviour
{
    [SerializeField] private RectTransform joystick;
    [SerializeField] private Animator playerAnimator;
    private void Update()
    {
        if (Input.touchCount > 0 && playerAnimator.GetBool("Moving"))
            joystick.anchoredPosition += 0.2f * Input.touches[0].deltaPosition;
        else
            joystick.anchoredPosition = new Vector2(0, -215);
    }
}