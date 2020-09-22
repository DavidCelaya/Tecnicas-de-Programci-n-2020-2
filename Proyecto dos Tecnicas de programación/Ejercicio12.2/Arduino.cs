using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ejercicio12
{
    public class Arduino
    {
        private SerialPort placa = new SerialPort();
        private string dato = "";
        private string puerto;

        //propiedad
        public double Voltaje
        {
            get
            {
                return Convert.ToInt32(dato);//Conviertiendo analogico a digital
            }
        }
        public event EventHandler NuevoDatoRecibido; //Creando nuestro evento

        public void ComenzarFlujoDatos()
        {
            try
            {
                if (placa.IsOpen)
                {
                    placa.Write("1#");
                }
            }
            catch (Exception error)
            {
                Console.WriteLine(error.Message);
            }
        }
        public void DetenerFlujoDatos()
        {
            try
            {
                if (placa.IsOpen)
                {
                    placa.Write("0#");
                }
            }
            catch (Exception error)
            {
                Console.WriteLine(error.Message);
            }
        }
        public void AbrirConexion(string puerto)
        {
            try
            {
                if (!placa.IsOpen)//La placa no esta abierta
                {
                    if (puerto != null || puerto != "")//No me pasa nada
                    {
                        placa.DataReceived += placa_DatoRecibido;//DataRecibed es un evento de los SerialPort y lo voy a implimaentar a Placa_DatoRecibido.
                        // '+=' es cuando queremos asociar un metodo a un evento
                        //El manejador del evento sera el metodo Placa_DatoRecibido
                        placa.PortName = puerto;
                        placa.Open();
                    }

                }
            }
            catch (Exception error)
            {
                Console.WriteLine(error.Message);
            }
        }

        private void placa_DatoRecibido(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                placa.DataReceived += placa_DatoRecibido;
                //\x02500\x03 -->Si leo 500
                dato = placa.ReadTo("\x03");
                string[] arreglo = dato.Split(new string[] { "\x02" }, StringSplitOptions.RemoveEmptyEntries); //Split= contar
                dato = arreglo[0];
                if (NuevoDatoRecibido != null) //Si no se a Lanzado
                {
                    NuevoDatoRecibido(this, new EventArgs()); //Lanzo el evento
                }
            }
            catch (Exception error)
            {
                Console.WriteLine(error.Message);
            }
        }

        public void CerrarConexion()
        {
            try
            {
                placa.DiscardInBuffer();//En el cable ignoralo
                placa.Close();
            }
            catch (Exception error)
            {
                Console.WriteLine(error.Message);
            }
        }
    }
}
