using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreSystem : MonoBehaviour
{
    private int score;
    public static ScoreSystem instance;

    public TextMeshProUGUI scoreDisplay;
    public TextMeshProUGUI buyText;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Update()
    {
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 3))
        {
            if (hit.collider.CompareTag("CanBuy"))
            {
                buyText.text = "Press E to buy " + hit.collider.gameObject.name + " for " + hit.collider.gameObject.GetComponent<Shop>().cost;
                if(Input.GetKeyDown(KeyCode.E)) 
                {
                    if (hit.collider.gameObject.GetComponent<Shop>() && score >= hit.collider.gameObject.GetComponent<Shop>().cost)
                    {
                        hit.collider.gameObject.GetComponent<Shop>().Buy();
                    }
                }
            }
            else
            {
                buyText.text = null;
            }
        }
    }
    public void UpdateScore(int value)
    {
        score += value;
        scoreDisplay.text = score.ToString();
    }
    public int CheckScore()
    {
        return score;
    }
}
