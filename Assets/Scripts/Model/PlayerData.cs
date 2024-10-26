using System;
using UnityEngine;

namespace Scripts.Model
{
    [Serializable]
    public class PlayerData
    {

        public int Coins;
        public int HP;
        public bool IsArmed;
        public int Swords;


        public PlayerData Clone()
        {
            var json = JsonUtility.ToJson(this);
            return JsonUtility.FromJson<PlayerData>(json);
        }
    }



}
