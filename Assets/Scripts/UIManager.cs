using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup canvasGroupGameClear;

    /// <summary>
    /// ゲームクリア表示を隠す
    /// </summary>
    public void HideGameClearSet()
    {

        // GameClearSet ゲームオブジェクトの透明度を 0 にして見えなくする
        canvasGroupGameClear.alpha = 0;
    }

    /// <summary>
    /// ゲームクリア表示を行う
    /// </summary>
    public void DisplayGameClearSet()
    {

        // GameClearSet ゲームオブジェクトの透明度を徐々に 1 してゲームクリア表示
        canvasGroupGameClear.DOFade(1.0f, 0.25f);
    }
}
