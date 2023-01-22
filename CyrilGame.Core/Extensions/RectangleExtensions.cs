using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyrilGame.Core.Extensions
{
    public static class RectangleExtensions
    {
        public static Rectangle AddVector( this Rectangle rectangle, Vector2 position) 
        {
            return new Rectangle( rectangle.X + (int)position.X, rectangle.Y + ( int ) position.Y, rectangle.Width + ( int ) position.X, rectangle.Height + ( int ) position.Y );
        }
    }
}
