using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp9
{
    public partial class Form1 : Form
    {
        List<Emitter> emitters = new List<Emitter>();
        
        Emitter emitter; // поле для эмиттера

        public Form1()
        {

            InitializeComponent();
            picDisplay.Image = new Bitmap(picDisplay.Width, picDisplay.Height); // привязываю изображение

            this.emitter = new Emitter // привязываю emitter к полю emitter
            {
                Direction = 0,
                Spreading = 10,
                SpeedMin = 10,
                SpeedMax = 10,
                ColorFrom = Color.Gold,
                ColorTo = Color.FromArgb(0, Color.Red),
                ParticlesPerTick = 10,
                X = picDisplay.Width / 2,
                Y = picDisplay.Height / 2,
            };

            emitters.Add(this.emitter); // добавляю для обновления и рендеринга
        }
        
        private void timer1_Tick(object sender, EventArgs e)
        {
            particlesInForm(); //привязываю метод particlesInForm() для счетчика частиц
            emitter.UpdateState();
            using (var g = Graphics.FromImage(picDisplay.Image))
            {
                g.Clear(Color.Black);
                emitter.Render(g);
            }

            picDisplay.Invalidate();
        }

        private void tbDirection_Scroll(object sender, EventArgs e) // трэкбар для положения
        {
            emitter.Direction = tbDirection.Value;
            lblDirection.Text = $"{tbDirection.Value}°";
        }

        private void tbDirection1_Scroll(object sender, EventArgs e) // трэкбар для разброса
        {
            emitter.Spreading = tbDirection1.Value;
            lblSpread.Text = $"{tbDirection1.Value}%";
        }

        private void tbParticle_Scroll(object sender, EventArgs e) // трэкбар для частиц в тик
        {
            emitter.ParticlesPerTick = tbParticle.Value;

        }

        private void tbLife_Scroll(object sender, EventArgs e) // трэкбар для количества хп
        {
            emitter.LifeMax = tbLife.Value;
        }

        private void trackBar1_Scroll(object sender, EventArgs e) // трэкбал для скорости
        {
            emitter.SpeedMax = trackBar1.Value;
            lblSpeed.Text = $"{trackBar1.Value}";
        }

        private void particlesInForm() // для вывода частиц в label
        {
            int count = 0;
            foreach (Particle particle in emitter.particles) // создаем цикл для проверки на удовлетворительность частиц
            {
                if (particle.X > 0 && particle.X < picDisplay.Width) // проверяем удовлетворяет ли по оси х
                {
                    if (particle.Y > 0 && particle.Y < picDisplay.Height) // проверяем, удовлетворяет ли по оси у
                    {
                        count++; // если удовлетворяет то счетчик нарастает, отсылает к timerTick
                    }
                }
            }
            label6.Text = $"Количество частиц: {count}"; // выводим количество частиц в label
        }
    }
}
