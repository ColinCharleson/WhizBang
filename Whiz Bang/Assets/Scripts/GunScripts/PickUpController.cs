using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PickUpController : MonoBehaviour
{
    public GunScript gunScript;
    public Rigidbody rb;
    public BoxCollider collider;
    public Transform player, gunPosition, camera;
    public TextMeshProUGUI ammunitionDisplay;

    public float pickUpRange;
    public float dropForwardForce, dropUpwardForce;

    public bool equipped;
    public static bool slotFull;


    private void Start()
    {
        //setup
        if(!equipped)
        {
            gunScript.enabled = false;
            rb.isKinematic = false;
            collider.isTrigger = false;
        }

        if (equipped)
        {
            gunScript.enabled = true;
            rb.isKinematic = true;
            collider.isTrigger = true;
            slotFull = true;
        }

    }

    private void Update()
    {
        Vector3 distanceToPlayer = player.position - transform.position;
        if(!equipped && distanceToPlayer.magnitude <= pickUpRange && Input.GetKeyDown(KeyCode.E) && !slotFull)
        {
            PickUp();
        }

        if (equipped && Input.GetKeyDown(KeyCode.Q))
        {
            Drop();
        }
    }

    private void PickUp()
    {
        equipped = true;
        slotFull = true;
        ammunitionDisplay.enabled = true;
        transform.SetParent(gunPosition);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.Euler(Vector3.zero);

        rb.isKinematic = true;
        collider.isTrigger = true;

        gunScript.enabled = true;
    }

    private void Drop()
    {
        equipped = false;
        slotFull = false;

        ammunitionDisplay.enabled = false;
        transform.SetParent(null);

        rb.isKinematic = false;
        collider.isTrigger = false;

        //Gun has momentum of player
        rb.velocity = player.GetComponent<Rigidbody>().velocity;

        //Force
        rb.AddForce(camera.forward * dropForwardForce, ForceMode.Impulse);
        rb.AddForce(camera.up * dropForwardForce, ForceMode.Impulse);

        //add randonom rotate

        float random = Random.Range(-1f, 1f);
        rb.AddTorque(new Vector3(random, random, random) * 10);
        
        gunScript.enabled = false;
    }

}
