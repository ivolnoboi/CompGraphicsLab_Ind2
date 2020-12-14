namespace Individual2
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
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.light1 = new System.Windows.Forms.CheckBox();
            this.light2 = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.matteCube = new System.Windows.Forms.CheckBox();
            this.matteSphere = new System.Windows.Forms.CheckBox();
            this.matteSphere2 = new System.Windows.Forms.CheckBox();
            this.mirrorCube = new System.Windows.Forms.CheckBox();
            this.mirrorSphere = new System.Windows.Forms.CheckBox();
            this.transparentCube = new System.Windows.Forms.CheckBox();
            this.transparentSphere = new System.Windows.Forms.CheckBox();
            this.enableMirror = new System.Windows.Forms.CheckBox();
            this.enableTransparent = new System.Windows.Forms.CheckBox();
            this.left = new System.Windows.Forms.RadioButton();
            this.right = new System.Windows.Forms.RadioButton();
            this.front = new System.Windows.Forms.RadioButton();
            this.back = new System.Windows.Forms.RadioButton();
            this.button2 = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox1.Location = new System.Drawing.Point(12, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(810, 751);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button1.Location = new System.Drawing.Point(842, 712);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(275, 51);
            this.button1.TabIndex = 1;
            this.button1.Text = "Render";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(837, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(186, 25);
            this.label1.TabIndex = 2;
            this.label1.Text = "Источники света";
            // 
            // light1
            // 
            this.light1.AutoSize = true;
            this.light1.Location = new System.Drawing.Point(842, 48);
            this.light1.Name = "light1";
            this.light1.Size = new System.Drawing.Size(212, 21);
            this.light1.TabIndex = 3;
            this.light1.Text = "Включить 1 источник света";
            this.light1.UseVisualStyleBackColor = true;
            // 
            // light2
            // 
            this.light2.AutoSize = true;
            this.light2.Location = new System.Drawing.Point(842, 76);
            this.light2.Name = "light2";
            this.light2.Size = new System.Drawing.Size(212, 21);
            this.light2.TabIndex = 4;
            this.light2.Text = "Включить 2 источник света";
            this.light2.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(837, 112);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(209, 25);
            this.label2.TabIndex = 5;
            this.label2.Text = "Зеркальность стен";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label3.Location = new System.Drawing.Point(832, 245);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(191, 25);
            this.label3.TabIndex = 12;
            this.label3.Text = "Элементы сцены";
            // 
            // matteCube
            // 
            this.matteCube.AutoSize = true;
            this.matteCube.Location = new System.Drawing.Point(837, 282);
            this.matteCube.Name = "matteCube";
            this.matteCube.Size = new System.Drawing.Size(115, 21);
            this.matteCube.TabIndex = 13;
            this.matteCube.Text = "Матовый куб";
            this.matteCube.UseVisualStyleBackColor = true;
            // 
            // matteSphere
            // 
            this.matteSphere.AutoSize = true;
            this.matteSphere.Location = new System.Drawing.Point(837, 309);
            this.matteSphere.Name = "matteSphere";
            this.matteSphere.Size = new System.Drawing.Size(133, 21);
            this.matteSphere.TabIndex = 14;
            this.matteSphere.Text = "Матовая сфера";
            this.matteSphere.UseVisualStyleBackColor = true;
            // 
            // matteSphere2
            // 
            this.matteSphere2.AutoSize = true;
            this.matteSphere2.Location = new System.Drawing.Point(837, 336);
            this.matteSphere2.Name = "matteSphere2";
            this.matteSphere2.Size = new System.Drawing.Size(145, 21);
            this.matteSphere2.TabIndex = 15;
            this.matteSphere2.Text = "Матовая сфера 2";
            this.matteSphere2.UseVisualStyleBackColor = true;
            // 
            // mirrorCube
            // 
            this.mirrorCube.AutoSize = true;
            this.mirrorCube.Location = new System.Drawing.Point(837, 363);
            this.mirrorCube.Name = "mirrorCube";
            this.mirrorCube.Size = new System.Drawing.Size(137, 21);
            this.mirrorCube.TabIndex = 16;
            this.mirrorCube.Text = "Зеркальный куб";
            this.mirrorCube.UseVisualStyleBackColor = true;
            // 
            // mirrorSphere
            // 
            this.mirrorSphere.AutoSize = true;
            this.mirrorSphere.Location = new System.Drawing.Point(837, 390);
            this.mirrorSphere.Name = "mirrorSphere";
            this.mirrorSphere.Size = new System.Drawing.Size(155, 21);
            this.mirrorSphere.TabIndex = 17;
            this.mirrorSphere.Text = "Зеркальная сфера";
            this.mirrorSphere.UseVisualStyleBackColor = true;
            // 
            // transparentCube
            // 
            this.transparentCube.AutoSize = true;
            this.transparentCube.Location = new System.Drawing.Point(837, 417);
            this.transparentCube.Name = "transparentCube";
            this.transparentCube.Size = new System.Drawing.Size(139, 21);
            this.transparentCube.TabIndex = 18;
            this.transparentCube.Text = "Прозрачный куб";
            this.transparentCube.UseVisualStyleBackColor = true;
            // 
            // transparentSphere
            // 
            this.transparentSphere.AutoSize = true;
            this.transparentSphere.Location = new System.Drawing.Point(837, 444);
            this.transparentSphere.Name = "transparentSphere";
            this.transparentSphere.Size = new System.Drawing.Size(157, 21);
            this.transparentSphere.TabIndex = 19;
            this.transparentSphere.Text = "Прозрачная сфера";
            this.transparentSphere.UseVisualStyleBackColor = true;
            // 
            // enableMirror
            // 
            this.enableMirror.AutoSize = true;
            this.enableMirror.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.enableMirror.Location = new System.Drawing.Point(842, 636);
            this.enableMirror.Name = "enableMirror";
            this.enableMirror.Size = new System.Drawing.Size(237, 24);
            this.enableMirror.TabIndex = 20;
            this.enableMirror.Text = "Включить зеркальность";
            this.enableMirror.UseVisualStyleBackColor = true;
            // 
            // enableTransparent
            // 
            this.enableTransparent.AutoSize = true;
            this.enableTransparent.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.enableTransparent.Location = new System.Drawing.Point(842, 666);
            this.enableTransparent.Name = "enableTransparent";
            this.enableTransparent.Size = new System.Drawing.Size(238, 24);
            this.enableTransparent.TabIndex = 21;
            this.enableTransparent.Text = "Включить прозрачность";
            this.enableTransparent.UseVisualStyleBackColor = true;
            // 
            // left
            // 
            this.left.AutoSize = true;
            this.left.Location = new System.Drawing.Point(842, 140);
            this.left.Name = "left";
            this.left.Size = new System.Drawing.Size(70, 21);
            this.left.TabIndex = 22;
            this.left.TabStop = true;
            this.left.Text = "Левая";
            this.left.UseVisualStyleBackColor = true;
            // 
            // right
            // 
            this.right.AutoSize = true;
            this.right.Location = new System.Drawing.Point(842, 167);
            this.right.Name = "right";
            this.right.Size = new System.Drawing.Size(78, 21);
            this.right.TabIndex = 23;
            this.right.TabStop = true;
            this.right.Text = "Правая";
            this.right.UseVisualStyleBackColor = true;
            // 
            // front
            // 
            this.front.AutoSize = true;
            this.front.Location = new System.Drawing.Point(842, 194);
            this.front.Name = "front";
            this.front.Size = new System.Drawing.Size(95, 21);
            this.front.TabIndex = 24;
            this.front.TabStop = true;
            this.front.Text = "Передняя";
            this.front.UseVisualStyleBackColor = true;
            // 
            // back
            // 
            this.back.AutoSize = true;
            this.back.Location = new System.Drawing.Point(842, 221);
            this.back.Name = "back";
            this.back.Size = new System.Drawing.Size(78, 21);
            this.back.TabIndex = 25;
            this.back.TabStop = true;
            this.back.Text = "Задняя";
            this.back.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(837, 510);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(133, 40);
            this.button2.TabIndex = 26;
            this.button2.Text = "Влево вперёд";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label4.Location = new System.Drawing.Point(837, 482);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(272, 25);
            this.label4.TabIndex = 27;
            this.label4.Text = "Переместить 2 источник";
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(976, 556);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(133, 40);
            this.button3.TabIndex = 28;
            this.button3.Text = "Вправо назад";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(837, 556);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(133, 40);
            this.button4.TabIndex = 29;
            this.button4.Text = "Влево назад";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(976, 510);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(133, 40);
            this.button5.TabIndex = 30;
            this.button5.Text = "Вправо вперёд";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1129, 793);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.back);
            this.Controls.Add(this.front);
            this.Controls.Add(this.right);
            this.Controls.Add(this.left);
            this.Controls.Add(this.enableTransparent);
            this.Controls.Add(this.enableMirror);
            this.Controls.Add(this.transparentSphere);
            this.Controls.Add(this.transparentCube);
            this.Controls.Add(this.mirrorSphere);
            this.Controls.Add(this.mirrorCube);
            this.Controls.Add(this.matteSphere2);
            this.Controls.Add(this.matteSphere);
            this.Controls.Add(this.matteCube);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.light2);
            this.Controls.Add(this.light1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.pictureBox1);
            this.Name = "Form1";
            this.Text = "Индивидуальное задание №2 Корнуэльская комната";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox light1;
        private System.Windows.Forms.CheckBox light2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox matteCube;
        private System.Windows.Forms.CheckBox matteSphere;
        private System.Windows.Forms.CheckBox matteSphere2;
        private System.Windows.Forms.CheckBox mirrorCube;
        private System.Windows.Forms.CheckBox mirrorSphere;
        private System.Windows.Forms.CheckBox transparentCube;
        private System.Windows.Forms.CheckBox transparentSphere;
        private System.Windows.Forms.CheckBox enableMirror;
        private System.Windows.Forms.CheckBox enableTransparent;
        private System.Windows.Forms.RadioButton left;
        private System.Windows.Forms.RadioButton right;
        private System.Windows.Forms.RadioButton front;
        private System.Windows.Forms.RadioButton back;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
    }
}

