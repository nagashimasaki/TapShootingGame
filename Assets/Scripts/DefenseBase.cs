using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(Rigidbody2D))]
public class DefenseBase : MonoBehaviour
{
    [Header("拠点の耐久力")]
    public int durability;

    [SerializeField]
    private GameObject enemyAttackEffectPrefab;

    // FloatingMessageObj プレファブ・ゲームオブジェクトをインスペクターよりアサインして登録する
    [SerializeField]
    private FloatingMessage floatingMessagePrefab;    

    [SerializeField, Header("フロート表示を行う位置情報")]
    private Transform floatingDamageTran;

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

        // GameData より 耐久力を取得
        durability = GameData.instance.GetDurability();

        // ゲーム開始時点の耐久力の値を最大値として代入
        maxDurability = durability;

        // 耐久力の表示更新
        gameManager.uiManager.DisplayDurability(durability, maxDurability);
    }


    private void OnTriggerEnter2D(Collider2D col)
    {

        // 侵入したコライダーのゲームオブジェクトのタグに Enemy が付いていたら
        if (col.gameObject.tag == "Enemy")
        {

            // ダメージの設定用
            int damage = 0;

            // 侵入してきたコライダーをオフにする(重複判定を防ぐため)
            col.GetComponent<CapsuleCollider2D>().enabled = false;

            // 侵入してきたコライダーのゲームオブジェクトに Bullet スクリプトがアタッチされていたら取得して bulet 変数に代入して、if 文の中の処理を行う
            if (col.gameObject.TryGetComponent(out Bullet bullet))
            {
                // 攻撃力を元にダメージを決定
                damage = bullet.bulletPower;

                Debug.Log("バレットの攻撃力 : " + bullet.bulletPower);
            }

            // 上の if 文が処理されず、侵入してきたコライダーのゲームオブジェクトに EnemyController スクリプトがアタッチされていたら取得して enemy 変数に代入して、if 文の中の処理を行う
            else if (col.gameObject.TryGetComponent(out EnemyController enemy))
            {
                // 攻撃力を元にダメージを決定
                damage = enemy.enemyData.power;

                Debug.Log("エネミーの攻撃力 : " + enemy.enemyData.power);
            }

            // 耐久力の更新とゲームオーバーの確認
            UpdateDurability(damage);

            // エネミーからのダメージ値用のフロート表示の生成
            CreateFloatingMessageToDamage(damage);

            // エネミーの攻撃演出用のエフェクト生成
            GenerateEnemyAttackEffect(col.gameObject.transform);

            // エネミーのゲームオブジェクトを破壊する
            Destroy(col.gameObject);
        }
    }

    /// <summary>
    /// 耐久力の更新
    /// </summary>
    private void UpdateDurability(int damage)
    {

        // 耐久力の値を減算する
        durability -= damage;

        // エネミーの攻撃力を反映しているか確認
        Debug.Log("エネミーからのダメージ : " + damage);

        // 耐久力の値を上限・下限値の範囲内に収まるか確認し、それを超えた場合には上限・下限値に置き換えて制限する
        durability = Mathf.Clamp(durability, 0, maxDurability);

        // 減算結果の値を Console ビューに表示して計算されているか確認
        Debug.Log("残りの耐久力 ; " + durability);

        // 耐久力の表示更新
        gameManager.uiManager.DisplayDurability(durability, maxDurability);

        // 耐久力が 0 以下になっていないか確認し、かつ、すでに isGameUp 変数が true になっていないかも確認
        if (durability <= 0 && gameManager.isGameUp == false)
        {

            Debug.Log("Game Over");

            // 耐久力が 0 以下なら、ゲーム終了とする判定を行う。ここで isGameUp 変数が true に切り替わるので、上の if 文の条件を満たさなくなり、この分岐内は１回しか処理されなくなる
            gameManager.SwitchGameUp(true);

            // ゲームオーバーの準備
            gameManager.PreparateGameOver();
        }
    }

    /// <summary>
    /// エネミーが拠点に侵入した際の攻撃演出用のエフェクト生成
    /// </summary>
    private void GenerateEnemyAttackEffect(Transform enemyTran)
    {

        // 拠点の位置(画面の中央)にエフェクトを生成
        GameObject enemyAttackEffect = Instantiate(enemyAttackEffectPrefab, enemyTran, false);

        // 生成されたエフェクトを TemporaryObjectContainerTran の子オブジェクトにする(引数に GetTemporaryObjectContainerTran メソッドの戻り値を利用)
        enemyAttackEffect.transform.SetParent(TransformHelper.GetTemporaryObjectContainerTran());

        // 3秒後にエフェクトを破壊する
        Destroy(enemyAttackEffect, 3.0f);
    }

    /// <summary>
    /// エネミーからのダメージ値用のフロート表示の生成
    /// </summary>
    /// <param name="damage"></param>
    private void CreateFloatingMessageToDamage(int damage)
    {

        // フロート表示の生成。生成位置は DefenseBase ゲームオブジェクト内の txtDurability ゲームオブジェクトの位置(子オブジェクト)
        FloatingMessage floatingMessage = Instantiate(floatingMessagePrefab, floatingDamageTran, false);

        // 生成したフロート表示の設定用メソッドを実行。引数として、エネミーからのダメージ値とフロート表示の種類を指定して渡す
        floatingMessage.DisplayFloatingMessage(damage, FloatingMessage.FloatingMessageType.PlayerDamage);
    }
}
