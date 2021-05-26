using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// このスクリプトがアタッチしているゲームオブジェクトにアタッチされている Rigidbody2D コンポーネントを取得する
[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour
{
    [Header("バレットの速度")]
    public float bulletSpeed;

    [Header("バレットの攻撃力")]
    public int bulletPower;

    /// <summary>
    /// バレットの制御
    /// </summary>
    public void ShotBullet(Vector3 direction)
    {
        // バレットの移動処理
        // バレットのゲームオブジェクトにアタッチされている Rigidbody2D コンポーネントを取得して、Rigidbody2D クラスの持つ AddForce メソッドを実行する
        // 発射する方向と速度を変更
        GetComponent<Rigidbody2D>().AddForce(direction * bulletSpeed);

       // Debug.Logでこの処理が実行されているか確認
        Debug.Log("発射速度 : " + bulletSpeed);

        // ５秒後にこのスクリプトがアタッチされているゲームオブジェクト(つまり、バレット)を破壊する
        Destroy(gameObject, 5f);
    }
}
