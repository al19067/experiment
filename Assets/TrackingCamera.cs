using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackingCamera : MonoBehaviour
{
    public Transform mainCamera;
    public float moveSpeed = 40.0f;
    public Vector3 moveDirection = Vector3.forward;
    public GameObject invisibleWall; // 透明な壁オブジェクト
    public GameObject dotPrefab; // ドットを表示するためのプレハブ
    public float rayLength = 100f;   // レイの長さ
    public float wallDistance = 5f; // カメラから壁までの距離

    private RaycastHit hit;  // ヒット位置を保存
    private GameObject currentDot = null;  // 現在表示されているドットの参照

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
        // カメラの移動
        mainCamera.Translate(moveDirection * moveSpeed * Time.deltaTime);

        // invisibleWallをカメラの前に配置
        // カメラの前に壁を固定する（距離を指定）
        invisibleWall.transform.position = mainCamera.position + mainCamera.forward * wallDistance; 

        // カメラとinvisibleWallの距離をコンソールに出力
        float distance = Vector3.Distance(mainCamera.position, invisibleWall.transform.position);
        Debug.Log("Camera to Invisible Wall Distance: " + distance);

        // レイをマウスカーソルの方向に飛ばす
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        // 透明な壁とレイの交差判定
        if (invisibleWall != null && invisibleWall.GetComponent<Collider>() != null)
        {
            // 透明な壁に対してレイを飛ばす
            if (Physics.Raycast(ray, out hit, rayLength, 1 << invisibleWall.layer)) // invisibleWallのレイヤーに対してのみレイを飛ばす
            {
                // ヒットした位置にドットを表示
                ShowDot(hit.point);
            }
            else
            {
                // ヒットしなかった場合はドットを非表示にする
                HideDot();
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
}