using UnityEngine;
using UnityEngine.UI;

public class SensitivitySlider : MonoBehaviour
{
    public Slider slider;

    void Start()
    {
        slider.value = PlayerPrefs.GetFloat("sensitivity", 2f);
        slider.onValueChanged.AddListener(SetSensitivity);
    }

    void SetSensitivity(float value)
    {
        PlayerPrefs.SetFloat("sensitivity", value);
        PlayerPrefs.Save();
    }
}