using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ejercicio12
{
    public partial class Juego : Form
    {
        private Graphics canvas;
        private int puntaje = 0;
        private int paso = 10;
        private int xdir = 0, ydir = 0;
        private Boolean ejex = true, ejey = true;
        //Variables enemigo
        private int Exdir = 0, Eydir = 0;
        private Boolean Eejex = true, Eejey = true;
        private int Exdir2 = 0, Eydir2 = 0;
        private Boolean Eejex2 = true, Eejey2 = true;
        private CuerpoSerpiente cabeza;
        private CuerpoSerpiente cabeza2, cabeza3;
        private Comida manzana;
        private Obstaculo obstaculo;
        private List<Obstaculo> listObstaculos = new List<Obstaculo>();
        private List<Obstaculo> listObstaculos2 = new List<Obstaculo>();
        private List<Obstaculo> listObstaculos3 = new List<Obstaculo>();
        private Random generador = new Random();
        private Arduino control = new Arduino();
        private int movimientoRandom, controlador = 0, controlador2 = 0;
        private int Ovalorx, OvalorY;//Me serviran como v. temporales para comparar con get /set
        private int dato;
        public Juego()
        {
            InitializeComponent();
            BuscarPuertos();
            control.NuevoDatoRecibido += control_NuevoDatoRecibido;
            canvas = escenario.CreateGraphics();
            cabeza = new CuerpoSerpiente(10, 10);
            obstaculo = new Obstaculo(20, 20);
            cabeza2 = new CuerpoSerpiente(150, 160);
            cabeza3 = new CuerpoSerpiente(30, 160);
            manzana = new Comida(escenario.Width, escenario.Height);
            timerAnimacion.Start();
        }
        private void control_NuevoDatoRecibido(object sender, EventArgs e)
        {
            try
            {
                ActualizarValores(control.Voltaje);
            }
            catch (Exception error)
            {
                Console.Write(error);
            }
        }
        delegate void ActualizaValoresDelegado(double voltaje);
        private void ActualizarValores(double voltaje)
        {
            try
            {
                if (this.InvokeRequired)//Los eventos serial estan en subprocesos(¡Se esta llevando acabo en un subproceso?)
                {
                    ActualizaValoresDelegado delegado = new ActualizaValoresDelegado(ActualizarValores);
                    this.Invoke(delegado, voltaje);
                }
                else
                {
                    dato = Convert.ToInt32( voltaje);
                    Mover_Control_Arduino();
                }
            }
            catch (Exception error)
            {
                Console.Write(error);
            }
        }
        private void BuscarPuertos()
        {
            //using System.IO.Ports;
            string[] puertos = SerialPort.GetPortNames(); //Comunicacion Serial
            foreach (string puerto in puertos)
            {
                try
                {                
                    control.AbrirConexion(puerto);
                    label2.Text = "Comunicación abierta en puerto " + puerto;
                }
                catch (Exception error)
                {
                    Console.Write(error.Message);
                }
            }        
        }
        private void timerAnimacion_Tick(object sender, EventArgs e)
        {
            Niveles();
            DibujarTodo();
            MoverTodo();
            ChocarPared();
            ChocarCuerpo();
            ColisonObstaculo();
            ChocarSerpienteEnemiga();
            if (cabeza.Colision(manzana))//Sepiente usuario
            {
                manzana = new Comida(escenario.Width, escenario.Height);
                puntaje++;
                textoPuntaje.Text = puntaje + "";
                cabeza.Agregar();
            }
            if (puntaje >= 4)
            {
                if (cabeza3.Colision(manzana))//Sepiente enemiga1
                {
                    manzana = new Comida(escenario.Width, escenario.Height);
                    cabeza3.Agregar();
                }
            }
            else
            if (puntaje >=2 )
            {
                if (cabeza2.Colision(manzana))//Sepiente enemiga1
                {
                    manzana = new Comida(escenario.Width, escenario.Height);
                    cabeza2.Agregar();
                }
            }
         
           
            
        }

        private void ChocarCuerpo()
        {
            /*---SERPIENTE USUARIO---*/
            CuerpoSerpiente cuerpoTemporal;
            try
            {
                cuerpoTemporal = cabeza.VerSiguiente.VerSiguiente;
            }
            catch (Exception error)
            {
                cuerpoTemporal = null;

            }
            while (cuerpoTemporal != null)//se ejecutará esto si la cabeza tiene siguiente hasta que el siguiente del siguiente sea la cola que su siguiente es nulo  mientras que exista cuerpo temporal osea que pase el try sin error
            {
                if (cabeza.Colision(cuerpoTemporal))
                {
                    FinDelJuego();
                }
                else
                {
                    cuerpoTemporal = cuerpoTemporal.VerSiguiente;
                }
            }                       
        }

        private void ChocarPared()
        {

            if (cabeza.X < 0 || cabeza.X + cabeza.Tamaño > escenario.Width || cabeza.Y < 0 || cabeza.Y + cabeza.Tamaño > escenario.Height)
            {
                FinDelJuego();
            }
            /*----  sepriente enemiga1---*/
            if (cabeza2.X <= 10)//Parte izquierda escenarío
            {
                timerMovimientoSerpienteenemiga.Stop();
                movimientoRandom = 1;
                MoverEnemigoX();
                controlador = generador.Next(0, 2);
                if (controlador == 1)
                {
                    movimientoRandom = 3;
                    MoverEnemigoY();
                }
                if (cabeza2.Y < 10) //Choca arriba
                {
                    movimientoRandom = 3;
                    MoverEnemigoY();
                }
            }
            else
           if (cabeza2.Y <= 10) //Choca Arriba
            {
                timerMovimientoSerpienteenemiga.Stop();
                movimientoRandom = 3;
                MoverEnemigoY();
                controlador = generador.Next(0, 2);
                if (controlador == 1)
                {
                    movimientoRandom = 2;
                    MoverEnemigoX();
                }
                if (cabeza2.X + cabeza2.Tamaño > escenario.Width - 20)//Choca izquierda
                {
                    movimientoRandom = 2;
                    MoverEnemigoX();
                }
            }
            else
           if (cabeza2.Y + cabeza2.Tamaño >= escenario.Height - 10) //Choca Abajo
            {
                timerMovimientoSerpienteenemiga.Stop();
                movimientoRandom = 4;
                MoverEnemigoY();
                controlador = generador.Next(0, 2);
                if (controlador == 1)
                {
                    movimientoRandom = 1;
                    MoverEnemigoX();
                }
                if (cabeza2.X < 10)//choca Derecha
                {
                    movimientoRandom = 1;
                    MoverEnemigoX();
                }
            }
            else
           if (cabeza2.X + cabeza2.Tamaño >= escenario.Width - 20)//choca Derecha
            {
                timerMovimientoSerpienteenemiga.Stop();
                movimientoRandom = 2;
                MoverEnemigoX();
                controlador = generador.Next(0, 2);
                if (controlador == 1)
                {
                    movimientoRandom = 3;
                    MoverEnemigoY();
                }
                if (cabeza2.Y + cabeza2.Tamaño > escenario.Height - 10)
                {
                    movimientoRandom = 4;
                    MoverEnemigoY();
                }
            }
            timerMovimientoSerpienteenemiga.Start();
            controlador = 0;
            /* NO HACER COLSION SERPIENTE 3*/
            if (cabeza3.X <= 10)//Parte izquierda escenarío
            {
                timerMovimientoSerpienteenemiga2.Stop();
                movimientoRandom = 1;
                MoverEnemigoX2();
                controlador2 = generador.Next(0, 2);
                if(controlador2==1)
                {
                    movimientoRandom = 3;
                    MoverEnemigoY2();
                }
                if (cabeza3.Y < 10) //Choca arriba
                {
                    movimientoRandom = 3;
                    MoverEnemigoY2();
                }                
            }
            else
           if (cabeza3.Y <= 10) //Choca Arriba
            {
                timerMovimientoSerpienteenemiga2.Stop();
                movimientoRandom = 3;
                MoverEnemigoY2();
                controlador2 = generador.Next(0, 2);
                if (controlador2 == 1)
                {
                    movimientoRandom = 2;
                    MoverEnemigoX2();
                }
                if (cabeza3.X + cabeza3.Tamaño > escenario.Width - 20)//Choca izquierda
                {
                    movimientoRandom = 2;
                    MoverEnemigoX2();
                }
            }
            else
           if (cabeza3.Y + cabeza3.Tamaño >= escenario.Height - 10) //Choca Abajo
            {
                timerMovimientoSerpienteenemiga2.Stop();
                movimientoRandom = 4;
                MoverEnemigoY2();
                controlador2 = generador.Next(0, 2);
                if (controlador2 == 1)
                {
                    movimientoRandom = 1;
                    MoverEnemigoX2();
                }
                if (cabeza3.X < 10)//choca Derecha
                {
                    movimientoRandom = 1;
                    MoverEnemigoX2();
                }
            }
            else
           if (cabeza3.X + cabeza3.Tamaño >= escenario.Width - 20)//choca Derecha
            {
                timerMovimientoSerpienteenemiga2.Stop();
                movimientoRandom = 2;
                MoverEnemigoX2();
                controlador2 = generador.Next(0, 2);
                if (controlador2 == 1)
                {
                    movimientoRandom = 3;
                    MoverEnemigoY2();
                }
                if (cabeza3.Y + cabeza3.Tamaño > escenario.Height - 10)
                {
                    movimientoRandom = 4;
                    MoverEnemigoY2();
                }
            }
            timerMovimientoSerpienteenemiga2.Start();
            controlador2 = 0;
        } 
        private void ChocarSerpienteEnemiga()
        {
            if(puntaje >=2)
            {
                if ( cabeza.Colision(cabeza2))
                {
                    FinDelJuego();
                }
            }
            else
            if(puntaje>=4)
            {
                if (cabeza.Colision(cabeza3))
                {
                    FinDelJuego();
                }
            }
            
            try
            {

                CuerpoSerpiente cuerpoTemporal2;
                try
                {
                    cuerpoTemporal2 = cabeza2.VerSiguiente.VerSiguiente;
                }
                catch (Exception error)
                {
                    cuerpoTemporal2 = null;

                }
                while (cuerpoTemporal2 != null)//se ejecutará esto si la cabeza tiene siguiente hasta que el siguiente del siguiente sea la cola que su siguiente es nulo  mientras que exista cuerpo temporal osea que pase el try sin error
                {
                    if (cabeza.Colision(cuerpoTemporal2))
                    {
                        FinDelJuego();
                    }
                    else
                    {
                        cuerpoTemporal2 = cuerpoTemporal2.VerSiguiente;
                    }
                }
                CuerpoSerpiente cuerpoTemporal3;
                try
                {
                    cuerpoTemporal3 = cabeza3.VerSiguiente.VerSiguiente;
                }
                catch (Exception error)
                {
                    cuerpoTemporal3 = null;

                }
                while (cuerpoTemporal3 != null)//se ejecutará esto si la cabeza tiene siguiente hasta que el siguiente del siguiente sea la cola que su siguiente es nulo  mientras que exista cuerpo temporal osea que pase el try sin error
                {
                    if (cabeza.Colision(cuerpoTemporal3))
                    {
                        FinDelJuego();
                    }
                    else
                    {
                        cuerpoTemporal3 = cuerpoTemporal3.VerSiguiente;
                    }
                }
            }
            catch(Exception error)
            {
                Console.WriteLine(error);
            }
        }

        private void FinDelJuego()
        {
            timerAnimacion.Stop();
            timerMovimientoSerpienteenemiga.Stop();
            timerMovimientoSerpienteenemiga2.Stop();
            timerCrecimiento.Stop();
            MessageBox.Show("Perdiste");
            xdir = 0;
            ydir = 0;
            puntaje = 0;
            textoPuntaje.Text = "0";
            ejex = true;
            ejey = true;
            cabeza = new CuerpoSerpiente(10, 10);
            obstaculo = new Obstaculo(20, 20);
            cabeza2 = new CuerpoSerpiente(200, 100);
            cabeza3 = new CuerpoSerpiente(180, 160);
            manzana = new Comida(escenario.Width, escenario.Height);
            timerAnimacion.Start();
        }

        private void MoverTodo()
        {
            cabeza.Mover(cabeza.X + xdir, cabeza.Y + ydir);
            if (puntaje >= 2)
            {
                cabeza2.Mover(cabeza2.X + Exdir, cabeza2.Y + Eydir);
                cabeza3.Mover(cabeza3.X + Exdir2, cabeza3.Y + Eydir2);
            }
            else
            if(puntaje >=4)
            {
                cabeza2.Mover(cabeza2.X + Exdir, cabeza2.Y + Eydir);
                cabeza3.Mover(cabeza3.X + Exdir2, cabeza3.Y + Eydir2);
            }                        
        }

        private void DibujarTodo()
        {
            canvas.Clear(Color.White);
            cabeza.Dibujar(canvas);
            manzana.Dibujar(canvas);
            obstaculo.Dibujar(canvas);
            Obstaculos();
         
            
            if(puntaje<2)
            {
                foreach (Obstaculo obstaculo in listObstaculos)
                {
                    obstaculo.Dibujar(canvas);
                }               
            }
            else
            if (puntaje >= 2 && puntaje < 4)
            {              
                foreach (Obstaculo obstaculo in listObstaculos2)
                {
                    obstaculo.Dibujar(canvas);
                }               
                cabeza2.Dibujar(canvas);                
            }
            else
            if (puntaje >= 4)
            {
                foreach (Obstaculo obstaculo in listObstaculos3)
                {
                    obstaculo.Dibujar(canvas);
                }
                cabeza2.Dibujar(canvas);
                cabeza3.Dibujar(canvas);
                
            }
        }
        private void ColisonObstaculo()
        {
            foreach (Obstaculo obstaculo in listObstaculos)
            {
                if (cabeza.Colision(obstaculo))
                {
                    FinDelJuego();
                }
                if (manzana.Colision(obstaculo))
                {
                    manzana = new Comida(escenario.Width, escenario.Height);
                }
                /*INTENTANDO HACER NO HACER COLSION SERPIENTE 3*/
                Ovalorx = obstaculo.ValorX;
                OvalorY = obstaculo.ValorY;
                    //Realizar las respectivas acciones
                if(cabeza3.Colision(obstaculo))
                {
                    if (cabeza3.X >= (Ovalorx - 20) || cabeza3.X < (Ovalorx + 20) || cabeza3.Y > (OvalorY + 20) || cabeza3.Y < (OvalorY + 20))
                    {
                        timerMovimientoSerpienteenemiga.Stop();
                        if (Eejex)
                        {
                            movimientoRandom = generador.Next(1, 3);
                            MoverEnemigoX();
                        }
                        else
                         if (Eejey)
                        {
                            movimientoRandom = generador.Next(3, 5);
                            MoverEnemigoY();
                        }
                    }
                }
                    timerMovimientoSerpienteenemiga.Start();                                 
            }
            if (puntaje >= 2)
            {
                foreach (Obstaculo obstaculo in listObstaculos2)
                {
                    if (cabeza.Colision(obstaculo))
                    {
                        FinDelJuego();
                    }
                    if (manzana.Colision(obstaculo))
                    {
                        manzana = new Comida(escenario.Width, escenario.Height);
                    }
                    /* COLSION SERPIENTE 2*/
                    Ovalorx = obstaculo.ValorX;
                    OvalorY = obstaculo.ValorY;
                    //Realizar las respectivas acciones
                    if (cabeza2.Colision(obstaculo))
                    {
                        if (cabeza2.X >= (Ovalorx - 20) || cabeza2.X < (Ovalorx + 20) || cabeza2.Y > (OvalorY + 20) || cabeza2.Y < (OvalorY + 20))
                        {
                            timerMovimientoSerpienteenemiga.Stop();
                            if (Eejex)
                            {
                                movimientoRandom = generador.Next(1, 3);
                                MoverEnemigoX();
                            }
                            else
                             if (Eejey)
                            {
                                movimientoRandom = generador.Next(3, 5);
                                MoverEnemigoY();
                            }
                        }
                    }
                    timerMovimientoSerpienteenemiga.Start();
                }
                if (puntaje >= 4)
                {
                    foreach (Obstaculo obstaculo in listObstaculos3)
                    {
                        if (cabeza.Colision(obstaculo))
                        {
                            FinDelJuego();
                        }
                        if (manzana.Colision(obstaculo))
                        {
                            manzana = new Comida(escenario.Width, escenario.Height);
                        }/* COLSION SERPIENTE 2*/
                        Ovalorx = obstaculo.ValorX;
                        OvalorY = obstaculo.ValorY;
                        //Realizar las respectivas acciones
                        if (cabeza2.Colision(obstaculo))
                        {
                            if (cabeza2.X >= (Ovalorx - 40) || (cabeza2.X+cabeza2.Tamaño) < (Ovalorx + 40) || (cabeza2.Y+cabeza2.Tamaño) > (OvalorY + 40) || cabeza2.Y < (OvalorY + 40))
                            {
                                timerMovimientoSerpienteenemiga.Stop();
                                if (Eejex)
                                {
                                    movimientoRandom = generador.Next(1, 3);
                                    MoverEnemigoX();
                                }
                                else
                                 if (Eejey)
                                {
                                    movimientoRandom = generador.Next(3, 5);
                                    MoverEnemigoY();
                                }
                            }
                        }
                        /* COLSION SERPIENTE 3*/
                        Ovalorx = obstaculo.ValorX;
                        OvalorY = obstaculo.ValorY;
                        //Realizar las respectivas acciones
                        if (cabeza3.Colision(obstaculo))
                        {
                            if (cabeza3.X >= (Ovalorx - 40) || (cabeza3.X+cabeza3.Tamaño) < (Ovalorx + 40) || (cabeza3.Y+cabeza3.Tamaño) > (OvalorY + 40) || cabeza3.Y < (OvalorY + 40))
                            {
                                timerMovimientoSerpienteenemiga2.Stop();
                                if (Eejex2)
                                {
                                    movimientoRandom = generador.Next(1, 3);
                                    MoverEnemigoX();
                                }
                                else
                                 if (Eejey2)
                                {
                                    movimientoRandom = generador.Next(3, 5);
                                    MoverEnemigoY2();
                                }
                            }
                        }
                        timerMovimientoSerpienteenemiga2.Start();
                    }
                }
            }
        }

        private void MoimientoSerpienteenemiga_Tick(object sender, EventArgs e)
        {
            if (controlador == 0)
            {
                if (Eejex)
                {
                    movimientoRandom = generador.Next(1, 3);
                    MoverEnemigoX();
                }
                else
                if (Eejey)
                {
                    movimientoRandom = generador.Next(3, 5);
                    MoverEnemigoY();
                }
            }
            
        }

        private void timerCrecimiento_Tick(object sender, EventArgs e)//CRECE ENEMIGO :V
        {
            if (puntaje >= 2)
            {
                cabeza2.Agregar();
            }
            else
            if(puntaje >= 4)
            {
                cabeza2.Agregar();
                cabeza3.Agregar();
            }

        }

        private void timerMovimientoSerpienteenemiga2_Tick(object sender, EventArgs e)
        {
            if (controlador2 == 0)
            {
                if (Eejex2)
                {
                    movimientoRandom = generador.Next(1, 3);
                    MoverEnemigoX2();
                }
                else
                if (Eejey2)
                {
                    movimientoRandom = generador.Next(3, 5);
                    MoverEnemigoY2();
                }
            }

        }


        private void Juego_KeyDown(object sender, KeyEventArgs e)
        {
            if (ejex)
            {
                if (e.KeyCode == Keys.Up)
                {
                    ydir = -paso;
                    xdir = 0;
                    ejex = false;
                    ejey = true;
                }
                if (e.KeyCode == Keys.Down)
                {
                    ydir = paso;
                    xdir = 0;
                    ejex = false;
                    ejey = true;
                }
            }

            if (ejey)
            {
                if (e.KeyCode == Keys.Right)
                {
                    ydir = 0;
                    xdir = paso;
                    ejex = true;
                    ejey = false;
                }
                if (e.KeyCode == Keys.Left)
                {
                    ydir = 0;
                    xdir = -paso;
                    ejex = true;
                    ejey = false;
                }
            }

        }
        private void Mover_Control_Arduino()
        {
            if (ejex)
            {
                if (control.Voltaje==1)
                {
                    ydir = -paso;
                    xdir = 0;
                    ejex = false;
                    ejey = true;
                }
                if (dato==3)
                {
                    ydir = paso;
                    xdir = 0;
                    ejex = false;
                    ejey = true;
                }
            }

            if (ejey)
            {
                if (dato==4)
                {
                    ydir = 0;
                    xdir = paso;
                    ejex = true;
                    ejey = false;
                }
                if (dato==2)
                {
                    ydir = 0;
                    xdir = -paso;
                    ejex = true;
                    ejey = false;
                }
            }
            if(dato == 5)
            {
                Application.Restart();
            }
        }

        private void Obstaculos()
        {
         
            if(puntaje<2)
            {
                ObstaculosN1();
               
            }

            if (puntaje >= 2 && puntaje <4)
            {
          
                ObstaculosN2();
            }
            else
            if(puntaje >= 4)
            {
                
                ObstaculosN3();
            }

        }

        private void Niveles()
        {
            if (puntaje >= 2)
            {
                timerMovimientoSerpienteenemiga.Start();
                timerCrecimiento.Start();
            }
            else
            if(puntaje>= 4)
            {
                timerMovimientoSerpienteenemiga2.Start();
            }
        }
        private void MoverEnemigoX()
        {

           if(Eejex)
            {
                if (movimientoRandom == 1)
                {
                    Eydir = -paso;
                    Exdir = 0;
                    Eejex = false;
                    Eejey = true;
                }
                if (movimientoRandom == 2)
                {
                    Eydir = paso;
                    Exdir = 0;
                    Eejex = false;
                    Eejey = true;
                }
            }

        }
        private void MoverEnemigoY()
        {
            if(Eejey)
            {
                if (movimientoRandom == 3)
                {
                    Eydir = 0;
                    Exdir = paso;
                    Eejex = true;
                    Eejey = false;
                }
                if (movimientoRandom == 4)
                {
                    Eydir = 0;
                    Exdir = -paso;
                    Eejex = true;
                    Eejey = false;
                }
            }

        }
        private void ObstaculosN1()
        {
            /*-----Superior Izquierda-----*/

            while (obstaculo.ValorY < 70 && obstaculo.ValorX == 20)
            {
                listObstaculos.Add(new Obstaculo(obstaculo.ValorX, obstaculo.ValorY));
                obstaculo.ValorY = obstaculo.ValorY + 10;

            }
            obstaculo.ValorY = 20;
            while (obstaculo.ValorX < 70 && obstaculo.ValorY == 20)
            {
                listObstaculos.Add(new Obstaculo(obstaculo.ValorX, obstaculo.ValorY));
                obstaculo.ValorX = obstaculo.ValorX + 10;
            }
            /*-----Superior Derecha-----*/
            obstaculo.ValorX = 230;
            while (obstaculo.ValorX < 270 && obstaculo.ValorY == 20)
            {
                listObstaculos.Add(new Obstaculo(obstaculo.ValorX, obstaculo.ValorY));
                obstaculo.ValorX = obstaculo.ValorX + 10;
            }
            obstaculo.ValorY = 20;
            while (obstaculo.ValorY < 70 && obstaculo.ValorX == 270)
            {
                listObstaculos.Add(new Obstaculo(obstaculo.ValorX, obstaculo.ValorY));
                obstaculo.ValorY = obstaculo.ValorY + 10;

            }
            /*-----Inferior Izquierda-----*/
            obstaculo.ValorX = 20;
            obstaculo.ValorY = 260;
            while (obstaculo.ValorY < 300 && obstaculo.ValorX == 20)
            {
                listObstaculos.Add(new Obstaculo(obstaculo.ValorX, obstaculo.ValorY));
                obstaculo.ValorY = obstaculo.ValorY + 10;

            }
            while (obstaculo.ValorX < 60)
            {
                listObstaculos.Add(new Obstaculo(obstaculo.ValorX, obstaculo.ValorY));
                obstaculo.ValorX = obstaculo.ValorX + 10;
            }
            /*-----Inferior Derecha-----*/
            obstaculo.ValorX = 230;
            while (obstaculo.ValorX < 270)
            {
                listObstaculos.Add(new Obstaculo(obstaculo.ValorX, obstaculo.ValorY));
                obstaculo.ValorX = obstaculo.ValorX + 10;
            }
            obstaculo.ValorY = 260;
            while (obstaculo.ValorY < 300)
            {
                listObstaculos.Add(new Obstaculo(obstaculo.ValorX, obstaculo.ValorY));
                obstaculo.ValorY = obstaculo.ValorY + 10;

            }

        }
        private void ObstaculosN2()
        {
            obstaculo.ValorY = 20;
            obstaculo.ValorX = 20;
            while (obstaculo.ValorY < 70 && obstaculo.ValorX == 20)
            {
                listObstaculos2.Add(new Obstaculo(obstaculo.ValorX, obstaculo.ValorY));
                obstaculo.ValorY = obstaculo.ValorY + 10;

            }
            obstaculo.ValorX = 20;
            obstaculo.ValorY = 20;
            while (obstaculo.ValorX < 70 && obstaculo.ValorY == 20)
            {
                listObstaculos2.Add(new Obstaculo(obstaculo.ValorX, obstaculo.ValorY));
                obstaculo.ValorX = obstaculo.ValorX + 10;
            }
            /*-----Superior Derecha-----*/
            obstaculo.ValorX = 230;
            while (obstaculo.ValorX < 270 && obstaculo.ValorY == 20)
            {
                listObstaculos2.Add(new Obstaculo(obstaculo.ValorX, obstaculo.ValorY));
                obstaculo.ValorX = obstaculo.ValorX + 10;
            }
            obstaculo.ValorY = 20;
            while (obstaculo.ValorY < 70 && obstaculo.ValorX == 270)
            {
                listObstaculos2.Add(new Obstaculo(obstaculo.ValorX, obstaculo.ValorY));
                obstaculo.ValorY = obstaculo.ValorY + 10;

            }
            /*-----Inferior Izquierda-----*/
            obstaculo.ValorX = 20;
            obstaculo.ValorY = 260;
            while (obstaculo.ValorY < 300 && obstaculo.ValorX == 20)
            {
                listObstaculos2.Add(new Obstaculo(obstaculo.ValorX, obstaculo.ValorY));
                obstaculo.ValorY = obstaculo.ValorY + 10;

            }
            while (obstaculo.ValorX < 60)
            {
                listObstaculos2.Add(new Obstaculo(obstaculo.ValorX, obstaculo.ValorY));
                obstaculo.ValorX = obstaculo.ValorX + 10;
            }
            /*-----Inferior Derecha-----*/
            obstaculo.ValorX = 230;
            while (obstaculo.ValorX < 270)
            {
                listObstaculos2.Add(new Obstaculo(obstaculo.ValorX, obstaculo.ValorY));
                obstaculo.ValorX = obstaculo.ValorX + 10;
            }
            obstaculo.ValorY = 260;
            while (obstaculo.ValorY < 300)
            {
                listObstaculos2.Add(new Obstaculo(obstaculo.ValorX, obstaculo.ValorY));
                obstaculo.ValorY = obstaculo.ValorY + 10;

            }

            /*-----Diagonal S.Izquierda-----*/
            obstaculo.ValorX = 20;
            obstaculo.ValorY = 20;
            while (obstaculo.ValorY < 80)
            {
                listObstaculos2.Add(new Obstaculo(obstaculo.ValorX, obstaculo.ValorY));
                obstaculo.ValorY = obstaculo.ValorY + 10;
                obstaculo.ValorX = obstaculo.ValorX + 10;

            }
            /*---EMPIEZA OBSATCULO MEDIANO S.IZQUIERDA---*/
            obstaculo.ValorY = 80;
            while (obstaculo.ValorX <= 110 && obstaculo.ValorY == 80)
            {
                listObstaculos2.Add(new Obstaculo(obstaculo.ValorX, obstaculo.ValorY));
                obstaculo.ValorX = obstaculo.ValorX + 10;
            }
            obstaculo.ValorX = 80;
            while (obstaculo.ValorY <= 110 && obstaculo.ValorX == 80)
            {
                listObstaculos2.Add(new Obstaculo(obstaculo.ValorX, obstaculo.ValorY));
                obstaculo.ValorY = obstaculo.ValorY + 10;

            }
            /*----DIAGONAL S.DERECHA---*/

            obstaculo.ValorY = 20;
            obstaculo.ValorX = 270;
            while (obstaculo.ValorX >= 220)
            {
                listObstaculos2.Add(new Obstaculo(obstaculo.ValorX, obstaculo.ValorY));
                obstaculo.ValorX = obstaculo.ValorX - 10;
                obstaculo.ValorY = obstaculo.ValorY + 10;
            }
            /*---EMPIEZA OBSATCULO MEDIANO S.DERECHA---*/
            obstaculo.ValorX = 180;
            obstaculo.ValorY = 80;
            while (obstaculo.ValorX < 220 && obstaculo.ValorY == 80)
            {
                listObstaculos2.Add(new Obstaculo(obstaculo.ValorX, obstaculo.ValorY));
                obstaculo.ValorX = obstaculo.ValorX + 10;
            }
            obstaculo.ValorX = 210;
            obstaculo.ValorY = 80;
            while (obstaculo.ValorY < 120 && obstaculo.ValorX == 210)
            {
                listObstaculos2.Add(new Obstaculo(obstaculo.ValorX, obstaculo.ValorY));
                obstaculo.ValorY = obstaculo.ValorY + 10;

            }
            /*-----DIAGONAL I.IZQUIERDA-----*/
            obstaculo.ValorX = 20;
            obstaculo.ValorY = 300;
            while (obstaculo.ValorY > 240)
            {
                listObstaculos2.Add(new Obstaculo(obstaculo.ValorX, obstaculo.ValorY));
                obstaculo.ValorY = obstaculo.ValorY - 10;
                obstaculo.ValorX = obstaculo.ValorX + 10;

            }
            /*---EMPIEZA OBSATCULO MEDIANO I.IZQUIERDA---*/
            obstaculo.ValorX = 80;
            obstaculo.ValorY = 210;
            while (obstaculo.ValorY < 250 && obstaculo.ValorX == 80)
            {
                listObstaculos2.Add(new Obstaculo(obstaculo.ValorX, obstaculo.ValorY));
                obstaculo.ValorY = obstaculo.ValorY + 10;

            }
            obstaculo.ValorY = 240;
            obstaculo.ValorX = 80;
            while (obstaculo.ValorX < 120)
            {
                listObstaculos2.Add(new Obstaculo(obstaculo.ValorX, obstaculo.ValorY));
                obstaculo.ValorX = obstaculo.ValorX + 10;
            }
            /*----DIAGONAL S.DERECHA---*/

            obstaculo.ValorY = 300;
            obstaculo.ValorX = 270;
            while (obstaculo.ValorX > 210)
            {
                listObstaculos2.Add(new Obstaculo(obstaculo.ValorX, obstaculo.ValorY));
                obstaculo.ValorX = obstaculo.ValorX - 10;
                obstaculo.ValorY = obstaculo.ValorY - 10;
            }

            /*---EMPIEZA OBSATCULO MEDIANO S.DERECHA---*/
            obstaculo.ValorX = 210;
            while (obstaculo.ValorX > 170)
            {
                listObstaculos2.Add(new Obstaculo(obstaculo.ValorX, obstaculo.ValorY));
                obstaculo.ValorX = obstaculo.ValorX - 10;
            }
            obstaculo.ValorX = 210;
            obstaculo.ValorY = 210;
            while (obstaculo.ValorY < 240)
            {
                listObstaculos2.Add(new Obstaculo(obstaculo.ValorX, obstaculo.ValorY));
                obstaculo.ValorY = obstaculo.ValorY + 10;

            }
        }
        private void ObstaculosN3()
        {
            obstaculo.ValorY = 20;
            obstaculo.ValorX = 20;
            while (obstaculo.ValorY < 70 && obstaculo.ValorX == 20)
            {
                listObstaculos3.Add(new Obstaculo(obstaculo.ValorX, obstaculo.ValorY));
                obstaculo.ValorY = obstaculo.ValorY + 10;

            }
            obstaculo.ValorX = 20;
            obstaculo.ValorY = 20;
            while (obstaculo.ValorX < 70 && obstaculo.ValorY == 20)
            {
                listObstaculos3.Add(new Obstaculo(obstaculo.ValorX, obstaculo.ValorY));
                obstaculo.ValorX = obstaculo.ValorX + 10;
            }
            /*-----Superior Derecha-----*/
            obstaculo.ValorX = 230;
            while (obstaculo.ValorX < 270 && obstaculo.ValorY == 20)
            {
                listObstaculos3.Add(new Obstaculo(obstaculo.ValorX, obstaculo.ValorY));
                obstaculo.ValorX = obstaculo.ValorX + 10;
            }
            obstaculo.ValorY = 20;
            while (obstaculo.ValorY < 70 && obstaculo.ValorX == 270)
            {
                listObstaculos3.Add(new Obstaculo(obstaculo.ValorX, obstaculo.ValorY));
                obstaculo.ValorY = obstaculo.ValorY + 10;

            }
            /*-----Inferior Izquierda-----*/
            obstaculo.ValorX = 20;
            obstaculo.ValorY = 260;
            while (obstaculo.ValorY < 300 && obstaculo.ValorX == 20)
            {
                listObstaculos3.Add(new Obstaculo(obstaculo.ValorX, obstaculo.ValorY));
                obstaculo.ValorY = obstaculo.ValorY + 10;

            }
            while (obstaculo.ValorX < 60)
            {
                listObstaculos3.Add(new Obstaculo(obstaculo.ValorX, obstaculo.ValorY));
                obstaculo.ValorX = obstaculo.ValorX + 10;
            }
            /*-----Inferior Derecha-----*/
            obstaculo.ValorX = 230;
            while (obstaculo.ValorX < 270)
            {
                listObstaculos3.Add(new Obstaculo(obstaculo.ValorX, obstaculo.ValorY));
                obstaculo.ValorX = obstaculo.ValorX + 10;
            }
            obstaculo.ValorY = 260;
            while (obstaculo.ValorY < 300)
            {
                listObstaculos3.Add(new Obstaculo(obstaculo.ValorX, obstaculo.ValorY));
                obstaculo.ValorY = obstaculo.ValorY + 10;

            }

            /*-----Diagonal S.Izquierda-----*/
            obstaculo.ValorX = 20;
            obstaculo.ValorY = 20;
            while (obstaculo.ValorY < 80)
            {
                listObstaculos3.Add(new Obstaculo(obstaculo.ValorX, obstaculo.ValorY));
                obstaculo.ValorY = obstaculo.ValorY + 10;
                obstaculo.ValorX = obstaculo.ValorX + 10;

            }
            /*---EMPIEZA OBSATCULO MEDIANO S.IZQUIERDA---*/
            obstaculo.ValorY = 80;
            while (obstaculo.ValorX <= 110 && obstaculo.ValorY == 80)
            {
                listObstaculos3.Add(new Obstaculo(obstaculo.ValorX, obstaculo.ValorY));
                obstaculo.ValorX = obstaculo.ValorX + 10;
            }
            obstaculo.ValorX = 80;
            while (obstaculo.ValorY <= 110 && obstaculo.ValorX == 80)
            {
                listObstaculos3.Add(new Obstaculo(obstaculo.ValorX, obstaculo.ValorY));
                obstaculo.ValorY = obstaculo.ValorY + 10;

            }
            /*----DIAGONAL S.DERECHA---*/

            obstaculo.ValorY = 20;
            obstaculo.ValorX = 270;
            while (obstaculo.ValorX >= 220)
            {
                listObstaculos3.Add(new Obstaculo(obstaculo.ValorX, obstaculo.ValorY));
                obstaculo.ValorX = obstaculo.ValorX - 10;
                obstaculo.ValorY = obstaculo.ValorY + 10;
            }
            /*---EMPIEZA OBSATCULO MEDIANO S.DERECHA---*/
            obstaculo.ValorX = 180;
            obstaculo.ValorY = 80;
            while (obstaculo.ValorX < 220 && obstaculo.ValorY == 80)
            {
                listObstaculos3.Add(new Obstaculo(obstaculo.ValorX, obstaculo.ValorY));
                obstaculo.ValorX = obstaculo.ValorX + 10;
            }
            obstaculo.ValorX = 210;
            obstaculo.ValorY = 80;
            while (obstaculo.ValorY < 120 && obstaculo.ValorX == 210)
            {
                listObstaculos3.Add(new Obstaculo(obstaculo.ValorX, obstaculo.ValorY));
                obstaculo.ValorY = obstaculo.ValorY + 10;

            }
            /*-----DIAGONAL I.IZQUIERDA-----*/
            obstaculo.ValorX = 20;
            obstaculo.ValorY = 300;
            while (obstaculo.ValorY > 240)
            {
                listObstaculos3.Add(new Obstaculo(obstaculo.ValorX, obstaculo.ValorY));
                obstaculo.ValorY = obstaculo.ValorY - 10;
                obstaculo.ValorX = obstaculo.ValorX + 10;

            }
            /*---EMPIEZA OBSATCULO MEDIANO I.IZQUIERDA---*/
            obstaculo.ValorX = 80;
            obstaculo.ValorY = 210;
            while (obstaculo.ValorY < 250 && obstaculo.ValorX == 80)
            {
                listObstaculos3.Add(new Obstaculo(obstaculo.ValorX, obstaculo.ValorY));
                obstaculo.ValorY = obstaculo.ValorY + 10;

            }
            obstaculo.ValorY = 240;
            obstaculo.ValorX = 80;
            while (obstaculo.ValorX < 120)
            {
                listObstaculos3.Add(new Obstaculo(obstaculo.ValorX, obstaculo.ValorY));
                obstaculo.ValorX = obstaculo.ValorX + 10;
            }
            /*----DIAGONAL S.DERECHA---*/

            obstaculo.ValorY = 300;
            obstaculo.ValorX = 270;
            while (obstaculo.ValorX > 210)
            {
                listObstaculos3.Add(new Obstaculo(obstaculo.ValorX, obstaculo.ValorY));
                obstaculo.ValorX = obstaculo.ValorX - 10;
                obstaculo.ValorY = obstaculo.ValorY - 10;
            }

            /*---EMPIEZA OBSATCULO MEDIANO S.DERECHA---*/
            obstaculo.ValorX = 210;
            while (obstaculo.ValorX > 170)
            {
                listObstaculos3.Add(new Obstaculo(obstaculo.ValorX, obstaculo.ValorY));
                obstaculo.ValorX = obstaculo.ValorX - 10;
            }
            obstaculo.ValorX = 210;
            obstaculo.ValorY = 210;
            while (obstaculo.ValorY < 240)
            {
                listObstaculos3.Add(new Obstaculo(obstaculo.ValorX, obstaculo.ValorY));
                obstaculo.ValorY = obstaculo.ValorY + 10;

            }
            /*---  PARTE HORIZONTAL 'X'---*/
            obstaculo.ValorX = 130;
            obstaculo.ValorY = 160;
            while (obstaculo.ValorX < 180 )
            {
                listObstaculos3.Add(new Obstaculo(obstaculo.ValorX, obstaculo.ValorY));
                obstaculo.ValorX = obstaculo.ValorX + 10;
            }
           
            /*---  PARTE VERTICAL 'X'---*/
            obstaculo.ValorX = 150;
            obstaculo.ValorY = 140;
            while (obstaculo.ValorY < 180)
            {
                listObstaculos3.Add(new Obstaculo(obstaculo.ValorX, obstaculo.ValorY));
                obstaculo.ValorY = obstaculo.ValorY + 10;
            }

            listObstaculos3.Add(new Obstaculo(80, 160));
            listObstaculos3.Add(new Obstaculo(210, 160));
            listObstaculos3.Add(new Obstaculo(140, 80));
            listObstaculos3.Add(new Obstaculo(140, 240));
        }
        private void MoverEnemigoX2()
        {

            if (Eejex2)
            {
                if (movimientoRandom == 1)
                {
                    Eydir2 = -paso;
                    Exdir2 = 0;
                    Eejex2 = false;
                    Eejey2 = true;
                }
                if (movimientoRandom == 2)
                {
                    Eydir2 = paso;
                    Exdir2 = 0;
                    Eejex2= false;
                    Eejey2 = true;
                }
            }

        }
        private void MoverEnemigoY2()
        {
            if (Eejey2)
            {
                if (movimientoRandom == 3)
                {
                    Eydir2 = 0;
                    Exdir2 = paso;
                    Eejex2 = true;
                    Eejey2 = false;
                }
                if (movimientoRandom == 4)
                {
                    Eydir2 = 0;
                    Exdir2 = -paso;
                    Eejex2 = true;
                    Eejey2 = false;
                }
            }

        }
    }
}
