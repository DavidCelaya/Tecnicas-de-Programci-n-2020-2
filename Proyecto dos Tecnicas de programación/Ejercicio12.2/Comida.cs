using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ejercicio12
{
    public class Comida:Sprite
    {
        private Random generador = new Random();
        private Brush brocha = new SolidBrush(Color.Red);

        public Comida(int anchoCanvas, int altoCanvas)
        {
            valorX = Generar(anchoCanvas / valorTamaño);
            valorY = Generar(altoCanvas / valorTamaño);            
        }

        private int Generar(int v)
        {
            int num = generador.Next(0, v) * valorTamaño;
            return num;
        }

        public void Dibujar(Graphics canvas)
        {
            canvas.FillRectangle(brocha, valorX, valorY, valorTamaño, valorTamaño);
        }

        public void Mover(int anchoCanvas, int altoCanvas)
        {
            valorX = Generar(anchoCanvas / valorTamaño);
            valorY = Generar(altoCanvas / valorTamaño);
        }
    }

    
}
