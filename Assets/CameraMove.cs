using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public Transform mainCamera;
    public float moveSpeed = 40.0f;
    public Vector3 moveDirection = Vector3.forward;
    public GameObject dotPrefab; // ドットを表示するためのプレハブ
    public float rayLength = 100f;   // レイの長さ
    public float dotDisappearDistance = 0.5f; // ドットが消える距離

    private RaycastHit hit;  // ヒット位置を保存
    private GameObject currentDot = null;  // 現在表示されているドットの参照

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
        // カメラの移動
        mainCamera.Translate(moveDirection * moveSpeed * Time.deltaTime);

        if(mainCamera.position.x > -140f){
            // レイをマウスカーソルの方向に飛ばす
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            // レイが何かに当たった場合、その位置にドットを表示
            if (Physics.Raycast(ray, out hit, rayLength)) // 全てのレイヤーに対してレイを飛ばす
            {
                // ヒットした位置にドットを表示
                ShowDot(hit.point);

                // ヒットした座標をコンソールに出力
                Debug.Log("Ray hit at: " + hit.point);
            }
            else
            {
                // ヒットしなかった場合はドットを非表示にする
                HideDot();
            }
        }
        else
        {
            HideDot();
        }

        // カメラの向きに基づく回転
        if (mainCamera.position.x < -40f)
        {
            mainCamera.rotation = Quaternion.Euler(0, 0, 0); // 正面
            moveDirection = Vector3.forward; // 進行方向を正面に設定
        }
        if (mainCamera.position.z > 50f)
        {
            mainCamera.rotation = Quaternion.Euler(0, 270, 0); // 左側
            moveDirection = Vector3.forward; // 進行方向を左に設定
        }
        if (mainCamera.position.x < -90f && mainCamera.position.z > 0f)
        {
            mainCamera.rotation = Quaternion.Euler(0, 180, 0); // 後ろ
            moveDirection = Vector3.forward; // 進行方向を後ろに設定
        }
        if (mainCamera.position.x < -90f && mainCamera.position.x > -145f && mainCamera.position.z < 0f)
        {
            mainCamera.rotation = Quaternion.Euler(0, 270, 0); // 左側
            moveDirection = Vector3.forward; // 進行方向を左に設定
        }
        if (mainCamera.position.x < -140f)
        {
            moveSpeed = 0;
            return;
        }

        // ドットとカメラの距離が一定以下になったらドットを消す
        if (currentDot != null)
        {
            float distance = Vector3.Distance(mainCamera.position, currentDot.transform.position);
            if (distance < dotDisappearDistance)
            {
                HideDot(); // カメラが近づいたらドットを非表示にする
            }
        }
    }

    // ドットを表示するためのメソッド
    void ShowDot(Vector3 position)
    {
        // すでにドットが表示されている場合、位置を更新
        if (currentDot != null)
        {
            currentDot.transform.position = position;  // ドットの位置を更新
        }
        else
        {
            // ドットが表示されていなければ新たに生成
            currentDot = Instantiate(dotPrefab, position, Quaternion.identity);
            currentDot.transform.localScale = Vector3.one * 0.1f;  // スケールを固定
        }
    }

    // ドットを非表示にするメソッド
    void HideDot()
    {
        if (currentDot != null)
        {
            Destroy(currentDot);  // ドットを削除
            currentDot = null;
        }
    }

    // FixedUpdateでドットのスケールを固定
    void FixedUpdate()
    {
        if (currentDot != null)
        {
            currentDot.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);  // 常にスケールを固定
        }
    }
}
