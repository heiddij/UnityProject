using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySystem : MonoBehaviour
{
    [Header("General Fields")]
    // List of items picked up
    public List<GameObject> items = new List<GameObject>();
    // Flag indicates if the inventory is open or not
    public bool isOpen;
    // Inventory System Window
    public GameObject uiWindow;
    public Image[] itemsImages;

    [Header("UI Item Description")]
    public GameObject uiDesceriptionWindow;
    public Image descritpionImage;
    public Text descriptionTitle;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            ToggleInventory();
        }
    }

    // Toggle inventory off/on
    void ToggleInventory()
    {
        isOpen = !isOpen;
        uiWindow.SetActive(isOpen);

        if (isOpen)
        {
            Debug.Log("Inventory is open");
        }
    }

    // Add the item to the items list
    public void PickUp(GameObject item)
    {
        items.Add(item); // Lis‰t‰‰n itemi items listaan
        UpdateUI(); 
    }

    // Refresh the UI elements in the Inventory Window
    private void UpdateUI()
    {
        // For each item in the "items" list
        // Show it in the respective slot in the "itemsImages"
        for (int i = 0; i < items.Count; i++)
        {
            itemsImages[i].sprite = items[i].GetComponent<SpriteRenderer>().sprite;
            itemsImages[i].gameObject.SetActive(true);
        }
    }

    //Hide all the items UI images
    void HideAll()
    {
        foreach(var i in itemsImages) { i.gameObject.SetActive(false); }
    }

    // Show ja hide itemin kuvaus, kun hiiri vied‰‰n itemin p‰‰lle. N‰it‰ metodeja kutsutaan unityn puolella:
    // jokaiselle itemille luotu Event Trigger -komponentti, jossa kutsutaan n‰it‰ metodeita
    public void ShowDescription(int id)
    {
        // Set the image
        descritpionImage.sprite = itemsImages[id].sprite;
        // Set the Text
        descriptionTitle.text = items[id].name; // Joko kuvaus tai pelkk‰ itemin  nimi
        // Show the window
        descritpionImage.gameObject.SetActive(true);
        descriptionTitle.gameObject.SetActive(true);
    }

    public void HideDescription()
    {
        descritpionImage.gameObject.SetActive(false);
        descriptionTitle.gameObject.SetActive(false);
    }
}