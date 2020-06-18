using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private int score;

    public void ChangeScore(int respect)
    {
        score += respect;
    }
}
