using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "EnemyDataSO", menuName = "Create EnemyDataSO")]
public class EnemyDataSO : ScriptableObject
{
    public List<EnemyData> enemyDataList = new List<EnemyData>();

    [Serializable]
    public class EnemyData
    {
        public int no;
        public int hp;
        public int power;
        public Sprite enemySprite;
        public EnemyType enemyType;
        public int exp;
        public float moveDuration;       // 拠点までの移動時間
        public MoveType moveType;        // 移動方法の種類
    }
}