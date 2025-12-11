namespace Laba1
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            add_btn = new Button();
            update_btn = new Button();
            delete_btn = new Button();
            shift_btn = new Button();
            up_btn = new Button();
            panel1 = new Panel();
            checkBox1 = new CheckBox();
            panel2 = new Panel();
            comboBox1 = new ComboBox();
            checkBox2 = new CheckBox();
            panel3 = new Panel();
            comboBox2 = new ComboBox();
            checkBox3 = new CheckBox();
            panel4 = new Panel();
            checkBox4 = new CheckBox();
            dataGridView1 = new DataGridView();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            panel3.SuspendLayout();
            panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            SuspendLayout();
            // 
            // add_btn
            // 
            add_btn.Location = new Point(1861, 71);
            add_btn.Margin = new Padding(4, 5, 4, 5);
            add_btn.Name = "add_btn";
            add_btn.Size = new Size(199, 47);
            add_btn.TabIndex = 0;
            add_btn.Text = "Добавить";
            add_btn.UseVisualStyleBackColor = true;
            add_btn.Click += add_btn_Click;
            // 
            // update_btn
            // 
            update_btn.Location = new Point(1861, 145);
            update_btn.Margin = new Padding(4, 5, 4, 5);
            update_btn.Name = "update_btn";
            update_btn.Size = new Size(199, 47);
            update_btn.TabIndex = 1;
            update_btn.Text = "Изменить";
            update_btn.UseVisualStyleBackColor = true;
            update_btn.Click += update_btn_Click;
            // 
            // delete_btn
            // 
            delete_btn.Location = new Point(1861, 229);
            delete_btn.Margin = new Padding(4, 5, 4, 5);
            delete_btn.Name = "delete_btn";
            delete_btn.Size = new Size(199, 47);
            delete_btn.TabIndex = 2;
            delete_btn.Text = "Удалить";
            delete_btn.UseVisualStyleBackColor = true;
            delete_btn.Click += delete_btn_Click;
            // 
            // shift_btn
            // 
            shift_btn.Location = new Point(2197, 71);
            shift_btn.Margin = new Padding(4, 5, 4, 5);
            shift_btn.Name = "shift_btn";
            shift_btn.Size = new Size(199, 47);
            shift_btn.TabIndex = 3;
            shift_btn.Text = "Перевод ";
            shift_btn.UseVisualStyleBackColor = true;
            shift_btn.Click += shift_btn_Click;
            // 
            // up_btn
            // 
            up_btn.Location = new Point(2197, 150);
            up_btn.Margin = new Padding(4, 5, 4, 5);
            up_btn.Name = "up_btn";
            up_btn.Size = new Size(199, 47);
            up_btn.TabIndex = 4;
            up_btn.Text = "Повышение";
            up_btn.UseVisualStyleBackColor = true;
            up_btn.Click += up_btn_Click;
            // 
            // panel1
            // 
            panel1.BackColor = SystemColors.ButtonHighlight;
            panel1.Controls.Add(checkBox1);
            panel1.Location = new Point(1814, 443);
            panel1.Margin = new Padding(6, 7, 6, 7);
            panel1.Name = "panel1";
            panel1.Size = new Size(869, 86);
            panel1.TabIndex = 6;
            // 
            // checkBox1
            // 
            checkBox1.AutoSize = true;
            checkBox1.Location = new Point(9, 10);
            checkBox1.Margin = new Padding(6, 7, 6, 7);
            checkBox1.Name = "checkBox1";
            checkBox1.Size = new Size(341, 36);
            checkBox1.TabIndex = 0;
            checkBox1.Text = "Вывести всех сотрудников";
            checkBox1.UseVisualStyleBackColor = true;
            checkBox1.CheckedChanged += checkBox1_CheckedChanged;
            // 
            // panel2
            // 
            panel2.BackColor = SystemColors.ButtonHighlight;
            panel2.Controls.Add(comboBox1);
            panel2.Controls.Add(checkBox2);
            panel2.Location = new Point(1814, 544);
            panel2.Margin = new Padding(6, 7, 6, 7);
            panel2.Name = "panel2";
            panel2.Size = new Size(869, 91);
            panel2.TabIndex = 7;
            // 
            // comboBox1
            // 
            comboBox1.FormattingEnabled = true;
            comboBox1.Location = new Point(490, 10);
            comboBox1.Margin = new Padding(6, 7, 6, 7);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(368, 40);
            comboBox1.TabIndex = 1;
            comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
            // 
            // checkBox2
            // 
            checkBox2.AutoSize = true;
            checkBox2.Location = new Point(9, 10);
            checkBox2.Margin = new Padding(6, 7, 6, 7);
            checkBox2.Name = "checkBox2";
            checkBox2.Size = new Size(423, 36);
            checkBox2.TabIndex = 0;
            checkBox2.Text = "Вывести сотрудников по позиции";
            checkBox2.UseVisualStyleBackColor = true;
            checkBox2.CheckedChanged += checkBox2_CheckedChanged;
            // 
            // panel3
            // 
            panel3.BackColor = SystemColors.ButtonHighlight;
            panel3.Controls.Add(comboBox2);
            panel3.Controls.Add(checkBox3);
            panel3.Location = new Point(1814, 650);
            panel3.Margin = new Padding(6, 7, 6, 7);
            panel3.Name = "panel3";
            panel3.Size = new Size(869, 91);
            panel3.TabIndex = 8;
            // 
            // comboBox2
            // 
            comboBox2.FormattingEnabled = true;
            comboBox2.Location = new Point(490, 10);
            comboBox2.Margin = new Padding(6, 7, 6, 7);
            comboBox2.Name = "comboBox2";
            comboBox2.Size = new Size(368, 40);
            comboBox2.TabIndex = 1;
            comboBox2.SelectedIndexChanged += comboBox2_SelectedIndexChanged;
            // 
            // checkBox3
            // 
            checkBox3.AutoSize = true;
            checkBox3.Location = new Point(9, 10);
            checkBox3.Margin = new Padding(6, 7, 6, 7);
            checkBox3.Name = "checkBox3";
            checkBox3.Size = new Size(403, 36);
            checkBox3.TabIndex = 0;
            checkBox3.Text = "Вывести сотрудников по отделу";
            checkBox3.UseVisualStyleBackColor = true;
            checkBox3.CheckedChanged += checkBox3_CheckedChanged;
            // 
            // panel4
            // 
            panel4.BackColor = SystemColors.ButtonHighlight;
            panel4.Controls.Add(checkBox4);
            panel4.Location = new Point(1814, 756);
            panel4.Margin = new Padding(6, 7, 6, 7);
            panel4.Name = "panel4";
            panel4.Size = new Size(869, 86);
            panel4.TabIndex = 9;
            // 
            // checkBox4
            // 
            checkBox4.AutoSize = true;
            checkBox4.Location = new Point(9, 10);
            checkBox4.Margin = new Padding(6, 7, 6, 7);
            checkBox4.Name = "checkBox4";
            checkBox4.Size = new Size(457, 36);
            checkBox4.TabIndex = 0;
            checkBox4.Text = "Вывести сотрудников на повышение";
            checkBox4.UseVisualStyleBackColor = true;
            checkBox4.CheckedChanged += checkBox4_CheckedChanged;
            // 
            // dataGridView1
            // 
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = false;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Location = new Point(26, 30);
            dataGridView1.Margin = new Padding(6, 7, 6, 7);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.ReadOnly = true;
            dataGridView1.RowHeadersWidth = 82;
            dataGridView1.Size = new Size(1775, 800);
            dataGridView1.TabIndex = 10;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(13F, 32F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSize = true;
            ClientSize = new Size(2732, 874);
            Controls.Add(dataGridView1);
            Controls.Add(panel4);
            Controls.Add(panel3);
            Controls.Add(panel2);
            Controls.Add(panel1);
            Controls.Add(up_btn);
            Controls.Add(shift_btn);
            Controls.Add(delete_btn);
            Controls.Add(update_btn);
            Controls.Add(add_btn);
            Margin = new Padding(4, 5, 4, 5);
            Name = "Form1";
            Text = "Form1";
            FormClosed += Form1_FormClosed;
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            panel3.ResumeLayout(false);
            panel3.PerformLayout();
            panel4.ResumeLayout(false);
            panel4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button add_btn;
        private System.Windows.Forms.Button update_btn;
        private System.Windows.Forms.Button delete_btn;
        private System.Windows.Forms.Button shift_btn;
        private System.Windows.Forms.Button up_btn;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.ComboBox comboBox2;
        private System.Windows.Forms.CheckBox checkBox3;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.CheckBox checkBox4;
        private System.Windows.Forms.DataGridView dataGridView1;
    }
}

