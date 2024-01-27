using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CursedMenu : MonoBehaviour
{
    public Button button;
    public float speed = 5f;
    public float angle = 45f; // Angle in degrees

    private RectTransform rectTransform;
    private Vector2 direction;

    void Start()
    {
        rectTransform = button.GetComponent<RectTransform>();

        // Calculate the initial direction based on the angle
        direction = Quaternion.Euler(0, 0, angle) * Vector2.right;

        // Set the initial color
        SetRandomColor();
    }

    void Update()
    {
        MoveButton();
    }

    void MoveButton()
    {
        Vector2 newPos = rectTransform.anchoredPosition;

        // Move the button based on the direction
        newPos += direction * speed * Time.deltaTime;

        // Check if the button hits the screen edges
        if (newPos.x <= 0 || newPos.x >= Screen.width - rectTransform.rect.width)
        {
            direction.x *= -1; // Reverse horizontal direction on hitting edges
            newPos.x = Mathf.Clamp(newPos.x, 0, Screen.width - rectTransform.rect.width);
            SetRandomColor();
        }
        if (newPos.y <= 0 || newPos.y >= Screen.height - rectTransform.rect.height)
        {
            direction.y *= -1; // Reverse vertical direction on hitting edges
            newPos.y = Mathf.Clamp(newPos.y, 0, Screen.height - rectTransform.rect.height);
            SetRandomColor();
        }

        rectTransform.anchoredPosition = newPos;
    }

    void SetRandomColor()
    {
        // Generate random color
        Color newColor = new Color(Random.value, Random.value, Random.value);

        // Apply the color to the button
        button.image.color = newColor;
    }
}