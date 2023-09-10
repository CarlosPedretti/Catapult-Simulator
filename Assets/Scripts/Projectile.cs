using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private TrebuchetController trebuchetController;
    private List<IProjectileInfoListener> infoListeners = new List<IProjectileInfoListener>();

    private float initialHeight;
    private Vector3 initialPosition;
    private float initialTime;
    private float velocity;

    void Start()
    {
        trebuchetController = FindObjectOfType<TrebuchetController>();

        initialHeight = transform.position.y;
        initialPosition = transform.position;
        initialTime = Time.time;

    }

    void Update()
    {
        // Calcular la altura actual
        float currentHeight = transform.position.y - initialHeight;

        // Calcular la distancia recorrida
        float currentDistance = Vector3.Distance(transform.position, initialPosition);

        // Calcular la velocidad actual
        velocity = currentDistance / (Time.time - initialTime);

        GameManager.Instance.UpdateTexts(currentHeight, currentDistance, velocity);


    }

    private void OnTriggerEnter(Collider other)
    {
        if (trebuchetController != null)
        {
            trebuchetController.SetLaunchTrigger = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (trebuchetController != null)
        {
            trebuchetController.SetLaunchTrigger = false;
        }
    }

    /* public void AddInfoListener(IProjectileInfoListener listener)
     {
         if (!infoListeners.Contains(listener))
         {
             infoListeners.Add(listener);
         }
     }

     public void RemoveInfoListener(IProjectileInfoListener listener)
     {
         if (infoListeners.Contains(listener))
         {
             infoListeners.Remove(listener);
         }
     }*/
}


