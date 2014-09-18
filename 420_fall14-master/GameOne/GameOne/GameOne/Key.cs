using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameOne
{
    class Key : AutomatedSprite
    {
        public Key(Texture2D texture, Vector2 myPosition)
            : base(new SpriteSheet(texture, new Point(0, 0), 1.0f), myPosition,
            new CollisionOffset(5, 5, 5, 5), new Vector2(0, 0))
        {
            Point frameSize = new Point(173, 168);
            spriteSheet.addSegment(frameSize, new Point(0, 0), new Point(0, 0), 10);

            spriteSheet.setCurrentSegment(0);
        }

        public override void Update(GameTime gameTime, Rectangle clientBounds)
        {
            //base.Update(gameTime, clientBounds);
        }

        public override void Collision(Sprite otherSprite)
        {
            //position.X += 20f;
            //speed *= -1;
        }
    }
}
