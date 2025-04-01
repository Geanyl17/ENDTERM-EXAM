using UnityEngine;
using System.Collections.Generic;
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

    void Start()
    {
        UpdateVisuals();
    }

    void Update()
    {
        // Check for Xbox controller button presses:
        // A button (JoystickButton0) -> Circle
        if (Input.GetKeyDown(KeyCode.JoystickButton0))
        {
            SetShape(HitAreaShape.Circle);
        }
        // B button (JoystickButton1) -> Square
        else if (Input.GetKeyDown(KeyCode.JoystickButton1))
        {
            SetShape(HitAreaShape.Square);
        }
        // X button (JoystickButton2) -> Triangle
        else if (Input.GetKeyDown(KeyCode.JoystickButton2))
        {
            SetShape(HitAreaShape.Triangle);
        }
        else if (Input.GetKeyDown(KeyCode.JoystickButton3))
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
        // Update the sprite based on the current shape.
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

    // Optional: Collision detection for notes can be added here.
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
