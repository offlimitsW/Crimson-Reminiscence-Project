using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public bool disable = false;
    public bool canMove { get; private set; } = true;
    public bool isSprinting => canSprint  && Input.GetKey(sprintKey);
    private bool ShouldJump => Input.GetKeyDown(jumpKey) && characterController.isGrounded;
    private bool ShouldCrouch => Input.GetKeyDown(crouchKey) && !duringCrouchAnimation && characterController.isGrounded;

    [Header("Fuctional Options")]
    [SerializeField] private bool canSprint = true;
    [SerializeField] private bool canJump = true;
    [SerializeField] private bool canCrouch = true;
    [SerializeField] private bool canHeadbob = true;
    [SerializeField] private bool willSlideOnSlopes = true;
    [SerializeField] private bool canZoom = true;
    [SerializeField] private bool canInteract = true;
    [SerializeField] private bool useFootsteps = true;

    [Header("Controls")]
    [SerializeField] private KeyCode sprintKey = KeyCode.LeftShift;
    [SerializeField] private KeyCode jumpKey = KeyCode.Space;
    [SerializeField] private KeyCode crouchKey = KeyCode.LeftControl;
    [SerializeField] private KeyCode flashLightKey = KeyCode.F;
    [SerializeField] private KeyCode interactKey = KeyCode.E;
    [SerializeField] private KeyCode zoomKey = KeyCode.Mouse1;

    [Header("Movement Parameters")]
    [SerializeField] private float walkSpeed = 3.5f;
    [SerializeField] private float sprintSpeed = 6.5f;
    [SerializeField] private float crouchSpeed = 1.5f;
    [SerializeField] private float slopeSpeed = 8f;

    [Header("Look Parameters")]
    [SerializeField, Range(1, 10)] private float lookSpeedX = 2.0f;
    [SerializeField, Range(1, 10)] private float lookSpeedY = 2.0f;
    [SerializeField, Range(1, 180)] private float upperLookLimit = 80.0f;
    [SerializeField, Range(1, 180)] private float lowerLookLimit = 80.0f;

    [Header("Health Parameters")]
    [SerializeField] private float maxHealth = 100;
    [SerializeField] private float timeBeforeRegenStarts = 3;
    [SerializeField] private float healthValueIncrement = 1;
    [SerializeField] private float healthTimeIncrement = 0.1f;
    private float currentHealth;
    private Coroutine regeneratingHealth;
    public static Action<float> onTakeDamage;
    public static Action<float> onDamage;
    public static Action<float> onHeal;
    public GameObject healthBar;
    public GameObject healthBar1;
    public GameObject healthBar2;
    public GameObject healthBar3;
    public GameObject healthBar4;
    public GameObject healthBar5;
    public GameObject healthBar6;
    public GameObject healthBar7;
    public GameObject healthBar8;
    public GameObject healthBar9;
    [SerializeField] private CanvasGroup damageDisplay;
    [SerializeField] private CanvasGroup damageDisplay2;
    [SerializeField] private AudioClip[] healthReBoot = default;

    [Header("Jumping Parameters")]
    [SerializeField] private float jumpForce = 8.0f;
    [SerializeField] private float gravity = 30.0f;

    [Header("Crouch Parameters")]
    [SerializeField] private float crouchHeight = 0.98f;
    [SerializeField] private float standingHeight = 1.85f;
    [SerializeField] private float timeToCrouch = 0.25f;
    [SerializeField] private Vector3 crouchingCenter = new Vector3(0, 0.3f, 0);
    [SerializeField] private Vector3 standingCenter = new Vector3(0, 0, 0);
    private bool isCrouching;
    private bool duringCrouchAnimation;

    [Header("Headbob Parameters")]
    [SerializeField] private float walkBobSpeed = 14f;
    [SerializeField] private float walkBobAmount = 0.03f;
    [SerializeField] private float sprintBobSpeed = 18f;
    [SerializeField] private float sprintBobAmount = 0.05f;
    [SerializeField] private float crouchBobSpeed = 8f;
    [SerializeField] private float crouchBobAmount = 0.0125f;
    private float defaultYPos = 0;
    private float timer;

    [Header("Gunbob Parameters")]
    [SerializeField] private float gunWalkBobAmount = 0.0075f;
    [SerializeField] private float gunSprintBobAmount = 0.015f;
    [SerializeField] private float gunCrouchBobAmount = 0.005f;
    private float gunDefaultYPos;
    private float gunDefaultXPos;
    private float gunDefaultXRot;
    private float gunDefaultZRot;

    [Header("Zoom Parameters")]
    [SerializeField] private float timeToZoom = 0.075f;
    [SerializeField] private float zoomFOV = 30f;
    [SerializeField] private bool isZoomed = false;
    private float defaultFOV;
    private Coroutine zoomRoutine;
    Animator anim;

    [Header("Footstep Parameters")]
    [SerializeField] private float baseStepSpeed = 0.5f;
    [SerializeField] private float crouchStepMultipler = 1.5f;
    [SerializeField] private float sprintStepMultipler = 0.6f;
    [SerializeField] private AudioSource footstepAudioSource = default;
    [SerializeField] private AudioClip[] wood = default;
    [SerializeField] private AudioClip[] metal = default;
    [SerializeField] private AudioClip[] grass = default;
    [SerializeField] private AudioClip[] gravel = default;
    [SerializeField] private AudioClip[] dirt = default;
    [SerializeField] private AudioClip[] stone = default;
    private float footstepTimer = 0;

    [Header("Weapons Parameters")]
    [SerializeField] private bool isHoldingPistol;
    [SerializeField] private bool flashLightOn;
    [SerializeField] private AudioClip[] flashLightClick = default;
    public GameObject gunPistol;
    public GameObject flashLight;

    private float GetCurrentOffset => isCrouching ? baseStepSpeed * crouchStepMultipler : isSprinting ? baseStepSpeed * sprintStepMultipler : baseStepSpeed;

    // Sliding Paramaters

    private Vector3 hitPointNormal;

    private bool IsSliding
    {
        get
        {
            Debug.DrawRay(transform.position, Vector3.down, Color.red);
            if(characterController.isGrounded && Physics.Raycast(transform.position, Vector3.down, out RaycastHit slopeHit, 2f))
            {
                hitPointNormal += slopeHit.normal;
                return Vector3.Angle(hitPointNormal, Vector3.up) > characterController.slopeLimit;
            }
            else
            {
                return false;
            }
        }
    }

    [Header("Interaction")]
    [SerializeField] private AudioClip[] interact = default;
    [SerializeField] private Vector3 interactionRayPoint = default;
    [SerializeField] private float interactionDistance = default;
    [SerializeField] private LayerMask interactionLayer;
    private Interactable currentInteractable;

    private void OnEnable()
    {
        onTakeDamage += ApplyDamage;
    }

    private void OnDisable()
    {
        onTakeDamage -= ApplyDamage;
    }

    private Camera playerCamera;
    private CharacterController characterController;

    private Vector3 moveDirection;
    private Vector2 currentInput;

    private float rotationX = 0;

    public static PlayerController Instance;

    void Awake()
    {
        Instance = this;
        flashLightOn = true;

        playerCamera = GetComponentInChildren<Camera>();
        characterController = GetComponent<CharacterController>();

        gunDefaultYPos = gunPistol.transform.localPosition.y;
        gunDefaultXPos = gunPistol.transform.localPosition.x;
        gunDefaultXRot = gunPistol.transform.localRotation.x;
        gunDefaultZRot = gunPistol.transform.localRotation.z;

        defaultYPos = playerCamera.transform.localPosition.y;
        defaultFOV = playerCamera.fieldOfView;

        currentHealth = maxHealth;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (!disable) 
        {
            if (canMove)
            {
                HandleMovementInput();
                HandleMouseLook();

                if (canJump)
                    HandleJump();

                if (canCrouch)
                    CanCrouch();

                if (canHeadbob)
                    HandleHeadbob();

                if (canZoom)
                    HandleZoom();

                if (useFootsteps)
                    HandleFootsteps();

                if (canInteract)
                {
                    HandleInteractionCheck();
                    HandleInteractionInput();
                }

                ApplyFinalMovements();
            }

            if (Input.GetKeyDown(flashLightKey) && flashLightOn == true)
            {
                flashLightOn = false;
                flashLight.SetActive(false);
                footstepAudioSource.PlayOneShot(flashLightClick[UnityEngine.Random.Range(0, flashLightClick.Length - 1)]);
            }
            else if (Input.GetKeyDown(flashLightKey) && flashLightOn == false)
            {
                flashLightOn = true;
                flashLight.SetActive(true);
                footstepAudioSource.PlayOneShot(flashLightClick[UnityEngine.Random.Range(0, flashLightClick.Length - 1)]);
            }

            if (isZoomed == true)
            {
                walkSpeed = 1.75f;
                sprintSpeed = 3.5f;
                crouchSpeed = 0.75f;

                walkBobSpeed = 7f;
                walkBobAmount = 0.015f;
                sprintBobSpeed = 9f;
                sprintBobAmount = 0.025f;
                crouchBobSpeed = 4f;
                crouchBobAmount = 0.0075f;

                baseStepSpeed = 0.85f;

                if (Input.GetKey(sprintKey) && Input.GetKey(KeyCode.A) && characterController.isGrounded || Input.GetKey(sprintKey) && Input.GetKey(KeyCode.D) && characterController.isGrounded)
                {
                    sprintSpeed = 2.75f;
                    sprintBobSpeed = 7f;
                    sprintBobAmount = 0.02f;

                    baseStepSpeed = 0.75f;
                }

                if (Input.GetKey(KeyCode.A) && characterController.isGrounded || Input.GetKey(KeyCode.D) && characterController.isGrounded)
                {
                    walkSpeed = 1.25f;
                    walkBobSpeed = 6.25f;
                    walkBobAmount = 0.00075f;

                    baseStepSpeed = 0.95f;
                }

                if (Input.GetKey(KeyCode.S) && characterController.isGrounded)
                {
                    walkSpeed = 1.5f;
                    walkBobSpeed = 6f;
                    walkBobAmount = 0.0015f;

                    baseStepSpeed = 0.95f;
                }
            }
            else
            {
                walkSpeed = 3.5f;
                sprintSpeed = 6.5f;
                crouchSpeed = 1.5f;

                walkBobSpeed = 14f;
                walkBobAmount = 0.03f;
                sprintBobSpeed = 18f;
                sprintBobAmount = 0.05f;
                crouchBobSpeed = 8f;
                crouchBobAmount = 0.0125f;

                baseStepSpeed = 0.5f;
            }

            if (currentHealth <= 0)
                healthBar.SetActive(false);
            else if (currentHealth >= 0)
                healthBar.SetActive(true);

            if (currentHealth < 10)
                healthBar1.SetActive(false);
            else if (currentHealth >= 10)
                healthBar1.SetActive(true);

            if (currentHealth < 20)
                healthBar2.SetActive(false);
            else if (currentHealth >= 20)
                healthBar2.SetActive(true);

            if (currentHealth < 30)
            {
                healthBar3.SetActive(false);
                canSprint = false;
            }
            else if (currentHealth >= 30)
            {
                healthBar3.SetActive(true);
                canSprint = true;
            }

            if (currentHealth < 40)
                healthBar4.SetActive(false);
            else if (currentHealth >= 40)
                healthBar4.SetActive(true);

            if (currentHealth < 50)
                healthBar5.SetActive(false);
            else if (currentHealth >= 50)
                healthBar5.SetActive(true);

            if (currentHealth < 60)
                healthBar6.SetActive(false);
            else if (currentHealth >= 60)
                healthBar6.SetActive(true);

            if (currentHealth < 70)
                healthBar7.SetActive(false);
            else if (currentHealth >= 70)
                healthBar7.SetActive(true);

            if (currentHealth < 80)
                healthBar8.SetActive(false);
            else if (currentHealth >= 80)
                healthBar8.SetActive(true);

            if (currentHealth < 90)
                healthBar9.SetActive(false);
            else if (currentHealth >= 100)
                healthBar9.SetActive(true);

            if (currentHealth >= 60 && damageDisplay.alpha > 0)
            {
                damageDisplay.alpha -= Time.deltaTime;
                if (damageDisplay.alpha <= 0)
                {
                    damageDisplay.alpha = 0;
                }
            }

            if (currentHealth >= 30 && damageDisplay2.alpha > 0)
            {
                damageDisplay2.alpha -= Time.deltaTime;
                if (damageDisplay2.alpha <= 0)
                {
                    damageDisplay2.alpha = 0;
                }
            }

            //Strafing and walk/sprint backwards
            if (isZoomed == false && Input.GetKey(sprintKey) && Input.GetKey(KeyCode.A) && characterController.isGrounded || isZoomed == false && Input.GetKey(sprintKey) && Input.GetKey(KeyCode.D) && characterController.isGrounded)
            {
                sprintSpeed = 4.5f;
                sprintBobSpeed = 15f;
                sprintBobAmount = 0.0425f;

                baseStepSpeed = 0.55f;
            }

            if (isZoomed == false && Input.GetKey(sprintKey) && Input.GetKey(KeyCode.S) && characterController.isGrounded)
            {
                sprintSpeed = 3.75f;
                sprintBobSpeed = 12f;
                sprintBobAmount = 0.035f;

                baseStepSpeed = 0.575f;
            }

            if (isZoomed == false && Input.GetKey(KeyCode.S) && characterController.isGrounded)
            {
                walkSpeed = 2.15f;
                walkBobSpeed = 10f;
                walkBobAmount = 0.02f;

                baseStepSpeed = 0.7f;
            }

            if (isZoomed == false && Input.GetKey(KeyCode.A) && characterController.isGrounded || isZoomed == false && Input.GetKey(KeyCode.D) && characterController.isGrounded)
            {
                walkSpeed = 2.75f;
            }

            if (RayCastPistol.Instance.isReloading == true)
                canSprint = false;
            else return;
        }       
    }

    private void ApplyDamage(float dmg)
    {
        currentHealth -= dmg;
        onDamage?.Invoke(currentHealth);

        damageDisplay.alpha = 1;

        if(currentHealth <= 30)
            damageDisplay2.alpha = 1;

        if (currentHealth <= 0)
            KillPlayer();
        else if (regeneratingHealth != null)
            StopCoroutine(regeneratingHealth);

        regeneratingHealth = StartCoroutine(RegenerateHealth());
    }

    private void KillPlayer()
    {
        currentHealth = 0;
        damageDisplay.alpha = 1;
        damageDisplay2.alpha = 1;

        if (regeneratingHealth != null)
            StopCoroutine(regeneratingHealth);

        print("Dead");
    }

    private void HandleMovementInput()
    {
        currentInput = new Vector2((isCrouching ? crouchSpeed : isSprinting ? sprintSpeed : walkSpeed) * Input.GetAxis("Vertical"), (isCrouching ? crouchSpeed : isSprinting ? sprintSpeed : walkSpeed) * Input.GetAxis("Horizontal"));

        float moveDirectionY = moveDirection.y;
        moveDirection = (transform.TransformDirection(Vector3.forward) * currentInput.x) + (transform.TransformDirection(Vector3.right) * currentInput.y);
        moveDirection.y = moveDirectionY;
    }

    private void HandleMouseLook()
    {
        rotationX -= Input.GetAxis("Mouse Y") * lookSpeedY;
        rotationX = Mathf.Clamp(rotationX, -upperLookLimit, lowerLookLimit);
        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
        transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeedX, 0);
    }

    private void HandleJump()
    {
        if (ShouldJump)
            moveDirection.y = jumpForce;
    }

    private void CanCrouch()
    {
        if (ShouldCrouch)
            StartCoroutine(CrouchStand());
    }

    private void HandleHeadbob()
    {
        if (!characterController.isGrounded) return;

        if (Mathf.Abs(moveDirection.x) > 0.1f || Mathf.Abs(moveDirection.z) > 0.1f)
        {
            timer += Time.deltaTime * (isCrouching ? crouchBobSpeed : isSprinting ? sprintBobSpeed : walkBobSpeed);
            playerCamera.transform.localPosition = new Vector3(
                playerCamera.transform.localPosition.x,
                defaultYPos + Mathf.Sin(timer) * (isCrouching ? crouchBobAmount : isSprinting ? sprintBobAmount : walkBobAmount), 
                playerCamera.transform.localPosition.z);

            gunPistol.transform.localPosition = new Vector3(
                gunPistol.transform.localPosition.x,
                gunDefaultXRot + gunDefaultZRot + gunDefaultYPos + Mathf.Sin(timer) * (isCrouching ? gunCrouchBobAmount : isSprinting ? gunSprintBobAmount : gunWalkBobAmount),
                gunPistol.transform.localPosition.z);
        }
    }

    private void HandleZoom()
    {
        if (Input.GetKeyDown(zoomKey))
        {

            if (zoomRoutine != null)
            {
                StopCoroutine(zoomRoutine);
                zoomRoutine = null;
            }

            zoomRoutine = StartCoroutine(ToggleZoom(true));
            isZoomed = true;
        }

        if (Input.GetKeyUp(zoomKey))
        {

            if (zoomRoutine != null)
            {
                StopCoroutine(zoomRoutine);
                zoomRoutine = null;
            }

            zoomRoutine = StartCoroutine(ToggleZoom(false));
            isZoomed = false;
        }
    }

    private void HandleInteractionCheck()
    {
        if(Physics.Raycast(playerCamera.ViewportPointToRay(interactionRayPoint), out RaycastHit hit, interactionDistance))
        {
            if(hit.collider.gameObject.layer == 7 && (currentInteractable == null || hit.collider.gameObject.GetInstanceID() != currentInteractable.GetInstanceID()))
            {
                hit.collider.TryGetComponent(out currentInteractable);

                if (currentInteractable)
                    currentInteractable.OnFocus();
            }
            else if(currentInteractable)
            {
                currentInteractable.OnLoseFocus();
                currentInteractable = null;
            }
        }
    }

    private void HandleInteractionInput()
    {
        if(Input.GetKeyDown(interactKey) && currentInteractable != null && Physics.Raycast(playerCamera.ViewportPointToRay(interactionRayPoint), out RaycastHit hit, interactionDistance, interactionLayer))
        {
            currentInteractable.OnInteract();
        }
    }

    private void ApplyFinalMovements()
    {
        if (!characterController.isGrounded)
            moveDirection.y -= gravity * Time.deltaTime;

        if (willSlideOnSlopes && IsSliding)
            moveDirection = new Vector3(hitPointNormal.x, -hitPointNormal.y, hitPointNormal.z) * slopeSpeed;

        characterController.Move(moveDirection * Time.deltaTime);
    }

    private void HandleFootsteps()
    {
        if (!characterController.isGrounded) return;
        if (currentInput == Vector2.zero) return;

        footstepTimer -= Time.deltaTime;

        if(footstepTimer <= 0)
        {
            if(Physics.Raycast(playerCamera.transform.position, Vector3.down, out RaycastHit hit, 4))
            {
                switch(hit.collider.tag)
                {
                    case "Footsteps/Wood":
                        footstepAudioSource.PlayOneShot(wood[UnityEngine.Random.Range(0, wood.Length - 1)]);
                        break;
                    case "Footsteps/Metal":
                        footstepAudioSource.PlayOneShot(metal[UnityEngine.Random.Range(0, metal.Length - 1)]);
                        break;
                    case "Footsteps/Grass":
                        footstepAudioSource.PlayOneShot(grass[UnityEngine.Random.Range(0, grass.Length - 1)]);
                        break;
                    case "Footsteps/Dirt":
                        footstepAudioSource.PlayOneShot(dirt[UnityEngine.Random.Range(0, dirt.Length - 1)]);
                        break;
                    case "Footsteps/Stone":
                        footstepAudioSource.PlayOneShot(stone[UnityEngine.Random.Range(0, stone.Length - 1)]);
                        break;
                    case "Footsteps/Gravel":
                        footstepAudioSource.PlayOneShot(gravel[UnityEngine.Random.Range(0, gravel.Length - 1)]);
                        break;
                    default:
                        footstepAudioSource.PlayOneShot(stone[UnityEngine.Random.Range(0, stone.Length - 1)]);
                        break;
                }
            }

            footstepTimer = GetCurrentOffset;
        }
    }

    private IEnumerator CrouchStand()
    {
        if (isCrouching && Physics.Raycast(playerCamera.transform.position, Vector3.up, 1f))
            yield break;

        duringCrouchAnimation = true;

        float timeElapsed = 0;
        float targetHeight = isCrouching ? standingHeight : crouchHeight;
        float currentHeight = characterController.height;
        Vector3 targetCenter = isCrouching ? standingCenter : crouchingCenter;
        Vector3 currentCenter = characterController.center;
        while(timeElapsed < timeToCrouch)
        {
            characterController.height = Mathf.Lerp(currentHeight, targetHeight, timeElapsed / timeToCrouch);
            characterController.center = Vector3.Lerp(currentCenter, targetCenter, timeElapsed / timeToCrouch);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        characterController.height = targetHeight;
        characterController.center = targetCenter;

        isCrouching = !isCrouching;

        duringCrouchAnimation = false;
    }

    private IEnumerator ToggleZoom(bool isEnter)
    {
        float targetFOV = isEnter ? zoomFOV : defaultFOV;
        float startingFOV = playerCamera.fieldOfView;
        Vector3 gunDefault = gunPistol.transform.localPosition;
        float timeElapsed = 0;

        while (timeElapsed < timeToZoom)
        {
            playerCamera.fieldOfView = Mathf.Lerp(startingFOV, targetFOV, timeElapsed / timeToZoom);


            //this should be that ^, but it doesn't work: characterController.center = Vector3.Lerp(gunDefault, targetGunPos, timeElapsed / timeToZoom);

            timeElapsed += Time.deltaTime;
            yield return null;
        }

        playerCamera.fieldOfView = targetFOV;
        zoomRoutine = null;
    }

    private IEnumerator RegenerateHealth()
    {
        yield return new WaitForSeconds(timeBeforeRegenStarts);
        WaitForSeconds timeToWait = new WaitForSeconds(healthTimeIncrement);

        while (currentHealth < maxHealth)
        {
            currentHealth += healthValueIncrement;

            if (currentHealth > maxHealth)
                currentHealth = maxHealth;

            if (currentHealth == 100)
                footstepAudioSource.PlayOneShot(healthReBoot[UnityEngine.Random.Range(0, healthReBoot.Length - 1)]);

            onHeal?.Invoke(currentHealth);
            yield return timeToWait;
        }

        regeneratingHealth = null;
    }
}
