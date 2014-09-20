using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace GameOne
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class SpriteManager : Microsoft.Xna.Framework.DrawableGameComponent
    {
        SpriteBatch spriteBatch;
        Sprite player;
        List<Sprite> spriteList = new List<Sprite>();

        public SpriteManager(Game game)
            : base(game)
        {
            // TODO: Construct any child components here
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);

            player = new GlitchPlayer(Game.Content.Load<Texture2D>(@"Images/result"));
            //spriteList.Add(new Enemy(Game.Content.Load<Texture2D>(@"Images/smallenemy")));
            spriteList.Add(new TronPlatform(Game.Content.Load<Texture2D>(@"Images/smallerplatform"), new Vector2(230, 250)));
            spriteList.Add(new TronPlatform(Game.Content.Load<Texture2D>(@"Images/smallerplatform"), new Vector2(330, 650)));
            spriteList.Add(new TronPlatform(Game.Content.Load<Texture2D>(@"Images/smallerplatform"), new Vector2(830, 450)));
            spriteList.Add(new TronPlatform(Game.Content.Load<Texture2D>(@"Images/smallerplatform"), new Vector2(1230, 220)));
            spriteList.Add(new TronPlatform(Game.Content.Load<Texture2D>(@"Images/smallerplatform"), new Vector2(1230, 750)));
            spriteList.Add(new TronPlatform(Game.Content.Load<Texture2D>(@"Images/smallerplatform"), new Vector2(1430, 500)));
            spriteList.Add(new Door(Game.Content.Load<Texture2D>(@"Images/lockedDoor"), new Vector2(1700, 905)));
            spriteList.Add(new Key(Game.Content.Load<Texture2D>(@"Images/key"), new Vector2(880, 375)));


            base.LoadContent();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            ((GlitchPlayer)player).onCollisionFalse();
            
            // collision
            foreach (Sprite sprite in spriteList)
                if (sprite.collisionRect.Intersects(player.collisionRect))
                {
                    player.Collision(sprite);
                    sprite.Collision(player);                   
                }
            // update each automated sprite
            foreach (Sprite sprite in spriteList)
                sprite.Update(gameTime, Game.Window.ClientBounds);
            player.Update(gameTime, Game.Window.ClientBounds);
            base.Update(gameTime);
            
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            player.Draw(gameTime, spriteBatch);
            foreach (Sprite sprite in spriteList)
                sprite.Draw(gameTime, spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
