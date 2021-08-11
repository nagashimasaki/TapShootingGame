using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;                  

[CreateAssetMenu(fileName = "MoveEventSO", menuName = "Create MoveEventSO")]
public class MoveEventSO : ScriptableObject
{

    // 拠点の位置 
    //private const float moveLimit = -3000.0f;     

    /// <summary>
    /// MoveType に応じた移動方法を決定してイベントとして登録
    /// </summary>
    /// <param name="moveType"></param>
    /// <returns></returns>
    public UnityAction<Transform, float> GetMoveEvent(MoveType moveType)
    {

        // moveType で分岐
        switch (moveType)
        {

            // Straight の場合
            case MoveType.Straight:
                return MoveStraight;

            // Meandering の場合
            case MoveType.Meandering:
                return MoveMeandering;

            // Boss_Horizontal の場合
            case MoveType.Boss_Horizontal:
                return MoveBossHorizontal;

            // 上記以外の場合
            default:
                return Stop;
        }
    }

    /// <summary>
    /// 直進移動
    /// </summary>
    /// <param name="tran"></param>
    /// <param name="duration"></param>
    private void MoveStraight(Transform tran, float duration)
    {
        Debug.Log("直進");

        //tran.DOLocalMoveY(moveLimit, duration);
    }

    /// <summary>
    /// 蛇行移動
    /// </summary>
    /// <param name="tran"></param>
    /// <param name="duration"></param>
    private void MoveMeandering(Transform tran, float duration)
    {
        Debug.Log("蛇行");

        // 左右方向の移動をループ処理することで行ったり来たりさせる。左右の移動幅はランダム、移動間隔は等速
        tran.DOLocalMoveX(tran.position.x + Random.Range(200.0f, 400.0f), 1f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);

        //tran.DOLocalMoveY(moveLimit, duration);
    }

    /// <summary>
    /// ボス・水平移動
    /// </summary>
    /// <param name="tran"></param>
    /// <param name="duration"></param>
    public void MoveBossHorizontal(Transform tran, float duration)
    {
        tran.localPosition = new Vector3(0, tran.localPosition.y, tran.localPosition.z);

        tran.DOLocalMoveY(-500, 3.0f).OnComplete(() => {

            Sequence sequence = DOTween.Sequence();　　
            sequence.Append(tran.DOLocalMoveX(tran.localPosition.x + 550, 2.5f).SetEase(Ease.Linear));　　// 右端に移動
            sequence.Append(tran.DOLocalMoveX(tran.localPosition.x - 550, 5.0f).SetEase(Ease.Linear));　　// 左端に移動
            sequence.Append(tran.DOLocalMoveX(tran.localPosition.x, 2.5f).SetEase(Ease.Linear));　　// 真ん中に移動
            sequence.AppendInterval(1.0f).SetLoops(-1, LoopType.Restart);　　// 真ん中の地点に到達したら、一定時間待機する。その後上の処理を無制限にループする
        });
    }

    /// <summary>
    /// 移動停止
    /// </summary>
    /// <param name="tran"></param>
    /// <param name="duration"></param>
    public void Stop(Transform tran, float duration)
    {

        Debug.Log("停止");
    }
}
