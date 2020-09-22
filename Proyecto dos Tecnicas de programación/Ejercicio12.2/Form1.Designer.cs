namespace Ejercicio12
{
    partial class Juego
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Juego));
            this.escenario = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textoPuntaje = new System.Windows.Forms.Label();
            this.timerAnimacion = new System.Windows.Forms.Timer(this.components);
            this.timerMovimientoSerpienteenemiga = new System.Windows.Forms.Timer(this.components);
            this.timerCrecimiento = new System.Windows.Forms.Timer(this.components);
            this.timerMovimientoSerpienteenemiga2 = new System.Windows.Forms.Timer(this.components);
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.escenario)).BeginInit();
            this.SuspendLayout();
            // 
            // escenario
            // 
            this.escenario.BackColor = System.Drawing.Color.White;
            this.escenario.Location = new System.Drawing.Point(13, 13);
            this.escenario.Name = "escenario";
            this.escenario.Size = new System.Drawing.Size(400, 400);
            this.escenario.TabIndex = 0;
            this.escenario.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label1.Location = new System.Drawing.Point(40, 430);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(126, 32);
            this.label1.TabIndex = 1;
            this.label1.Text = "puntaje:";
            // 
            // textoPuntaje
            // 
            this.textoPuntaje.AutoSize = true;
            this.textoPuntaje.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textoPuntaje.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.textoPuntaje.Location = new System.Drawing.Point(162, 428);
            this.textoPuntaje.Name = "textoPuntaje";
            this.textoPuntaje.Size = new System.Drawing.Size(32, 32);
            this.textoPuntaje.TabIndex = 2;
            this.textoPuntaje.Text = "0";
            // 
            // timerAnimacion
            // 
            this.timerAnimacion.Tick += new System.EventHandler(this.timerAnimacion_Tick);
            // 
            // timerMovimientoSerpienteenemiga
            // 
            this.timerMovimientoSerpienteenemiga.Tick += new System.EventHandler(this.MoimientoSerpienteenemiga_Tick);
            // 
            // timerCrecimiento
            // 
            this.timerCrecimiento.Interval = 5000;
            this.timerCrecimiento.Tick += new System.EventHandler(this.timerCrecimiento_Tick);
            // 
            // timerMovimientoSerpienteenemiga2
            // 
            this.timerMovimientoSerpienteenemiga2.Tick += new System.EventHandler(this.timerMovimientoSerpienteenemiga2_Tick);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label2.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.label2.Location = new System.Drawing.Point(215, 441);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(46, 17);
            this.label2.TabIndex = 3;
            this.label2.Text = "label2";
            // 
            // Juego
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.ClientSize = new System.Drawing.Size(516, 472);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textoPuntaje);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.escenario);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Juego";
            this.Text = " ";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Juego_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.escenario)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox escenario;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label textoPuntaje;
        private System.Windows.Forms.Timer timerAnimacion;
        private System.Windows.Forms.Timer timerCrecimiento;
        public System.Windows.Forms.Timer timerMovimientoSerpienteenemiga;
        private System.Windows.Forms.Timer timerMovimientoSerpienteenemiga2;
        private System.Windows.Forms.Label label2;
    }
}

