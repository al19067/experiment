using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public Transform mainCamera;
    public float moveSpeed = 40.0f;
    public Vector3 moveDirection = Vector3.forward;
    public GameObject dotPrefab; // �h�b�g��\�����邽�߂̃v���n�u
    public float rayLength = 100f;   // ���C�̒���
    public float dotDisappearDistance = 0.5f; // �h�b�g�������鋗��

    private RaycastHit hit;  // �q�b�g�ʒu��ۑ�
    private GameObject currentDot = null;  // ���ݕ\������Ă���h�b�g�̎Q��

    // Start is called before the first frame update
    void Start()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main.transform;
        }

        if (dotPrefab == null)
        {
            Debug.LogError("Dot Prefab is not assigned!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        // �J�����̈ړ�
        mainCamera.Translate(moveDirection * moveSpeed * Time.deltaTime);

        if(mainCamera.position.x > -140f){
            // ���C���}�E�X�J�[�\���̕����ɔ�΂�
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            // ���C�������ɓ��������ꍇ�A���̈ʒu�Ƀh�b�g��\��
            if (Physics.Raycast(ray, out hit, rayLength)) // �S�Ẵ��C���[�ɑ΂��ă��C���΂�
            {
                // �q�b�g�����ʒu�Ƀh�b�g��\��
                ShowDot(hit.point);

                // �q�b�g�������W���R���\�[���ɏo��
                Debug.Log("Ray hit at: " + hit.point);
            }
            else
            {
                // �q�b�g���Ȃ������ꍇ�̓h�b�g���\���ɂ���
                HideDot();
            }
        }
        else
        {
            HideDot();
        }

        // �J�����̌����Ɋ�Â���]
        if (mainCamera.position.x < -40f)
        {
            mainCamera.rotation = Quaternion.Euler(0, 0, 0); // ����
            moveDirection = Vector3.forward; // �i�s�����𐳖ʂɐݒ�
        }
        if (mainCamera.position.z > 50f)
        {
            mainCamera.rotation = Quaternion.Euler(0, 270, 0); // ����
            moveDirection = Vector3.forward; // �i�s���������ɐݒ�
        }
        if (mainCamera.position.x < -90f && mainCamera.position.z > 0f)
        {
            mainCamera.rotation = Quaternion.Euler(0, 180, 0); // ���
            moveDirection = Vector3.forward; // �i�s���������ɐݒ�
        }
        if (mainCamera.position.x < -90f && mainCamera.position.x > -145f && mainCamera.position.z < 0f)
        {
            mainCamera.rotation = Quaternion.Euler(0, 270, 0); // ����
            moveDirection = Vector3.forward; // �i�s���������ɐݒ�
        }
        if (mainCamera.position.x < -140f)
        {
            moveSpeed = 0;
            return;
        }

        // �h�b�g�ƃJ�����̋��������ȉ��ɂȂ�����h�b�g������
        if (currentDot != null)
        {
            float distance = Vector3.Distance(mainCamera.position, currentDot.transform.position);
            if (distance < dotDisappearDistance)
            {
                HideDot(); // �J�������߂Â�����h�b�g���\���ɂ���
            }
        }
    }

    // �h�b�g��\�����邽�߂̃��\�b�h
    void ShowDot(Vector3 position)
    {
        // ���łɃh�b�g���\������Ă���ꍇ�A�ʒu���X�V
        if (currentDot != null)
        {
            currentDot.transform.position = position;  // �h�b�g�̈ʒu���X�V
        }
        else
        {
            // �h�b�g���\������Ă��Ȃ���ΐV���ɐ���
            currentDot = Instantiate(dotPrefab, position, Quaternion.identity);
            currentDot.transform.localScale = Vector3.one * 0.1f;  // �X�P�[�����Œ�
        }
    }

    // �h�b�g���\���ɂ��郁�\�b�h
    void HideDot()
    {
        if (currentDot != null)
        {
            Destroy(currentDot);  // �h�b�g���폜
            currentDot = null;
        }
    }

    // FixedUpdate�Ńh�b�g�̃X�P�[�����Œ�
    void FixedUpdate()
    {
        if (currentDot != null)
        {
            currentDot.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);  // ��ɃX�P�[�����Œ�
        }
    }
}
