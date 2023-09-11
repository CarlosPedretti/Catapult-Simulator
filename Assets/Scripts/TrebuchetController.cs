using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TrebuchetController : MonoBehaviour
{
    [SerializeField] public float weightMass;
    [SerializeField] private float projectileDestroyDelay;
    [SerializeField] private Rigidbody weightRb;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private GameObject slingArm;
    [SerializeField] private bool autoLaunch;


    private GameObject newProjectileReference;

    private bool trueTrigger;
    private bool thereIsAmmo;

    [SerializeField] private Transform projectileSpawnPoint;

    private bool isCoroutineRunning = false;

    [SerializeField] private Toggle autoLaunchToggle;
    [SerializeField] private Slider rotationSlider;


    void Start()
    {
        weightRb.mass = weightMass;
        thereIsAmmo = false;
    }

    void Update()
    {
        LaunchProjectile();
        Positioning();
        AddProjectile();
        UserSliderRotation();

        autoLaunchToggle.isOn = autoLaunch;
        weightRb.mass = weightMass;
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

                thereIsAmmo = false;


            }
            if (Input.GetKeyDown(KeyCode.Space))
            {

                weightRb.isKinematic = false;
                Destroy(newProjectileReference, projectileDestroyDelay);

                thereIsAmmo = false;
            }
        }
        else
        {

            if (Input.GetKeyDown(KeyCode.Space))
            {
                weightRb.isKinematic = false;

                Destroy(newProjectileReference, projectileDestroyDelay);

                thereIsAmmo = false;

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
                    float forceMagnitude = weightMass - (weightMass * 0.18f);
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

      if (Input.GetKeyDown(KeyCode.R) && weightRb.isKinematic == true && thereIsAmmo == false)
      {

        if (projectilePrefab != null && projectileSpawnPoint != null)
        {
            GameObject newProjectile = Instantiate(projectilePrefab, projectileSpawnPoint.position, projectileSpawnPoint.rotation);

            thereIsAmmo = true;

            newProjectileReference = newProjectile;

            Rigidbody newProjectileRb = newProjectile.GetComponent<Rigidbody>();
            if (newProjectileRb != null)
            {

            }

            HingeJoint newProjectileHinge = newProjectile.GetComponent<HingeJoint>();
            if (newProjectileHinge != null && slingArm != null)
            {

                newProjectileHinge.connectedBody = slingArm.GetComponent<Rigidbody>();

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

        GameManager.Instance.UpdateRotationText(rotationValue);
    }

    public Vector3 GetLaunchPoint()
    {
        if (projectileSpawnPoint != null)
        {
            return projectileSpawnPoint.position;
        }
        else
        {
            return transform.position;
        }
    }

}
