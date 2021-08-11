using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

[RequireComponent(typeof(CapsuleCollider2D))]
public class EnemyController : MonoBehaviour
{
    // SetUpEnemy メソッドにて引数として受けとったエネミーのデータを代入する
    [Header("エネミーのデータ情報")]
    public EnemyDataSO.EnemyData enemyData;

    // エネミーの画像の設定用　　　
    [SerializeField]
    private Image imgEnemy;

    [SerializeField]
    private Slider slider;

    // ヒット演出用のエフェクトのプレファブをインスペクターよりアサインして登録する
    [SerializeField]
    private GameObject bulletEffectPrefab;

    // HPの最大値を代入する変数
    private int maxHp;

    // エネミーのHP
    private int hp;

    // EnemyGenerator を利用するための変数
    private EnemyGenerator enemyGenerator;

    /// <summary>
    /// エネミーの設定
    /// </summary>
    public void SetUpEnemy(EnemyDataSO.EnemyData enemyData)
    {

        // 引数で届いた EnemyData を代入する
        this.enemyData = enemyData;

        // ボスではない場合
        if (this.enemyData.enemyType != EnemyType.Boss)
        {

            // エネミーの X 軸(左右)の位置を、ゲーム画面に収まる範囲でランダムな位置に変更
            transform.localPosition = new Vector3(transform.localPosition.x + Random.Range(-650, 650), transform.localPosition.y, 0);
        }
        else
        {

            // ボスの位置を徐々に下方向に変更
            //transform.DOLocalMoveY(transform.localPosition.y - 500, 3.0f);

            // ボスの場合、サイズを大きくする
            transform.localScale = Vector3.one * 2.0f;

            // Hpゲージの位置を高い位置にする
            slider.transform.localPosition = new Vector3(0, 150, 0);
        }

        // 画像を EnemyData の画像にする
        imgEnemy.sprite = this.enemyData.enemySprite;

        // EnemyData より Hp の値を最大値として代入
        maxHp = this.enemyData.hp;

        // hp を設定
        hp = maxHp;

        // Hpゲージの表示更新
        DisplayHpGauge();

        // 移動タイプに応じた移動方法を選択して実行
        SetMoveByMoveType();
    }

    void Update()
    {
        // ボス以外なら
        if (enemyData.enemyType != EnemyType.Boss)
        {

            // このスクリプトがアタッチしているゲームオブジェクトを徐々に移動する
            transform.Translate(0, -0.05f, 0);
        }
    }

    // 侵入判定
    private void OnTriggerEnter2D(Collider2D col)
    {

        // バレットが接触したら
        if (col.gameObject.tag == "Bullet")
        {

            // バレットの破壊処理を呼び出す(メソッド名を修正すれば、一緒に修正される)
            DestroyBullet(col);

            // 侵入してきたコライダーのゲームオブジェクトに Bullet スクリプトがアタッチされていたら取得して bullet 変数に代入して、if 文の中の処理を行う
            if (col.gameObject.TryGetComponent(out Bullet bullet))
            {

                // HPの更新処理とエネミーの破壊確認の処理を呼び出す
                UpdateHp(bullet);

                // バレットのヒット演出用エフェクトの生成
                GenerateBulletEffect(col.gameObject.transform);
            }
        }
    }

    /// <summary>
    /// バレットの破壊処理
    /// </summary>
    private void DestroyBullet(Collider2D col)
    {
        // バレットを破壊する
        Destroy(col.gameObject);
    }

    /// <summary>
    /// Hpの更新処理とエネミーの破壊確認処理
    /// </summary>
    private void UpdateHp(Bullet bullet)
    {
        // hpの減算処理
        hp -= bullet.bulletPower;

        // Hp の値の上限・下限を確認して範囲内に制限
        hp = Mathf.Clamp(hp, 0, maxHp);

        // HPゲージの表示更新
        DisplayHpGauge();

        if (hp <= 0)
        {
            hp = 0;

            // ボスの場合
            if (enemyData.enemyType == EnemyType.Boss)
            {

                // ボス討伐済みの状態にする
                enemyGenerator.SwitchBossDestroyed(true);
            }

            // Exp を TotalExp に加算
            GameData.instance.UpdateTotalExp(enemyData.exp);

            // 最新の TotapExp を利用して表示更新
            enemyGenerator.PreparateDisplayTotalExp(enemyData.exp);

            // このゲームオブジェクトを破壊する
            Destroy(gameObject);
        }
        else
        {

            Debug.Log("残り Hp : " + hp);
        }
    }

