namespace GameAirWar
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.MapPictureBox = new System.Windows.Forms.PictureBox();
            this.AvionesData = new System.Windows.Forms.DataGridView();
            this.HangarData = new System.Windows.Forms.DataGridView();
            this.IdH = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Edges = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IDAviones = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cantAviones = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cantGasolina = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.weight = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Origen = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Destino = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Gasolina = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.MapPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.AvionesData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.HangarData)).BeginInit();
            this.SuspendLayout();
            // 
            // MapPictureBox
            // 
            this.MapPictureBox.Location = new System.Drawing.Point(9, 10);
            this.MapPictureBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.MapPictureBox.Name = "MapPictureBox";
            this.MapPictureBox.Size = new System.Drawing.Size(818, 719);
            this.MapPictureBox.TabIndex = 0;
            this.MapPictureBox.TabStop = false;
            this.MapPictureBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MapPictureBox_MouseDown);
            this.MapPictureBox.MouseUp += new System.Windows.Forms.MouseEventHandler(this.MapPictureBox_MouseUp);
            // 
            // AvionesData
            // 
            this.AvionesData.AllowUserToAddRows = false;
            this.AvionesData.AllowUserToDeleteRows = false;
            this.AvionesData.BackgroundColor = System.Drawing.Color.FloralWhite;
            this.AvionesData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.AvionesData.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Id,
            this.weight,
            this.Origen,
            this.Destino,
            this.Gasolina});
            this.AvionesData.Location = new System.Drawing.Point(842, 10);
            this.AvionesData.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.AvionesData.Name = "AvionesData";
            this.AvionesData.ReadOnly = true;
            this.AvionesData.RowHeadersVisible = false;
            this.AvionesData.RowHeadersWidth = 51;
            this.AvionesData.RowTemplate.Height = 24;
            this.AvionesData.Size = new System.Drawing.Size(446, 417);
            this.AvionesData.TabIndex = 2;
            // 
            // HangarData
            // 
            this.HangarData.AllowUserToAddRows = false;
            this.HangarData.BackgroundColor = System.Drawing.Color.FloralWhite;
            this.HangarData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.HangarData.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.IdH,
            this.Edges,
            this.IDAviones,
            this.cantAviones,
            this.cantGasolina});
            this.HangarData.Location = new System.Drawing.Point(842, 431);
            this.HangarData.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.HangarData.Name = "HangarData";
            this.HangarData.ReadOnly = true;
            this.HangarData.RowHeadersVisible = false;
            this.HangarData.RowHeadersWidth = 51;
            this.HangarData.RowTemplate.Height = 24;
            this.HangarData.Size = new System.Drawing.Size(446, 281);
            this.HangarData.TabIndex = 3;
            // 
            // IdH
            // 
            this.IdH.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.IdH.FillWeight = 90.32258F;
            this.IdH.HeaderText = "ID";
            this.IdH.MinimumWidth = 6;
            this.IdH.Name = "IdH";
            this.IdH.ReadOnly = true;
            // 
            // Edges
            // 
            this.Edges.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Edges.FillWeight = 300F;
            this.Edges.HeaderText = "Edges";
            this.Edges.MinimumWidth = 6;
            this.Edges.Name = "Edges";
            this.Edges.ReadOnly = true;
            // 
            // IDAviones
            // 
            this.IDAviones.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.IDAviones.FillWeight = 90.32258F;
            this.IDAviones.HeaderText = "Cant. Aviones Actuales";
            this.IDAviones.MinimumWidth = 6;
            this.IDAviones.Name = "IDAviones";
            this.IDAviones.ReadOnly = true;
            // 
            // cantAviones
            // 
            this.cantAviones.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.cantAviones.FillWeight = 90.32258F;
            this.cantAviones.HeaderText = "Cant. Aviones Totales";
            this.cantAviones.MinimumWidth = 6;
            this.cantAviones.Name = "cantAviones";
            this.cantAviones.ReadOnly = true;
            // 
            // cantGasolina
            // 
            this.cantGasolina.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.cantGasolina.FillWeight = 95F;
            this.cantGasolina.HeaderText = "Cant. Gasolina";
            this.cantGasolina.MinimumWidth = 6;
            this.cantGasolina.Name = "cantGasolina";
            this.cantGasolina.ReadOnly = true;
            // 
            // Id
            // 
            this.Id.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Id.HeaderText = "Id";
            this.Id.MinimumWidth = 6;
            this.Id.Name = "Id";
            this.Id.ReadOnly = true;
            // 
            // weight
            // 
            this.weight.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.weight.FillWeight = 30F;
            this.weight.HeaderText = "precio";
            this.weight.Name = "weight";
            this.weight.ReadOnly = true;
            // 
            // Origen
            // 
            this.Origen.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Origen.FillWeight = 40F;
            this.Origen.HeaderText = "Origen";
            this.Origen.MinimumWidth = 6;
            this.Origen.Name = "Origen";
            this.Origen.ReadOnly = true;
            // 
            // Destino
            // 
            this.Destino.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Destino.FillWeight = 40F;
            this.Destino.HeaderText = "Destino";
            this.Destino.MinimumWidth = 6;
            this.Destino.Name = "Destino";
            this.Destino.ReadOnly = true;
            // 
            // Gasolina
            // 
            this.Gasolina.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Gasolina.FillWeight = 30F;
            this.Gasolina.HeaderText = "Gasolina";
            this.Gasolina.MinimumWidth = 6;
            this.Gasolina.Name = "Gasolina";
            this.Gasolina.ReadOnly = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1370, 740);
            this.Controls.Add(this.HangarData);
            this.Controls.Add(this.AvionesData);
            this.Controls.Add(this.MapPictureBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "AirWar Game";
            ((System.ComponentModel.ISupportInitialize)(this.MapPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.AvionesData)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.HangarData)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox MapPictureBox;
        private System.Windows.Forms.DataGridView AvionesData;
        private System.Windows.Forms.DataGridView HangarData;
        private System.Windows.Forms.DataGridViewTextBoxColumn IdH;
        private System.Windows.Forms.DataGridViewTextBoxColumn Edges;
        private System.Windows.Forms.DataGridViewTextBoxColumn IDAviones;
        private System.Windows.Forms.DataGridViewTextBoxColumn cantAviones;
        private System.Windows.Forms.DataGridViewTextBoxColumn cantGasolina;
        private System.Windows.Forms.DataGridViewTextBoxColumn Id;
        private System.Windows.Forms.DataGridViewTextBoxColumn weight;
        private System.Windows.Forms.DataGridViewTextBoxColumn Origen;
        private System.Windows.Forms.DataGridViewTextBoxColumn Destino;
        private System.Windows.Forms.DataGridViewTextBoxColumn Gasolina;
    }
}

