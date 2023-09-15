using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameStateManager : MonoBehaviour
{
    public int playerHealth = 1;
    public List<Image> lifeImages;
    public int score;
    public TextMeshProUGUI textMeshProText;
    public GameObject gameOverCanvas;

    // Start is called before the first frame update
    void Start()
    {
        score = 0;
        textMeshProText.text = "Score: " + score;
    }

    void OnEnable()
    {
        GunnerFSM.OnEnemyKilled += OnIncreaseScore;
    }

    void OnDisable()
    {
        GunnerFSM.OnEnemyKilled -= OnIncreaseScore;
    }

    public void OnPlayerDamaged()
    {
        playerHealth -= 1;
        lifeImages[7 - playerHealth].enabled = false;
        Debug.Log("Player Damaged");

        if (playerHealth <= 0)
        {
            // Game Over
            OnGameOver();
        }
    }

    public void OnIncreaseScore()
    {
        score += 100;
        textMeshProText.text = "Score: " + score;
    }

    public void OnGameOver()
    {
        Time.timeScale = 1 / 60f;
        gameOverCanvas.SetActive(true);
    }
}
