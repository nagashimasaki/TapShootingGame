using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D))]
public class DefenseBase : MonoBehaviour
{
    [Header("拠点の耐久力")]
    public int durability;

    [SerializeField]
    private Text txtDurability;

    private int maxDurability;     // 耐久力の最大値を代入しておく

    void Start()
    {

        // ゲーム開始時点の耐久力の値を最大値として代入
        maxDurability = durability;

        // 耐久力の表示更新
        DisplayDurability();
    }


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

        // 耐久力の値を上限・下限値の範囲内に収まるか確認し、それを超えた場合には上限・下限値に置き換えて制限する
        durability = Mathf.Clamp(durability, 0, maxDurability);

        // 減算結果の値を Console ビューに表示して計算されているか確認
        Debug.Log("残りの耐久力 ; " + durability);

        // 耐久力の表示更新
        DisplayDurability();

        // TODO ゲージの表示を耐久力の値に合わせて更新
        // TODO 耐久力が 0 以下になっていないか確認
        // TODO 耐久力が 0 以下なら、ゲームオーバーの判定を行う
    }

    /// <summary>
    /// 耐久力の表示更新
    /// </summary>
    private void DisplayDurability()
    {

        // 画面に耐久力の値を　現在値 / 最大値　の形式で表示する
        txtDurability.text = durability + "  / " + maxDurability;

        // TODO ゲージの表示を耐久力の値に合わせて更新
    }

}
