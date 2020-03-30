﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.Menus;

namespace GenericModConfigMenu.UI
{
    public class Table : Container
    {
        private List<Element[]> rows = new List<Element[]>();

        private Vector2 size;
        public Vector2 Size
        {
            get { return size; }
            set
            {
                size = value;
                Scrollbar.LocalPosition = new Vector2(value.X - 24, Scrollbar.LocalPosition.Y);
                Scrollbar.BackSize = new Vector2( Scrollbar.BackSize.X, value.Y);
            }
        }

        public const int RowPadding = 16;
        private int rowHeight;
        public int RowHeight
        {
            get { return rowHeight; }
            set
            {
                rowHeight = value + RowPadding;
                UpdateScrollbar();
            }
        }

        public int RowCount { get { return rows.Count; } }

        public Scrollbar Scrollbar { get; }
        
        public Table()
        {
            Scrollbar = new Scrollbar();
            Scrollbar.LocalPosition = new Vector2(0, 0);
            Scrollbar.BackSize = new Vector2(24, 0);
            AddChild(Scrollbar);
        }

        public void AddRow( Element[] elements )
        {
            rows.Add(elements);
            foreach ( var child in elements )
            {
                AddChild(child);
            }
            UpdateScrollbar();
        }

        private void UpdateScrollbar()
        {
            Scrollbar.FrontSize = (int)((Size.Y / (rowHeight * rows.Count)) * Size.Y);
            if (Scrollbar.FrontSize > Scrollbar.BackSize.Y)
                Scrollbar.FrontSize = (int)Scrollbar.BackSize.Y;
        }

        public override void Update()
        {
            int ir = 0;
            foreach (var row in rows)
            {
                foreach (var element in row)
                {
                    element.LocalPosition = new Vector2(element.LocalPosition.X, ir * RowHeight - Scrollbar.ScrollPercent * rows.Count * RowHeight);
                    if (element.Position.Y + RowHeight - RowPadding < Position.Y || element.Position.Y > Position.Y + Size.Y)
                        continue;
                    element.Update();
                }
                ++ir;
            }
            Scrollbar.Update();

            //base.Update();
        }

        public void ForceUpdateEvenHidden()
        {
            int ir = 0;
            foreach (var row in rows)
            {
                foreach (var element in row)
                {
                    element.LocalPosition = new Vector2(element.LocalPosition.X, ir * RowHeight - Scrollbar.ScrollPercent * rows.Count * RowHeight);
                    element.Update();
                }
                ++ir;
            }
            Scrollbar.Update();
        }

        public override void Draw(SpriteBatch b)
        {
            IClickableMenu.drawTextureBox(b, (int) Position.X - 32, (int) Position.Y - 32, (int) Size.X + 64, (int) Size.Y + 64, Color.White);
            
            foreach (var row in rows)
            {
                foreach (var element in row)
                {
                    if (element.Position.Y + RowHeight - RowPadding < Position.Y || element.Position.Y > Position.Y + Size.Y)
                        continue;
                    if (element == RenderLast)
                        continue;
                    element.Draw(b);
                }
            }
            
            if (RenderLast != null)
                RenderLast.Draw(b);

            Scrollbar.Draw(b);
        }
    }
}
