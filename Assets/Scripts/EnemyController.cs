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
}
