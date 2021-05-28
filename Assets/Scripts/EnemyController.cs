using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;      
using DG.Tweening;

[RequireComponent(typeof(CapsuleCollider2D))]
public class EnemyController : MonoBehaviour
{

    [Header("エネミーのHP")]
    public int hp;

    [Header("エネミーの攻撃力")]
    public int attackPower;

    [SerializeField]
    private Slider slider;

    // HPの最大値を代入する変数
    private int maxHp;

    /// <summary>
    /// エネミーの設定
    /// </summary>
    public void SetUpEnemy()
    {     

        // エネミーの X 軸(左右)の位置を、ゲーム画面に収まる範囲でランダムな位置に変更
        transform.localPosition = new Vector3(transform.localPosition.x + Random.Range(-650, 650), transform.localPosition.y, 0);

        // ゲーム開始時点のHpの値を最大値として代入
        maxHp = hp;

        // Hpゲージの表示更新
        DisplayHpGauge();
    }

    void Update()
    {

        // このスクリプトがアタッチしているゲームオブジェクトを徐々に移動する
        transform.Translate(0, -0.01f, 0);

        // 一定地点までエネミーが移動したら = このゲームオブジェクトの位置が一定値(-1500)を超えたら
        if (transform.localPosition.y < -3000)
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

        // 侵入したコライダーのゲームオブジェクトの Tag が Bullet なら
        if (col.gameObject.tag == "Bullet")
        {

            // 侵入判定の確認
            Debug.Log("侵入したオブジェクト名 : " + col.gameObject.name);

            // バレットのゲームオブジェクトを破壊する
            DestroyBullet(col);

            // 侵入してきたコライダーのゲームオブジェクトに Bullet スクリプトがアタッチされていたら取得して bullet 変数に代入して、if 文の中の処理を行う
            if (col.gameObject.TryGetComponent(out Bullet bullet))
            {
                // HPの更新処理とエネミーの破壊確認の処理を呼び出す
                UpdateHp(bullet);
            }
        }
    }

    /// <summary>
    /// バレットとエネミーの破壊処理
    /// </summary>
    private void DestroyBullet(Collider2D col)
    {

        // 侵入判定の確認
        Debug.Log("侵入したオブジェクト名 : " + col.gameObject.tag);

        // バレットを破壊する
        Destroy(col.gameObject);
    }

    /// <summary>
    /// Hpの更新処理とエネミーの破壊確認処理
    /// </summary>
    private void UpdateHp(Bullet bullet)
    {

        // hp 変数から 15 減らす　(hpの減算処理)
        hp -= bullet.bulletPower;

        // Hp の値の上限・下限を確認して範囲内に制限
        hp = Mathf.Clamp(hp, 0, maxHp);

        // HPゲージの表示更新
        DisplayHpGauge();

        // hp が 0 以下になったら
        if (hp <= 0)
        {
            hp = 0;

            // エネミーの破壊処理を行う  
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
}
