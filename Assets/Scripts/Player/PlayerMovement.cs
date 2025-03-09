using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private CharacterController controller;
    [SerializeField] private float baseSpeed = 5f;
    [SerializeField] private float baseJumpHeight = 2f;
    [SerializeField] private float baseGravity = -9.81f;

    [SerializeField] private float dashDistance = 10f;
    [SerializeField] private float dashCooldown = 2f;
    [SerializeField] private float dashTime = 0.1f;

    private Vector3 velocity;
    private bool isGrounded;
    private bool isDashing = false;
    private bool canDash = true;

    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundDistance = 0.4f;
    [SerializeField] private LayerMask groundMask;

    private PlayerStats playerStats;

    private void Start()
    {
        playerStats = FindObjectOfType<PlayerStats>();
        if (playerStats == null)
        {
            Debug.LogError("PlayerStats не найден!");
        }
    }

    private void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        float currentSpeed = baseSpeed * playerStats.moveSpeed; // Скорость зависит от moveSpeed игрока
        float currentJumpHeight = baseJumpHeight * (playerStats.moveSpeed * 0.9f); // Умеренное влияние moveSpeed на прыжки
        float currentGravity = baseGravity * (playerStats.moveSpeed * 1.1f); // Немного быстрее падает, если скорость выше

        if (!isDashing)
        {
            controller.Move(move * currentSpeed * Time.deltaTime);
        }

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            AudioManager.Instance.PlayJumpSound();
            velocity.y = Mathf.Sqrt(currentJumpHeight * -2f * currentGravity);
        }

        velocity.y += currentGravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            AudioManager.Instance.PlayDashSound();
            StartCoroutine(Dash(move, currentSpeed));
        }
    }

    private IEnumerator Dash(Vector3 moveDirection, float currentSpeed)
    {
        isDashing = true;
        canDash = false;

        velocity.y = 0;

        Vector3 dashDirection = moveDirection.normalized;

        float dashEndTime = Time.time + dashTime;

        while (Time.time < dashEndTime)
        {
            controller.Move(dashDirection * (dashDistance / dashTime) * Time.deltaTime);
            yield return null;
        }

        isDashing = false;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }
}
