using UnityEngine;
using UnityEngine.UI; // For UI Button functionality

public class HitAreaSwitcher : MonoBehaviour
{
    public HitAreaShape currentShape = HitAreaShape.Circle;

    // Reference to the SpriteRenderer on the hit area
    public SpriteRenderer hitAreaSpriteRenderer;

    // Sprites for each shape
    public Sprite circleSprite;
    public Sprite squareSprite;
    public Sprite triangleSprite;
    public Sprite RectangleSprite;

    // References to UI Buttons for each shape
    public Button circleButton;
    public Button squareButton;
    public Button triangleButton;
    public Button rectangleButton;

    void Start()
    {
        // Update the visuals on start
        UpdateVisuals();

        // Add listeners for the buttons
        circleButton.onClick.AddListener(() => SetShape(HitAreaShape.Circle));
        squareButton.onClick.AddListener(() => SetShape(HitAreaShape.Square));
        triangleButton.onClick.AddListener(() => SetShape(HitAreaShape.Triangle));
        rectangleButton.onClick.AddListener(() => SetShape(HitAreaShape.Rectangle));
    }

    void Update()
    {
        // Check for controller button presses (Xbox controller example)
        if (Input.GetKeyDown(KeyCode.JoystickButton0)) // A button -> Circle
        {
            SetShape(HitAreaShape.Circle);
        }
        else if (Input.GetKeyDown(KeyCode.JoystickButton1)) // B button -> Square
        {
            SetShape(HitAreaShape.Square);
        }
        else if (Input.GetKeyDown(KeyCode.JoystickButton2)) // X button -> Triangle
        {
            SetShape(HitAreaShape.Triangle);
        }
        else if (Input.GetKeyDown(KeyCode.JoystickButton3)) // Y button -> Rectangle
        {
            SetShape(HitAreaShape.Rectangle);
        }
    }

    void SetShape(HitAreaShape newShape)
    {
        currentShape = newShape;
        UpdateVisuals();
    }

    void UpdateVisuals()
    {
        // Update the sprite based on the current shape
        switch (currentShape)
        {
            case HitAreaShape.Circle:
                hitAreaSpriteRenderer.sprite = circleSprite;
                break;
            case HitAreaShape.Square:
                hitAreaSpriteRenderer.sprite = squareSprite;
                break;
            case HitAreaShape.Triangle:
                hitAreaSpriteRenderer.sprite = triangleSprite;
                break;
            case HitAreaShape.Rectangle:
                hitAreaSpriteRenderer.sprite = RectangleSprite;
                break;
        }
    }

    // Optional: Collision detection for notes can be added here
    void OnTriggerEnter2D(Collider2D other)
    {
        // Example collision logic:
        // Note note = other.GetComponent<Note>();
        // if (note != null && note.noteShape == currentShape)
        // {
        //     Debug.Log("Correct hit!");
        // }
    }
}
