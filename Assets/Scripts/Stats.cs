using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Michael
{
    public class Stats : Singleton<Stats>
    {
        public Action<StatType, string> OnStatUpdated;
        public enum StatType
        {
            Generation,
            AliveCells,
        }
        [Serializable]
        struct Stat
        {
            public StatType type;
            public TextMeshProUGUI text;
            public string format;
        }
        [SerializeField] Stat[] stats;
        void Start()
        {
            OnStatUpdated += UpdateStatText;
        }

        void UpdateStatText(StatType type, string content)
        {
            foreach (var stat in stats)
            {
                if (stat.type == type)
                {
                    stat.text.text = string.Format(stat.format, content);
                    break;
                }
            }
        }
    }
}
