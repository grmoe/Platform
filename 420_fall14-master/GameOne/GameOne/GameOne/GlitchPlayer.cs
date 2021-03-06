﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace GameOne
{
    class GlitchPlayer : UserControlledSprite
    {
        // state pattern
        const int NUM_STATES = 4;
        enum GlitchPlayerState
        {
            Walking,
            Climbing,
            Jumping,
            Sleeping
        }
        GlitchPlayerState currentState;
        AbstractState[] states;

        // constants for this particular sprite
        static Point glitchNumberOfFrames = new Point(21, 6);
        static CollisionOffset glitchCollisionOffset = new CollisionOffset(30, 0, 60, 60);
        static Vector2 glitchSpeed = new Vector2(64, 32);
        static Vector2 glitchFriction = new Vector2(0.8f, 1f);
        static Point glitchFrameSize = new Point(192, 160);

        // constructor
        public GlitchPlayer(Texture2D image)
            : base(new SpriteSheet(image, glitchNumberOfFrames, .5f),
           new Vector2(70,0), glitchCollisionOffset, glitchSpeed, glitchFriction)
        {
            // set the segments and frame size
            spriteSheet.addSegment(glitchFrameSize, new Point(0, 0), new Point(11, 0), 20);
            spriteSheet.addSegment(glitchFrameSize, new Point(0, 1), new Point(18, 1), 50);
            spriteSheet.addSegment(glitchFrameSize, new Point(0, 2), new Point(8, 3), 40);
            spriteSheet.addSegment(glitchFrameSize, new Point(0, 4), new Point(20, 5), 50);

            // define the states
            states = new AbstractState[NUM_STATES];
            states[(Int32)GlitchPlayerState.Walking] = new WalkingState(this);
            states[(Int32)GlitchPlayerState.Sleeping] = new SleepingState(this);
            states[(Int32)GlitchPlayerState.Jumping] = new JumpingState(this);
            states[(Int32)GlitchPlayerState.Climbing] = new ClimbingState(this);

            // start in Walking state
            switchState(GlitchPlayerState.Walking);
        }

        public void onCollisionFalse()
        {
            onGround = false;
        }

        public void reverseCollisionOffset()
        {
            if (reverseGravity)
            {
                glitchCollisionOffset = new CollisionOffset(0, 30, 60, 60);
            }
            else {
                glitchCollisionOffset = new CollisionOffset(30, 0, 60, 60);
            }
        }

        /*
         * Update the sprite.
         */
        public override void Update(GameTime gameTime, Rectangle clientBounds)
        {
            // Update the sprite (base class)
            base.Update(gameTime, clientBounds);

            // call Update for the current state
            states[(Int32)currentState].Update(gameTime, clientBounds);

            //System.Diagnostics.Debug.WriteLine(glitchCollisionOffset.north);
            

        }

        /*
         * Called when this sprite has collided with something else.
         */
        public override void Collision(Sprite otherSprite)
        {
            if (otherSprite.GetType().ToString().Equals("GameOne.Enemy"))
            {
                position.X = 0;
                position.Y = 645 - collisionOffset.south;
                reverseGravity = false;
            }
            if (!reverseGravity)
                CollisionNoReverseGravity(otherSprite);
            else
                CollisionReverseGravity(otherSprite);
            
        }

        private void CollisionNoReverseGravity(Sprite otherSprite)
        {
            Boolean myFlag = false;
            if (otherSprite.GetType().ToString().Equals("GameOne.Platform") || otherSprite.GetType().ToString().Equals("GameOne.TronPlatform"))
            {
                if (collisionRect.Right <= (otherSprite.collisionRect.Left + 5))
                {
                    position.X += -5f;
                    velocity.X += -1f;
                    onGround = false;
                    myFlag = true;
                }
                if (collisionRect.Left >= (otherSprite.collisionRect.Right - 5))
                {
                    position.X += 5f;
                    velocity.X += 1f;
                    onGround = false;
                    myFlag = true;
                }
                if (collisionRect.Top >= (otherSprite.collisionRect.Bottom - 50) && myFlag == false && velocity.Y < 0f)
                {
                    velocity.Y = 0f;
                    onGround = false;
                    myFlag = true;
                }
                if (collisionRect.Bottom - 50 <= otherSprite.collisionRect.Top && velocity.Y >= 0f && myFlag == false)
                {
                    velocity.Y = 0f;
                    onGround = true;
                }
            }
        }

        private void CollisionReverseGravity(Sprite otherSprite) 
        {
            Boolean myFlag = false;
            if (otherSprite.GetType().ToString().Equals("GameOne.Platform") || otherSprite.GetType().ToString().Equals("GameOne.TronPlatform"))
            {
                if (collisionRect.Right <= (otherSprite.collisionRect.Left + 5))
                {
                    position.X += -5f;
                    velocity.X += -1f;
                    onGround = false;
                    myFlag = true;
                }
                if (collisionRect.Left >= (otherSprite.collisionRect.Right - 5))
                {
                    position.X += 5f;
                    velocity.X += 1f;
                    onGround = false;
                    myFlag = true;
                }
                if (collisionRect.Bottom <= (otherSprite.collisionRect.Top + 50) && myFlag == false && velocity.Y > 0f)
                {
                    velocity.Y = 0f;
                    onGround = false;
                    myFlag = true;
                }
                if (collisionRect.Top + 50 >= otherSprite.collisionRect.Bottom  && velocity.Y <= 0f && myFlag == false)
                {
                    velocity.Y = 0f;
                    onGround = true;
                }
            }
        }

        /*
         * Implement the State Pattern!
         */
        private void switchState(GlitchPlayerState newState)
        {
            pauseAnimation = false; // just in case

            // switch the state to the given state
            currentState = newState;
            spriteSheet.setCurrentSegment((Int32)newState);
            currentFrame = spriteSheet.currentSegment.startFrame;
        }

        public bool isOnGround() { return onGround; }


        /** STATES **/
        private abstract class AbstractState
        {
            protected readonly GlitchPlayer player;
            protected int timeSinceLastGravityShift = 0;
            protected int renableGravityShiftTime = 500;

            protected AbstractState(GlitchPlayer player)
            {
                this.player = player;
            }

            public virtual void Update(GameTime gameTime, Rectangle clientBounds)
            {
            }
        }

        /* Walking State */
        private class WalkingState : AbstractState
        {
            Point stillFrame;
            int timeSinceLastMove = 0;
            
            const int timeForSleep = 3000;

            public WalkingState(GlitchPlayer player)
                : base(player)
            {
                // define the standing still frame
                stillFrame = new Point(14, 0);
            }

            public override void Update(GameTime gameTime, Rectangle clientBounds)
            {
                // pause animation if the sprite is not moving
                if (player.direction.X == 0 || !player.onGround)
                {
                    player.pauseAnimation = true;
                    player.currentFrame = stillFrame; // standing frame
                }
                else
                {
                    timeSinceLastMove = 0;
                    player.pauseAnimation = false;
                }

                // perform a jump?
                if (GamePad.GetState(PlayerIndex.One).Buttons.A == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.W))
                {
                    if (player.onGround)
                    {
                        timeSinceLastMove = 0;
                        player.switchState(GlitchPlayerState.Jumping);
                        if (!player.reverseGravity)
                            player.velocity.Y += -500f;
                        else
                            player.velocity.Y += 500f;
                    }
                }

                //System.Diagnostics.Debug.WriteLine(timeSinceLastGravityShift);
                //System.Diagnostics.Debug.WriteLine(renableGravityShiftTime);
                timeSinceLastGravityShift += gameTime.ElapsedGameTime.Milliseconds;
                if ((timeSinceLastGravityShift > renableGravityShiftTime) && (GamePad.GetState(PlayerIndex.One).Buttons.LeftShoulder == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Space)))
                {
                    timeSinceLastGravityShift = 0;
                    if (player.reverseGravity)
                        player.reverseGravity = false;
                    else
                        player.reverseGravity = true;
                    player.reverseCollisionOffset();
                    
                }

                // transition to sleep state?
                timeSinceLastMove += gameTime.ElapsedGameTime.Milliseconds;
                if (timeSinceLastMove > timeForSleep)
                {
                    timeSinceLastMove = 0;
                    player.switchState(GlitchPlayerState.Sleeping);
                }
 
            }
        }

        /* Sleeping State */
        private class SleepingState : AbstractState
        {
            Vector2 sleepingPosition;
            Boolean fallingToSleep = true;

            public SleepingState(GlitchPlayer player)
                : base(player)
            {
            }

            public override void Update(GameTime gameTime, Rectangle clientBounds)
            {
                // just started Sleeping state
                if (fallingToSleep)
                {
                    sleepingPosition = player.position;  // remember the current position
                    fallingToSleep = false;
                }

                // have we hit the end of the animation?
                if (player.currentFrame == player.spriteSheet.currentSegment.endFrame)
                {
                    player.pauseAnimation = true;
                }

                // did we move? if so, switch to Walking state
                if (sleepingPosition != player.position)
                {
                    fallingToSleep = true;
                    player.switchState(GlitchPlayerState.Walking);
                }
                if (GamePad.GetState(PlayerIndex.One).Buttons.A == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.W))
                {
                    if (player.onGround)
                    {
                        player.switchState(GlitchPlayerState.Jumping);
                        if (!player.reverseGravity)
                            player.velocity.Y += -500f;
                        else
                            player.velocity.Y += 500f;
                    }
                }
                timeSinceLastGravityShift += gameTime.ElapsedGameTime.Milliseconds;
                if ((timeSinceLastGravityShift > renableGravityShiftTime) && (GamePad.GetState(PlayerIndex.One).Buttons.LeftShoulder == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Space)))
                {
                    
                    timeSinceLastGravityShift = 0;
                    if (player.reverseGravity)
                        player.reverseGravity = false;
                    else
                        player.reverseGravity = true;
                    player.reverseCollisionOffset();

                }
            }
        }


        /* Jumping State */
        private class JumpingState : AbstractState
        {

            public JumpingState(GlitchPlayer player)
                : base(player)
            {
            }

            public override void Update(GameTime gameTime, Rectangle clientBounds)
            {
                // animate once through -- then go to standing still frame
                if (player.currentFrame == player.spriteSheet.currentSegment.endFrame)
                {
                    player.switchState(GlitchPlayerState.Walking);
                    player.currentFrame = new Point(14, 0);  // start standing still
                }

                timeSinceLastGravityShift += gameTime.ElapsedGameTime.Milliseconds;
                if ((timeSinceLastGravityShift > renableGravityShiftTime) && (GamePad.GetState(PlayerIndex.One).Buttons.LeftShoulder == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Space)))
                {
                    
                    timeSinceLastGravityShift = 0;
                    if (player.reverseGravity)
                        player.reverseGravity = false;
                    else
                        player.reverseGravity = true;
                    player.reverseCollisionOffset();
                }
            }
        }

        /* Climbing State */
        private class ClimbingState : AbstractState
        {

            public ClimbingState(GlitchPlayer player)
                : base(player)
            {
            }

            public override void Update(GameTime gameTime, Rectangle clientBounds)
            {

            }
        }
    }
}