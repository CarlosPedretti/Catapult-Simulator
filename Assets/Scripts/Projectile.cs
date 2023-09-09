using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    private TrebuchetController trebuchetController;

    void Start()
    {
        trebuchetController = FindObjectOfType<TrebuchetController>();
    }

    private void OnTriggerEnter(Collider other)
    {

        if (trebuchetController != null)
        {
            trebuchetController.SetAutoLaunch(true);
        }
    }
}
