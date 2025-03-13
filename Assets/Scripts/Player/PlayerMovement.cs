using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private CharacterController controller;
    [SerializeField] private float baseSpeed = 5f;
    [SerializeField] private float baseJumpHeight = 2f;
    [SerializeField] private float baseGravity = -9.81f;

    [SerializeField] private float baseDashDistance = 10f;
    [SerializeField] private float baseDashCooldown = 2f;
    [SerializeField] private float baseDashTime = 0.1f;

    [Header("Влияние скорости на рывок")]
    [SerializeField] private float dashDistanceImpact = 0.2f;  // Влияние скорости на дальность рывка
    [SerializeField] private float dashCooldownImpact = 0.1f;  // Влияние скорости на откат рывка
    [SerializeField] private float dashTimeImpact = 0.1f;  // Влияние скорости на время рывка

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

        // Используем actualMoveSpeed вместо старого moveSpeed
        float currentSpeed = baseSpeed * playerStats.actualMoveSpeed;
        float currentJumpHeight = baseJumpHeight * (1 + (playerStats.actualMoveSpeed - 1) * 0.9f);
        float currentGravity = baseGravity * (1 + (playerStats.actualMoveSpeed - 1) * 1.1f);

        // Дальность рывка и его скорость восстановления теперь зависят от скорости передвижения
        float currentDashDistance = baseDashDistance * (1 + (playerStats.actualMoveSpeed - 1) * dashDistanceImpact);
        float currentDashCooldown = baseDashCooldown / (1 + (playerStats.actualMoveSpeed - 1) * dashCooldownImpact);
        float currentDashTime = baseDashTime / (1 + (playerStats.actualMoveSpeed - 1) * dashTimeImpact);

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
            StartCoroutine(Dash(move, currentDashDistance, currentDashTime, currentDashCooldown));
        }
    }

    private IEnumerator Dash(Vector3 moveDirection, float dashDistance, float dashTime, float dashCooldown)
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