    /// <summary>
    /// HPゲージの表示更新
    /// </summary>
    private void DisplayHpGauge()
    {

        // HPゲージを現在値に合わせて制御
        slider.DOValue((float)hp / maxHp, 0.25f);
    }

    /// <summary>
    /// 被バレット時のヒット演出用のエフェクト生成
    /// </summary>
    　　/// <param name="bulletTran"></param>
    private void GenerateBulletEffect(Transform bulletTran)
    {

        // ヒット演出用のエフェクトを、バレットのぶつかった位置で生成
        GameObject effect = Instantiate(bulletEffectPrefab, bulletTran, false);

        // エフェクトをエネミーの子オブジェクトにする
        effect.transform.SetParent(transform);

        Destroy(effect, 3.0f);
    }

    /// <summary>
    /// エネミーの追加設定
    /// </summary>
    /// <param name="enemyGenerator"></param>
    public void AdditionalSetUpEnemy(EnemyGenerator enemyGenerator)
    {

        // 引数で届いた情報を変数に代入してスクリプト内で利用できる状態にする
        this.enemyGenerator = enemyGenerator;

        Debug.Log("追加設定完了");
    }

    /// <summary>
    /// 移動タイプに応じた移動方法を選択して実行
    /// </summary>
    private void SetMoveByMoveType()
    {

        // moveType で分岐
        switch (enemyData.moveType)
        {

            // Straight の場合
            case MoveType.Straight:
                MoveStraight();
                break;

            // Meandering の場合
            case MoveType.Meandering:
                MoveMeandering();
                break;

            // Boss_Horizontal の場合
            case MoveType.Boss_Horizontal:
                MoveBossHorizontal();
                break;

        }
    }

    /// <summary>
    /// 直進移動
    /// </summary>
    private void MoveStraight()
    {
        Debug.Log("直進");

        //transform.DOLocalMoveY(-3000, enemyData.moveDuration);
    }

    /// <summary>
    /// 蛇行移動
    /// </summary>
    private void MoveMeandering()
    {
        Debug.Log("蛇行");

        // 左右方向の移動をループ処理することで行ったり来たりさせる。左右の移動幅はランダム、移動間隔は等速
        transform.DOLocalMoveX(transform.position.x + Random.Range(200.0f, 400.0f), 1f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);

        //transform.DOLocalMoveY(-3000, enemyData.moveDuration);
    }

    /// <summary>
    /// ボス・水平移動
    /// </summary>
    public void MoveBossHorizontal()
    {

        transform.localPosition = new Vector3(0, transform.localPosition.y, transform.localPosition.z);

        transform.DOLocalMoveY(-600, 3.0f).OnComplete(() =>
        {

            Sequence sequence = DOTween.Sequence();

            // 右端に移動
            sequence.Append(transform.DOLocalMoveX(transform.localPosition.x + 550, 2.5f).SetEase(Ease.Linear));   

            // 左端に移動
            sequence.Append(transform.DOLocalMoveX(transform.localPosition.x - 550, 5.0f).SetEase(Ease.Linear)); 
            
            // 真ん中に移動
            sequence.Append(transform.DOLocalMoveX(transform.localPosition.x, 2.5f).SetEase(Ease.Linear));

            // 真ん中の地点に到達したら、一定時間待機する。その後上の処理を無制限にループする
            sequence.AppendInterval(1.0f).SetLoops(-1, LoopType.Restart);　　　　　　　　　　　　　　　　　　　  　
        });
    }
}