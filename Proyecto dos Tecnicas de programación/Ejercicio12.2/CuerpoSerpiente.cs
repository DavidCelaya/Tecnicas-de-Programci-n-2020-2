using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ejercicio12
{
    public class CuerpoSerpiente:Sprite
    {
        private Brush brocha = new SolidBrush(Color.Blue);
        private CuerpoSerpiente siguiente;

        public CuerpoSerpiente VerSiguiente
        {
            get
            {
                return siguiente;
            }
        }

        public CuerpoSerpiente(int x, int y)
        {
            valorX = x;
            valorY = y;
            siguiente = null;
        }

        public void Dibujar(Graphics canvas)
        {
            if (siguiente != null)
            {
                siguiente.Dibujar(canvas);
            }
            canvas.FillRectangle(brocha, valorX, valorY, valorTamaño, valorTamaño);
        }

        public void Mover(int x,int  y)
        {
            if(siguiente != null)
            {
                siguiente.Mover(valorX, valorY);

            }
            valorX = x;  //Actualiza el ultimo valor
            valorY = y;
        }

        public void Agregar()
        {
            if(siguiente==null)
            {
                siguiente = new CuerpoSerpiente(valorX, valorY);
            }
            else
            {
                siguiente.Agregar();
            }
        }
    }
}
