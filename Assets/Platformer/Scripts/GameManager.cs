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
    public TextMeshProUGUI worldText;

    private int coins;
    private int score;
    
    // Start is called before the first frame update
    void Start()
    {
        worldText.text = $"World \n  1-1";
        CharacterController.OnBrickHit += BrickOnOnBrickHit;
        CharacterController.OnQuestionHit += QuestionOnOnQuestionHit;
    }

    private void OnDestroy()
    {
        CharacterController.OnBrickHit -= BrickOnOnBrickHit;
        CharacterController.OnQuestionHit -= QuestionOnOnQuestionHit;
    }

    void BrickOnOnBrickHit()
    {
        score += 100;
        scoreText.text = $"Score: \n {score}";
    }

    void QuestionOnOnQuestionHit()
    {
        coins++;
        coinText.text = $"x{coins}";

        score += 100;
        scoreText.text = $"Score: \n {score}";
    }

    // Update is called once per frame
    void Update()
    {
        int intTime = 100 - (int)Time.realtimeSinceStartup;
        string timeStr = $"Time \n {intTime}";
        timerText.text = timeStr;
        if (intTime == 0)
        {
            Debug.Log("Time Up");
        }

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
                score += 100;
                scoreText.text = $"Score: \n {score}";
                Destroy(hit.collider.gameObject);
            }
        }
    }

    void IncreaseCoins()
    {
        coins++;
        coinText.text = $"x{coins}";

        score += 200;
        scoreText.text = $"Score: \n {score}";
    }
}
