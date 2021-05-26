using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider2D))]
public class EnemyController : MonoBehaviour
{

    void Update()
    {

        // このスクリプトがアタッチしているゲームオブジェクトを徐々に移動する
        transform.Translate(0, -0.01f, 0);

        // 一定地点までエネミーが移動したら = このゲームオブジェクトの位置が一定値(-1500)を超えたら
        if (transform.localPosition.y < -1500)
        {

            //  このスクリプトがアタッチしているゲームオブジェクトを破壊する
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// 侵入判定
    /// </summary>
    /// <param name="col"></param>
    private void OnTriggerEnter2D(Collider2D col)
    {

        // 侵入したコライダーのゲームオブジェクトの Tag が Bullet なら　=>　Enemy ゲームオブジェクトのコライダーに、Bullet ゲームオブジェクトのコライダーが侵入していたら
        if (col.gameObject.tag == "Bullet")
        {

            // 侵入判定の確認
            Debug.Log("侵入したオブジェクト名 : " + col.gameObject.name);

            // バレットのゲームオブジェクトを破壊する
            DestroyObjects(col);
        }
    }

    /// <summary>
    /// バレットとエネミーの破壊処理
    /// </summary>
    private void DestroyObjects(Collider2D col)
    {
        // 侵入判定の確認
        Debug.Log("侵入したオブジェクト名 : " + col.gameObject.tag);

        // バレットを破壊する
        Destroy(col.gameObject);

        // このゲームオブジェクトを破壊する
        Destroy(gameObject);
    }
}
