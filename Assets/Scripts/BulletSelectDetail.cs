using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BulletSelectDetail : MonoBehaviour
{
    [SerializeField]
    private Button btnBulletSelect;

    private BulletSelectManager bulletSelectManager;


    /// <summary>
    /// 初期設定
    /// </summary>
    /// <param name="bulletSelectManager"></param>
    public void SetUpBulletSelectDetail(BulletSelectManager bulletSelectManager)
    {

        this.bulletSelectManager = bulletSelectManager;

        // ボタンにメソッド登録
        btnBulletSelect.onClick.AddListener(OnClickBulletSelect);
    }

    /// <summary>
    /// バレット選択
    /// </summary>
    public void OnClickBulletSelect()
    {
        Debug.Log("バレット選択");
    }
}
