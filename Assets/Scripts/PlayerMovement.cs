using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private CharacterController controller;
    [SerializeField] private float speed = 12f;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float jumpHeight = 3f;

    [SerializeField] private float dashDistance = 10f;  // ������������� ��������� �����
    [SerializeField] private float dashCooldown = 2f;   // ����� ������ �����
    [SerializeField] private float dashTime = 0.1f;     // �����, �� ������� ���������� �����

    private Vector3 velocity;
    private bool isGrounded;
    private bool isDashing = false;
    private bool canDash = true;

    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundDistance = 0.4f;
    [SerializeField] private LayerMask groundMask;

    private void Update()
    {
        // ��������, ��������� �� �������� �� �����
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        // ���� ��� ��������
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        // ���������� ���������� �������� ��� �����
        if (!isDashing)
        {
            controller.Move(move * speed * Time.deltaTime);
        }

        // ������
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // ����������
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        // �����
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(Dash(move));
        }
    }

    // ������� ��� �����
    private IEnumerator Dash(Vector3 moveDirection)
    {
        isDashing = true;
        canDash = false;

        // �������� ������������ �������� ����� ������
        velocity.y = 0;

        // ����������� ������ ��������, ����� ������ ������������� ���������� �����
        Vector3 dashDirection = moveDirection.normalized;

        // ��������� ������ ��������� �����
        float dashEndTime = Time.time + dashTime;

        while (Time.time < dashEndTime)
        {
            // ���������� ������ �� ������������� ���������� �� �������� �����
            controller.Move(dashDirection * (dashDistance / dashTime) * Time.deltaTime);
            yield return null;
        }

        isDashing = false;

        // �������� ������ �����
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }
}
