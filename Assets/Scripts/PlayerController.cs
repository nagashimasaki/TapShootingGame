using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Bullet ゲームオブジェクトのプレファブの情報を登録するための変数
    public GameObject bulletPrefab;

    private GameManager gameManager;

    void Update()
    {

        // ゲーム終了の状態になっているなら
        if (gameManager.isGameUp)
        {　　// ! が先頭についていないので、gameManager.isGameUp == true と同じ条件式

            // 画面のタップの処理が動作しなくなる
            return;
        }

        // マウスの左クリックをしたら
        if (Input.GetMouseButtonDown(0))
        {

            // 画面をタップ(クリック)した位置をカメラのスクリーン座標の情報を通じてワールド座標に変換
            Vector3 tapPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Debug.Log("タップした位置情報 : " + tapPos);

            // 方向を計算(マウスクリックの位置からキャラの位置を減算する)
            Vector3 direction = tapPos - transform.position;
            //Debug.Log("方向 : " + direction);

            // 方向の情報から、不要な Z成分(Z軸情報) の除去を行う
            direction = Vector3.Scale(direction, new Vector3(1, 1, 0));

            // 正規化処理を行い、単位ベクトルとする(方向の情報は持ちつつ、距離による速度差をなくして一定値にする)
            direction = direction.normalized;
            //Debug.Log("正規化処理後の方向 : " + direction);

            //上3行の処理を1行にまとめたもの
            //Vector3 direction = Vector3.Scale(tapPos - transform.position, new Vector3(1, 1, 0).normalized);
            // バレット生成
            GenerateBullet(direction);
        }
    }

    /// <summary>
    /// バレットの生成
    /// </summary>
    private void GenerateBullet(Vector3 direction)
    {

        // bulletPrefab 変数の値(Bullet ゲームオブジェクト)のクローンを生成し、戻り値を bulletObj 変数に代入。生成位置は PlayerSet ゲームオブジェクトの子オブジェクトを指定
        GameObject bulletObj = Instantiate(bulletPrefab, transform);  

        // bulletObj 変数(Bullet ゲームオブジェクトが代入されている)にアタッチされている Bullet スクリプトの情報を取得し、ShotBullet メソッドに処理を行うように命令を出す
        bulletObj.GetComponent<Bullet>().ShotBullet(direction);
    }

    /// <summary>
    /// PlayerControllerの設定
    /// </summary>
    /// <param name="gameManager"></param>
    public void SetUpPlayer(GameManager gameManager)
    {

        // PlayerController スクリプトの gameManager 変数に、引数で届いた GameManager スクリプトの情報を代入
        this.gameManager = gameManager;
    }
}
