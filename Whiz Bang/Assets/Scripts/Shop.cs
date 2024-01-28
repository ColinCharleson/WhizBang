using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    public int cost;

    private bool isExpanding;
    private Vector3 desiredScale;
    private int mapSize = 1;
    public enum Options
    {
        Zone,
        Gift,
        other
    }

    // Create a public variable of the enum type
    public Options selectedOption;
    public void Buy()
    {
        if (ScoreSystem.instance.CheckScore() >= cost)
        {
            switch (selectedOption)
            {
                case Options.Zone:
                    if (!isExpanding)
                    {
                        ScoreSystem.instance.UpdateScore(-cost);
                        ZoneBubble();
                    }
                    break;
                case Options.Gift:
                    ScoreSystem.instance.UpdateScore(-cost);
                    Present();
                    break;
                default: break;
            }
        }
    }

    private void Update()
    {
        if (isExpanding)
        {
            Debug.Log(Vector3.Distance(transform.localScale, desiredScale));
            transform.localScale = Vector3.Lerp(transform.localScale, desiredScale, Time.deltaTime);

            if (Vector3.Distance(transform.localScale, desiredScale) >= 0.1f)
            {
                isExpanding = true;
            }
            else
            {
                isExpanding = false;
                mapSize++;
            }
        }

        if(mapSize >= 5)
        {
            Destroy(this);
        }
    }

    private void ZoneBubble()
    {
        cost += 500;
        desiredScale = new Vector3(transform.localScale.x * 1.5f, transform.localScale.y * 1.5f, transform.localScale.z * 1.5f);
        
        if(Vector3.Distance(transform.localScale, desiredScale) >= 0.1f)
        {
            isExpanding = true;
        }
        else
        {
            isExpanding = false;
            mapSize++;
        }
    }
    private void Present()
    {
        gameObject.transform.parent.GetChild(0).gameObject.SetActive(true);
        Destroy(gameObject);
    }
}
