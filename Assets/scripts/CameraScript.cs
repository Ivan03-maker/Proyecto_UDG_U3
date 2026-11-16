using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public Transform target; // Arrastra aquí a John
    public float smoothSpeed = 5f; // Velocidad de seguimiento fluido
    public Vector3 offset = new Vector3(0f, 0f, -10f); // Distancia en Z para que la cámara no tape a John

    void LateUpdate()
    {
        if (target != null)
        {
            // Posición deseada siguiendo a John en X e Y, manteniendo la Z
            Vector3 desiredPosition = new Vector3(target.position.x, target.position.y, offset.z);
            
            // Mover la cámara suavemente hacia la posición deseada (seguimiento fluido)
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
            
            transform.position = smoothedPosition;
        }
    }
}