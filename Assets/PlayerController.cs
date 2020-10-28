using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region  Variables
    private RotateToMouse rotateToMouse;
    private Movement movement;
    private Status status;
    private WeaponAssaultRifle weapon;

    [SerializeField]
    private KeyCode keyCodeRun = KeyCode.LeftShift;
    [SerializeField]
    private KeyCode keyCodeJump = KeyCode.Space;
    [SerializeField]
    private KeyCode keyCodeReload = KeyCode.R;
    #endregion

    private void Awake()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        rotateToMouse = this.GetComponent<RotateToMouse>();
        movement = this.GetComponent<Movement>();
        status = this.GetComponent<Status>();
        weapon = this.GetComponentInChildren<WeaponAssaultRifle>();
    }
      
    private void Update()
    {
        UpdateRotate();
        UpdateMove();
        UpdateJump();
        UpdateAttack();
        UpdateReload();
    }

    private void UpdateRotate()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        rotateToMouse.UpdateRotate(mouseX, mouseY);
    }

    private void UpdateMove()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        if (horizontal != 0 || vertical != 0)
        {
            if (Input.GetKey(keyCodeRun))
            {
                movement.MoveSpeed = status.RunSpeed;
                weapon.Animator.SetFloat("movementSpeed", 1.0f);
            }
            else
            {
                movement.MoveSpeed = status.WalkSpeed;
                weapon.Animator.SetFloat("movementSpeed", 0.5f);
            }
        }
        else
        {
            weapon.Animator.SetFloat("movementSpeed", 0.0f);
        }

        movement.UpdateMove(horizontal, vertical);
    }

    private void UpdateJump()
    {
        if(Input.GetKeyDown(keyCodeJump))
        {
            bool isPossibleJump = movement.Jump();

            if(isPossibleJump)
            {
                weapon.Animator.SetTrigger("onJump");
            }
        }
    }

    private void UpdateAttack()
    {
        if(Input.GetMouseButton(0))
        {
            weapon.StartAttack();
        }
        else if(Input.GetMouseButtonUp(0))
        {
            weapon.StopAttack();
        }
    }

    private void UpdateReload()
    {
        if(Input.GetKeyDown(keyCodeReload))
        {
            weapon.StartReload();
        }
    }

    public void TakeDamage(int damage)
    {
        bool isDie = status.DecreaseHP(damage);

        if(isDie == true)
        {
            Debug.Log("Game Over");
        }
    }
}