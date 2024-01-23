
using UnityEngine;

public class GunSystem : MonoBehaviour
{
    //Gun Stats
    public int damage;
    public float timeBetweenShooting, spread, range, realodTime, timeBetweenShots;
    public int magazineSize, burst;
    public bool fullAuto;
    int bulletsLeft, bulletsShot;

    //bools
    bool shooting, readyToShoot, reloading;

    //Refrence

    public Camera cam;
    public Transform attackPoint;
    public RaycastHit rayHit;
    public LayerMask whatIsEnemy;


    private void MyInput()
    {
 /*       if (allowButtonHold) shooting = Input.GetKey(KeyCode.Mouse(0);

        else shooting = Input.GetKeyDown(KeyCode.Mouse0);

        if(Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize

        */
    }

}
