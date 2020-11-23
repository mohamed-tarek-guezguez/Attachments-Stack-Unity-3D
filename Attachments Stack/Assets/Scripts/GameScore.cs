using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameScore : MonoBehaviour
{
    public static GameScore Instance;

    [SerializeField] private Text scoreTXT = default;

    private int scorePoints;

    void Awake()
    {
        if (!Instance)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    public void AddScore(int points)
    {
        scorePoints += points;
        scoreTXT.text = scorePoints.ToString();
    }

    public void ResetScore()
    {
        scorePoints = 0;
        scoreTXT.text = "0";
    }
}
