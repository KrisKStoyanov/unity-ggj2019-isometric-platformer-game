using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharController : MonoBehaviour
{
    public AudioSource playerAS;

    public Camera charCamera;

    public CharacterController charController;
    public Rigidbody rBody;

    public Vector2 inputDetector = Vector2.zero;
    public Vector2 joystickLook = Vector2.zero;
    public Vector3 velocity = Vector3.zero;

    public Transform armPivot;

    private Quaternion lastEular;

    public float walkSpeed = 8f;
    public float runSpeed = 16f;
    public float transitionSpeed = 1.0f;
   
    public float jumpForce = 10.0f;

    public bool walking = false;
    public bool sprinting = false;

    public float movementSpeed = 0.0f;

    public float fallAmp = 2.5f;

    public Room curRoom;
    public Transform intPos;

    public float rangeToInteract = 5.0f;
    public float throwForce = 500.0f;

    public Vector3 lookRotation = Vector3.zero;
    public Vector3 movement = Vector3.zero;
    public Vector3 pickupOffset = Vector3.zero;

    //Animation properties
    private Animator charAnimator;

    static readonly int anim_Jump = Animator.StringToHash("Jump");
    static readonly int anim_Fall = Animator.StringToHash("Fall");
    static readonly int anim_PickUp = Animator.StringToHash("PickUp");
    static readonly int anim_Throw = Animator.StringToHash("Throw");

    private bool anim_Jumping = false;
    private bool anim_Falling = false;
    private bool anim_PickingUp = false;
    private bool anim_Throwing = false;

    public bool carrying = false;

    public PhysicalItem heldItem;

    //Keymapping - Dual Player Setup
    public string playerTag;

    //Audio control mapping
    public AudioSource PlayerAudio;
    
    public void Awake()
    {

        playerTag = gameObject.transform.tag;

        charController = GetComponent<CharacterController>();
        rBody = GetComponent<Rigidbody>();
        charAnimator = armPivot.GetComponent<Animator>();
        playerAS = GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //Movement mapping
        inputDetector = new Vector2(Input.GetAxis(playerTag + "_Horizontal"), -Input.GetAxis(playerTag + "_Vertical"));
        joystickLook = new Vector2(Input.GetAxis(playerTag + "_Joystick_Horizontal"), -Input.GetAxis(playerTag + "_Joystick_Vertical"));

        float currentSpeed;

        if (Input.GetAxis(playerTag + "_Sprint") != 0)
        //if(Input.GetKeyDown(key_Sprint))
        {
            currentSpeed = runSpeed;
            sprinting = true;
            walking = false;
        }
        else
        {
            currentSpeed = walkSpeed;
            sprinting = false;
            walking = true;
        }
        
        if (charController.isGrounded && Input.GetButtonDown(playerTag + "_Jump"))
        {
            Jump(jumpForce);
        }
        else
        {
            Fall(fallAmp);
        }

        if(inputDetector != Vector2.zero)
        {
            movementSpeed = Mathf.Lerp(movementSpeed, currentSpeed, Time.deltaTime * transitionSpeed);
        }
        else
        {
            movementSpeed = transitionSpeed;
        }

        //movement = new Vector3(axis_Movement.normalized.x, 0, axis_Movement.normalized.y) * movementSpeed;
        //lookRotation = new Vector3(axis_Direction.normalized.x, 0, axis_Direction.normalized.y);
        movement = new Vector3(inputDetector.x, 0, inputDetector.y) * movementSpeed;
        lookRotation = new Vector3(joystickLook.x, 0, joystickLook.y);

        //Camera perspective movement
        Quaternion rotator = Quaternion.Euler(0, charCamera.transform.eulerAngles.y, 0);
        movement = rotator * movement;
        lookRotation = rotator * lookRotation;

        bool changedRot = false;
        if (movement != Vector3.zero)
        {
            lastEular = transform.rotation = Quaternion.LookRotation(movement);
            changedRot = true;
        }
        if (lookRotation != Vector3.zero)
        {
            lastEular = transform.rotation = Quaternion.LookRotation(lookRotation);
            changedRot = true;
        }
        if (!changedRot)
        {
            transform.rotation = lastEular;
        }

        //Interactive System
        if (Input.GetButtonDown(playerTag + "_Interact"))
        //if(Input.GetKeyDown(key_Interact))
        {
            if(heldItem != null)
            {
                ThrowItem(heldItem.gameObject, charController.transform.forward, throwForce);
            }
            else
            {
                if(curRoom.ReturnClosestInteractible(charController.transform.position, rangeToInteract) != null)
                {
                    PhysicalItem tempPhysItem = curRoom.ReturnClosestInteractible(charController.transform.position, rangeToInteract).GetComponent<PhysicalItem>();
                    if (!tempPhysItem.isPickedUp)
                    {
                        PickUp(tempPhysItem.gameObject);
                    }
                }  
            }
        }

        movement += velocity;

        charController.Move(movement * Time.deltaTime);
    }

    public void Jump(float _jumpAmp)
    {
        anim_Jumping = true;
        anim_Falling = false;
        velocity.y = 0.0f;
        velocity = new Vector3(velocity.x, _jumpAmp, velocity.z);
        charAnimator.SetBool(anim_Jump, anim_Jumping);
        charAnimator.SetBool(anim_Fall, anim_Falling);

        //playerAS.PlayOneShot(AudioManager.instance.jumpFallSounds[Random.Range(0, AudioManager.instance.jumpFallSounds.Length)]);
    }

    public void Fall(float _fallAmp)
    {
        if (heldItem == null)
        {
            anim_Jumping = false;
            anim_Falling = true;
            charAnimator.SetBool(anim_Jump, anim_Jumping);
            charAnimator.SetBool(anim_Fall, anim_Falling);
        }

        velocity += Physics.gravity * Time.deltaTime * _fallAmp;
    }

    public void PickUp(GameObject interactiveObj)
    {
        heldItem = interactiveObj.GetComponent<PhysicalItem>();
        heldItem.transform.position = intPos.position + pickupOffset;
        heldItem.transform.SetParent(intPos.parent);
        heldItem.GetComponent<Rigidbody>().isKinematic = true;
        heldItem.isPickedUp = true;

        anim_Jumping = true;
        anim_Falling = false;

        charAnimator.SetBool(anim_Jump, anim_Jumping);
        charAnimator.SetBool(anim_Fall, anim_Falling);

        playerAS.PlayOneShot(AudioManager.instance.interactSounds[Random.Range(0, AudioManager.instance.interactSounds.Length)]);
    }

    public void ThrowItem(GameObject interactiveObj, Vector3 direction, float force)
    {
        heldItem.transform.SetParent(null);
        heldItem.GetComponent<Rigidbody>().isKinematic = false;
        heldItem.GetComponent<Rigidbody>().velocity = Vector3.zero;
        heldItem.GetComponent<Rigidbody>().AddForce(direction.normalized * force);
        heldItem.isPickedUp = false;
        heldItem = null;

        anim_Jumping = false;
        anim_Falling = true;

        charAnimator.SetBool(anim_Jump, anim_Jumping);
        charAnimator.SetBool(anim_Fall, anim_Falling);

        playerAS.PlayOneShot(AudioManager.instance.throwSounds[Random.Range(0, AudioManager.instance.throwSounds.Length)]);
    }
    
}
