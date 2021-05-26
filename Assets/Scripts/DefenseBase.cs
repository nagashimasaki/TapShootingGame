using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class DefenseBase : MonoBehaviour
{
    [Header("拠点の耐久力")]
    public int durability;

    private void OnTriggerEnter2D(Collider2D col)
    {

        if (col.gameObject.tag == "Enemy")
        {

            // 侵入してきたコライダーのゲームオブジェクトに EnemyController スクリプトがアタッチされていたら取得して enemy 変数に代入
            if (col.gameObject.TryGetComponent(out EnemyController enemy))
            {

                //耐久力の更新処理を呼び出す
                UpdateDurability(enemy);
            }

            // エネミーのゲームオブジェクトを破壊する
            Destroy(col.gameObject);
        }
    }

    /// <summary>
    /// 耐久力の更新
    /// </summary>
    private void UpdateDurability(EnemyController enemy)
    {

        // 耐久力の値を減算する
        durability -= enemy.attackPower;
        // 減算結果の値を Console ビューに表示して計算されているか確認
        Debug.Log("残りの耐久力 ; " + durability);

        // TODO 耐久力が 0 以下になっていないか確認
        // TODO 耐久力が 0 以下なら、ゲームオーバーの判定を行う
    }

}
