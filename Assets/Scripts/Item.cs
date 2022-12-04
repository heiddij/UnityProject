using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider2D))]
public class Item : MonoBehaviour
{

    // Collider Trigger
    // Interaction type

    public enum InteractionType { NONE, PickUp, Examine } // An enumeration type (enum type) is a value type defined by a set of named constants of the underlying integral numeric type
    public InteractionType type;
    public UnityEvent customEvent;

    [SerializeField] public string descriptionText;

    private void Reset() // Asettaa default valuen komponenttiin
    {
        GetComponent<BoxCollider2D>().isTrigger = true; // Asetetaan trigger trueksi oletuksena
        gameObject.layer = 15;
    }

    public void Interact()
    {
        switch(type)
        {
            case InteractionType.PickUp:
                //Add the item to the pickUpItems -list
                FindObjectOfType<InventorySystem>().PickUp(gameObject);
                //Disable the item
                gameObject.SetActive(false);
                Debug.Log("PICKUP");
                break;
            case InteractionType.Examine: // Tää video jätettiin väliin, tee myöhemmin jos tarvii
                Debug.Log("EXAMINE");
                break;
            default:
                Debug.Log("NULL ITEM");
                break;
        }

        // Invoke (call) the custom event(s)
        // Eli nyt inspectorissa Item-scriptin alla Custom events -lista, johon voi lisätä TAPAHTUMIA, mitä tapahtuu, kun
        // item-scriptin omaavan jutun kanssa ollaan tekemisissä (tähän vois esim. laittaa taikasauvan saamisen tai 
        // keskusteluikkunan ilmestymisen tms tms...
        customEvent.Invoke();
    }
}
