using UnityEngine;

public static class TransformHelper
{

    private static Transform temporaryObjectContainerTran;

    /// <summary>
    /// temporaryObjectContainerTranに情報をセット
    /// </summary>
    /// <param name="newTran"></param>
    public static void SetTemporaryObjectContainerTran(Transform newTran)
    {

        //引数で貰った情報（newTran）を temporaryObjectContainerTran 変数に入れる
        temporaryObjectContainerTran = newTran;

        //コンソールに表示する
        Debug.Log("temporaryObjectContainerTran 変数に位置情報をセット完了");
    }

    /// <summary>
    /// temporaryObjectContainerTranの情報を取得
    /// </summary>
    /// <returns></returns>
    public static Transform GetTemporaryObjectContainerTran()
    {

        //他のスクリプトが GetTemporaryObjectContainerTran メソッドを呼び出したときに temporaryObjectContainerTran 変数の情報を使えるようにする
        return temporaryObjectContainerTran;
    }
}
