using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{
    [SerializeField, Header("エネミーのプレファブ")]
    private EnemyController enemySetPrefab;

    [SerializeField, Header("エネミー生成までの準備時間")]
    public float preparateTime;

    private int generateCount;     // 生成したエネミーの数をカウントするための変数

    private float timer;           // 準備時間の計測用の変数

    private GameManager gameManager;

    [Header("エネミーの最大生成数")]
    public int maxGenerateCount;

    [Header("エネミーの生成完了管理用")]
    public bool isGenerateEnd;

    [Header("ボス討伐管理用")]
    public bool isBossDestroyed;

    [Header("エネミーのスクリプタブル・オブジェクト")]
    public EnemyDataSO enemyDataSO;

    // Normal タイプのエネミーのデータだけ代入されている List
    private List<EnemyDataSO.EnemyData> normalEnemyDatas = new List<EnemyDataSO.EnemyData>();　　　

    // Boss タイプのエネミーのデータだけ代入されている List
    private List<EnemyDataSO.EnemyData> bossEnemyDatas = new List<EnemyDataSO.EnemyData>();      　


    [Header("エネミー移動用のスクリプタブル・オブジェクト")]
    public MoveEventSO moveEventSO;

    [SerializeField, Header("生成したエネミーのリスト")]
    private List<EnemyController> enemiesList = new List<EnemyController>();

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

        // GameData よりエネミーの最大生成数を取得
        maxGenerateCount = GameData.instance.GetMaxGenerateCount();
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

                // 生成完了の状態にする = Update メソッドでこの値を利用して、生成完了状態になったら生成の準備処理が実行されないように制御する
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

        // プレファブからエネミーのクローンを生成する。生成位置は EnemyGenerator の位置。戻り値の値は EnemyController 型になる
        EnemyController enemyController = Instantiate(enemySetPrefab, transform, false);

        // EnemyController スクリプトの SetUpEnemy メソッドを実行する　=>　Start メソッドの代わりになる処理
        enemyController.SetUpEnemy(enemyData);

        // Boss 以外でも追加設定を行う
        enemyController.AdditionalSetUpEnemy(this);

        // List に生成したエネミーの情報を追加
        enemiesList.Add(enemyController);
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
        {

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

    /// <summary>
    /// TotalExp の表示更新準備
    /// </summary>
    /// <param name="exp"></param>
    public void PreparateDisplayTotalExp(int exp)
    {

        // GameManager スクリプトから UIManager スクリプトの UpdateDisplayTotalExp メソッドを実行する
        gameManager.uiManager.UpdateDisplayTotalExp(GameData.instance.GetTotalExp());

        // TODO 引数の exp 変数は後々利用する

    }

    /// <summary>
    /// プレイヤーとエネミーとの位置から方向を判定する準備
    /// </summary>
    /// <returns></returns>
    public Vector3 PreparateGetPlayerDirection(Vector3 enemyPos)
    {
        return gameManager.GetPlayerDirection(enemyPos);
    }

    /// <summary>
    /// enemiesList に登録されているエネミーのうち、リストに残っているエネミーのゲームオブジェクトを破壊し、enemiesList をクリアする
    /// </summary>
    public void ClearEnemiesList()
    {

        // enemiesList の要素(中身)を１つずつ順番に、要素の最大値になるまで判定していく
        for (int i = 0; i < enemiesList.Count; i++)
        {

            // 要素が空ではない(プレイヤーに破壊されずにゲーム画面に残っている)なら
            if (enemiesList[i] != null)
            {

                // そのエネミーのゲームオブジェクトを破壊する
                Destroy(enemiesList[i].gameObject);
            }
        }

        // リストをクリア(要素が何もない状態)にする
        enemiesList.Clear();
    }

    /// <summary>
    /// 一時オブジェクト(バレット、エフェクトなど)をすべて破棄(利用状況に応じて、①か②のいずれかの処理を実装する)
    /// </summary>
    public void DestroyTemporaryObjectContainer()
    {

        // ① TemporaryObjectContainer ゲームオブジェクトを破壊する(プロパティを利用している場合)
        Destroy(TransformHelper.GetTemporaryObjectContainerTran().gameObject);

        // ② TemporaryObjectContainer ゲームオブジェクトを破壊する(プロパティを利用していない場合)
        Destroy(TransformHelper.GetTemporaryObjectContainerTran().gameObject);
    }

}
