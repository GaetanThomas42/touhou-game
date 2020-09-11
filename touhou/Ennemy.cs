using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace touhou
{
    public class Enemy
    {
        public Vector2 Pos;
        private Vector2 _dir;
        private readonly Vector2 _bounds;

        public Enemy(Vector2 bounds, int level)
        {
            _bounds = bounds;
            Pos = new Vector2(new Random().Next(20, (int) _bounds.X-20), 0);
            _dir = new Vector2(Pos.X < _bounds.X / 2 ? new Random().Next(-1, 1) : new Random().Next(1, 2),
                new Random().Next(1, 2));
            _dir.Y += level * (float) 0.1;
        }

        public void Move(Texture2D texture)
        {
            if (Pos.X < texture.Width ||
                Pos.X + texture.Width > _bounds.X)
            {
                _dir.X = -_dir.X;
            }

            Pos += _dir;
        }
    }

}