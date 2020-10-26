using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GroupFPS.Player
{
    [RequireComponent(typeof(PlayerInput))]
    public class PlayerController : MonoBehaviour
    {
        #region Variables
        [Header("Looking")]
        public float mouseSensitivity = 100f;//used to control the speed of the camera movement
        float xRotation;//used in mouse look to control the range of the camera movement
        Transform cameraTransform;//transform of the players camera
        [Header("Movement")]
        public float speed = 2f;//players movement speed
        public float jumpSpeed = 50f;//players jump speed/force
        public float gravity = -10f;//the rate of gravity
        public Vector3 velocity;//used to update the players position in movement
        CharacterController charControl;//reference to the players character controller
        public LayerMask groundLayerMask;//layer mask of the ground layer
        bool groundedCheck => Physics.Raycast(gameObject.transform.position, Vector3.down, 1.1f, groundLayerMask);//used to check if the player is on the ground
        [Header("Input")]
        PlayerInput playerInput;
        InputAction moveAction;
        InputAction lookAction;
        InputAction jumpAction;
        InputAction fireAction;
        [Header("Guns and Shooting")]

        [Header("Animation")]
        public Animator anim;//animator component on the player
        #endregion
        void MouseLook(Vector2 inputVector)
        {
            //Old Input
            //float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            //float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
            float mouseX = inputVector.x;
            float mouseY = inputVector.y;

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);
            cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            transform.Rotate(Vector3.up * mouseX);
        }
        void PlayerMovement(Vector2 inputVector)
        {
            if (groundedCheck && velocity.y < -2)
            {
                velocity.y = 0f;
            }

            //Old Input
            //float x = Input.GetAxis("Horizontal");
            //float z = Input.GetAxis("Vertical");

            float x = inputVector.x;
            float z = inputVector.y;

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

            Vector3 move = (transform.right * x) + (transform.forward * z);
            charControl.Move(move * speed * Time.deltaTime);

            velocity.y += gravity * Time.deltaTime;
            charControl.Move(velocity * Time.deltaTime);

            if (jumpAction.ReadValue<float>() == 1 && groundedCheck)
            {
                Debug.Log("jump");
                velocity.y += Mathf.Sqrt(jumpSpeed * -1 * gravity);
            }
            
        }
        private void Awake()
        {
            Cursor.lockState = CursorLockMode.Locked;
            cameraTransform = GameObject.FindWithTag("MainCamera").GetComponent<Camera>().transform;
            charControl = gameObject.GetComponent<CharacterController>();
        }
        private void Start()
        {
            #region Set Up Player Inputs
            playerInput = gameObject.GetComponent<PlayerInput>();

            moveAction = playerInput.actions.FindAction("Move");
            moveAction.Enable();

            lookAction = playerInput.actions.FindAction("Look");
            lookAction.Enable();

            jumpAction = playerInput.actions.FindAction("Jump");
            jumpAction.Enable();

            fireAction = playerInput.actions.FindAction("Fire");
            fireAction.Enable();
            fireAction.performed += OnFirePerformed;
            fireAction.canceled += OnFireCanceled;
            #endregion
        }
        private void Update()
        {
            MouseLook(lookAction.ReadValue<Vector2>());
            PlayerMovement(moveAction.ReadValue<Vector2>());
        }

        private void OnFirePerformed(InputAction.CallbackContext _context)
        {

        }
        private void OnFireCanceled(InputAction.CallbackContext _context)
        {

        }
    }
}