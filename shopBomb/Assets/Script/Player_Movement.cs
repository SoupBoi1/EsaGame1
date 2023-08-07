using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class Player_Movement : MonoBehaviour
{
    //gameobject/componets
    public CharacterController CharacterController;
    public Transform groundCheck;
    public Transform CameraT;
    public Player_Camera camera;
    public GameManage gameManager;
    public GameObject HoldObj;

    // changeable public paramenter
    public float speed = 12f;
    public bool Jumpable;
    public float MinJumpHieght = 2f;
    public float JumpHieght = 5f;
    public int JumpAmount = 2;
    public float isGroundedRadius = .3f;
    public bool isMoveable = true;
    public bool isJumpable = true;

    //private value
    public float JumpBhold;
    public Vector3 vilocity;
    public bool isGrounded;
    public Vector3 gravity;
    Vector3 moveDir;
    float v_move;
    float h_move;
    Vector2 moveDirtion;
    private bool fall;
    Vector3 FJumpPosition;
    Vector3 LJumpPosition;
    bool interacting;
    RaycastHit hit;
    GameObject hoverObj;


    //input map
    public InputActionAsset actionsAssests;
    InputActionMap onFootMap;
    InputActionMap utilityMap;
    InputAction moveAction;
    InputAction jumpAction;
    InputAction interactAction;
    InputAction pauseAction;


    
    // Start is called before the first frame update

   

    void Start()
    {
        gravity = Physics.gravity;

        //rayHit = camera.hit; 

        onFootMap = actionsAssests.FindActionMap("OnFoot");
        utilityMap = actionsAssests.FindActionMap("Utility");
        onFootMap.Enable();
        utilityMap.Enable();
        pauseAction = utilityMap.FindAction("pause");

        moveAction = onFootMap.FindAction("Move");
        interactAction = onFootMap.FindAction("Interact");
        jumpAction = onFootMap.FindAction("Jump");

        moveAction.performed += context => OnMove(context);
        moveAction.canceled += ctx => OnMove(ctx);

        jumpAction.performed += context => OnJump(context);
        jumpAction.canceled += ctx => OnJump(ctx);

        interactAction.performed += context => OnInteract(context);
        interactAction.canceled += ctx => OnInteract(ctx);

        //    pauseAction.performed+= context => gameManager.PauseGame();
        pauseAction.performed += ctx => gameManager.PauseingGame();



    }

    // Update is called once per frame

    private void FixedUpdate()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, isGroundedRadius);

        int layerMask = 1 << 8;
        layerMask = ~layerMask;

        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(CameraT.position, CameraT.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, layerMask))
        {
            Debug.DrawRay(CameraT.position, CameraT.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);

            print(hit.collider.gameObject.tag);
       
        }
        else
        {
            Debug.DrawRay(CameraT.position, CameraT.TransformDirection(Vector3.forward) * 1000, Color.white);
            //Debug.Log("Did not Hit");
        }

    }

    void Update()
    {
        Move(moveDirtion);
        
        if (isGrounded == true)
        {
            
            vilocity.y = -2f;
            

        }
        else
        {
            
            vilocity += gravity*2*Time.deltaTime;
        }
        Jump();
        CharacterController.Move(vilocity* Time.deltaTime);
    }



    private void OnMove(InputAction.CallbackContext context)
    {

        moveDirtion = context.ReadValue<Vector2>();
    }

    private void Move(Vector2 D)
    {
        if (isMoveable) { 
        v_move = D.y * speed * Time.deltaTime;
        h_move = D.x * speed * Time.deltaTime;

        Vector3 moveDir = (transform.forward * v_move) + (transform.right * h_move);
        CharacterController.Move(moveDir);
        }

    }

    void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Jumpable = true;
            FJumpPosition = transform.position;
        }
        else if (context.canceled)
        {
            Jumpable = false;
            fall = true;
            LJumpPosition = transform.position;
        }
    }

    void Jump()
    {
        //bool fall;
        if (Jumpable && isJumpable)
        {
            JumpBhold -= Time.deltaTime;

            if (isGrounded == true)
            {
                vilocity.y += Mathf.Sqrt(JumpHieght * -2 * gravity.y);
               
            }

        }

        else if (Jumpable == false)
        {
            LJumpPosition = transform.position;
            float DealtY = Mathf.Abs( LJumpPosition.y- FJumpPosition.y );
            
            if (DealtY <= MinJumpHieght && fall == true)
            {
                print("EEEE" + DealtY);
                vilocity.y = 0;
                fall = false;

            }

        }
    }


    void OnInteract(InputAction.CallbackContext context)
    {
        
        if (context.performed)
        {
        

        }
        else if (context.canceled)
        {
            if (!interacting)
            {
                 PickUpItem(hit.collider.gameObject);
                interacting = true;
            }
            else
            {
                PlaceItem();
                interacting = false;
            }

           
        }
    }

    /**
     * 
     * places what ever object under place holder
     */
     
    private void PlaceItem()
    {
        //TODO
    }

    /**
     * take the object with tag of item and places it in the placehold object
     */
    void PickUpItem(GameObject item)
    {
        GameObject go = item;
        //  if (go.GetComponent<Item>())
        //{
        if (go.tag == "Item")
        {
            go.transform.position = HoldObj.transform.position;
            go.transform.parent = HoldObj.transform;
            BoxCollider c = go.GetComponent<BoxCollider>();
            //go.transform.position = Vector3.zero;
            c.enabled = false;
            go.layer = 8;

        }
    }

}
