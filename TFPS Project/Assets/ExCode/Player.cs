using UnityEngine;
using Photon.Pun;
using System.Collections;

public class PlayerController : MonoBehaviourPunCallbacks
{
    [Header("Base setup")]
    public float moveSpeed = 7.5f;
    public float runningSpeed = 11.5f;
    public float jumpPower = 8.0f;
    public float lookSpeed = 2.0f;
    public float lookXLimit = 45.0f;

    [Header("Laser Setup")]
    public float laserLength = 10f;
    public Color laserColor = Color.red;

    private Rigidbody rb;
    private Camera playerCamera;
    private PhotonView pv;
    private LineRenderer laserLine;

    private int jumpCount = 0;
    private float rotationX = 0;
    private PlayerController otherPlayer;

    [SerializeField]
    private float cameraYOffset = 1.2f;

    private bool isViewingOther = false;
    private bool hasViewSwitched = false;
    private float viewSwitchDelay = 5f;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        pv = GetComponent<PhotonView>();

        playerCamera = new GameObject("PlayerCamera").AddComponent<Camera>();
        playerCamera.transform.SetParent(transform);
        playerCamera.transform.localPosition = new Vector3(0, cameraYOffset, 0);

        if (pv.IsMine)
        {
            playerCamera.gameObject.SetActive(true);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            StartCoroutine(AutoSwitchViewAfterDelay());
        }
        else
        {
            playerCamera.gameObject.SetActive(false);
        }

        // LineRenderer 설정
        laserLine = gameObject.AddComponent<LineRenderer>();
        laserLine.startWidth = 0.05f;
        laserLine.endWidth = 0.05f;
        laserLine.material = new Material(Shader.Find("Sprites/Default"));
        laserLine.startColor = laserColor;
        laserLine.endColor = laserColor;
        laserLine.positionCount = 2;
    }

    void Update()
    {
        if (pv.IsMine)
        {
            HandleMovement();
            HandleMouseInput();
            UpdateLaserLine();
        }
    }

    private void HandleMovement()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        bool isRunning = Input.GetKey(KeyCode.LeftShift);

        Vector3 forward = transform.forward;
        Vector3 right = transform.right;

        Vector3 moveDirection = forward * v + right * h;
        moveDirection.Normalize();

        float currentSpeed = isRunning ? runningSpeed : moveSpeed;
        rb.MovePosition(rb.position + moveDirection * currentSpeed * Time.deltaTime);

        if (Input.GetKey(KeyCode.Space) && jumpCount < 2)
        {
            rb.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
            jumpCount++;
        }
    }

    private void HandleMouseInput()
    {
        float mouseY = -Input.GetAxis("Mouse Y") * lookSpeed;
        rotationX += mouseY;
        rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);

        float mouseX = Input.GetAxis("Mouse X") * lookSpeed;
        transform.rotation *= Quaternion.Euler(0, mouseX, 0);

        // 시점 변경 중에도 자신의 캐릭터 회전을 네트워크로 동기화
        photonView.RPC("SyncRotation", RpcTarget.All, rotationX, transform.rotation);
    }

    [PunRPC]
    private void SyncRotation(float rotX, Quaternion bodyRotation)
    {
        if (!pv.IsMine)
        {
            rotationX = rotX;
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation = bodyRotation;
        }
    }

    private void UpdateLaserLine()
    {
        Vector3 start = playerCamera.transform.position;
        Vector3 end = start + playerCamera.transform.forward * laserLength;

        laserLine.SetPosition(0, start);
        laserLine.SetPosition(1, end);

        // 레이저 라인 정보를 네트워크로 동기화
        photonView.RPC("SyncLaserLine", RpcTarget.All, start, end);
    }

    [PunRPC]
    private void SyncLaserLine(Vector3 start, Vector3 end)
    {
        laserLine.SetPosition(0, start);
        laserLine.SetPosition(1, end);
    }

    private IEnumerator AutoSwitchViewAfterDelay()
    {
        yield return new WaitForSeconds(viewSwitchDelay);
        if (!hasViewSwitched)
        {
            ToggleView();
            hasViewSwitched = true;
        }
    }

    private void ToggleView()
    {
        isViewingOther = !isViewingOther;

        if (isViewingOther)
        {
            // 다른 플레이어 찾기
            PlayerController[] players = FindObjectsOfType<PlayerController>();
            foreach (PlayerController player in players)
            {
                if (player != this)
                {
                    otherPlayer = player;
                    break;
                }
            }

            if (otherPlayer != null)
            {
                playerCamera.gameObject.SetActive(false);
                otherPlayer.playerCamera.gameObject.SetActive(true);
                // 레이저 라인은 계속 활성화 상태를 유지합니다.
            }
        }
        else
        {
            playerCamera.gameObject.SetActive(true);
            if (otherPlayer != null)
            {
                otherPlayer.playerCamera.gameObject.SetActive(false);
            }
        }

        // 시점 전환 시 네트워크로 동기화
        photonView.RPC("SyncViewToggle", RpcTarget.All, isViewingOther);
    }

    [PunRPC]
    private void SyncViewToggle(bool isViewing)
    {
        isViewingOther = isViewing;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            jumpCount = 0;
        }
    }
}