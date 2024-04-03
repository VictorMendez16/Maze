using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FillStatusBar : MonoBehaviour
{
    public Health playerHealth;
    public Image fillImage;
    private Slider slider;

    void Awake()
    {
        slider = GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        // Disable the slider when bar is at 0
        if (slider.value <= slider.minValue)
        {
            fillImage.enabled = false;
        }
        // Renable the slider wen the bar is greater than 0 and the slider is not enabled
        if (slider.value > slider.minValue && !fillImage.enabled)
        {
            fillImage.enabled = true;
        }
        float fillValue = playerHealth.currentHealth / playerHealth.maxHealth;

        // Changin color of the slider depending on health
        if (fillValue <= slider.maxValue / 3) // Critical condition
        {
            fillImage.color = Color.red;
        }
        else if (fillValue > (slider.maxValue / 3) && fillValue <= (slider.maxValue / 3) * 2) // Warning condition 
        {
            fillImage.color = Color.yellow;
        }
        else
        {
            fillImage.color = Color.green;
        }
        slider.value = fillValue;
    }
}
