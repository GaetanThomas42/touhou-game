using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace touhou
{
    public class Game1 : Game
    {
        private readonly GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private SpriteFont _font;
        private Player _player;
        private List<Enemy> _enemies = new List<Enemy>();
        private Texture2D _enemyTexture;
        private Texture2D _bulletTexture;
        private Texture2D _lifeTexture;
        private double _lastShot;
        private Vector2 _bounds;
        private int _level = 1;
        private int _score = 0;
        private int _lives = 3;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            _bounds = new Vector2(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);
            _player = new Player(new Vector2(_bounds.X / 2,
                    _bounds.Y / (float) 1.2),
                _bounds.X / 150,
                Content.Load<Texture2D>("ball"));

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _font = Content.Load<SpriteFont>("Display");

            _player.Texture = Content.Load<Texture2D>("ball");
            _bulletTexture = Content.Load<Texture2D>("bullet");
            _enemyTexture = Content.Load<Texture2D>("enemy");
            _lifeTexture = Content.Load<Texture2D>("life");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            if (_lives > 0)
            {
                           var totalSeconds = gameTime.TotalGameTime.TotalSeconds;

            if (gameTime.TotalGameTime.Seconds / 10 > _level)
            {
                _level++;
            }

            if (gameTime.TotalGameTime.Seconds / 2 > _level && _enemies.Count < 2 * _level)
            {
                Console
                    .WriteLine(_enemies.Count);
                _enemies.Add(new Enemy(_bounds, _level));
            }

            var kState = Keyboard.GetState();
            var mState = Mouse.GetState();


            if (kState.IsKeyDown(Keys.Q) && _player.Pos.X > _player.Texture.Width)

            {
                _player.Move(1);
            }

            if (kState.IsKeyDown(Keys.D) &&
                _player.Pos.X < _bounds.X - _player.Texture.Width)
            {
                _player.Move(2);
            }

            if (kState.IsKeyDown(Keys.Space) && _lastShot < totalSeconds - 0.7)
            {
                _lastShot = totalSeconds;
                _player.Shoot(new Vector2(mState.X, mState.Y), _bounds);
            }

            for (var i = 0; i < _enemies.Count; i++)
            {
                _enemies[i].Move(_enemyTexture);
                if (_enemies[i].Pos.Y > _bounds.Y)
                {
                    _enemies.RemoveAt(i);
                    break;
                }
                if (new Rectangle((int) _enemies[i].Pos.X, (int) _enemies[i].Pos.Y, _enemyTexture.Width,
                    _enemyTexture.Height).Intersects(new Rectangle((int) _player.Pos.X,
                    (int) _player.Pos.Y,
                    _player.Texture.Width,_player.Texture.Height)))
                {
                    _enemies.RemoveAt(i);
                    _lives--;
                    break;
                }
            }

            for (var j = 0; j < _player.Bullets.Count; j++)
            {
                _player.Bullets[j].Move();
                if (_player.Bullets[j].Pos.Y < 0)
                {
                    _player.Bullets.RemoveAt(j);
                    break;
                }

                for (var k = 0; k < _enemies.Count; k++)
                {
                    if (new Rectangle((int) _enemies[k].Pos.X, (int) _enemies[k].Pos.Y, _enemyTexture.Width,
                        _enemyTexture.Height).Intersects(new Rectangle((int) _player.Bullets[j].Pos.X,
                        (int) _player.Bullets[j].Pos.Y,
                        _bulletTexture.Width, _bulletTexture.Height)))
                    {
                        _enemies.RemoveAt(k);
                        _score++;
                        break;
                    }
                }
            }
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();
            _spriteBatch.DrawString(_font, "Level " + _level, new Vector2(15, 15), Color.Black);
            _spriteBatch.DrawString(_font, "Score " + _score, new Vector2(_bounds.X- 150, 15), Color.Black);
            _spriteBatch.Draw(_player.Texture,
                _player.Pos,
                null,
                Color.White,
                0f,
                new Vector2(_player.Texture.Width / 2, _player.Texture.Height / 10),
                Vector2.One,
                SpriteEffects.None,
                0f);
            if (_lives == 0)
            {
                _spriteBatch.DrawString(_font, "Perdu", new Vector2((float)(_bounds.X / 2.3), _bounds.Y/2), Color.Black);

            }
            foreach (var bullet in _player.Bullets)
            {
                _spriteBatch.Draw(_bulletTexture, bullet.Pos, Color.White);
            }
            for(var i =0;i < _lives;i++)
            {
                _spriteBatch.Draw(_lifeTexture, new Vector2((float)(_bounds.X / 2.3 + i * _lifeTexture.Width),15), Color.White);
            }
            foreach (var enemy in _enemies)
            {
                _spriteBatch.Draw(_enemyTexture, enemy.Pos, Color.White);
            }

            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }

    public class Player
    {
        public Vector2 Pos;
        private float _speed;
        public Texture2D Texture;
        public readonly List<Bullet> Bullets = new List<Bullet>();

        public Player(Vector2 pos, float speed, Texture2D texture)
        {
            Pos = pos;
            Texture = texture;
            _speed = speed;
        }

        public void Shoot(Vector2 mPos, Vector2 bounds)
        {
            Bullets.Add(
                new Bullet(new Vector2(Pos.X - Texture.Width / 4, Pos.Y - Texture.Width / 4), bounds));
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

    public class Enemy
    {
        public Vector2 Pos;
        public Vector2 Dir;
        public Vector2 Bounds;

        public Enemy(Vector2 bounds, int level)
        {
            Bounds = bounds;
            Pos = new Vector2(new Random().Next(20, (int) Bounds.X-20), 0);
            Dir = new Vector2(Pos.X < bounds.X / 2 ? new Random().Next(-1, 1) : new Random().Next(1, 2),
                new Random().Next(1, 2));
            Dir.Y += level * (float) 0.1;
        }

        public void Move(Texture2D texture)
        {
            if (Pos.X < texture.Width ||
                Pos.X + texture.Width > Bounds.X)
            {
                Dir.X = -Dir.X;
            }

            Pos += Dir;
        }
    }

    public class Bullet
    {
        public Vector2 Pos;
        private Vector2 _speed = new Vector2(0, 2);
        private Vector2 _bounds;

        public Bullet(Vector2 pos, Vector2 bounds)
        {
            _bounds = bounds;
            Pos = pos;
        }

        public void Move()
        {
            Pos -= _speed;
        }
    }
}