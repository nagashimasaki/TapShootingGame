using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

[RequireComponent(typeof(Rigidbody2D))]
public class DefenseBase : MonoBehaviour
{
    [Header("拠点の耐久力")]
    public int durability;

    [SerializeField]
    private Text txtDurability;

    [SerializeField]
    private Slider slider;

    [SerializeField]
    private GameObject enemyAttackEffectPrefab;

    // 耐久力の最大値を代入しておく
    private int maxDurability;

    // GameManager スクリプトの情報を代入するための変数
    private GameManager gameManager;

    /// <summary>
    /// DefenseBaseの設定
    /// </summary>
    public void SetUpDefenceBase(GameManager gameManager)
    {

        // 引数を利用して、GameManager スクリプトの情報を受け取って、用意しておいた変数に代入
        this.gameManager = gameManager;

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

            // エネミーの攻撃演出用のエフェクト生成
            GenerateEnemyAttackEffect();

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

        // 耐久力が 0 以下になっていないか確認し、かつ、すでに isGameUp 変数が true になっていないかも確認
        if (durability <= 0 && gameManager.isGameUp == false)
        {

            Debug.Log("Game Over");

            // 耐久力が 0 以下なら、ゲーム終了とする判定を行う。ここで isGameUp 変数が true に切り替わるので、上の if 文の条件を満たさなくなり、この分岐内は１回しか処理されなくなる
            gameManager.SwitchGameUp(true);
        }
    }

    /// <summary>
    /// 耐久力の表示更新
    /// </summary>
    private void DisplayDurability()
    {

        // 画面に耐久力の値を　現在値 / 最大値　の形式で表示する
        txtDurability.text = durability + "  / " + maxDurability;

        // ゲージの表示を耐久力の値に合わせて更新(最初は durability / maxDurability の結果が 1.0f になるので、ゲージは最大値になる)
        slider.DOValue((float)durability / maxDurability, 0.25f);
    }

    /// <summary>
    /// エネミーが拠点に侵入した際の攻撃演出用のエフェクト生成
    /// </summary>
    private void GenerateEnemyAttackEffect()
    {

        // 拠点の位置(画面の中央)にエフェクトを生成
        GameObject enemyAttackEffect = Instantiate(enemyAttackEffectPrefab, transform, false);  //　　<=　☆　第2引数はどんな情報になっていますか？

        // 3秒後にエフェクトを破壊する
        Destroy(enemyAttackEffect, 3.0f);
    }
}
