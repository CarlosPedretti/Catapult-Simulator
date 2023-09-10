using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI velocityText;
    [SerializeField] private TextMeshProUGUI heightText;
    [SerializeField] private TextMeshProUGUI distanceText;

    public static GameManager Instance { get; private set; }


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

    }


    public void UpdateTexts(float currentHeight, float currentDistance, float velocity)
    {
        if (velocityText != null)
        {
            velocityText.text = "Velocidad: " + velocity.ToString("F2") + " m/s";
        }

        if (heightText != null)
        {
            heightText.text = "Altura: " + currentHeight.ToString("F2") + " m";
        }

        if (distanceText != null)
        {
            distanceText.text = "Distancia: " + currentDistance.ToString("F2") + " m";
        }
    }

}