using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Movement
    public float moveSpeed = 3f;
    float posX = 0;
    float posZ = 0;

    // Rotation
    public float rotateSpeedH = 60f;
    public float rotateSpeedV = 100f;
    float yaw = 0;
    float pitch = 0;
 
    void Update()
    {
        // Move
        posX = Input.GetAxisRaw("Horizontal") * Time.deltaTime * moveSpeed;
        posZ = Input.GetAxisRaw("Vertical") * Time.deltaTime * moveSpeed;
        transform.localPosition += new Vector3(posX, 0, posZ);
        
        // Rotate
        if (Input.GetMouseButton(1))
        {
            yaw += Input.GetAxisRaw("Mouse X") * Time.deltaTime * rotateSpeedH;
            pitch -= Input.GetAxis("Mouse Y") * Time.deltaTime * rotateSpeedV;
            transform.localEulerAngles = new Vector3(pitch, yaw, 0);
        }
    }
}
