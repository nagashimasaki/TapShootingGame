using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Bullet ゲームオブジェクトのプレファブの情報を登録するための変数
    public GameObject bulletPrefab;　　　

    void Update()
    {

        // マウスの左クリックをしたら
        if (Input.GetMouseButtonDown(0))
        {

            // バレット生成
            GenerateBullet();
        }
    }

    /// <summary>
    /// バレットの生成
    /// </summary>
    private void GenerateBullet()
    {

        // bulletPrefab 変数の値(Bullet ゲームオブジェクト)のクローンを生成し、戻り値を bulletObj 変数に代入。生成位置は PlayerSet ゲームオブジェクトの子オブジェクトを指定
        GameObject bulletObj = Instantiate(bulletPrefab, transform);  //　<=　☆　代入処理に修正

        // bulletObj 変数(Bullet ゲームオブジェクトが代入されている)にアタッチされている Bullet スクリプトの情報を取得し、ShotBullet メソッドに処理を行うように命令を出す
        bulletObj.GetComponent<Bullet>().ShotBullet();

    }
}
