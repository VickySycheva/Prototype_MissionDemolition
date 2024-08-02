using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class Slingshot : MonoBehaviour
{
    static private Slingshot S;

    public GameObject prefabProjectile;
    public GameObject launchPoint;
    public GameObject projectile;

    public Vector3 launchPos;
    public bool aimingMode;
    public float velocityMult = 8f;

    private Rigidbody projectileRb;

    static public Vector3 LAUNCH_POS
    {
        get 
        {
            if (S == null ) return Vector3.zero;
            return S.launchPos;
        }
    }

    void Awake()
    {
        S = this;
        Transform launchPointTrans = transform.Find("LaunchPoint");
        launchPoint = launchPointTrans.gameObject;
        launchPoint.SetActive(false);
        launchPos = launchPointTrans.position;
    }
    void OnMouseEnter()
    {
        launchPoint.SetActive(true);
    }
    void OnMouseExit()
    {
        launchPoint.SetActive(false);
    }

    void OnMouseDown()
    {
        aimingMode = true;
        projectile = Instantiate(prefabProjectile) as GameObject;
        projectile.transform.position = launchPos;
        projectileRb = projectile.GetComponent<Rigidbody>();
        projectileRb.isKinematic = true;
    }

    void Update()
    {
        if(!aimingMode) return;
        
        Vector3 mousePos2D = Input.mousePosition;
        mousePos2D.z = -Camera.main.transform.position.z;
        Vector3 mousePos3D = Camera.main.ScreenToWorldPoint(mousePos2D);

        Vector3 mouseDelta = mousePos3D-launchPos;
        float maxMagnitude = this.GetComponent<SphereCollider>().radius;

        if(mouseDelta.magnitude > maxMagnitude)
        {
            mouseDelta.Normalize();
            mouseDelta *= maxMagnitude;
        }

        Vector3 projPos = launchPos + mouseDelta;
        projectile.transform.position = projPos;

        if(Input.GetMouseButtonUp(0))
        {
            aimingMode = false;
            projectileRb.isKinematic = false;
            projectileRb.velocity = -mouseDelta * velocityMult;
            FollowCam.POI = projectile;
            projectile = null;
            MissionDemotion.ShotFired();
            ProjectileLine.S.poi = projectile;
        }
    }
}
