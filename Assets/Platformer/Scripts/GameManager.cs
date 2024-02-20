using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI timerText;

    public TextMeshProUGUI coinText;

    public TextMeshProUGUI scoreText;

    private int coins;

    private int score;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        int intTime = 400 - (int)Time.realtimeSinceStartup;
        string timeStr = $"Time \n {intTime}";
        timerText.text = timeStr;

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit) && hit.collider.CompareTag("question"))
            {
                // Increase coins
                IncreaseCoins();
                AudioSource src = hit.collider.gameObject.GetComponent<AudioSource>();
                src.Play();
            }
            else if (Physics.Raycast(ray, out hit) && hit.collider.CompareTag("brick"))
            {
                // destroy brick and play sound
                AudioSource src = hit.collider.gameObject.GetComponent<AudioSource>();
                src.Play();
                Destroy(hit.collider.gameObject);
            }
        }
    }

    void IncreaseCoins()
    {
        coins++;
        coinText.text = coins.ToString();

        score += 200;
        scoreText.text = score.ToString();
    }
}
