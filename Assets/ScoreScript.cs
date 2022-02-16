using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreScript : MonoBehaviour
{
    public void UpdateScore(int score, int total) => GetComponent<TMP_Text>().text = $"Score: {score} / {total}";
}
