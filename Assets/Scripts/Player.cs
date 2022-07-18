using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private CharacterController _controller;
    [SerializeField]
    private float _speed;
    private float _gravity = 9.81f;
    private float downWardForce;
    [SerializeField]
    private ParticleSystem _particle;
    [SerializeField]
    private float _shootRate = 0f;
    private float _shootCoolDown = 0f;
    [SerializeField]
    private GameObject _hiteffect;
    [SerializeField]
    private AudioClip _shotSound;
    private AudioSource _audioSource;
    [SerializeField]
    private int _maxAmmo;
    [SerializeField]
    private int _currentAmmo;
    private bool _isReloading;
    private UIHandler _uiHandler;


    private void Start() {
        _controller = this.GetComponent<CharacterController>();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        _audioSource = this.GetComponent<AudioSource>();
        _currentAmmo = _maxAmmo;
        _uiHandler = GameObject.Find("Canvas").GetComponent<UIHandler>();
    }

    private void Update() {
        CalculateMovement();
        Shoot();
        if(Input.GetKeyDown(KeyCode.Escape)){
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        if(Input.GetKeyDown(KeyCode.R) && _isReloading == false){
            StartCoroutine(ReloadRoutine());
            _isReloading = true;
        }
    }

    private void CalculateMovement(){
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        if(_controller.isGrounded){
            downWardForce = -_gravity*Time.deltaTime;
        }
        else{
            downWardForce -= _gravity*Time.deltaTime;
        }
        if(Input.GetKeyDown(KeyCode.Space)){
            downWardForce = 5f;
        }
        Vector3 velocity = new Vector3(horizontalInput*_speed,downWardForce,verticalInput*_speed);
        velocity = this.transform.TransformDirection(velocity);
        _controller.Move(velocity*Time.deltaTime);
    }

    private void Shoot(){
        if(Time.time > _shootCoolDown){
            if(Input.GetMouseButton(0) && _currentAmmo > 0 && _isReloading == false){
                _particle.Play();
                _audioSource.Play();
                _shootCoolDown = Time.time + _shootRate;
                _currentAmmo--;
                _uiHandler.UpdateAmmo(_currentAmmo);
                //Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width/2f,Screen.height/2f,0f));
                Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f,0.5f,0f));
                RaycastHit hitInfo;
                if(Physics.Raycast(ray,out hitInfo,Mathf.Infinity)){
                    GameObject hitEffectObject = Instantiate(_hiteffect,hitInfo.point,Quaternion.LookRotation(hitInfo.normal));
                    hitEffectObject.transform.localScale = new Vector3(1.5f,1.5f,1.5f);
                    hitEffectObject.GetComponent<ParticleSystem>().Play();
                    Debug.Log(hitInfo.transform.name);
                    Destroy(hitEffectObject.gameObject,1f);
                    Destructible crateScript = hitInfo.transform.GetComponent<Destructible>();
                    if(crateScript != null){
                        crateScript.DestructCrate();
                    }
                }
            }
        }
    }

    IEnumerator ReloadRoutine(){
        yield return new WaitForSeconds(1.5f);
        _currentAmmo = _maxAmmo;
        _uiHandler.UpdateAmmo(_currentAmmo);
        _isReloading = false;
    }
}
