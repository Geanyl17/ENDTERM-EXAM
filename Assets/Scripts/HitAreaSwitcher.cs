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
                hitAreaSpriteRenderer.sprite = circleSprite; //green
                break;
            case HitAreaShape.Square:
                hitAreaSpriteRenderer.sprite = squareSprite; //red  
                break;
            case HitAreaShape.Triangle:
                hitAreaSpriteRenderer.sprite = triangleSprite; //blue
                break;
            case HitAreaShape.Rectangle:
                hitAreaSpriteRenderer.sprite = RectangleSprite; //yellow AND shape is X
                break;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Note note = other.GetComponent<Note>();
        if (note != null)
        {
            if (note.noteShape == currentShape)
            {
                GameManager.Instance.RegisterHit(); // Register hit in GameManager
                Debug.Log($"✅ HIT! Matched: {note.noteShape}");
                Destroy(other.gameObject); // or trigger score FX
            }
            else
            {
                GameManager.Instance.RegisterMiss(); // Register miss in GameManager
                Debug.Log($"❌ MISS! Expected {currentShape}, got {note.noteShape}");
                Destroy(other.gameObject); // or trigger miss FX
            }
        }
    }

}
