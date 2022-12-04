using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // Scoree ja lives tekstej� varten

public class GameSession : MonoBehaviour
{
    // HUOM!!! SIIRR� GAMESESSION-OBJEKTI MENU-SCENELLE, JOS HALUAT ETT� EL�M�T N�KYY JO T�SS�

    [SerializeField] public int playerLives = 3;
    [SerializeField] int score = 0;

    [SerializeField] Text livesText;
    [SerializeField] Text scoreText;
    [SerializeField] Sprite[] livesSprites;
    [SerializeField] Image livesImage;

    private void Awake()
    {
        // singleton pattern:
        int numGameSessions = FindObjectsOfType<GameSession>().Length; // Haetaan GameSession objektien lkm
        if (numGameSessions > 1) // Jos gamesession jo olemassa, niin tuhotaan kyseinen uusi gamesession ja pidet��n nykyinen (eli el�m�t ja scoret siirtyy scenelt� toiselle)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject); // Jos ainoastaan yksi olemassa (eli t�m� uusi) ei tuhota nykyist� gamesessionia
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        livesText.text = playerLives.ToString(); // playerin el�m�t stringiksi ja lives-tekstikentt��n
        scoreText.text = score.ToString();
        livesImage.sprite = livesSprites[playerLives - 1];
    }

    public void AddToScore(int pointsToAdd)
    {
        score += pointsToAdd;
        scoreText.text = score.ToString();
    }

    public void ProcessPlayerDeath() // When player dies, this method is called
    {
        if (playerLives > 1)
        {
            TakeLife(); // Jos el�mi� enemm�n kuin yksi, otetaan yksi el�m� pois
        }
        else
        {
            ResetGameSession(); // Jos el�mi� vain yksi j�ljell�, resetoidaan game session
        }
    }

    private void TakeLife()
    {
        playerLives--;
        var currentSceneIndex = SceneManager.GetActiveScene().buildIndex; // Haetaan nykyisen scenen index
        SceneManager.LoadScene(currentSceneIndex); // Ladataan nykyinen scene uudestaan
        livesText.text = playerLives.ToString(); // P�ivitet��n lives-tekstikentt�
        livesImage.sprite = livesSprites[playerLives - 1];
    }

    private void ResetGameSession()
    {
        SceneManager.LoadScene(0); // Ladataan eka scene
        Destroy(gameObject); // Tuhotaan nykyinen game session ja uusi luodaan uuden scenen latautuessa (el�m�t ja scoret nollaantuu)
    }

}
