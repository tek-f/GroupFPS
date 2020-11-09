using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using GunBall.Weapons;
using GunBall.Ball;
using Mirror;

namespace GunBall.Mirror
{
    [RequireComponent(typeof(PlayerInput))]
    [RequireComponent(typeof(NetworkTransform))]
    public class NetworkPlayerController : NetworkBehaviour
    {
        public int TeamID
        {
            get
            {
                return teamID;
            }
            set
            {
                teamID = value;
            }
        }
        public GeneralGun Primary
        {
            get
            {
                return primary;
            }
        }
        public int LoadoutCount
        {
            get
            {
                return loadout.Count;
            }
        }
        #region Variables
        [Header("Game")]
        [SerializeField] int teamID;
        [SerializeField] GameObject playerCanvas;
        [Header("Looking")]
        public float mouseSensitivity = 100f;//used to control the speed of the camera movement
        float xRotation;//used in mouse look to control the range of the camera movement
        [SerializeField] Transform cameraTransform;//transform of the players camera
        [Header("Movement")]
        [SerializeField] float currentSpeed, normalSpeed = 5f, sprintSpeed = 15f, crouchSpeed = 2f, devSpeed = 50f;//players movement speeds
        [SerializeField] float jumpSpeed = 50f;//players jump speed/force
        [SerializeField] float gravity = -10f;//the rate of gravity
        float mag;
        [SerializeField] bool isCrouching, isSprinting, devSpeedBool;
        [SerializeField] Vector3 velocity, move;//used to update the players position in movement
        CharacterController charControl;//reference to the players character controller
        public LayerMask groundLayerMask;//layer mask of the ground layer

        bool groundedCheck => Physics.Raycast(gameObject.transform.position, Vector3.down, 1.1f, groundLayerMask);//used to check if the player is on the ground
        [Header("Input")]
        [SerializeField] PlayerInput playerInput;
        InputAction moveAction;
        InputAction lookAction;
        InputAction jumpAction;
        InputAction reloadAction;
        InputAction fireAction;
        InputAction swapAction;
        InputAction interactAction;
        InputAction crouchAction;
        InputAction sprintAction;
        InputAction meleeAction;
        InputAction testAction;
        [Header("Guns")]
        [SerializeField] GeneralGun currentGun;
        [SerializeField] GeneralGun pistol;
        [SerializeField] GeneralGun primary = null;
        [SerializeField] List<GeneralGun> loadout = new List<GeneralGun>();
        int equipedGunID;
        [Header("Ball")]
        [SerializeField] GeneralBall gameBall;
        bool holdingBall, ballInReach, throwingBall;
        [SerializeField] Vector3 ballPosition;
        [SerializeField] float ballPickUpDistance, ballThrowForceModifier, chargableThrowForce, startThrowTimeStamp, minThrowForce, maxThrowForce;
        [SerializeField] GameObject ballPickUpIndicatorUI;
        [Header("Animation")]
        public Animator anim;//animator component on the player
        #endregion

        #region Input Performed
        private void OnJumpPerformed(InputAction.CallbackContext _context)
        {
            if (groundedCheck)
            {
                Debug.Log("jump");
                velocity.y += jumpSpeed;
            }
        }
        private void OnFirePerformed(InputAction.CallbackContext _context)
        {
            if (!holdingBall)
            {
                currentGun.Shoot();
            }
        }
        private void OnReloadPerformed(InputAction.CallbackContext _context)
        {
            if (!holdingBall)
            {
                currentGun.Reload();
            }
        }
        private void OnSwapPerformed(InputAction.CallbackContext _context)
        {
            if (!holdingBall)
            {
                SwapWeapon();
            }
        }
        private void OnInteractPerformed(InputAction.CallbackContext _context)
        {
            if (ballInReach && !holdingBall)
            {
                PickUpBall();
            }
            else if (holdingBall)
            {
                StartBallThrow();
            }
        }
        private void OnInteractCancelled(InputAction.CallbackContext _context)
        {
            if (holdingBall && throwingBall)
            {
                ThrowBall();
            }
        }
        private void OnCrouchPerformed(InputAction.CallbackContext _context)
        {
            //Crouch
        }
        private void OnSprintPerformed(InputAction.CallbackContext _context)
        {
            ToggleSprint();
        }
        private void OnMeleePerformed(InputAction.CallbackContext _context)
        {
            //melee attack
        }
        private void OnTestPerformed(InputAction.CallbackContext _context)
        {
            ToggleDevSpeed();
        }
        #endregion

