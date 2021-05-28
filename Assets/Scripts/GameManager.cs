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

    void Start()
    {
        // ゲームが終了していない(未終了)状態 = false に切り替える
        SwitchGameUp(false);

        // DefenseBase スクリプトに用意した、DefenseBase の設定を行うための SetUpDefenseBase メソッドを呼び出す。引数として GameManager の情報を渡す
        defenseBase.SetUpDefenceBase(this);　　//← SetUpDefenceBase の fence の c が s になっていた
    }

    /// <summary>
    /// ゲーム終了状態の切り替え
    /// </summary>
    /// <param name="isSwitch"></param>
    public void SwitchGameUp(bool isSwitch)
    {

        // isGameUp の値を引数の値に切り替える
        isGameUp = isSwitch;
    }
}
