namespace StarCatalog.WinFormsApp
{
    partial class MainForm
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
            this.dataGridViewStars = new System.Windows.Forms.DataGridView();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnFilter = new System.Windows.Forms.Button();
            this.btnResetFilter = new System.Windows.Forms.Button();
            this.btnCalculateAvg = new System.Windows.Forms.Button();
            this.lblAvgRadius = new System.Windows.Forms.Label();
            this.cmbFilterType = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewStars)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridViewStars
            // 
            this.dataGridViewStars.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewStars.Location = new System.Drawing.Point(2, 2);
            this.dataGridViewStars.Name = "dataGridViewStars";
            this.dataGridViewStars.RowHeadersWidth = 51;
            this.dataGridViewStars.RowTemplate.Height = 24;
            this.dataGridViewStars.Size = new System.Drawing.Size(838, 339);
            this.dataGridViewStars.TabIndex = 0;
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(2, 347);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(129, 42);
            this.btnAdd.TabIndex = 1;
            this.btnAdd.Text = "Добавить";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.Location = new System.Drawing.Point(137, 347);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(119, 42);
            this.btnEdit.TabIndex = 2;
            this.btnEdit.Text = "Изменить";
            this.btnEdit.UseVisualStyleBackColor = true;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(262, 347);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(124, 42);
            this.btnDelete.TabIndex = 3;
            this.btnDelete.Text = "Удалить";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnFilter
            // 
            this.btnFilter.Location = new System.Drawing.Point(534, 377);
            this.btnFilter.Name = "btnFilter";
            this.btnFilter.Size = new System.Drawing.Size(193, 41);
            this.btnFilter.TabIndex = 4;
            this.btnFilter.Text = "Фильтр по Жанру";
            this.btnFilter.UseVisualStyleBackColor = true;
            this.btnFilter.Click += new System.EventHandler(this.btnFilter_Click);
            // 
            // btnResetFilter
            // 
            this.btnResetFilter.Location = new System.Drawing.Point(733, 347);
            this.btnResetFilter.Name = "btnResetFilter";
            this.btnResetFilter.Size = new System.Drawing.Size(107, 71);
            this.btnResetFilter.TabIndex = 5;
            this.btnResetFilter.Text = "Сбросить фильтр";
            this.btnResetFilter.UseVisualStyleBackColor = true;
            this.btnResetFilter.Click += new System.EventHandler(this.btnResetFilter_Click);
            // 
            // btnCalculateAvg
            // 
            this.btnCalculateAvg.Location = new System.Drawing.Point(846, 12);
            this.btnCalculateAvg.Name = "btnCalculateAvg";
            this.btnCalculateAvg.Size = new System.Drawing.Size(119, 42);
            this.btnCalculateAvg.TabIndex = 6;
            this.btnCalculateAvg.Text = "Средний Диаметр";
            this.btnCalculateAvg.UseVisualStyleBackColor = true;
            this.btnCalculateAvg.Click += new System.EventHandler(this.btnCalculateAvg_Click);
            // 
            // lblAvgRadius
            // 
            this.lblAvgRadius.AutoSize = true;
            this.lblAvgRadius.Location = new System.Drawing.Point(846, 57);
            this.lblAvgRadius.Name = "lblAvgRadius";
            this.lblAvgRadius.Size = new System.Drawing.Size(10, 16);
            this.lblAvgRadius.TabIndex = 8;
            this.lblAvgRadius.Text = " ";
            // 
            // cmbFilterType
            // 
            this.cmbFilterType.FormattingEnabled = true;
            this.cmbFilterType.Location = new System.Drawing.Point(534, 347);
            this.cmbFilterType.Name = "cmbFilterType";
            this.cmbFilterType.Size = new System.Drawing.Size(193, 24);
            this.cmbFilterType.TabIndex = 9;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1131, 486);
            this.Controls.Add(this.cmbFilterType);
            this.Controls.Add(this.lblAvgRadius);
            this.Controls.Add(this.btnCalculateAvg);
            this.Controls.Add(this.btnResetFilter);
            this.Controls.Add(this.btnFilter);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnEdit);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.dataGridViewStars);
            this.Name = "MainForm";
            this.Text = "MainForm";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewStars)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridViewStars;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnFilter;
        private System.Windows.Forms.Button btnResetFilter;
        private System.Windows.Forms.Button btnCalculateAvg;
        private System.Windows.Forms.Label lblAvgRadius;
        private System.Windows.Forms.ComboBox cmbFilterType;
    }
}