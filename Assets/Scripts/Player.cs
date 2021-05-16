using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Rigidbody2D PlayerRB;
    Animator PlayerAM;
    CapsuleCollider2D PlayerCC;
    public float Speed;
    public float JumpForce;
    bool isGround;
    bool isMainGround;
    float JumpCooltime;

    void Awake()
    {
        PlayerRB = GetComponent<Rigidbody2D>();
        PlayerAM = GetComponent<Animator>();
        PlayerCC = GetComponent<CapsuleCollider2D>();

        // 스피드 설정 안되면 자동 초기화
        if (Speed == 0)
            Speed = 5;

        // 점프 설정 안되면 자동 초기화
        if (JumpForce == 0)
            JumpForce = 9.4f;
    }


    // Update is called once per frame
    void Update()
    {
        KeyBoardInput();
        DrawRayCheckGround();

        if (JumpCooltime > 0)
            JumpCooltime -= Time.deltaTime;
        else
            if (PlayerCC.isTrigger)
                PlayerCC.isTrigger = false;

        Debug.Log(isMainGround);
    }

    void DrawRayCheckGround()
    {

        Debug.DrawRay(PlayerRB.position + new Vector2(-0.16f * transform.localScale.x, 0), Vector2.down, new Color(0, 1, 0));
        RaycastHit2D rayHit = Physics2D.Raycast(PlayerRB.position + new Vector2(-0.16f * transform.localScale.x, 0), Vector2.down, 1, LayerMask.GetMask("Ground"));
        RaycastHit2D rHMainGround = Physics2D.Raycast(PlayerRB.position + new Vector2(-0.16f * transform.localScale.x, 0), Vector2.down, 1, LayerMask.GetMask("MainGround"));

        if (rayHit.collider != null)
            if (rayHit.distance < 0.6f)
            {
                if (PlayerRB.velocity.y < 0)
                {
                    isGround = true;
                    PlayerAM.SetBool("Jump", false);
                }
            }
            else
            {
                isGround = false;
                PlayerAM.SetBool("Jump", true);
            }
        else if(rHMainGround.collider != null)
            if (rHMainGround.distance < 0.6f)
            {
                if (PlayerRB.velocity.y < 0)
                {
                    isGround = true;
                    isMainGround = true;
                    PlayerAM.SetBool("Jump", false);
                }
            }
            else
            {
                isGround = false;
                isMainGround = false;
                PlayerAM.SetBool("Jump", true);
            }
        else
        {
            isGround = false;
            isMainGround = false;
            PlayerAM.SetBool("Jump", true);
        }
    }

    void KeyBoardInput()
    {
        PlayerRB.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * Speed, PlayerRB.velocity.y);
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))
        {
            // 움직임
            if (Input.GetKey(KeyCode.LeftArrow))
                transform.localScale = new Vector3(1, 1, 1);
            else
                transform.localScale = new Vector3(-1, 1, 1);

            // 애니메이션
            PlayerAM.SetBool("Run", true);
            PlayerAM.speed = Speed/3.3f;
        }
        else
        {
            PlayerAM.SetBool("Run", false);
            PlayerAM.speed = 1;
        }

        // 점프     
        if (!Input.GetKey(KeyCode.DownArrow) && Input.GetKey(KeyCode.LeftAlt) && isGround && JumpCooltime <= 0)
        {
            PlayerRB.velocity = new Vector2(0, JumpForce);
            JumpCooltime = 0.35f;
        }

        // 하강 점프  ( 메인 그라운드가 아닐때만 사용가능 )
        if (Input.GetKey(KeyCode.DownArrow) && Input.GetKey(KeyCode.LeftAlt) && isGround && JumpCooltime <= 0)
        {
            if (!isMainGround && PlayerRB.velocity.y >= 0) 
            {
                PlayerRB.velocity = new Vector2(0, 4);
                PlayerCC.isTrigger = true;
                JumpCooltime = 0.35f;
            }
        }

        
    }

}
