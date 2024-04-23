using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace MonoGameDirectX;

public class InputManager
{
    
    public event Action Onclick;
    public void Update(GameTime gametime)
    {
        if (Mouse.GetState().LeftButton == ButtonState.Pressed)
        {
            Onclick?.Invoke();
        }
    }
    
    
}