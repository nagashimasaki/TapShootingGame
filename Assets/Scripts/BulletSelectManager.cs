using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSelectManager : MonoBehaviour
{
    [SerializeField]
    private BulletSelectDetail bulletSelectDetailPrefab;　　// BulletSelectDetail スクリプトがアタッチされている、プレファブの btnBulletSelect ゲームオブジェクトをアサインする

    [SerializeField]
    private Transform bulletTran;                           // バレット選択ボタンの生成位置として利用するゲームオブジェクトをアサインする

    private const int maxBulletBtnNum = 0;                  // バレット選択ボタンの最大数。定数として宣言する

    public List<BulletSelectDetail> bulletSelectDetailList = new List<BulletSelectDetail>();      // 生成されたバレット選択ボタンの List


    void Start()
    {
        // バレット選択ボタンの生成
        StartCoroutine(GenerateBulletSelectDetail());
    }

    /// <summary>
    /// バレット選択用ボタンの生成
    /// </summary>
    /// <returns></returns>
    public IEnumerator GenerateBulletSelectDetail()
    {

        // ボタンの最大数を目標値に処理を繰り返す
        for (int i = 0; i < maxBulletBtnNum; i++)
        {

            // バレットボタン生成
            BulletSelectDetail bulletSelectDetail = Instantiate(bulletSelectDetailPrefab, bulletTran, false);

            // バレットボタンの設定
            bulletSelectDetail.SetUpBulletSelectDetail(this);

            // リストに追加
            bulletSelectDetailList.Add(bulletSelectDetail);

            // 0.25 秒だけ処理を中断(順番にボタンが生成されるようにする演出)
            yield return new WaitForSeconds(0.25f);
        }
    }
}
