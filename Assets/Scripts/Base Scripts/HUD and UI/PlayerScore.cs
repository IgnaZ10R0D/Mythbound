using UnityEngine;

public class PlayerScore : MonoBehaviour
{
    public int score = 0;

    public void AddPoints(int scoreToAdd)
    {
        score += scoreToAdd;
    }

    public int GetScore()
    {
        return score;
    }
}

