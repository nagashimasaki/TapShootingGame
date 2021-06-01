using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{
    [SerializeField, Header("エネミーのプレファブ")]
    private GameObject enemySetPrefab;

    [SerializeField, Header("エネミー生成までの準備時間")]
    public float preparateTime;

    // 生成したエネミーの数をカウントするための変数
    private int generateCount;

    // 準備時間の計測用の変数
    private float timer;

    private GameManager gameManager;

    /// <summary>
    /// EnemyGenerator の設定
    /// </summary>
    /// <param name="gameManager"></param>
    public void SetUpEnemyGenerator(GameManager gameManager)
    {

        // EnemyGenerator スクリプトの gameManager 変数に、引数で届いた GameManager スクリプトの情報を代入
        this.gameManager = gameManager;
    }

    /// <summary>
    /// エネミー生成の準備
    /// </summary>
    private void PreparateGenerateEnemy()
    {

        // 時間の計測
        timer += Time.deltaTime;

        // 準備時間を超えたら
        if (timer >= preparateTime)
        {

            // 次回の時間の計測を行うためにリセット
            timer = 0;

            // エネミーの生成
            GenerateEnemy();

            // 生成したエネミーの数をカウントアップ
            generateCount++;

            Debug.Log("生成したエネミーの数 : " + generateCount);
        }
    }

    /// <summary>
    /// エネミーの生成
    /// </summary>
    private void GenerateEnemy()
    {

        // プレファブからエネミーのクローンを生成する。生成位置は EnemyGenerator の位置
        // エネミーのゲームオブジェクトにアタッチされている EnemyController スクリプトの情報を取得して変数に代入
        // EnemyController スクリプトの SetUpEnemy メソッドを実行する　=>　Start メソッドの代わりになる処理
        Instantiate(enemySetPrefab, transform, false).GetComponent<EnemyController>().SetUpEnemy();
    }

    void Update()
    {

        // ゲーム終了の状態ではないなら
        if (!gameManager.isGameUp)
        {     // ! が先頭についているので、gameManager.isGameUp == false と同じ条件式


            // エネミー生成の準備
            PreparateGenerateEnemy();
        }
    }
}
