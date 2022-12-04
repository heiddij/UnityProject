using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WandPickup : MonoBehaviour
{

    public void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerPrefs.SetInt("WandPickedUp", 1); // Kun wand poimitaan, asetetaan playerprefeihin avaimen WandPickedUp arvoksi 1. T�m� pysyy aina ykk�sen�,
        // vaikka peli lopetettais ja alotettais alusta. Eli wandin poiminta t�ytyy toteuttaa niin, ett� se ker�t��n just ennen seuraavaan leveliin siirtymist�,
        // peli tallennetaan levelin l�pip��syyn ja taikasauva pysyy pelaajalla. 
        gameObject.SetActive(false);
    }
}
