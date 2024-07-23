using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Player : MonoBehaviour
{
    // �̵� �ӵ�
    public float moveSpeed;
    public float jumpPower; // �����ϴ� ��

    PhotonView pv;  // �÷��̾��� PhotonView ������Ʈ

    int jumpCount; // ���� Ƚ��

    Rigidbody rb; // �÷��̾��� Rigidbody ������Ʈ


    // Start is called before the first frame update
    void Awake()
    {
        // �÷��̾��� Rigidbody,PhotonView ������Ʈ�� �����ͼ� ����
        rb = GetComponent<Rigidbody>();
        pv = GetComponent<PhotonView>();

        // �� ĳ���� �϶��� ����
        if (pv.IsMine)
        {
            // Main Camera �˻��ؼ� ��������
            Transform camera = Camera.main.transform;

            // Main Camera�� �θ� ���� ����
            camera.SetParent(transform);

            // ���� �������� ������ ��ġ�� �̵�
            camera.localPosition = new Vector3(0, 1.2f, 0.4f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // �� ĳ���Ͱ� �ƴ϶�� �Լ��� Ż���Ͽ� �Ʒ� �ڵ� ���� �Ұ�
        if (!pv.IsMine) return;

        // ����Ű �Ǵ� WASDŰ �Է��� ���ڷ� �޾Ƽ� ����
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        // x�࿡��  h�� ����, z�࿡�� v�� ���� ���� ���� ����
        Vector3 dir = new Vector3(h, 0, v);

        // ��� ������ �ӵ��� �����ϵ��� ����ȭ
        dir.Normalize();

        // �̵��� ���⿡ ���ϴ� �ӵ� ���ϱ� (��� ��⿡�� ������ �ӵ�)
        // transform.position += dir * moveSpeed * Time.deltaTime;

        // ���� �ۿ��� �̿��� ����
        rb.MovePosition(rb.position + (dir * moveSpeed * Time.deltaTime)); 

        // Space Ű�� ���� ��, ������ Ƚ���� 2ȸ �̸�
        if (Input.GetKey(KeyCode.Space) && jumpCount < 2)
        {
            // ���� �� �߻�
            rb.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);

            // ������ ������ ���� Ƚ�� ����
            jumpCount++;
        }
    }

    // � ��ü�� �浹�� ������ ������ ȣ��
    private void OnCollisionEnter(Collision collision)
    {
        // �浹�� ��ü�� �±װ� "Ground"
        if(collision.gameObject.tag == "Ground")
        {
            //  ���� Ƚ�� �ʱ�ȭ
            jumpCount = 0;
        }
    }
}
