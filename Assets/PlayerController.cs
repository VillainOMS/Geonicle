using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float dashSpeed = 10f;
    public float jumpForce = 5f;
    public Transform weaponTransform;

    private CharacterController controller;
    private Vector3 velocity;
    private Vector3 moveDirection;
    private bool isDashing = false;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        Move();
        Shoot();
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            StartCoroutine(Dash());
        }
    }

    void Move()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        // ������������ ����������� ��������
        moveDirection = transform.right * moveX + transform.forward * moveZ;
        controller.Move(moveDirection * moveSpeed * Time.deltaTime);

        // ������
        if (Input.GetButtonDown("Jump") && controller.isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpForce * -2f * Physics.gravity.y);
        }

        // ��������� ������������ ��������
        velocity.y += Physics.gravity.y * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    IEnumerator Dash()
    {
        isDashing = true;
        float dashDuration = 0.2f; // ����������������� �����
        float startTime = Time.time;

        // ���������, ��������� �� �����
        if (moveDirection.magnitude > 0.1f)
        {
            // ������ ����� � ����������� �������� ��������
            while (Time.time < startTime + dashDuration)
            {
                controller.Move(moveDirection * dashSpeed * Time.deltaTime);
                yield return null;
            }
        }

        isDashing = false;
    }

    void Shoot()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            // ������ �������� (����� ������������ Raycast ��� Instantiate ��� ����)
        }
    }
}