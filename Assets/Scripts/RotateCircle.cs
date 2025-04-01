using UnityEngine;

public class RotateCircleWithInput : MonoBehaviour
{
    public Joystick joystick; // Assign in Inspector (for mobile)
    public float deadZone = 0.2f; // Ignore small joystick movement

    void Update()
    {
        float horizontal = 0f;
        float vertical = 0f;

        // Check for controller input first
        if (Mathf.Abs(Input.GetAxis("Horizontal")) > deadZone || Mathf.Abs(Input.GetAxis("Vertical")) > deadZone)
        {
            horizontal = Input.GetAxis("Horizontal");
            vertical = Input.GetAxis("Vertical");
        }
        // If no controller input, use on-screen joystick
        else if (joystick != null)
        {
            horizontal = joystick.Horizontal;
            vertical = joystick.Vertical;
        }

        // Create a vector from the input
        Vector2 inputVector = new Vector2(horizontal, vertical);

        // Only rotate if the input is above the deadzone
        if (inputVector.magnitude >= deadZone)
        {
            float angle = Mathf.Atan2(vertical, horizontal) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }
}
