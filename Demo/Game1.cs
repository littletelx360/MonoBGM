using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoBGM;
using System.IO;

namespace Demo
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private Song song;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            // song = new Song(Path.Combine(Content.RootDirectory, "test.ogg"));
            song = Content.Load<Song>("sample");
        }

        private bool keyDown = false;

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            var keyboard = Keyboard.GetState();

            if (keyboard.IsKeyDown(Keys.Space))
            {
                keyDown = true;
            }
            else if (keyDown && keyboard.IsKeyUp(Keys.Space))
            {
                if (song.State == Song.SongState.Stopped)
                {
                    song.Play();
                }
                else if (song.State == Song.SongState.Playing)
                {
                    song.Stop(Song.StopMode.FadeOut);
                }
                keyDown = false;
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            base.Draw(gameTime);
        }
    }
}