        #region Movement and Looking
        [Client]
        void MouseLook(Vector2 inputVector)
        {
            float mouseX = inputVector.x;
            float mouseY = inputVector.y;
            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);
            cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            transform.Rotate(Vector3.up * mouseX);
        }
        [Client]
        void PlayerMovement(Vector2 inputVector)
        {
            bool grounded = groundedCheck;
            float x = inputVector.x;
            float z = inputVector.y;
            if (grounded)
            {
                if (grounded && velocity.y < 0)
                {
                    velocity.y = 0f;
                }

                if (anim)
                {
                    if (z != 0 || x != 0)
                    {
                        anim.SetBool("moving", true);
                    }
                    else
                    {
                        anim.SetBool("moving", false);
                    }
                }

                move = ((transform.right * x) + (transform.forward * z)) * currentSpeed;
                mag = move.magnitude;
            }
            else
            {

                Vector3 inputDirection = ((transform.right * x) + (transform.forward * z)) / 2;
                inputDirection = Vector3.Lerp(move, inputDirection, 0.1f);
                move += inputDirection;
                move.Normalize();
                move *= mag;
            }
            charControl.Move(move * Time.deltaTime);

            velocity.y += gravity * Time.deltaTime;
            charControl.Move(velocity * Time.deltaTime);

        }
        #endregion
        [Client]
        public void PlayerSetUp(GeneralBall ball)
        {
            gameBall = ball;
            cameraTransform = gameObject.GetComponentInChildren<Camera>().transform;
            charControl = gameObject.GetComponent<CharacterController>();

            Cursor.lockState = CursorLockMode.Locked;
            currentSpeed = normalSpeed;

            #region Pistol Set Up
            currentGun = pistol;
            equipedGunID = 0;
            currentGun.PlayerSetUp(gameObject);
            currentGun.UpdateUI();
            #endregion
        }
        [Client]
        public void PickUpWeapon(GeneralGun gunToPickUp)
        {
            if (primary == null)
            {
                primary = gunToPickUp;
                if (!holdingBall)
                {
                    SwapWeapon();
                    return;
                }
                else
                {
                    gunToPickUp.gameObject.SetActive(false);
                }
            }
        }
        [Client]
        void PickUpBall()
        {
            holdingBall = true;
            ballPickUpIndicatorUI.SetActive(false);
            currentGun.gameObject.SetActive(false);
            gameBall.transform.SetParent(cameraTransform);
            gameBall.transform.localPosition = ballPosition;
            if (gameBall.gameObject.GetComponent<Rigidbody>())
            {
                Destroy(gameBall.GetComponent<Rigidbody>());
            }
        }
        [Client]
        void StartBallThrow()
        {
            throwingBall = true;
            currentSpeed = crouchSpeed;
            startThrowTimeStamp = Time.time;
            chargableThrowForce = minThrowForce;
        }
        [Client]
        void ThrowBall()
        {
            holdingBall = false;
            throwingBall = false;
            currentSpeed = normalSpeed;
            gameBall.transform.SetParent(null);
            currentGun.gameObject.SetActive(true);
            Rigidbody ballRigidbidy = gameBall.gameObject.AddComponent<Rigidbody>();
            ballRigidbidy.velocity = charControl.velocity;
            ballRigidbidy.AddForce(cameraTransform.forward * chargableThrowForce, ForceMode.Impulse);
        }
        [Client]
        public void SwapWeapon()
        {
            if (!holdingBall && primary != null)
            {
                currentGun.gameObject.SetActive(false);
                if (equipedGunID == 0)
                {
                    equipedGunID = 1;
                    currentGun = primary;
                }
                else if (equipedGunID == 1)
                {
                    equipedGunID = 0;
                    currentGun = pistol;
                }
                //currentGun = loadout[equipedGunID];
                currentGun.gameObject.SetActive(true);
            }
            currentGun.UpdateUI();
        }
        [Client]
        void ToggleSprint()
        {
            if (isSprinting)
            {
                isSprinting = false;
                currentSpeed = normalSpeed;
            }
            else
            {
                isSprinting = true;
                currentSpeed = sprintSpeed;
            }
        }
        [Client]
        void ToggleDevSpeed()
        {
            if (devSpeedBool)
            {
                devSpeedBool = false;
                currentSpeed = normalSpeed;
            }
            else
            {
                devSpeedBool = true;
                currentSpeed = devSpeed;
            }
        }
        public override void OnStartAuthority()
        {
            enabled = true;
            playerCanvas.SetActive(true);
            cameraTransform.GetComponent<Camera>().enabled = true;
            cameraTransform.GetComponent<AudioListener>().enabled = true;


            #region Set Up Player Inputs
            playerInput = gameObject.GetComponent<PlayerInput>();

            moveAction = playerInput.actions.FindAction("Move");
            moveAction.Enable();

            lookAction = playerInput.actions.FindAction("Look");
            lookAction.Enable();

            jumpAction = playerInput.actions.FindAction("Jump");
            jumpAction.Enable();
            jumpAction.performed += OnJumpPerformed;

            interactAction = playerInput.actions.FindAction("Interact");
            interactAction.Enable();
            interactAction.performed += OnInteractPerformed;
            interactAction.canceled += OnInteractCancelled;

            swapAction = playerInput.actions.FindAction("Swap");
            swapAction.Enable();
            swapAction.performed += OnSwapPerformed;

            reloadAction = playerInput.actions.FindAction("Reload");
            reloadAction.Enable();
            reloadAction.performed += OnReloadPerformed;

            fireAction = playerInput.actions.FindAction("Fire");
            fireAction.Enable();
            fireAction.performed += OnFirePerformed;

            sprintAction = playerInput.actions.FindAction("Sprint");
            sprintAction.Enable();
            sprintAction.performed += OnSprintPerformed;

            crouchAction = playerInput.actions.FindAction("Crouch");
            crouchAction.Enable();
            crouchAction.performed += OnCrouchPerformed;

            meleeAction = playerInput.actions.FindAction("Melee");
            meleeAction.Enable();
            meleeAction.performed += OnMeleePerformed;

            testAction = playerInput.actions.FindAction("Test");
            testAction.Enable();
            testAction.performed += OnTestPerformed;
            #endregion

            //TEMP
            PlayerSetUp(GameObject.FindWithTag("Ball").GetComponent<GeneralBall>());
        }
        [Client]
        private void OnEnable() => playerInput.enabled = true;
        [Client]
        private void OnDisable() => playerInput.enabled = false;

