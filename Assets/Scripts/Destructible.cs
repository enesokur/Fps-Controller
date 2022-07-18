using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructible : MonoBehaviour
{
    [SerializeField]
    private GameObject _crateBreakable;
    [SerializeField]
    private float _explosionForce;
    public void DestructCrate(){
        Instantiate(_crateBreakable,this.transform.position,transform.rotation);
        Collider[] colliders = Physics.OverlapSphere(this.transform.position,20f);
        foreach(Collider singlecollider in colliders){
            Rigidbody rb = singlecollider.transform.gameObject.GetComponent<Rigidbody>();
            if(rb != null){
                rb.AddExplosionForce(_explosionForce,this.transform.position,20f);
            }
        }
        Destroy(this.gameObject);
    }
}
