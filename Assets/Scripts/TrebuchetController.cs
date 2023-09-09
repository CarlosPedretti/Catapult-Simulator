using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrebuchetController : MonoBehaviour
{
    [SerializeField] private float weightMass;
    [SerializeField] private Rigidbody weightRb;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private GameObject slingArm;
    [SerializeField] private bool autoLaunch;

    private GameObject newProjectileReference;

    private bool trueTrigger;

    //[SerializeField] private Transform slingArm;
    [SerializeField] private Transform projectileSpawnPoint;

    private bool isCoroutineRunning = false;

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
    }

    private void LaunchProjectile()
    {   
        if (autoLaunch == true)
        {
            //ACA ESTA EL ERROR SOLUCIONALO BOLUDAZO!!!!! ES EL TRUE TRIGGER. LA NUEVA INSTANCIA ESTA BIEN, EL TEMA ES QUE SE DA LA CONDICION Y SE DESACTIVA EL HINGE AUTOMATICAMENTE.
            if (trueTrigger == true)
            {
                if (projectilePrefab != null)
                {
                    HingeJoint hingleToDestroy;
                    hingleToDestroy = newProjectileReference.GetComponent<HingeJoint>();

                    Destroy(hingleToDestroy);

                }
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                weightRb.isKinematic = false;

            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                weightRb.isKinematic = false;

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
                    float forceMagnitude = weightMass - (weightMass * 0.60f);
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

    public void SetAutoLaunch(bool value)
    {
        trueTrigger = value;
    }
}
