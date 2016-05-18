using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    public class Adventurer : Unit
    {
        public bool Selectable = true;
        string _selText;
        int _selected; 
        public override void Start()
        {
            base.Start();
            selectable = true;
            _selected = 0;
            UnitManager.Instance.AddSelectable(this);
        }
        public override void Update()
        {
            base.Update();
            _selText = "";
            if ((_selected & (1 << 0)) != 0) _selText += "0";
            if ((_selected & (1 << 1)) != 0) _selText += "1";
        }
        public override void Select(int player)
        {
            _selected |= 1 << player;
        }
        public override void Deselect(int player)
        {
            _selected &= ~(1 << player);
        }
        void OnGUI()
        {
            if (_selected != 0)
            {
                GUI.Box(new Rect(ScreenPos.x - 20, ScreenPos.y - 20, 40, 40), _selText);
            }
        }
    }
}
