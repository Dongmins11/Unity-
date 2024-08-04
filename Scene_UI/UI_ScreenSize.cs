using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_ScreenSize : MonoBehaviour
{
    private Vector2 previousScreenSize;
    public Vector3 initialPosition;
    public Vector3 initialScale;

    void Start()
    {
        previousScreenSize = new Vector2(Screen.width, Screen.height);
        initialPosition = transform.position;
        initialScale = transform.localScale;

        UI_ScaleUpdate();
    }

    void Update()
    {
        UI_ScaleUpdate();
    }

    void UpdateUI()
    {
        if (Screen.width != previousScreenSize.x || Screen.height != previousScreenSize.y)
        {
            previousScreenSize.x = Screen.width;
            previousScreenSize.y = Screen.height;

            float screenWidth = Screen.width;
            float screenHeight = Screen.height;

            float currentRatio = screenWidth / screenHeight;

            float newSizeX = initialScale.x * currentRatio;
            float newSizeY = initialScale.y * currentRatio;
            float sizeZ = 1.0f;

            transform.localScale = new Vector3(newSizeX, newSizeY, sizeZ);
        }

    }

    void UI_ScaleUpdate()
    {
        if (Screen.width != previousScreenSize.x || Screen.height != previousScreenSize.y)
        {
            // ����� ȭ�� ũ�⸦ ����
            previousScreenSize.x = Screen.width;
            previousScreenSize.y = Screen.height;


            // ���� ȭ���� ����, ���� ���� ��� (��: 1920x1080 ȭ��)
            float screenWidth = 1920f;
            float screenHeight = 1080f;
            float currentAspect = screenWidth / screenHeight;

            // ���ϴ� ���� (��: 16:9)
            float targetAspect = 16f / 9f;

            // ���� ������ ���ϴ� ������ ���� ���
            float ratio = currentAspect / targetAspect;

            // ������ �� ����
            Vector3 newScale = initialScale;
            newScale.x *= ratio; // ���� ���� ����

            // ������ �� ����
            Vector3 newPosition = initialPosition;
            newPosition.x *= ratio; // ���� ���� ����

            // Quad�� �����ϰ� ������ ������Ʈ
            transform.localScale = newScale;
            transform.position = newPosition;
        }
    }

}

