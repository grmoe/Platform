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
        protected int currentNum = 4;
        protected int keyCount = 0;
        public Key(Texture2D texture, Vector2 myPosition)
            : base(new SpriteSheet(texture, new Point(0, 0), 1.0f), myPosition,
            new CollisionOffset(5, 5, 5, 5), new Vector2(0, 0))
        {
            Point frameSize = new Point(28, 53);
            spriteSheet.addSegment(frameSize, new Point(0, 0), new Point(0, 0), 10);
            spriteSheet.setCurrentSegment(0);
            
        }

        public override void Update(GameTime gameTime, Rectangle clientBounds)
        {
            //base.Update(gameTime, clientBounds);
        }

        public override void Collision(Sprite otherSprite)
        {
            bool changePlace = false;
            while (changePlace==false){
                Random rnd1 = new Random();
                int ranNum = rnd1.Next(12);
                effects = SpriteEffects.None;
                if (ranNum == 0 && currentNum!=0)
                {
                    position = new Vector2(1280, 675);
                    currentNum = 0;
                    changePlace = true;
                }
                else if (ranNum == 1 && currentNum != 1)
                {
                    position = new Vector2(280, 175);
                    currentNum = 1;
                    changePlace = true;
                }
                else if (ranNum == 2 && currentNum != 2)
                {
                    position = new Vector2(380, 575);
                    currentNum = 2;
                    changePlace = true;
                }
                else if (ranNum == 3 && currentNum != 3)
                {
                    position = new Vector2(880, 375);
                    currentNum = 3;
                    changePlace = true;
                }
                else if (ranNum == 4 && currentNum != 4)
                {
                    position = new Vector2(1280, 145);
                    currentNum = 4;
                    changePlace = true;
                }
                else if (ranNum == 5 && currentNum != 5)
                {
                    position = new Vector2(1480, 425);
                    currentNum = 5;
                    changePlace = true;
                }
                else if (ranNum == 6 && currentNum != 6)
                {
                    effects = SpriteEffects.FlipVertically | SpriteEffects.FlipHorizontally;
                    position = new Vector2(1280, 810);
                    currentNum = 6;
                    changePlace = true;
                }
                else if (ranNum == 7 && currentNum != 7)
                {
                    effects = SpriteEffects.FlipVertically | SpriteEffects.FlipHorizontally;
                    position = new Vector2(280, 310);
                    currentNum = 7;
                    changePlace = true;
                }
                else if (ranNum == 8 && currentNum != 8)
                {
                    effects = SpriteEffects.FlipVertically | SpriteEffects.FlipHorizontally;
                    position = new Vector2(380, 710);
                    currentNum = 8;
                    changePlace = true;
                }
                else if (ranNum == 9 && currentNum != 9)
                {
                    effects = SpriteEffects.FlipVertically | SpriteEffects.FlipHorizontally;
                    position = new Vector2(880, 510);
                    currentNum = 9;
                    changePlace = true;
                }
                else if (ranNum == 10 && currentNum != 10)
                {
                    effects = SpriteEffects.FlipVertically | SpriteEffects.FlipHorizontally;
                    position = new Vector2(1280, 280);
                    currentNum = 10;
                    changePlace = true;
                }
                else if (ranNum ==11 && currentNum != 11)
                {
                    effects = SpriteEffects.FlipVertically | SpriteEffects.FlipHorizontally;
                    position = new Vector2(1480, 560);
                    currentNum = 11;
                    changePlace = true;
                }
            }
            keyCount = keyCount + 1;
            if (keyCount >= 5)
            {

            }


        }
    }
}
