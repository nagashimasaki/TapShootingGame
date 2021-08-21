using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class FloatingMessage : MonoBehaviour
{
    [SerializeField]
    private Text txtFloatingMessage;

    /// <summary>
    /// フロート表示の種類
    /// </summary>
    public enum FloatingMessageType
    {
        EnemyDamage,   // エネミー被ダメージ(プレイヤーのバレット接触時)
        PlayerDamage,  // プレイヤー被ダメージ(耐久力減算時)
        GetExp,        // Exp 獲得
        BulletCost,    // バレットコスト支払い
    }

    /// <summary>
    /// フロート表示の制御
    /// </summary>
    /// <param name="floatingValue"></param>
    /// <param name="floatingMessageType"></param>
    public void DisplayFloatingMessage(int floatingValue, FloatingMessageType floatingMessageType)
    {

        // フロート表示の位置を毎回同じ位置にしないようにランダム要素を加える
        transform.localPosition = new Vector3(transform.localPosition.x + Random.Range(-20, 20), transform.localPosition.y + Random.Range(-10, 10), 0);

        // フロート表示に数字の値を代入
        txtFloatingMessage.text = floatingValue.ToString();

        // フロート表示の数字の色を設定
        txtFloatingMessage.color = GetMessageColor(floatingMessageType);

        // フロート表示を上方向へ移動させて、移動し終わったら破壊
        transform.DOLocalMoveY(transform.localPosition.y + 50, 1.0f).OnComplete(() => { Destroy(gameObject); });
    }

    /// <summary>
    /// フロート表示の色を設定
    /// </summary>
    /// <param name="floatingMessageType"></param>
    /// <returns></returns>
    private Color GetMessageColor(FloatingMessageType floatingMessageType)
    {
        switch (floatingMessageType)
        {
            case FloatingMessageType.EnemyDamage:
            case FloatingMessageType.PlayerDamage:
                return Color.red;
            case FloatingMessageType.GetExp:
                return Color.yellow;
            case FloatingMessageType.BulletCost:
                return Color.blue;
            default:
                return Color.white;
        }
    }
}
