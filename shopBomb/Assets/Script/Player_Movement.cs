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
    public GameManage gameManager; 

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

    //input map
    public InputActionAsset actionsAssests;
    InputActionMap onFootMap;
    InputActionMap utilityMap;
    InputAction moveAction;
    InputAction jumpAction;
    InputAction pauseAction;


    
    // Start is called before the first frame update

   

    void Start()
    {
        gravity = Physics.gravity;

        onFootMap = actionsAssests.FindActionMap("OnFoot");
        utilityMap = actionsAssests.FindActionMap("Utility");
        onFootMap.Enable();
        utilityMap.Enable();
        pauseAction = utilityMap.FindAction("pause");

        moveAction = onFootMap.FindAction("Move");

        jumpAction = onFootMap.FindAction("Jump");

        moveAction.performed += context => OnMove(context);
        moveAction.canceled += ctx => OnMove(ctx);

        jumpAction.performed += context => OnJump(context);
        jumpAction.canceled += ctx => OnJump(ctx);
        
    //    pauseAction.performed+= context => gameManager.PauseGame();
        pauseAction.canceled += ctx => gameManager.PauseingGame();

    }

    // Update is called once per frame

    private void FixedUpdate()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, isGroundedRadius);
 
        
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

    
}
