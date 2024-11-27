using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackingCamera : MonoBehaviour
{
    public Transform mainCamera;
    public float moveSpeed = 40.0f;
    public Vector3 moveDirection = Vector3.forward;
    public GameObject invisibleWall; // �����ȕǃI�u�W�F�N�g
    public GameObject dotPrefab; // �h�b�g��\�����邽�߂̃v���n�u
    public float rayLength = 100f;   // ���C�̒���
    public float wallDistance = 5f; // �J��������ǂ܂ł̋���

    private RaycastHit hit;  // �q�b�g�ʒu��ۑ�
    private GameObject currentDot = null;  // ���ݕ\������Ă���h�b�g�̎Q��

    // Start is called before the first frame update
    void Start()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main.transform;
        }

        if (invisibleWall == null)
        {
            Debug.LogError("Invisible wall is not assigned!");
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

        // invisibleWall���J�����̑O�ɔz�u
        // �J�����̑O�ɕǂ��Œ肷��i�������w��j
        invisibleWall.transform.position = mainCamera.position + mainCamera.forward * wallDistance; 

        // �J������invisibleWall�̋������R���\�[���ɏo��
        float distance = Vector3.Distance(mainCamera.position, invisibleWall.transform.position);
        Debug.Log("Camera to Invisible Wall Distance: " + distance);

        // ���C���}�E�X�J�[�\���̕����ɔ�΂�
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        // �����ȕǂƃ��C�̌�������
        if (invisibleWall != null && invisibleWall.GetComponent<Collider>() != null)
        {
            // �����ȕǂɑ΂��ă��C���΂�
            if (Physics.Raycast(ray, out hit, rayLength, 1 << invisibleWall.layer)) // invisibleWall�̃��C���[�ɑ΂��Ă̂݃��C���΂�
            {
                // �q�b�g�����ʒu�Ƀh�b�g��\��
                ShowDot(hit.point);
            }
            else
            {
                // �q�b�g���Ȃ������ꍇ�̓h�b�g���\���ɂ���
                HideDot();
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
}