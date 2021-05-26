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

            // 耐久力の値を減算する
            durability -= 10;

            // 減算結果の値を Console ビューに表示して計算されているか確認
            Debug.Log("残りの耐久力 ; " + durability);

            // エネミーのゲームオブジェクトを破壊する
            Destroy(col.gameObject);
        }
    }
}
