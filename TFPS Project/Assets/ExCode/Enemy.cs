using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;   // UI���� Ŭ���� ���

public class Enemy : MonoBehaviour
{
    // �� ü�¹�
    public Slider hpBar;
    // �� ü��
    public float hp = 100;

    // ���� �޴� ���
    void Damaged(float damage)
    {
        // ���� ���� ��������ŭ ü�� ����
        hp -= damage;

        // ������ ü���� hp�ٿ� ǥ��
        hpBar.value = hp;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
