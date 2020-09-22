using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ejercicio12
{
    class Obstaculo: Sprite
    {
        private Brush brocha = new SolidBrush(Color.Green);
        private Obstaculo siguiente;
        public Obstaculo(int x, int y)
        {
            valorX = x;
            valorY = y;           
        }

        public void Dibujar(Graphics canvas)
        {
            canvas.FillRectangle(brocha, valorX, valorY, valorTamaño, valorTamaño);
        }

        public int ValorX
        {
            get
            {
                return valorX;
            }
            set
            {
                valorX = value;
            }
        }
        public int ValorY
        {
            get
            {
                return valorY;
            }
            set
            {
                valorY = value;
            }
        }
    }
}
