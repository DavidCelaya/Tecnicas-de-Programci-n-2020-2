using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ejercicio12
{
    public class Sprite
    {
        protected int valorX, valorY, valorTamaño;

        public int X
        {
            get
            {
                return valorX;
            }
        }

        public int Y
        {
            get
            {
                return valorY;
            }
        }

        public int Tamaño
        {
            get
            {
                return valorTamaño;
            }
        }
        public Sprite()
        {
            valorTamaño = 10;
        }
        public Boolean Colision(Sprite otro)
        {
            int difx = Math.Abs(this.X - otro.X);
            int dify = Math.Abs(this.Y - otro.Y);
            if(difx<valorTamaño && dify<valorTamaño)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
