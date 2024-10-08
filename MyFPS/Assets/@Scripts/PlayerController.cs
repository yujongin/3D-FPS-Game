using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float walkingSpeed = 7;
    public float mouseSens = 1;
    public Transform cameraTransform;
    float horizontalAngle;
    float verticalAngle;
    InputAction moveAction;
    InputAction lookAction;

    CharacterController characterController;
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        InputActionAsset inputActions = GetComponent<PlayerInput>().actions;

        moveAction = inputActions.FindAction("Move");
        lookAction = inputActions.FindAction("Look");

        characterController = GetComponent<CharacterController>();
        verticalAngle = 0;
        horizontalAngle = transform.localEulerAngles.y;
    }

    void Update()
    {
        //Move 평행이동
        Vector2 moveVector = moveAction.ReadValue<Vector2>();

        //3차원 이동이기 때문에 y축이아닌 z축으로 움직일 수 있도록 vector3로 변환
        Vector3 move = new Vector3(moveVector.x, 0, moveVector.y);

        // 대각선으로 움직일 때 벡터의 크기가 1이 넘가기 때문에 Nomalize를 통해 크기를 1로 줄임
        if (move.magnitude > 1)
        {
            move.Normalize();
        }

        //스피드와 deltaTime을 곱해서 원하는 속도로 움직이게 조정
        move = move * walkingSpeed * Time.deltaTime;

        // 현재 gameObject의 방향으로 벡터를 돌린다. 쳐다보는 방향에 따라 좌우앞뒤 벡터가 달라지도록 하는 것
        move = transform.TransformDirection(move);

        //CharacterController를 사용해 움직임
        characterController.Move(move);

        //좌우 방향
        Vector2 look = lookAction.ReadValue<Vector2>();
        float turnPlayer = look.x * mouseSens;
        horizontalAngle += turnPlayer;

        if (horizontalAngle >= 360) horizontalAngle -= 360;
        if (horizontalAngle < 0) horizontalAngle += 360;

        Vector3 currentAngle = transform.localEulerAngles;
        currentAngle.y = horizontalAngle;
        transform.localEulerAngles = currentAngle;

        //마우스 상하
        float turnCam = look.y * mouseSens;
        verticalAngle -= turnCam;
        verticalAngle = Mathf.Clamp(verticalAngle, -89f, 89f);
        currentAngle = cameraTransform.localEulerAngles;
        currentAngle.x = verticalAngle;
        cameraTransform.localEulerAngles = currentAngle;

    }
}
