using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
{

    public static GameData instance;

    [SerializeField, Header("獲得した合計 Exp")]
    private int totalExp;

    private void Awake()
    {

        if (instance == null)
        {

            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {

            Destroy(gameObject);
        }
    }

    /// <summary>
    /// TotalExp の更新
    /// </summary>
    /// <param name="exp"></param>
    public void UpdateTotalExp(int exp)
    {
        totalExp += exp;
    }

    /// <summary>
    /// TotalExp の取得
    /// </summary>
    /// <returns></returns>
    public int GetTotalExp()
    {
        return totalExp;
    }
}
