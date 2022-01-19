using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp9
{


    public class Emitter
    {
        public float GravitationX = 0; // гравитаци по горизонтали х
        public float GravitationY = 1; // гравитация по вертикали у

        public List<IImpactPoint> impactPoints = new List<IImpactPoint>(); // список частиц для рендеринга и т.д.

        public List<Particle> particles = new List<Particle>(); // тут хранятся частицы

        public int ParticlesCount = 500; // максимальное количество частиц при спавне

        public int X; // координата центра эмиттера по оси х, вместо мыши
        public int Y; // координата центра эмиттера по оси у
        public int Direction = 0; // вектор направления куда сыпет эмиттер
        public int Spreading = 360; // разброс частиц
        public int SpeedMin = 1; // минимальная скорость частицы
        public int SpeedMax = 100; // максимальная скорость частицы
        public int RadiusMin = 2; // минимальный радиус частицы
        public int RadiusMax = 10; // максимальный радиус частицы
        public int LifeMin = 0; // минимальное время жизни частицы
        public int LifeMax = 100; // максимальное время жизни частицы

        public int ParticlesPerTick = 1; // генерация 1 частицы в тик


        public Color ColorFrom = Color.White; // начальный цвет частицы
        public Color ColorTo = Color.FromArgb(0, Color.Black); // конечный цвет частицы, который перетекает с начального


        public virtual void ResetParticle(Particle particle) // метод сброса частицы
        {
            particle.Life = Particle.rand.Next(LifeMin, LifeMax);

            particle.X = X;
            particle.Y = Y;

            var direction = Direction
                + (double)Particle.rand.Next(Spreading)
                - Spreading / 2;

            var speed = Particle.rand.Next(SpeedMin, SpeedMax);

            particle.SpeedX = (float)(Math.Cos(direction / 180 * Math.PI) * speed); // рассчитываем скорости частиц с условием гравитации
            particle.SpeedY = -(float)(Math.Sin(direction / 180 * Math.PI) * speed);

            particle.Radius = Particle.rand.Next(RadiusMin, RadiusMax);
        }

        public virtual Particle CreateParticle() // генерация частицы для переопределения
        {
            var particle = new ParticleColorful();
            particle.FromColor = ColorFrom;
            particle.ToColor = ColorTo;

            return particle;
        }



        public void UpdateState()
        {
            int particlesToCreate = ParticlesPerTick;

            foreach (var particle in particles)
            {
                if (particle.Life <= 0) // проверка умерла ли частица
                {
                    ResetParticle(particle); // вызов сброса частицы

                    if (particlesToCreate > 0)
                    {
                        particlesToCreate -= 1; // уменьшаем счётчик созданных частиц на 1

                        ResetParticle(particle); // вызов сброса частицы
                    }
                }
                else
                {
                    foreach (var point in impactPoints)
                    {
                        point.ImpactParticle(particle);
                    }


                    particle.SpeedX += GravitationX; // пересчитываем гравитацию, так как она воздействует на скорость
                    particle.SpeedY += GravitationY;

                    particle.X += particle.SpeedX;
                    particle.Y += particle.SpeedY;
                }
            }
            while (particlesToCreate >= 1) // срабатывает в начале работы эмиттера, пока не накомится критическое количество частиц
            {
                particlesToCreate -= 1;
                var particle = CreateParticle(); // вызываем создание частицы
                ResetParticle(particle); // вызов сброса частицы
                particles.Add(particle); // добавление частицы
            }

        }

        public void Render(Graphics g) // отрисовка частиц
        {
            foreach (var particle in particles)
            {
                particle.Draw(g);
            }

            foreach (var point in impactPoints) // это по факту нам не нужно, так как нет точек гравитации
            {
                point.Render(g); // это добавили
            }
        }

    }

    public abstract class IImpactPoint // абстрактный класс, для чего нужен написал ниже
    {
        public float X; // 2 координаты по осям х и у
        public float Y;

        public abstract void ImpactParticle(Particle particle); // нужен для изменения состояния частиц, притягивание, отталкивание и т.д.

        public void Render(Graphics g) // метод для отрисовки точки
        {
            g.FillEllipse(
                    new SolidBrush(Color.Red),
                    X - 5,
                    Y - 5,
                    10,
                    10
                );
        }
    }
    
}

