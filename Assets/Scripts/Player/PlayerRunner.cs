
using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class PlayerRunner : MonoBehaviour
{
    [Header("Running")]
    public float laneWidth = 3f;
    public float runSpeed = 10f;
    public float maxSpeed = 22f;
    public float acceleration = 0.15f;
    public float laneChangeSpeed = 12f;

    [Header("Jumping")]
    public float jumpHeight = 2.2f;
    public float gravity = -25f;

    [Header("Sliding")]
    public float slideDuration = 0.8f;

    [Header("Shield")]
    public GameObject shieldVisual;
    private bool hasShield = false;

    [Header("Speed Boost")]
    public float speedBoostMultiplier = 1f;
    public GameObject speedBoostVisual;
    private Coroutine speedBoostCoroutine;

    private CharacterController controller;
    private int lane = 1;
    private float verticalVelocity;
    private float normalHeight;
    private Vector3 normalCenter;
    private float elapsedTime;
    private bool sliding;

    void Awake()
    {
        controller = GetComponent<CharacterController>();

        normalHeight = controller.height;
        normalCenter = controller.center;

        if (shieldVisual != null)
            shieldVisual.SetActive(false);

        if (speedBoostVisual != null)
            speedBoostVisual.SetActive(false);
    }

    // SHIELD SYSTEM

    public void ActivateShield()
    {
        hasShield = true;

        if (shieldVisual != null)
            shieldVisual.SetActive(true);

        Debug.Log("SHIELD ACTIVATED!");
    }

    public bool UseShield()
    {
        if (!hasShield)
            return false;

        hasShield = false;

        if (shieldVisual != null)
            shieldVisual.SetActive(false);

        Debug.Log("SHIELD USED!");

        return true;
    }

    // SPEED BOOST SYSTEM

    public void ActivateSpeedBoost(float multiplier, float duration)
    {
        if (speedBoostCoroutine != null)
        {
            StopCoroutine(speedBoostCoroutine);
        }

        speedBoostCoroutine = StartCoroutine(
            SpeedBoostRoutine(multiplier, duration)
        );
    }

    IEnumerator SpeedBoostRoutine(float multiplier, float duration)
    {
        speedBoostMultiplier = multiplier;

        if (speedBoostVisual != null)
            speedBoostVisual.SetActive(true);

        Debug.Log("SPEED BOOST ACTIVATED!");

        yield return new WaitForSeconds(duration);

        speedBoostMultiplier = 1f;

        if (speedBoostVisual != null)
            speedBoostVisual.SetActive(false);

        speedBoostCoroutine = null;

        Debug.Log("SPEED BOOST ENDED!");
    }

    // PLAYER MOVEMENT

    void Update()
    {
        HandleInput();

        elapsedTime += Time.deltaTime;

        float normalSpeed = Mathf.Min(
            maxSpeed,
            runSpeed + acceleration * elapsedTime
        );

        float speed = normalSpeed * speedBoostMultiplier;

        float targetX = (lane - 1) * laneWidth;

        float moveX = (targetX - transform.position.x)
                      * laneChangeSpeed;

        if (controller.isGrounded && verticalVelocity < 0f)
            verticalVelocity = -2f;

        verticalVelocity += gravity * Time.deltaTime;

        Vector3 movement = new Vector3(
            moveX,
            verticalVelocity,
            speed
        );

        controller.Move(movement * Time.deltaTime);
    }

    void HandleInput()
    {
        // LEFT LANE
        if (Input.GetKeyDown(KeyCode.LeftArrow) ||
            Input.GetKeyDown(KeyCode.A))
        {
            lane = Mathf.Max(0, lane - 1);
        }

        // RIGHT LANE
        if (Input.GetKeyDown(KeyCode.RightArrow) ||
            Input.GetKeyDown(KeyCode.D))
        {
            lane = Mathf.Min(2, lane + 1);
        }

        // JUMP
        if (Input.GetKeyDown(KeyCode.Space) &&
            controller.isGrounded && !sliding)
        {
            verticalVelocity = Mathf.Sqrt(
                jumpHeight * -2f * gravity
            );
        }

        // SLIDE
        if ((Input.GetKeyDown(KeyCode.DownArrow) ||
             Input.GetKeyDown(KeyCode.S)) &&
            controller.isGrounded && !sliding)
        {
            StartCoroutine(Slide());
        }
    }

    IEnumerator Slide()
    {
        sliding = true;

        controller.height = normalHeight * 0.5f;

        controller.center = new Vector3(
            normalCenter.x,
            normalCenter.y - normalHeight * 0.25f,
            normalCenter.z
        );

        yield return new WaitForSeconds(slideDuration);

        controller.height = normalHeight;
        controller.center = normalCenter;

        sliding = false;
    }
}
