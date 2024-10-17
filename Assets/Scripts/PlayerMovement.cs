using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private CharacterController controller;
    [SerializeField] private float speed = 12f;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float jumpHeight = 3f;

    [SerializeField] private float dashDistance = 10f;  // Фиксированная дальность рывка
    [SerializeField] private float dashCooldown = 2f;   // Время отката рывка
    [SerializeField] private float dashTime = 0.1f;     // Время, за которое происходит рывок

    private Vector3 velocity;
    private bool isGrounded;
    private bool isDashing = false;
    private bool canDash = true;

    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundDistance = 0.4f;
    [SerializeField] private LayerMask groundMask;

    private void Update()
    {
        // Проверка, находится ли персонаж на земле
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        // Ввод для движения
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        // Применение нормальной скорости или рывка
        if (!isDashing)
        {
            controller.Move(move * speed * Time.deltaTime);
        }

        // Прыжок
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // Гравитация
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        // Рывок
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(Dash(move));
        }
    }

    // Корутин для рывка
    private IEnumerator Dash(Vector3 moveDirection)
    {
        isDashing = true;
        canDash = false;

        // Обнуляем вертикальную скорость перед рывком
        velocity.y = 0;

        // Нормализуем вектор движения, чтобы задать фиксированное расстояние рывка
        Vector3 dashDirection = moveDirection.normalized;

        // Вычисляем момент окончания рывка
        float dashEndTime = Time.time + dashTime;

        while (Time.time < dashEndTime)
        {
            // Перемещаем игрока на фиксированное расстояние за заданное время
            controller.Move(dashDirection * (dashDistance / dashTime) * Time.deltaTime);
            yield return null;
        }

        isDashing = false;

        // Ожидание отката рывка
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }
}
