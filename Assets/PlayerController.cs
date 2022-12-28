using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float rotationRate = 90f;
    public Transform cursor;
    public Vector3Int targetBlock;
    public Vector3Int placeBlock;
    Vector3 controlRotation = Vector3.zero;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        float pitch = Input.GetAxisRaw("Mouse Y");
        float yaw = Input.GetAxisRaw("Mouse X");
        controlRotation.y += yaw * rotationRate * Time.deltaTime;
        controlRotation.x = Mathf.Clamp(controlRotation.x - pitch * rotationRate * Time.deltaTime, -89f, 89f);
        Camera.main.transform.rotation = Quaternion.Euler(controlRotation);
        

        float xInput = Input.GetAxisRaw("Horizontal");
        float yInput = Input.GetAxisRaw("Vertical");
        Vector3 moveDirection = Camera.main.transform.forward * yInput + Camera.main.transform.right * xInput;
        moveDirection.Normalize();
        Camera.main.transform.position += moveDirection * moveSpeed * Time.deltaTime;

        RaycastHit hit;
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        if (Physics.Raycast(ray, out hit, 15f)){
            Vector3 point = hit.point - (hit.normal * 0.1f);
            targetBlock = new Vector3Int(
                Mathf.RoundToInt(point.x),
                Mathf.RoundToInt(point.y),
                Mathf.RoundToInt(point.z)
            );
            cursor.position = targetBlock;
            Vector3 p = targetBlock + hit.normal;
            placeBlock = new Vector3Int(
                Mathf.RoundToInt(p.x),
                Mathf.RoundToInt(p.y),
                Mathf.RoundToInt(p.z)
            );
        }
        

        if (Input.GetMouseButtonDown(0)) Chunk.SetVoxelType(placeBlock.x, placeBlock.y, placeBlock.z, Voxel.Type.Grass);
    }
}
