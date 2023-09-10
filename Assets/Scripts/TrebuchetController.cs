using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TrebuchetController : MonoBehaviour
{
    [SerializeField] private float weightMass;
    [SerializeField] private float projectileDestroyDelay;
    [SerializeField] private Rigidbody weightRb;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private GameObject slingArm;
    [SerializeField] private bool autoLaunch;

    private GameObject newProjectileReference;

    private bool trueTrigger;

    [SerializeField] private Transform projectileSpawnPoint;

    private bool isCoroutineRunning = false;

    [SerializeField] private Toggle autoLaunchToggle;
    [SerializeField] private Slider rotationSlider;



    //[SerializeField] private AnimationCurve curve;

    void Start()
    {
        weightRb.mass = weightMass;

    }

    void Update()
    {
        LaunchProjectile();
        Positioning();
        AddProjectile();
        UserSliderRotation();

        autoLaunchToggle.isOn = autoLaunch;
    }

    private void LaunchProjectile()
    {   
        if (autoLaunch == true)
        {

            if (trueTrigger == true)
            {

                HingeJoint hingleToDestroy;
                hingleToDestroy = newProjectileReference.GetComponent<HingeJoint>();

                Destroy(hingleToDestroy);

                
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {

                weightRb.isKinematic = false;
                Destroy(newProjectileReference, projectileDestroyDelay);

            }
        }
        else
        {

            if (Input.GetKeyDown(KeyCode.Space))
            {
                weightRb.isKinematic = false;

                Destroy(newProjectileReference, projectileDestroyDelay);

            }

            if (Input.GetKeyUp(KeyCode.Space))
            {
                HingeJoint hingleToDestroy;
                hingleToDestroy = newProjectileReference.GetComponent<HingeJoint>();

                Destroy(hingleToDestroy);
            }
        }

    }

    private void Positioning()
    {
        if (Input.GetKeyUp(KeyCode.E) && !isCoroutineRunning)
        {
            if (slingArm != null)
            {
                Rigidbody slingArmRB = slingArm.GetComponent<Rigidbody>();
                if (slingArmRB != null)
                {
                    if (weightRb.isKinematic == false)
                    {
                        StartCoroutine(ApplyDownwardForceForTime(5.0f));

                    }
                }
            }
        }

        IEnumerator ApplyDownwardForceForTime(float duration)
        {
            isCoroutineRunning = true;

            if (slingArm != null)
            {
                Rigidbody slingArmRB = slingArm.GetComponent<Rigidbody>();
                if (slingArmRB != null)
                {
                    float forceMagnitude = weightMass - (weightMass * 0.45f);
                    Vector3 downwardForce = -slingArm.transform.up * forceMagnitude;

                    float elapsedTime = 0.0f;

                    while (elapsedTime < duration)
                    {
                        slingArmRB.AddRelativeForce(downwardForce);
                        elapsedTime += Time.deltaTime;
                        yield return null;
                    }
                }
            }

            if (weightRb.isKinematic == false)
            {
              weightRb.isKinematic = true;
            }

            isCoroutineRunning = false;
        }

    }

    private void AddProjectile()
    {

      if (Input.GetKeyDown(KeyCode.R) && weightRb.isKinematic == true)
      {

        if (projectilePrefab != null && projectileSpawnPoint != null)
        {
            // Clona el prefab del proyectil y colócalo en la posición de projectileSpawnPoint.
            GameObject newProjectile = Instantiate(projectilePrefab, projectileSpawnPoint.position, projectileSpawnPoint.rotation);

            newProjectileReference = newProjectile;

            // Asegúrate de que el nuevo proyectil tenga un Rigidbody.
            Rigidbody newProjectileRb = newProjectile.GetComponent<Rigidbody>();
            if (newProjectileRb != null)
            {
                // Configura cualquier otra propiedad del Rigidbody si es necesario.
            }

            // Asegúrate de que el nuevo proyectil tenga un HingeJoint.
            HingeJoint newProjectileHinge = newProjectile.GetComponent<HingeJoint>();
            if (newProjectileHinge != null && slingArm != null)
            {
                // Configura el punto de anclaje y el punto de conexión al slingArm's Rigidbody.
                newProjectileHinge.connectedBody = slingArm.GetComponent<Rigidbody>();
                //newProjectileHinge.anchor = Vector3.zero; // Punto de anclaje local en el proyectil.
                //newProjectileHinge.connectedAnchor = Vector3.zero; // Punto de anclaje local en el slingArm.
            }
        }
      }

    }

    public bool SetLaunchTrigger
    {
        get
        {
            return trueTrigger;
        }
        set
        {
            trueTrigger = value;
        }
    }

    public bool SetAutoLaunch
    {
        get
        {
            return autoLaunch;
        }
        set
        {
            autoLaunch = value;
        }
    }

    public void UserToggle(bool newValue)
    {
        autoLaunch = newValue;
    }

    public void UserSliderRotation()
    {
        float rotationValue = rotationSlider.value;
        transform.rotation = Quaternion.Euler(0f, rotationValue, 0f);
    }

    public Vector3 GetLaunchPoint()
    {
        if (projectileSpawnPoint != null)
        {
            return projectileSpawnPoint.position;
        }
        else
        {
            // Si el punto de lanzamiento no está configurado, devuelve la posición del objeto TrebuchetController.
            return transform.position;
        }
    }

}
