using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp9
{
    public class Particle
    {
        public int Radius; // радиус частицы
        public float X; // положение частицы по оси х
        public float Y; // положение частицы по оси у

        public float SpeedX; // скорость перемещения по оси X
        public float SpeedY; // скорость перемещения по оси Y

        public static Random rand = new Random(); // определение генератора

        public float Life; // запас здоровья частицы

        public virtual void Draw(Graphics g)
        {
            float k = Math.Min(1f, Life / 100);

            int alpha = (int)(k * 255); // рассчитываем альфа канал от 0 до 255, нужен чтобы задать прозрачность

            
            var color = Color.FromArgb(alpha, Color.Black); // создаем цвет из уже существующего, но привязываем к нему еще и значение альфа канала
            var b = new SolidBrush(color);


            g.FillEllipse(b, X - Radius, Y - Radius, Radius * 2, Radius * 2);

            b.Dispose();
        }
    }

    public class ParticleColorful : Particle
    {

        public Color FromColor; // начальный цвет
        public Color ToColor; // конечный цвет в который перетекает

        public static Color MixColor(Color color1, Color color2, float k) // метод для смешивания цветов
        {
            return Color.FromArgb(
                (int)(color2.A * k + color1.A * (1 - k)), // смешивание по типу argb
                (int)(color2.R * k + color1.R * (1 - k)),
                (int)(color2.G * k + color1.G * (1 - k)),
                (int)(color2.B * k + color1.B * (1 - k))
            );
        }

        public override void Draw(Graphics g) // отрисовка частиц
        {
            float k = Math.Min(1f, Life / 100);


            var color = MixColor(ToColor, FromColor, k); // обратный порядок цветов, так как f уменьшается
            var b = new SolidBrush(color);

            g.FillEllipse(b, X - Radius, Y - Radius, Radius * 2, Radius * 2);

            b.Dispose();
        }
    }
}
