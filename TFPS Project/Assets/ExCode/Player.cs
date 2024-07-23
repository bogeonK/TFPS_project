using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Player : MonoBehaviour
{
    // 이동 속도
    public float moveSpeed;
    public float jumpPower; // 점프하는 힘

    PhotonView pv;  // 플레이어의 PhotonView 컴포넌트

    int jumpCount; // 점프 횟수

    Rigidbody rb; // 플레이어의 Rigidbody 컴포넌트


    // Start is called before the first frame update
    void Awake()
    {
        // 플레이어의 Rigidbody,PhotonView 컴포넌트를 가져와서 저장
        rb = GetComponent<Rigidbody>();
        pv = GetComponent<PhotonView>();

        // 내 캐릭터 일때만 실행
        if (pv.IsMine)
        {
            // Main Camera 검색해서 가져오기
            Transform camera = Camera.main.transform;

            // Main Camera의 부모를 나로 설정
            camera.SetParent(transform);

            // 나를 기준으로 적당한 위치로 이동
            camera.localPosition = new Vector3(0, 1.2f, 0.4f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // 내 캐릭터가 아니라면 함수를 탈출하여 아래 코드 실행 불가
        if (!pv.IsMine) return;

        // 방향키 또는 WASD키 입력을 숫자로 받아서 저장
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        // x축에서  h의 값을, z축에는 v의 값을 넣은 변수 생성
        Vector3 dir = new Vector3(h, 0, v);

        // 모든 방향의 속도가 동일하도록 정규화
        dir.Normalize();

        // 이동할 방향에 원하는 속도 곱하기 (모든 기기에서 동일한 속도)
        // transform.position += dir * moveSpeed * Time.deltaTime;

        // 물리 작용을 이용해 적용
        rb.MovePosition(rb.position + (dir * moveSpeed * Time.deltaTime)); 

        // Space 키를 누를 때, 점프한 횟수가 2회 미만
        if (Input.GetKey(KeyCode.Space) && jumpCount < 2)
        {
            // 위로 힘 발생
            rb.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);

            // 점프할 때마다 점프 횟수 증가
            jumpCount++;
        }
    }

    // 어떤 물체와 충돌을 시작한 순간에 호출
    private void OnCollisionEnter(Collision collision)
    {
        // 충돌한 물체의 태그가 "Ground"
        if(collision.gameObject.tag == "Ground")
        {
            //  점프 횟수 초기화
            jumpCount = 0;
        }
    }
}
