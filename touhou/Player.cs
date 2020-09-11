using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace touhou
{
    public class Player
    {
        public Vector2 Pos;
        private readonly float _speed;
        public Texture2D Texture;
        public readonly List<Bullet> Bullets = new List<Bullet>();

        public Player(Vector2 pos, float speed, Texture2D texture)
        {
            Pos = pos;
            Texture = texture;
            _speed = speed;
        }

        public void Shoot(Vector2 bounds)
        {
            Bullets.Add(
                new Bullet(new Vector2(Pos.X - (float)Texture.Width / 4, Pos.Y - (float)Texture.Width / 4), bounds));
        }

        public void Move(int dir)
        {
            switch (dir)
            {
                case 1:
                    Pos.X -= _speed;
                    break;
                case 2:
                    Pos.X += _speed;
                    break;
            }
        }
    }

}