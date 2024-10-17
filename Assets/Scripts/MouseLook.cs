using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    [SerializeField] private float mouseSensitivity = 100f; 
    [SerializeField] private Transform playerBody;         

    private float xRotation = 0f;


    // Start is called before the first frame update
    void Start()
    {
        // Прячем курсор мыши
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        // Получаем ввод от мыши
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Ограничиваем вращение по оси X (вверх-вниз)
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // Вращаем камеру вверх-вниз
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // Вращаем тело игрока влево-вправо
        playerBody.Rotate(Vector3.up * mouseX);
    }
}
