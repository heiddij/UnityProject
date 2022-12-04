using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{

    [SerializeField] float LevelLoadDelay = 2f;
    [SerializeField] float LevelExitSlowMoFactor = 0.2f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        StartCoroutine(LoadNextLevel()); // Kun osuu oveen (prefab), ladataan seuraava leveli
    }

    IEnumerator LoadNextLevel()
    {
        Time.timeScale = LevelExitSlowMoFactor; // SlowMotion kun pelaaja menee exitille
        yield return new WaitForSecondsRealtime(LevelLoadDelay); // Odottaa 2s ajan, jonka jälkeen menee eteenpäin
        Time.timeScale = 1f; // Normaali nopeus

        var currentSceneIndex = SceneManager.GetActiveScene().buildIndex; // Haetaan nykyisen levelin index (muista lisätä scene/level build settingsissä!!)
        SceneManager.LoadScene(currentSceneIndex + 1); // Lataa seuraavan scenen (index+1)
    }
}
