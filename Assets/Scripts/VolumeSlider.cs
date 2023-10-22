using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    private Slider slider;
    private AudioSource mainAudo;
    // Start is called before the first frame update
    void Start()
    {
        slider = GetComponent<Slider>();
        GameManager gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        mainAudo= gameManager.GetComponent<AudioSource>();

        mainAudo.volume = slider.value;
        slider.onValueChanged.AddListener(ChangeVolume);
    }

    // Update is called once per frame
    void ChangeVolume(float value)
    {
        mainAudo.volume = value;
    }
}
