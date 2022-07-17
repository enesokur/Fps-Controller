using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    [SerializeField]
    private float _sensitivity;
    [SerializeField]
    private GameObject lookY;
    private void Update() {
        MouseRotation();
    }

    private void MouseRotation(){
        float mouseX = Input.GetAxis("Mouse X");
        Vector3 rotation = this.transform.localEulerAngles;
        rotation.y += mouseX*_sensitivity;
        this.transform.localEulerAngles = rotation;

        float mouseY = Input.GetAxis("Mouse Y");
        Vector3 rotation2 = lookY.transform.localEulerAngles;
        rotation2.x -= mouseY*_sensitivity;
        lookY.transform.localEulerAngles = rotation2;
    }
}
