using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerPrefab : MonoBehaviour
{
    public RuntimeAnimatorController[] animCon;
    private Animator anim;
    private SpriteRenderer spriter;
    private Vector3 lastPosition;
    private Vector3 currentPosition;
    private uint playerId;
    TextMeshPro myText;

    private Vector2 velocityVec;
    private Vector2 lastDirectionVec;
    
    void Awake()
    {
        anim = GetComponent<Animator>();
        spriter = GetComponent<SpriteRenderer>();
        myText = GetComponentInChildren<TextMeshPro>();
    }

    public void Init(uint playerId, string id)
    {
        anim.runtimeAnimatorController = animCon[playerId];
        lastPosition = Vector3.zero;
        currentPosition = Vector3.zero;

        velocityVec = Vector2.zero;
        lastDirectionVec = Vector2.zero;

        this.playerId = playerId;

        if (id.Length > 5) {
            myText.text = id[..5];
        } else {
            myText.text = id;
        }
        myText.GetComponent<MeshRenderer>().sortingOrder = 6;
    }

    void OnEnable()
    {    
        anim.runtimeAnimatorController = animCon[playerId];
    }

    // 서버로부터 위치 업데이트를 수신할 때 호출될 메서드
    public void UpdatePosition(float x, float y, float velX, float velY)
    {
        lastPosition = currentPosition;
        currentPosition = new Vector3(x, y);
        //transform.position = currentPosition;
        transform.position = Vector2.Lerp(transform.position, currentPosition, Time.deltaTime);

        velocityVec = new Vector2 (velX, velY);

        if (velocityVec.x > 0) { lastDirectionVec = Vector2.right; }
        if (velocityVec.x < 0) { lastDirectionVec = Vector2.left; }

        UpdateAnimation();
    }

    void LateUpdate()
    {
        if (!GameManager.instance.isLive)
        {
            return;
        }

        UpdateAnimation();
    }

    private void UpdateAnimation()
    {
        // 현재 위치와 이전 위치를 비교하여 이동 벡터 계산
        // Vector2 inputVec = currentPosition - lastPosition;

        anim.SetFloat("Speed", velocityVec.magnitude);

        //Debug.Log($"{lastDirectionVec}");

        if (velocityVec.x != 0)
        {
            //Debug.Log($"{lastDirectionVec}");
            spriter.flipX = lastDirectionVec.x < 0;
        }
        //spriter.flipX = lastDirectionVec.x < 0;
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (!GameManager.instance.isLive)
        {
            return;
        }
    }
}
