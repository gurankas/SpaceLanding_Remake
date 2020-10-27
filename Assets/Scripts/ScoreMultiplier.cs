using UnityEngine;

public class ScoreMultiplier : MonoBehaviour
{
    public int ScoreMul;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<BaseCharacter>() != null)
        {
            other.GetComponent<BaseCharacter>().ScoreMultiplier = ScoreMul;
        }
    }
}
