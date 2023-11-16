using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TraversalButton : MonoBehaviour
{
    Image image;
    public Image enemyIcon;

    private void OnEnable()
    {
        GameColour.OnColourUpdated += UpdateColour;
    }

    private void OnDisable()
    {
        GameColour.OnColourUpdated -= UpdateColour;
    }

   
    // Update is called once per frame
    void UpdateColour(Color colour)
    {
        if (!image)
        {
            image = GetComponent<Image>();
        }
        image.color = colour;
        enemyIcon.color = colour;
    }

    public void ToggleEnemyIcon(bool toggle)
    {
        enemyIcon.gameObject.SetActive(toggle);
    }
}
