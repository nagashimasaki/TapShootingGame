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

    [Header("エネミーの最大生成数")]
    public int maxGenerateCount;

    [Header("エネミーの生成完了管理用")]
    public bool isGenerateEnd;

    [Header("ボス討伐管理用")]
    public bool isBossDestroyed;

    private void Start()
    {

        //ゲームスタート時には生成未完了の状態にする
        isGenerateEnd = false;

        //ゲームスタート時にはボスを未討伐状態にする
        isBossDestroyed = false;
    }

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

            // 今までに生成したエネミーの数が、エネミーの最大生成数の値と同じか超えたら
            if (generateCount >= maxGenerateCount)
            {

                // 生成完了の状態にする 
                isGenerateEnd = true;

                Debug.Log("生成完了");

                // ボスの生成
                StartCoroutine(GenerateBoss());

            }
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

        // 生成完了の状態になったら生成処理を行わないようにする
        if (isGenerateEnd)
        {
            return;
        }

        // ゲーム終了の状態ではないなら
        if (!gameManager.isGameUp)
        {     // ! が先頭についているので、gameManager.isGameUp == false と同じ条件式


            // エネミー生成の準備
            PreparateGenerateEnemy();
        }
    }

    /// <summary>
    /// ボスの生成
    /// </summary>
    private IEnumerator GenerateBoss()
    {

        // TODO ボス出現の警告演出
        Debug.Log("ボス出現の警告演出");


        yield return new WaitForSeconds(1.0f);

        // TODO ボス生成

        // TODO ボス討伐(仮)
        SwitchBossDestroyed(true);
    }

    /// <summary>
    /// ボス討伐状態の切り替え
    /// </summary>
    /// <param name="isSwitch"></param>
    public void SwitchBossDestroyed(bool isSwitch)
    {
        // ボスの討伐状態を切り替え
        isBossDestroyed = isSwitch;

        Debug.Log("ボス討伐");

        // ボス討伐に合わせて、ゲーム終了の状態に切り替える
        gameManager.SwitchGameUp(isBossDestroyed);


        // TODO ゲームクリアの準備

    }
}