        #region Start
        //private void Start()
        //{
        //    #region Set Up Player Inputs
        //    playerInput = gameObject.GetComponent<PlayerInput>();
        //    playerInput.enabled = true;

        //    moveAction = playerInput.actions.FindAction("Move");
        //    moveAction.Enable();

        //    lookAction = playerInput.actions.FindAction("Look");
        //    lookAction.Enable();

        //    jumpAction = playerInput.actions.FindAction("Jump");
        //    jumpAction.Enable();
        //    jumpAction.performed += OnJumpPerformed;

        //    interactAction = playerInput.actions.FindAction("Interact");
        //    interactAction.Enable();
        //    interactAction.performed += OnInteractPerformed;
        //    interactAction.canceled += OnInteractCancelled;

        //    swapAction = playerInput.actions.FindAction("Swap");
        //    swapAction.Enable();
        //    swapAction.performed += OnSwapPerformed;

        //    reloadAction = playerInput.actions.FindAction("Reload");
        //    reloadAction.Enable();
        //    reloadAction.performed += OnReloadPerformed;

        //    fireAction = playerInput.actions.FindAction("Fire");
        //    fireAction.Enable();
        //    fireAction.performed += OnFirePerformed;

        //    sprintAction = playerInput.actions.FindAction("Sprint");
        //    sprintAction.Enable();
        //    sprintAction.performed += OnSprintPerformed;

        //    crouchAction = playerInput.actions.FindAction("Crouch");
        //    crouchAction.Enable();
        //    crouchAction.performed += OnCrouchPerformed;

        //    meleeAction = playerInput.actions.FindAction("Melee");
        //    meleeAction.Enable();
        //    meleeAction.performed += OnMeleePerformed;

        //    testAction = playerInput.actions.FindAction("Test");
        //    testAction.Enable();
        //    testAction.performed += OnTestPerformed;
        //    #endregion

        //    //TEMP
        //    PlayerSetUp(GameObject.FindWithTag("Ball").GetComponent<GeneralBall>());
        //}
        #endregion

        [Client]
        private void Update()
        {
            MouseLook(lookAction.ReadValue<Vector2>());
            PlayerMovement(moveAction.ReadValue<Vector2>());
            if (!holdingBall)
            {
                RaycastHit raycastHit;
                if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out raycastHit, ballPickUpDistance))
                {
                    if (raycastHit.transform.GetComponent<GeneralBall>())
                    {
                        ballPickUpIndicatorUI.SetActive(true);
                        ballInReach = true;
                    }
                    else
                    {
                        ballPickUpIndicatorUI.SetActive(false);
                        ballInReach = false;
                    }
                }
            }
            if (throwingBall)
            {
                chargableThrowForce += (ballThrowForceModifier * Time.deltaTime);
                chargableThrowForce = Mathf.Clamp(chargableThrowForce, minThrowForce, maxThrowForce);
            }
        }
    }
}