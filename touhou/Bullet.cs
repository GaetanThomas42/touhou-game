using Microsoft.Xna.Framework;

namespace touhou
{
    public class Bullet
    {
        public Vector2 Pos;
        private readonly Vector2 _speed = new Vector2(0, 2);

        public Bullet(Vector2 pos, Vector2 bounds)
        {
            Pos = pos;
        }

        public void Move()
        {
            Pos -= _speed;
        }
    }

}