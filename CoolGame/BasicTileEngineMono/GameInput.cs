using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace BasicTile
{
    public class GameInput
    {
        KeyboardState KBState;
        KeyboardState KBStateP;
        MouseState MState;
        MouseState MStateP;

        //..P = Pressed
        //..PR = PressedAndReleased
        public Command _buttonA_PR { get; set; }
        public Command _buttonD_PR { get; set; }
        public Command _buttonE_PR { get; set; }
        public Command _buttonM_PR { get; set; }
        public Command _buttonQ_PR { get; set; }
        public Command _buttonR_PR { get; set; }
        public Command _buttonS_PR {get;set;}
        public Command _buttonW_PR { get; set; }
        public Command _buttonLeftShift_P { get; set; }

        public Command _buttonEnter_PR { get; set; }
        public Command _buttonDelete_PR { get; set; }
        public Command _buttonEnd_PR { get; set; }
        public Command _buttonHome_PR { get; set; }
        public Command _buttonNum1_P { get; set; }
        public Command _buttonNum2_P { get; set; }
        public Command _buttonNum3_P { get; set; }
        public Command _buttonNum4_P { get; set; }
        public Command _buttonNum5_P { get; set; }
        public Command _buttonNum6_P { get; set; }
        public Command _buttonNum7_P { get; set; }
        public Command _buttonNum8_P { get; set; }
        public Command _buttonNum9_P { get; set; }

        //Mouse commands
        //..U: Up
        //..D: Down
        //..S: Single Point
        //..Dr: Dragged Point
        //..Hld: Held
        //..LShf: Left Shift Pressed
        public Command _mouseScroll_D { get; set; }
        public Command _mouseScroll_U { get; set; } 
        public Command _mouseLeft_P { get; set; }
        public Command _mouseLeft_R { get; set; }
        public Command _mouseLeft_PR { get; set; }
        public Command _mouseLeft_PR_S { get; set; }
        public Command _mouseLeft_PR_Dr { get; set; }
        public Command _mouseLeft_P_Hld { get; set; }
        public Command _mouseLeft_P_LShf { get; set; }
        public Command _mouseLeft_P_NoLShf { get; set; }

        public Command _mouseRight_PR { get; set; }

        //check whether mouse is dragging
        public bool IsDragging { get; set; }
        public bool IsFirstTimePressMouseLeft { get; set; }
        public Point FirstMouseClickPosition { get; set; }
        public Point MousePosition
        {
            get
            {
                return MState.Position;
            }
        }

        public GameInput()
        {
            KBState = Keyboard.GetState();
            MState = Mouse.GetState();

            KBStateP = KBState;
            MStateP = MState;

            IsDragging = false;
            IsFirstTimePressMouseLeft = true;

            //Initialize Binding Inputs
            //TODO: create binder functions

            //Reference:
            //http://stackoverflow.com/questions/9712932/2d-xna-game-mouse-clicking

            //mouse bind
            _mouseScroll_D = new ZoomInCommand();
            _mouseScroll_U = new ZoomOutCommand();

            //_mouseLeft_P

            //keyboard bind
            _buttonDelete_PR = new DebuggingToggleCommand();
            _buttonA_PR = null;
            _buttonQ_PR = null;
            _buttonE_PR = null;
            _buttonM_PR = null;
            _buttonEnter_PR = null;

        }

        public Queue<Command> HandleInput()
        {
            Queue<Command> cmdQueue = new Queue<Command>();

            KBStateP = KBState;
            MStateP = MState;

            KBState = Keyboard.GetState();
            MState = Mouse.GetState();

            //Mouse input
            if (MState.ScrollWheelValue < MStateP.ScrollWheelValue)
                cmdQueue.Enqueue(_mouseScroll_D);

            if (MState.ScrollWheelValue > MStateP.ScrollWheelValue)
                cmdQueue.Enqueue(_mouseScroll_U);

            //when this is the first time left pressed, until it has been released
            if (MState.LeftButton == ButtonState.Pressed && 
                IsFirstTimePressMouseLeft)
            {
                FirstMouseClickPosition = MState.Position;
                IsFirstTimePressMouseLeft = false;

                cmdQueue.Enqueue(_mouseLeft_P);
            }

            //when mouse has been pressed and released
            if (MState.LeftButton == ButtonState.Released &&
                MStateP.LeftButton == ButtonState.Pressed)
            {
                //reset first time switch
                IsFirstTimePressMouseLeft = true;

                cmdQueue.Enqueue(_mouseLeft_PR);
            }

            if (MState.RightButton == ButtonState.Released &&
                MStateP.RightButton == ButtonState.Pressed)
            {
                cmdQueue.Enqueue(_mouseRight_PR);
            }

            //when mouse has been pressed and released at the point of click
            if (MState.LeftButton == ButtonState.Released &&
                MStateP.LeftButton == ButtonState.Pressed &&
                !MouseMoved(FirstMouseClickPosition, MState.Position) )
            {
                //reset first time switch
                IsFirstTimePressMouseLeft = true;
                cmdQueue.Enqueue(_mouseLeft_PR_S);
            }

            //when mouse has been pressed and released but not at the point of click
            if (MState.LeftButton == ButtonState.Released && 
                MStateP.LeftButton == ButtonState.Pressed &&
                MouseMoved(FirstMouseClickPosition, MState.Position))
            {
                //reset first time switch
                IsFirstTimePressMouseLeft = true;
                cmdQueue.Enqueue(_mouseLeft_PR_Dr);
            }

            //when mouse is being held
            if (MState.LeftButton == ButtonState.Pressed &&
                MStateP.LeftButton == ButtonState.Pressed)
            {
                cmdQueue.Enqueue(_mouseLeft_P_Hld);
            }


            //Keyboard input (pressed and released)
            if (IsPressedAndReleased(Keys.A))
                cmdQueue.Enqueue(_buttonA_PR);

            if (IsPressedAndReleased(Keys.D))
                cmdQueue.Enqueue(_buttonD_PR);

            if (IsPressedAndReleased(Keys.E))
                cmdQueue.Enqueue( _buttonE_PR);

            if (IsPressedAndReleased(Keys.M))
                cmdQueue.Enqueue( _buttonM_PR);

            if (IsPressedAndReleased(Keys.Q))
                cmdQueue.Enqueue( _buttonQ_PR);

            if (IsPressedAndReleased(Keys.R))
                cmdQueue.Enqueue(_buttonR_PR);

            if (IsPressedAndReleased(Keys.S))
                cmdQueue.Enqueue(_buttonS_PR);

            if (IsPressedAndReleased(Keys.W))
                cmdQueue.Enqueue(_buttonW_PR);

            if (IsPressedAndReleased(Keys.Enter) )
                cmdQueue.Enqueue( _buttonEnter_PR);

            if (IsPressedAndReleased(Keys.Delete))
                cmdQueue.Enqueue( _buttonDelete_PR);

            if (IsPressedAndReleased(Keys.Home))
                cmdQueue.Enqueue(_buttonHome_PR);

            if (IsPressedAndReleased(Keys.End))
                cmdQueue.Enqueue(_buttonEnd_PR);

            //pressed
            if (KBState.IsKeyDown(Keys.LeftShift))
                cmdQueue.Enqueue(_buttonLeftShift_P);

            //Handle num keys
            if (KBState.IsKeyDown(Keys.NumPad1))
                cmdQueue.Enqueue(_buttonNum1_P);

            if (KBState.IsKeyDown(Keys.NumPad2))
                cmdQueue.Enqueue(_buttonNum2_P);

            if (KBState.IsKeyDown(Keys.NumPad3))
                cmdQueue.Enqueue(_buttonNum3_P);

            if (KBState.IsKeyDown(Keys.NumPad4))
                cmdQueue.Enqueue(_buttonNum4_P);

            if (KBState.IsKeyDown(Keys.NumPad6))
                cmdQueue.Enqueue(_buttonNum6_P);

            if (KBState.IsKeyDown(Keys.NumPad7))
                cmdQueue.Enqueue(_buttonNum7_P);

            if (KBState.IsKeyDown(Keys.NumPad8))
                cmdQueue.Enqueue(_buttonNum8_P);

            if (KBState.IsKeyDown(Keys.NumPad9))
                cmdQueue.Enqueue(_buttonNum9_P);

            //Hybrid states
            if (KBState.IsKeyUp(Keys.LeftShift) && 
                MStateP.LeftButton == ButtonState.Pressed &&
                MState.LeftButton == ButtonState.Released)
            {
                cmdQueue.Enqueue(_mouseLeft_P_NoLShf);
            }

            if (KBState.IsKeyDown(Keys.LeftShift) &&
                MStateP.LeftButton == ButtonState.Pressed &&
                MState.LeftButton == ButtonState.Released)
            {
                cmdQueue.Enqueue(_mouseLeft_P_LShf);
            }

            return cmdQueue;
        }

        private bool MouseMoved(Point one, Point two)
        {
            if (Math.Abs(one.X - two.X) > 5 || Math.Abs(one.Y - two.Y) > 5)
                return true;
            else
                return false;
        }

        private bool IsPressedAndReleased(Keys key)
        {
            return KBState.IsKeyUp(key) && KBStateP.IsKeyDown(key);
        }
    }
}

