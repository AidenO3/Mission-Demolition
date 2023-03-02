using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slingshot : MonoBehaviour
{
    public int velocityMult = 10;
    [Header("Inscribed")]
    public GameObject projectilePrefab;

    [Header("Dynamic")]
    public GameObject launchPoint;
    public GameObject projectile;
    public bool aimingMode;

    private void Awake()
    {
        launchPoint = transform.Find("LaunchPoint").gameObject;
        launchPoint.SetActive(false);
    }

    void Start()
    {


    }

   
    void Update()
    {
        if (!aimingMode)
        {
            return;
        }

        Vector3 mousePos2D = Input.mousePosition;
        mousePos2D.z = -Camera.main.transform.position.z;
        Vector3 mousePos3D = Camera.main.ScreenToWorldPoint(mousePos2D);

        Vector3 mouseDelta = mousePos3D - launchPoint.transform.position;
        float maxMagnitude = this.GetComponent<SphereCollider>().radius;
        if (mouseDelta.magnitude > maxMagnitude)
        {
            mouseDelta.Normalize(); //set vector magnitude to 1
            mouseDelta *= maxMagnitude;
        }

        Vector3 projPos = launchPoint.transform.position + mouseDelta;
        projectile.transform.position = projPos;

        if (Input.GetMouseButtonUp(0)) {
            aimingMode = false;
            launchPoint.SetActive(false);
            Rigidbody projRb = projectile.GetComponent<Rigidbody>();
            projRb.isKinematic = false;
            projRb.velocity = -mouseDelta * velocityMult;
            FollowCam.POI = projectile;
            projectile = null;
        }
    }

    private void OnMouseEnter()
    {
        launchPoint.SetActive(true);

    }

    private void OnMouseExit()
    {
        launchPoint.SetActive(false);

    }

    private void OnMouseDown()
    {
        aimingMode = true;
        projectile = Instantiate<GameObject>(projectilePrefab);
        projectile.transform.position = launchPoint.transform.position;
        projectile.GetComponent<Rigidbody>().isKinematic = true;
        projectile.SetActive(true);
        print("marble created");
    }
}
