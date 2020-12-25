using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Rigidbody2D PlayerRB;
    Animator PlayerAM;
    public float Speed;
    public float JumpForce;
    bool isGround;

    GameObject RayCasted_Ground;

    void Awake()
    {
        PlayerRB = GetComponent<Rigidbody2D>();
        PlayerAM = GetComponent<Animator>();

        // 스피드 설정 안되면 자동 초기화
        if (Speed == 0)
            Speed = 5;

        // 점프 설정 안되면 자동 초기화
        if (JumpForce == 0)
            JumpForce = 10;
    }


    // Update is called once per frame
    void Update()
    {
        KeyBoardInput();
        DrawRayCheckGround();

    }

    private void DrawRayCheckGround()
    {
        Collider2D r;


        Debug.DrawRay(PlayerRB.position, Vector2.down, new Color(0, 1, 0));
        RaycastHit2D rayHit = Physics2D.Raycast(PlayerRB.position, Vector2.down, 1, LayerMask.GetMask("Ground"));
        if (rayHit.collider != null)
        {
            RayCasted_Ground = rayHit.collider.gameObject;

            r = RayCasted_Ground.GetComponent<Collider2D>();
            if (r.isTrigger)
                r.isTrigger = false;
        }
        else
        {
            if (RayCasted_Ground != null)
            {
                r = RayCasted_Ground.GetComponent<Collider2D>();
                if (!r.isTrigger)
                    r.isTrigger = true;
                RayCasted_Ground = null;
            }

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
        if (Input.GetKeyDown(KeyCode.UpArrow) && isGround)
        {
            PlayerRB.velocity = new Vector2(0, JumpForce);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isGround = true;
            PlayerAM.SetBool("Jump", false);
        }
    }
    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isGround = false;
            PlayerAM.SetBool("Jump", true);
        }
    }
}
