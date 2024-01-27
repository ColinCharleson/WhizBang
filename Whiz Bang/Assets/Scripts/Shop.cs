using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    public int cost;

    private bool isExpanding;
    private Vector3 desiredScale;
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
        ScoreSystem.instance.UpdateScore(-cost);

        switch (selectedOption)
        {
            case Options.Zone:
                ZoneBubble();
                break;
            case Options.Gift:
                Present();
                break;
            default: break;
        }
    }

    private void Update()
    {
        if (isExpanding)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, desiredScale, Time.deltaTime);
        }
    }

    private void ZoneBubble()
    {
        cost *= 2;
        desiredScale = new Vector3(transform.localScale.x * 1.5f, transform.localScale.y * 1.5f, transform.localScale.z * 1.5f);
        
        if(transform.localScale != desiredScale)
        {
            isExpanding = true;
        }
        else
        {
            isExpanding = false;
        }
    }
    private void Present()
    {
        Destroy(gameObject);
    }
}
