using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Controller : MonoBehaviour
{

    //holds references to necessary gameObjects
    public Camera playerCamera;
    public Transform swordSpawnPoint;
    public Transform camBackPos;
    public GameObject slashPlane;

    //hold prefab reference
    public GameObject swordSwinging;

    //sets player's move speed
    [SerializeField] private float moveSpeed = 10f;

    //animator variables
    bool isOnGround;
    //bool isIdle;
    //bool isWalking = false;

    //sets camera variables
    [SerializeField] private float cameraLock = 25f;
    private Vector3 originalRot;


    public Vector3 jump = new Vector3(0.0f, 1.0f, 0.0f);
    public float jumpForce = 0.5f;
    Animator animator;
    Rigidbody rb;




    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        //isIdle = true;
        rb = GetComponent<Rigidbody>();
    }

    void OnCollisionStay()
    {
        isOnGround = true;
        animator.SetBool("isJumping", false);
    }

    // Update is called once per frame
    void Update()
    {

        #region MOVEMENT CONTROLS
        //checks if the left stick is being used
        if ((Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0) && !GMController.Instance.cameraZoom)
        {

            if (Input.GetButton("Jump") && isOnGround)
            {
                rb.AddForce(jump * jumpForce, ForceMode.Impulse);
                isOnGround = false;
                animator.SetBool("isIdle", false);
                animator.SetBool("isRunning", false);
                animator.SetBool("isJumping", true);


            }
            else
            {
                animator.SetBool("isIdle", false);
                animator.SetBool("isRunning", true);
            }

            //sets direction of movement
           
            Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));

            //moves to new position and face new position
            transform.Translate(movement * moveSpeed * Time.deltaTime, Space.World);
            transform.rotation = Quaternion.LookRotation(movement);
        }
#endregion

        #region ZOOMED IN CAMERA CONTROLS
        else if ((Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0) && GMController.Instance.cameraZoom)
        {
            //get direction to rotate to
            Vector3 camRot = new Vector3(-Input.GetAxis("Vertical"), Input.GetAxis("Horizontal"), playerCamera.transform.rotation.z);

            //have camera look at new direction using cameraLock to determine how far to look
            playerCamera.transform.eulerAngles = originalRot + (camRot * cameraLock);

            //have player look in same direction as camera around the y axis
            camRot = new Vector3(transform.rotation.x, camRot.y * cameraLock, transform.rotation.z);
            transform.eulerAngles = originalRot + camRot;
        }
        #endregion

        else
        {
            animator.SetBool("isIdle", true);
            animator.SetBool("isRunning", false);

        }
        #region SWING CONTROLS
        //if player hits swing button, swing sword
        if (Input.GetButtonDown("Fire1"))
        {
            //if camera is zoomed, disable swing
            if (!GMController.Instance.cameraZoom)
            {
                SwingSword();
            }
        }
        #endregion

        #region JUMP CONTROLLS
        if (Input.GetButton("Jump") && isOnGround && !GMController.Instance.cameraZoom)
        {
            rb.AddForce(jump * jumpForce, ForceMode.Impulse);
            isOnGround = false;
            animator.SetBool("isIdle", false);
            animator.SetBool("isRunning", false);
            animator.SetBool("isJumping", true);

        }
#endregion

        #region keyboardCameraZoom
        if (Input.GetButtonDown("Fire2"))
        {
            GMController.Instance.slowTime();

            playerCamera.transform.position = camBackPos.position;
            playerCamera.transform.rotation = transform.rotation;

            GMController.Instance.cameraZoom = true;
            isOnGround = false;
        }

        if (Input.GetButtonUp("Fire2"))
        {
            GMController.Instance.normalTime();

            playerCamera.transform.position = new Vector3(transform.position.x - 5, playerCamera.transform.position.y + 15, transform.position.z + 200);
            playerCamera.transform.eulerAngles = new Vector3(transform.rotation.x + 75, transform.rotation.y, transform.rotation.z);

            GMController.Instance.cameraZoom = false;
            isOnGround = true;
        }
        #endregion

        #region CONTROLLER CAMERA ZOOM
        if (Input.GetAxis("Fire2") != 0 && !GMController.Instance.cameraZoom)
        {
            //slow time down
            GMController.Instance.slowTime();

            //set camera position behind player
            playerCamera.transform.position = camBackPos.position;
            playerCamera.transform.rotation = transform.rotation;

            //set zoom to true
            GMController.Instance.cameraZoom = true;

            //activate slash plane
            slashPlane.SetActive(true);
            slashPlane.transform.GetChild(0).gameObject.SetActive(true);


            //sets original rotation of camera
            originalRot = transform.eulerAngles;
        }
        else if (Input.GetAxis("Fire2") == 0 && GMController.Instance.cameraZoom)
        {
            //resume time
            GMController.Instance.normalTime();

            //send camera back above player
            playerCamera.transform.position = new Vector3(transform.position.x - 5, playerCamera.transform.position.y + 15, transform.position.z + 200);
            playerCamera.transform.eulerAngles = new Vector3(transform.rotation.x + 75, transform.rotation.y, transform.rotation.z);

            //set zoom to false
            GMController.Instance.cameraZoom = false;

            //deactivate slash plane
            slashPlane.SetActive(false);
            slashPlane.transform.GetChild(0).gameObject.SetActive(false);

        }
        #endregion

        #region PLANE SPIN
        if (Input.GetAxis("RightStickHor") != 0 && GMController.Instance.cameraZoom)
        {
            slashPlane.transform.eulerAngles += new Vector3(0f, 0f, Input.GetAxis("RightStickHor") * -5);
        }
#endregion

        UpdateCamera();
    }

    //make camera follow player
    private void UpdateCamera()
    {
        if (!GMController.Instance.cameraZoom)
        {
            playerCamera.transform.position = new Vector3(transform.position.x, playerCamera.transform.position.y, transform.position.z - 2);
        }
    }

    //create sword object in front of player and parent the sword to the player
    private void SwingSword()
    {
        GameObject sword = Instantiate(swordSwinging, transform);
    }
 }
