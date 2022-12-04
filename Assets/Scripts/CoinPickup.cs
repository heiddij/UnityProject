using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPickup : MonoBehaviour
{

    [SerializeField] AudioClip coinPickUpSFX; // Soundeffect coinin poiminnalle
    [SerializeField] int coinPoints = 1; // Pisteet, jotka saa kolikon keruusta

    bool wasCollected = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && !wasCollected)
        {
            wasCollected = true;
            FindObjectOfType<GameSession>().AddToScore(coinPoints); // Kutsutaan gamesessionin AddToScore -metodia ja annetaan parametrin‰ coinpoints (AddToScore lis‰‰ sen scoreen)
            AudioSource.PlayClipAtPoint(coinPickUpSFX, Camera.main.transform.position); // coinSFX soi KAMERAN positiossa, koska kolikko tuhoutuu, joten ei voi soida sen kohdalla
                                                                                        // K‰yt‰ siis t‰t‰ kaikissa SFX:ssa joissa objekti tuhoutuu
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }

}
