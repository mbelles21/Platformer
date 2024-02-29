using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionAnimator : MonoBehaviour
{
    public int totalSections = 5;
    private int startSection = 0;
    
    public float updateInterval = 1.0f;
    private float timer = 0.0f;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        timer += Time.deltaTime;
        if (timer >= updateInterval)
        {
            timer = 0.0f;
            
            startSection = (startSection + 1) % totalSections;
            float normalizedOffset = (float)startSection / totalSections;
            GetComponent<Renderer>().material.mainTextureOffset = new Vector2(0, -normalizedOffset);
        }
    }
}
