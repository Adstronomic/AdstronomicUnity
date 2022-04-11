using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject myCamera;
    public Adstronomic myAdstronomic;

    private Rigidbody myRigidbody;

    void Start()
    {
        this.myAdstronomic = myCamera.GetComponent<Adstronomic>();
        this.myRigidbody = gameObject.GetComponent<Rigidbody>();

        this.myAdstronomic.initialize("n7OBnmYgYWJzrQwgFnQv");
    }

    void Update()
    {
        this.myRigidbody.AddRelativeForce(Vector3.forward * Input.GetAxis("Vertical") * 4096 * Time.deltaTime);

        gameObject.transform.Rotate(0, Input.GetAxis("Horizontal") * 256 * Time.deltaTime, 0);
        gameObject.transform.eulerAngles = new Vector3(0, gameObject.transform.rotation.eulerAngles.y, 0);

        this.myCamera.transform.position = gameObject.transform.position + gameObject.transform.forward * - 4 + new Vector3(0, 1, 0);
        this.myCamera.transform.eulerAngles = new Vector3(11.25f, gameObject.transform.rotation.eulerAngles.y, 0);
    }

    void OnCollisionEnter(Collision myCollision) {
        if(myCollision.transform.tag == "Banner Tag") StartCoroutine(myAdstronomic.loadBanner());
        else if(myCollision.transform.tag == "Interstitial Tag") StartCoroutine(myAdstronomic.loadInterstitial(this.myCamera));
        else if(myCollision.transform.tag == "Rewarded Tag") StartCoroutine(myAdstronomic.loadRewarded(this.myCamera));
    }
}
