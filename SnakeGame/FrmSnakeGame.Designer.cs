﻿namespace SnakeGame
{
    partial class FrmSnakeGame
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
            this.components = new System.ComponentModel.Container();
            this.timerforGame = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // timerforGame
            // 
            this.timerforGame.Interval = 300;
            this.timerforGame.Tick += new System.EventHandler(this.TimerforGame_Tick);
            // 
            // FrmSnakeGame
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(684, 482);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "FrmSnakeGame";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Змейка на C#";
            this.Load += new System.EventHandler(this.FrmSnakeGame_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.FrmSnakeGame_Paint);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FrmSnakeGame_KeyDown);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Timer timerforGame;
    }
}

