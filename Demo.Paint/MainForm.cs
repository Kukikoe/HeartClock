﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Demo.Paint
{
    public partial class MainForm : Form
    { 
        private Point _leftTopPoint;
        private bool _isMouseDown = false;

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Paint(object sender, PaintEventArgs e)
        {
            Graphics gr = e.Graphics;
            PointF centerPoint = new PointF(ClientSize.Width / 2.0F, ClientSize.Height / 2.0F);
            int baseRadius = ClientSize.Width > ClientSize.Height ? ClientSize.Height / 2 : ClientSize.Width / 2;

            GraphicsPath gp = new GraphicsPath();
            int width_heart = 45;
            int hight_heart = 20;
            gp.AddBezier(
                new Point(ClientSize.Width / 2, ClientSize.Height / 2 - hight_heart / 3),
                new Point(ClientSize.Width / 2 - width_heart / 3, ClientSize.Height / 2 - hight_heart - 10),
                new Point(ClientSize.Width / 2 - width_heart, ClientSize.Height / 2 - hight_heart / 3),
                new Point(ClientSize.Width / 2, ClientSize.Height / 2 + hight_heart / 2 + 10)
                );

            gp.AddBezier(
               new Point(ClientSize.Width / 2, ClientSize.Height / 2 - hight_heart / 3),
               new Point(ClientSize.Width / 2 + width_heart / 3, ClientSize.Height / 2 - hight_heart - 10),
               new Point(ClientSize.Width / 2 + width_heart, ClientSize.Height / 2 - hight_heart / 3),
               new Point(ClientSize.Width / 2, ClientSize.Height / 2 + hight_heart / 2 + 10)
               );
            Brush br = new SolidBrush(Color.Red);
            gr.FillPath(br, gp);

            DrawArrow(Color.Black, 1, gr, baseRadius - 45, DateTime.Now.Second * 6);
            DrawArrow(Color.Black, 3, gr, baseRadius - 45, DateTime.Now.Minute * 6);
            DrawArrow(Color.Black, 5, gr, baseRadius / 2 - 40, DateTime.Now.Hour * 30);
            DrawScale(Color.Black, 1, gr, baseRadius - 30);
            GraphicsContainer container =
                             gr.BeginContainer(
                                 new Rectangle(ClientSize.Width / 2, ClientSize.Height / 2, ClientSize.Width, ClientSize.Height),
                                 new Rectangle(0, 0, ClientSize.Width, ClientSize.Height),
                                 GraphicsUnit.Pixel);
            gr.FillEllipse(Brushes.Black, 0 - 12 / 2, 0 - 12 / 2, 12, 12);
            gr.FillEllipse(Brushes.White, 0 - 4 / 2, 0 - 4 / 2, 4, 4);
            gr.EndContainer(container);        
        }
        private void DrawScale(Color color, int penWidth, Graphics gr, int radius)
        {
            int tmp = radius;
            PointF centerPoint = new PointF(ClientSize.Width / 2.0F, ClientSize.Height / 2.0F);
            GraphicsPath gp = new GraphicsPath();
            for (int i = 1; i < 13; ++i)
            {               
                GraphicsContainer container =
                                gr.BeginContainer(
                                    new Rectangle(0, 0, ClientSize.Width, ClientSize.Height),
                                    new Rectangle(0, 0, ClientSize.Width, ClientSize.Height),
                                    GraphicsUnit.Pixel);
                SizeF sd = gr.MeasureString(i.ToString(), this.Font);
                if (i == 12)
                {
                    radius /= 2;
                }
                else if (i == 6)
                {
                    radius += 30;
                }
                else if (i == 3)
                {
                    radius += 15;
                }
                else if (i == 9)
                {
                    radius += 15;
                }
                PointF rp = new PointF(centerPoint.X + (radius - 15) * (float)Math.Cos(((i * 30 + 270) % 360) * Math.PI / 180.0F) - sd.Width / 2, centerPoint.Y + (radius -15) * (float)Math.Sin(((i * 30 + 270) % 360) * Math.PI / 180.0F) - sd.Height / 2);
                gr.DrawString(i.ToString(), this.Font, new SolidBrush(Color.Black), rp);
                radius = tmp;            
                gr.EndContainer(container);
            }
        }

        private void DrawArrow(Color color, int penWidth, Graphics gr, int length, int angle)
        {
            GraphicsContainer container =
                            gr.BeginContainer(
                                new Rectangle(ClientSize.Width / 2, ClientSize.Height / 2, ClientSize.Width, ClientSize.Height),
                                new Rectangle(0, 0, ClientSize.Width, ClientSize.Height),
                                GraphicsUnit.Pixel);
            gr.RotateTransform(angle);

            gr.DrawLine(new Pen(color, penWidth),
                new Point(0, 0),
                new Point(0, -length)
            );
            gr.EndContainer(container);
        }

        private void MainForm_ResizeEnd(object sender, EventArgs e)
        {
            Invalidate();
        }

        private void timerTick(object sender, EventArgs e)
        {
            Invalidate();
        }

        private void MainForm_MouseDown(object sender, MouseEventArgs e)
        {
            _isMouseDown = true;
            _leftTopPoint.X = e.X;
            _leftTopPoint.Y = e.Y;
        }

        private void MainForm_MouseUp(object sender, MouseEventArgs e)
        {
            _isMouseDown = false;
        }

        private void MainForm_MouseMove(object sender, MouseEventArgs e)
        {
            if (_isMouseDown)
            {
                this.Location = new Point((Cursor.Position.X - this.Location.X), (Cursor.Position.Y - this.Location.Y));
            }
        }
    }
}
