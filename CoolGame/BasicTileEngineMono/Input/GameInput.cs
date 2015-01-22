using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace BasicTileEngineMono.Input
{
    public class GameInput
    {
        KeyboardState _kbState;
        KeyboardState _kbStateP;
        MouseState _mState;
        MouseState _mStateP;

        //..P = Pressed
        //..PR = PressedAndReleased
        public Command ButtonAPr { get; set; }
        public Command ButtonDPr { get; set; }
        public Command ButtonEPr { get; set; }
        public Command ButtonMPr { get; set; }
        public Command ButtonQPr { get; set; }
        public Command ButtonRPr { get; set; }
        public Command ButtonSPr {get;set;}
        public Command ButtonWPr { get; set; }
        public Command ButtonLeftShiftP { get; set; }

        public Command ButtonEnterPr { get; set; }
        public Command ButtonDeletePr { get; set; }
        public Command ButtonEndPr { get; set; }
        public Command ButtonHomePr { get; set; }
        public Command ButtonNum1P { get; set; }
        public Command ButtonNum2P { get; set; }
        public Command ButtonNum3P { get; set; }
        public Command ButtonNum4P { get; set; }
        public Command ButtonNum5P { get; set; }
        public Command ButtonNum6P { get; set; }
        public Command ButtonNum7P { get; set; }
        public Command ButtonNum8P { get; set; }
        public Command ButtonNum9P { get; set; }

        //Mouse commands
        //..U: Up
        //..D: Down
        //..S: Single Point
        //..Dr: Dragged Point
        //..Hld: Held
        //..LShf: Left Shift Pressed
        public Command MouseScrollD { get; set; }
        public Command MouseScrollU { get; set; } 
        public Command MouseLeftP { get; set; }
        public Command MouseLeftR { get; set; }
        public Command MouseLeftPr { get; set; }
        public Command MouseLeftPrS { get; set; }
        public Command MouseLeftPrDr { get; set; }
        public Command MouseLeftPHld { get; set; }
        public Command MouseLeftPlShf { get; set; }
        public Command MouseLeftPnoLShf { get; set; }

        public Command MouseRightPr { get; set; }

        //check whether mouse is dragging
        public bool IsDragging { get; set; }
        public bool IsFirstTimePressMouseLeft { get; set; }
        public Point FirstMouseClickPosition { get; set; }
        public Point MousePosition
        {
            get
            {
                return _mState.Position;
            }
        }

        public GameInput()
        {
            _kbState = Keyboard.GetState();
            _mState = Mouse.GetState();

            _kbStateP = _kbState;
            _mStateP = _mState;

            IsDragging = false;
            IsFirstTimePressMouseLeft = true;

            //Initialize Binding Inputs
            //TODO: create binder functions

            //Reference:
            //http://stackoverflow.com/questions/9712932/2d-xna-game-mouse-clicking

            //mouse bind
            MouseScrollD = new ZoomInCommand();
            MouseScrollU = new ZoomOutCommand();

            //_mouseLeft_P

            //keyboard bind
            ButtonDeletePr = new DebuggingToggleCommand();
            ButtonAPr = null;
            ButtonQPr = null;
            ButtonEPr = null;
            ButtonMPr = null;
            ButtonEnterPr = null;

        }

        public Queue<Command> HandleInput()
        {
            Queue<Command> cmdQueue = new Queue<Command>();

            _kbStateP = _kbState;
            _mStateP = _mState;

            _kbState = Keyboard.GetState();
            _mState = Mouse.GetState();

            //Mouse input
            if (_mState.ScrollWheelValue < _mStateP.ScrollWheelValue)
                cmdQueue.Enqueue(MouseScrollD);

            if (_mState.ScrollWheelValue > _mStateP.ScrollWheelValue)
                cmdQueue.Enqueue(MouseScrollU);

            //when this is the first time left pressed, until it has been released
            if (_mState.LeftButton == ButtonState.Pressed && 
                IsFirstTimePressMouseLeft)
            {
                FirstMouseClickPosition = _mState.Position;
                IsFirstTimePressMouseLeft = false;

                cmdQueue.Enqueue(MouseLeftP);
            }

            //when mouse has been pressed and released
            if (_mState.LeftButton == ButtonState.Released &&
                _mStateP.LeftButton == ButtonState.Pressed)
            {
                //reset first time switch
                IsFirstTimePressMouseLeft = true;

                cmdQueue.Enqueue(MouseLeftPr);
            }

            if (_mState.RightButton == ButtonState.Released &&
                _mStateP.RightButton == ButtonState.Pressed)
            {
                cmdQueue.Enqueue(MouseRightPr);
            }

            //when mouse has been pressed and released at the point of click
            if (_mState.LeftButton == ButtonState.Released &&
                _mStateP.LeftButton == ButtonState.Pressed &&
                !MouseMoved(FirstMouseClickPosition, _mState.Position) )
            {
                //reset first time switch
                IsFirstTimePressMouseLeft = true;
                cmdQueue.Enqueue(MouseLeftPrS);
            }

            //when mouse has been pressed and released but not at the point of click
            if (_mState.LeftButton == ButtonState.Released && 
                _mStateP.LeftButton == ButtonState.Pressed &&
                MouseMoved(FirstMouseClickPosition, _mState.Position))
            {
                //reset first time switch
                IsFirstTimePressMouseLeft = true;
                cmdQueue.Enqueue(MouseLeftPrDr);
            }

            //when mouse is being held
            if (_mState.LeftButton == ButtonState.Pressed &&
                _mStateP.LeftButton == ButtonState.Pressed)
            {
                cmdQueue.Enqueue(MouseLeftPHld);
            }


            //Keyboard input (pressed and released)
            if (IsPressedAndReleased(Keys.A))
                cmdQueue.Enqueue(ButtonAPr);

            if (IsPressedAndReleased(Keys.D))
                cmdQueue.Enqueue(ButtonDPr);

            if (IsPressedAndReleased(Keys.E))
                cmdQueue.Enqueue( ButtonEPr);

            if (IsPressedAndReleased(Keys.M))
                cmdQueue.Enqueue( ButtonMPr);

            if (IsPressedAndReleased(Keys.Q))
                cmdQueue.Enqueue( ButtonQPr);

            if (IsPressedAndReleased(Keys.R))
                cmdQueue.Enqueue(ButtonRPr);

            if (IsPressedAndReleased(Keys.S))
                cmdQueue.Enqueue(ButtonSPr);

            if (IsPressedAndReleased(Keys.W))
                cmdQueue.Enqueue(ButtonWPr);

            if (IsPressedAndReleased(Keys.Enter) )
                cmdQueue.Enqueue( ButtonEnterPr);

            if (IsPressedAndReleased(Keys.Delete))
                cmdQueue.Enqueue( ButtonDeletePr);

            if (IsPressedAndReleased(Keys.Home))
                cmdQueue.Enqueue(ButtonHomePr);

            if (IsPressedAndReleased(Keys.End))
                cmdQueue.Enqueue(ButtonEndPr);

            //pressed
            if (_kbState.IsKeyDown(Keys.LeftShift))
                cmdQueue.Enqueue(ButtonLeftShiftP);

            //Handle num keys
            if (_kbState.IsKeyDown(Keys.NumPad1))
                cmdQueue.Enqueue(ButtonNum1P);

            if (_kbState.IsKeyDown(Keys.NumPad2))
                cmdQueue.Enqueue(ButtonNum2P);

            if (_kbState.IsKeyDown(Keys.NumPad3))
                cmdQueue.Enqueue(ButtonNum3P);

            if (_kbState.IsKeyDown(Keys.NumPad4))
                cmdQueue.Enqueue(ButtonNum4P);

            if (_kbState.IsKeyDown(Keys.NumPad6))
                cmdQueue.Enqueue(ButtonNum6P);

            if (_kbState.IsKeyDown(Keys.NumPad7))
                cmdQueue.Enqueue(ButtonNum7P);

            if (_kbState.IsKeyDown(Keys.NumPad8))
                cmdQueue.Enqueue(ButtonNum8P);

            if (_kbState.IsKeyDown(Keys.NumPad9))
                cmdQueue.Enqueue(ButtonNum9P);

            //Hybrid states
            if (_kbState.IsKeyUp(Keys.LeftShift) && 
                _mStateP.LeftButton == ButtonState.Pressed &&
                _mState.LeftButton == ButtonState.Released)
            {
                cmdQueue.Enqueue(MouseLeftPnoLShf);
            }

            if (_kbState.IsKeyDown(Keys.LeftShift) &&
                _mStateP.LeftButton == ButtonState.Pressed &&
                _mState.LeftButton == ButtonState.Released)
            {
                cmdQueue.Enqueue(MouseLeftPlShf);
            }

            return cmdQueue;
        }

        public void HandleInput(ref Queue<Command> commandQueue)
        {
            foreach (Command c in HandleInput())
                commandQueue.Enqueue(c);
        }

        private static bool MouseMoved(Point one, Point two)
        {
            return Math.Abs(one.X - two.X) > 5 || Math.Abs(one.Y - two.Y) > 5;
        }

        private bool IsPressedAndReleased(Keys key)
        {
            return _kbState.IsKeyUp(key) && _kbStateP.IsKeyDown(key);
        }
    }
}

