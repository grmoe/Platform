﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameOne
{
    class TronPlatform : AutomatedSprite
    {
        public TronPlatform(Texture2D texture, Vector2 myPosition)
            : base(new SpriteSheet(texture, new Point(0, 0), 1.0f), myPosition,
            new CollisionOffset(0, 0, 0, 0), new Vector2(0, 0))
        {
            Point frameSize = new Point(124, 43);
            spriteSheet.addSegment(frameSize, new Point(0, 0), new Point(0, 0), 10);

            spriteSheet.setCurrentSegment(0);
        }

        public override void Update(GameTime gameTime, Rectangle clientBounds)
        {
        }

        public override void Collision(Sprite otherSprite)
        {
        }
    }
}
