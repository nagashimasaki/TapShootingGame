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

    [Header("エネミーのスクリプタブル・オブジェクト")]
    public EnemyDataSO enemyDataSO;

    // Normal タイプのエネミーのデータだけ代入されている List Debug　用に public
    public List<EnemyDataSO.EnemyData> normalEnemyDatas = new List<EnemyDataSO.EnemyData>();

    public List<EnemyDataSO.EnemyData> bossEnemyDatas = new List<EnemyDataSO.EnemyData>();

    /// <summary>
    /// EnemyGenerator の設定
    /// </summary>
    /// <param name="gameManager"></param>
    public void SetUpEnemyGenerator(GameManager gameManager)
    {

        // EnemyGenerator スクリプトの gameManager 変数に、引数で届いた GameManager スクリプトの情報を代入
        this.gameManager = gameManager;

        // 引数で指定したエネミーのタイプのリストを作成
        normalEnemyDatas = GetEnemyTypeList(EnemyType.Normal);

        bossEnemyDatas = GetEnemyTypeList(EnemyType.Boss);
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
    private void GenerateEnemy(EnemyType enemyType = EnemyType.Normal)
    {

        // ランダムな値を代入するための変数を宣言
        int randomEnemyNo;

        // EnemyData を代入するための変数を宣言
        EnemyDataSO.EnemyData enemyData = null;

        // EnemyType に合わせて生成するエネミーの種類を決定し、そのエネミーの種類ごとのリストからランダムな EnemyData を取得
        switch (enemyType)
        {
            case EnemyType.Normal:
                randomEnemyNo = Random.Range(0, normalEnemyDatas.Count);
                enemyData = normalEnemyDatas[randomEnemyNo];
                break;
            case EnemyType.Boss:
                randomEnemyNo = Random.Range(0, bossEnemyDatas.Count);
                enemyData = bossEnemyDatas[randomEnemyNo];
                break;
        }

        // プレファブからエネミーのクローンを生成する。生成位置は EnemyGenerator の位置
        GameObject enemySetObj = Instantiate(enemySetPrefab, transform, false);    //  <=  左辺に GameObject 型の変数を用意して、インスタンスされたエネミーの情報を戻り値で受け取る

        // エネミーのゲームオブジェクト(enemySetObj 変数の値)にアタッチされている EnemyController スクリプトの情報を取得して変数に代入
        EnemyController enemyController = enemySetObj.GetComponent<EnemyController>();

        // EnemyController スクリプトの SetUpEnemy メソッドを実行する
        enemyController.SetUpEnemy(enemyData);　　　　　　　

        // 追加設定を行う
        enemyController.AdditionalSetUpEnemy(this);
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

        yield return new WaitForSeconds(1.0f);

        // ボス生成
        GenerateEnemy(EnemyType.Boss);
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

        // ゲームクリアの準備
        gameManager.PreparateGameClear();
    }

    /// <summary>
    /// 引数で指定されたエネミーの種類のListを作成し、作成した値を戻す
    /// </summary>
    /// <param name="enemyType"></param>
    /// <returns></returns>
    private List<EnemyDataSO.EnemyData> GetEnemyTypeList(EnemyType enemyType)
    {

        List<EnemyDataSO.EnemyData> enemyDatas = new List<EnemyDataSO.EnemyData>();

        // 引数のタイプのエネミーのデータだけを抽出して enemyDatas リストに EnemyData を追加して、List を作成していく
        for (int i = 0; i < enemyDataSO.enemyDataList.Count; i++)
        {
            if (enemyDataSO.enemyDataList[i].enemyType == enemyType)
            {
                enemyDatas.Add(enemyDataSO.enemyDataList[i]);
            }
        }

        // 抽出処理の結果を戻す
        return enemyDatas;
    }

}
