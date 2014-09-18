using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameOne
{
    class Enemy : AutomatedSprite
    {
        int timeMoving = 0;
        
        public Enemy(Texture2D texture)
            :base(new SpriteSheet(texture, new Point(0, 0), 1.0f), new Vector2(600, 50), 
            new CollisionOffset(5, 5, 5, 5), new Vector2(-1f, 0))
        {
            Point frameSize = new Point(86, 86);
            spriteSheet.addSegment(frameSize, new Point(0, 0), new Point(0, 0), 10);
            onGround = true;
            turnOffGravity();
            spriteSheet.setCurrentSegment(0);
        }

        public override void Update(GameTime gameTime, Rectangle clientBounds)
        {
            timeMoving += gameTime.ElapsedGameTime.Milliseconds;
            if (timeMoving > 3000)
            {
                speed *= -1;
                timeMoving = 0;
            }

            base.Update(gameTime, clientBounds);
        }

        public override void Collision(Sprite otherSprite)
        {
        }
    }
}
