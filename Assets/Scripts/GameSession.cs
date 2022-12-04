using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // Scoree ja lives tekstejä varten

public class GameSession : MonoBehaviour
{
    // HUOM!!! SIIRRÄ GAMESESSION-OBJEKTI MENU-SCENELLE, JOS HALUAT ETTÄ ELÄMÄT NÄKYY JO TÄSSÄ

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
        if (numGameSessions > 1) // Jos gamesession jo olemassa, niin tuhotaan kyseinen uusi gamesession ja pidetään nykyinen (eli elämät ja scoret siirtyy sceneltä toiselle)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject); // Jos ainoastaan yksi olemassa (eli tämä uusi) ei tuhota nykyistä gamesessionia
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        livesText.text = playerLives.ToString(); // playerin elämät stringiksi ja lives-tekstikenttään
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
            TakeLife(); // Jos elämiä enemmän kuin yksi, otetaan yksi elämä pois
        }
        else
        {
            ResetGameSession(); // Jos elämiä vain yksi jäljellä, resetoidaan game session
        }
    }

    private void TakeLife()
    {
        playerLives--;
        var currentSceneIndex = SceneManager.GetActiveScene().buildIndex; // Haetaan nykyisen scenen index
        SceneManager.LoadScene(currentSceneIndex); // Ladataan nykyinen scene uudestaan
        livesText.text = playerLives.ToString(); // Päivitetään lives-tekstikenttä
        livesImage.sprite = livesSprites[playerLives - 1];
    }

    private void ResetGameSession()
    {
        SceneManager.LoadScene(0); // Ladataan eka scene
        Destroy(gameObject); // Tuhotaan nykyinen game session ja uusi luodaan uuden scenen latautuessa (elämät ja scoret nollaantuu)
    }

}
