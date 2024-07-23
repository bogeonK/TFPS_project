using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFire : MonoBehaviour
{
    // �� ȿ�� ������ ��Ƶ� ����
    public GameObject shootEffectPref;

    // Start is called before the first frame update
    void Start()
    {
        // ���콺 Ŀ�� �����
        Cursor.visible = false;

        // ���콺 Ŀ���� ����ȭ���� ����� ���ϰ� ���
        Cursor.lockState = CursorLockMode.Confined;
    }

    // Update is called once per frame
    void Update()
    {
        // ���콺 ��Ŭ�� 
        if (Input.GetMouseButtonDown(0))
        {
            // ȭ�� ������� �����ϴ� Ray ����
            Ray ray = Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f));

            // Ray ���� ��ü�� ��Ƶ� ����
            RaycastHit hit;

            // Ray�� �߻��ϰ�, Ray�� ���� ��ü�� hit�� ����, ���� ��ü�� ��������
            if(Physics.Raycast(ray, out hit))
            {
                // ���� ��ġ��, ���� ǥ���� ������ �Ǵ� ������ �� ȿ�� �������� ���纻 ����
                GameObject shootEffect = Instantiate(shootEffectPref, hit.point + hit.normal * 0.01f, Quaternion.LookRotation(hit.normal));

                // �Ѿ� �ڱ��� ���� ������Ʈ�� �ڽ����� ����
                shootEffect.transform.SetParent(hit.transform);
                
                // Ray�� ���� ��ü�� ���̶��
                if (hit.transform.tag == "Enemy")
                {
                    // ������ 10��ŭ ������
                    hit.transform.SendMessage("Damaged", 10);
                }

            }
        }
    }
}
