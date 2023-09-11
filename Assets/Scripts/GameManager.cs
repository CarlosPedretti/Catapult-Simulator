using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    [SerializeField] private TrebuchetController trebuchetController;

    [SerializeField] private TextMeshProUGUI velocityText;
    [SerializeField] private TextMeshProUGUI heightText;
    [SerializeField] private TextMeshProUGUI distanceText;
    [SerializeField] private TextMeshProUGUI rotationText;

    [SerializeField] private TMP_InputField weightMassInputField;


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

        weightMassInputField.onValueChanged.AddListener(UpdateWeightMass);

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

    public void UpdateRotationText(float rotation)
    {
        if (rotationText != null)
        {
            rotationText.text = "Rotation: " + rotation.ToString() + "°";
        }
    }

    private void UpdateWeightMass(string newValue)
    {
        if (float.TryParse(newValue, out float newWeightMass))
        {
            trebuchetController.weightMass = newWeightMass;
        }
        else
        {
            Debug.LogWarning("El valor ingresado no es válido.");
        }
    }

}