using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public float rotCamXAxisSpeed = 5;
    [SerializeField]
    float rotCamYAxisSpeed = 3;

    public float limitMinX = -80;
    public float limitMaxX = 50;
    float eulerAngleX;
    float eulerAngleY;

    public void UpdateRotate(float mouseX, float mouseY)
    {
        eulerAngleY += mouseX * rotCamYAxisSpeed;
        eulerAngleX -= mouseY * rotCamXAxisSpeed; // 마우스 아래는 - 이므로 오브젝트 x축이 + 방향으로 회전해야 아래를 보기 때문

        // 카메라 x축 회전의 범위 설정 (위아래)
        eulerAngleX = ClampAngle(eulerAngleX, limitMinX, limitMaxX);

        transform.rotation = Quaternion.Euler(eulerAngleX, eulerAngleY, 0);
    }

    float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360) angle += 360;
        if (angle > 360) angle -= 360;

        return Mathf.Clamp(angle, min, max);
    }
}
