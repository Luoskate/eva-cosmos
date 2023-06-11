using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnchorsPopUp : MonoBehaviour {
    public Transform cameraTransform; // Référence au transform de la caméra
    public Vector3 offset; // Décalage par rapport à la position de la caméra

    // Update est appelée une fois par frame
    void Update() {
        if (cameraTransform != null) {
            // Faire en sorte que la position et la rotation de l'UI suivent celle de la caméra avec un décalage
            transform.position = cameraTransform.position + offset;
            transform.rotation = cameraTransform.rotation;
        }
    }
}
