using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WandPickup : MonoBehaviour
{

    public void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerPrefs.SetInt("WandPickedUp", 1); // Kun wand poimitaan, asetetaan playerprefeihin avaimen WandPickedUp arvoksi 1. Tämä pysyy aina ykkösenä,
        // vaikka peli lopetettais ja alotettais alusta. Eli wandin poiminta täytyy toteuttaa niin, että se kerätään just ennen seuraavaan leveliin siirtymistä,
        // peli tallennetaan levelin läpipääsyyn ja taikasauva pysyy pelaajalla. 
        gameObject.SetActive(false);
    }
}
