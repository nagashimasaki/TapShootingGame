using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // true になると、ゲームが終了している状態、と判断する
    [Header("ゲーム終了判定値")]
    public bool isGameUp;

    [SerializeField]
    private DefenseBase defenseBase;

    [SerializeField]
    private PlayerController playerController;

    [SerializeField]
    private EnemyGenerator enemyGenerator;

    [SerializeField]
    private Transform temporaryObjectContainerTran;

    public UIManager uiManager;

    void Start()
    {
        // ゲームが終了していない(未終了)状態 = false に切り替える
        SwitchGameUp(false);

        // DefenseBase スクリプトに用意した、DefenseBase の設定を行うための SetUpDefenseBase メソッドを呼び出す。引数として GameManager の情報を渡す
        defenseBase.SetUpDefenceBase(this);

        // PlayerController スクリプトに用意した、PlayerController の設定を行うための SetUpPlayer メソッドを呼び出す。引数として GameManager の情報を渡す
        playerController.SetUpPlayer(this);

        // EnemyGeneratorの初期設定
        enemyGenerator.SetUpEnemyGenerator(this);
        
        // TransformHelper スクリプトの temporaryObjectContainerTran 変数に情報を渡す
        TransformHelper.SetTemporaryObjectContainerTran(temporaryObjectContainerTran);

        // GameClearSet ゲームオブジェクトを見えない状態にする
        uiManager.HideGameClearSet();

        // GameOverSet ゲームオブジェクトを見えない状態にする
        uiManager.HideGameOverSet();
    }

    /// <summary>
    /// ゲーム終了状態の切り替え
    /// </summary>
    /// <param name="isSwitch"></param>
    public void SwitchGameUp(bool isSwitch)
    {

        // isGameUp の値を引数の値に切り替える
        isGameUp = isSwitch;

        // ゲーム終了の状態の場合
        if (isGameUp)
        {

            // 画面に残っているエネミーをすべて破壊する
            enemyGenerator.ClearEnemiesList();

            // 一時オブジェクトを破壊する(子オブジェクトである、ボスのエネミーのバレットやエフェクトなども一緒に破壊される)
            enemyGenerator.DestroyTemporaryObjectContainer();
        }
    }

    /// <summary>
    /// ゲームクリアの準備
    /// </summary>
    public void PreparateGameClear()
    {

        // ゲームクリアの表示を行う
        uiManager.DisplayGameClearSet();
    }

    /// <summary>
    /// ゲームオーバーの準備
    /// </summary>
    public void PreparateGameOver()
    {

        // ゲームオーバーの表示を行う
        uiManager.DisplayGameOverSet();
    }

    /// <summary>
    /// プレイヤーとエネミーとの位置から方向を判定
    /// </summary>
    /// <returns></returns>
    public Vector3 GetPlayerDirection(Vector3 enemyPos)
    {
        return (playerController.transform.position - enemyPos).normalized;
    }
}
