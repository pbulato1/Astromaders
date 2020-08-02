using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Astromaders
{
    //draw explosion animation
    class Explosion
    {
        public static void animateExplosion(object sender, EventArgs e)
        {
            int i = 0;
            ImageBrush image = new ImageBrush();
            MainWindow.seconds[0]++;
            animate(i, image);
            MainWindow.explosions[i].Fill = image;
        }

        public static void animateExplosion1(object sender, EventArgs e)
        {
            int i = 1;
            ImageBrush image = new ImageBrush();
            MainWindow.seconds[i]++;
            animate(i, image);
            MainWindow.explosions[i].Fill = image;
        }

        public static void animateExplosion2(object sender, EventArgs e)
        {
            int i = 2;
            ImageBrush image = new ImageBrush();
            MainWindow.seconds[i]++;
            animate(i, image);
            MainWindow.explosions[i].Fill = image;
        }

        public static void animateExplosion3(object sender, EventArgs e)
        {
            int i = 3;
            ImageBrush image = new ImageBrush();
            MainWindow.seconds[i]++;
            animate(i, image);
            MainWindow.explosions[i].Fill = image;
        }
        public static void animateExplosion4(object sender, EventArgs e)
        {
            int i = 4;
            ImageBrush image = new ImageBrush();
            MainWindow.seconds[i]++;
            animate(i, image);
            MainWindow.explosions[i].Fill = image;
        }
        public static void animateExplosion5(object sender, EventArgs e)
        {
            int i = 5;
            ImageBrush image = new ImageBrush();
            MainWindow.seconds[i]++;
            animate(i, image);
            MainWindow.explosions[i].Fill = image;
        }
        public static void animateExplosion6(object sender, EventArgs e)
        {
            int i = 6;
            ImageBrush image = new ImageBrush();
            MainWindow.seconds[i]++;
            animate(i, image);
            MainWindow.explosions[i].Fill = image;
        }
        public static void animateExplosion7(object sender, EventArgs e)
        {
            int i = 7;
            ImageBrush image = new ImageBrush();
            MainWindow.seconds[i]++;
            animate(i, image);
            MainWindow.explosions[i].Fill = image;
        }
        public static void animateExplosion8(object sender, EventArgs e)
        {
            int i = 8;
            ImageBrush image = new ImageBrush();
            MainWindow.seconds[i]++;
            animate(i, image);
            MainWindow.explosions[i].Fill = image;
        }
        public static void animateExplosion9(object sender, EventArgs e)
        {
            int i = 9;
            ImageBrush image = new ImageBrush();
            MainWindow.seconds[i]++;
            animate(i, image);
            MainWindow.explosions[i].Fill = image;
        }

        private static void animate(int i, ImageBrush image)
        {
            switch (MainWindow.seconds[i])
            {
                case 1:
                    image.ImageSource = new BitmapImage(new Uri("pack://application:,,,/graphics/explosions/enemyExplosion/0.png"));
                    break;
                case 2:
                    image.ImageSource = new BitmapImage(new Uri("pack://application:,,,/graphics/explosions/enemyExplosion/1.png"));
                    break;
                case 3:
                    image.ImageSource = new BitmapImage(new Uri("pack://application:,,,/graphics/explosions/enemyExplosion/2.png"));
                    break;
                case 4:
                    image.ImageSource = new BitmapImage(new Uri("pack://application:,,,/graphics/explosions/enemyExplosion/3.png"));
                    break;
                case 5:
                    image.ImageSource = new BitmapImage(new Uri("pack://application:,,,/graphics/explosions/enemyExplosion/4.png"));
                    break;
                case 6:
                    image.ImageSource = new BitmapImage(new Uri("pack://application:,,,/graphics/explosions/enemyExplosion/5.png"));
                    break;
                case 7:
                    image.ImageSource = new BitmapImage(new Uri("pack://application:,,,/graphics/explosions/enemyExplosion/6.png"));
                    break;
                case 8:
                    image.ImageSource = new BitmapImage(new Uri("pack://application:,,,/graphics/explosions/enemyExplosion/7.png"));
                    break;
                case 9:
                    image.ImageSource = new BitmapImage(new Uri("pack://application:,,,/graphics/explosions/enemyExplosion/8.png"));
                    break;
                case 10:
                    image.ImageSource = new BitmapImage(new Uri("pack://application:,,,/graphics/explosions/enemyExplosion/9.png"));
                    break;
                case 11:
                    image.ImageSource = new BitmapImage(new Uri("pack://application:,,,/graphics/explosions/enemyExplosion/10.png"));
                    break;
                case 12:
                    image.ImageSource = new BitmapImage(new Uri("pack://application:,,,/graphics/explosions/enemyExplosion/11.png"));
                    break;
                case 13:
                    image.ImageSource = new BitmapImage(new Uri("pack://application:,,,/graphics/explosions/enemyExplosion/12.png"));
                    break;
                case 14:
                    image.ImageSource = new BitmapImage(new Uri("pack://application:,,,/graphics/explosions/enemyExplosion/13.png"));
                    break;
                case 15:
                    image.ImageSource = new BitmapImage(new Uri("pack://application:,,,/graphics/explosions/enemyExplosion/14.png"));
                    break;
                case 16:
                    image.ImageSource = new BitmapImage(new Uri("pack://application:,,,/graphics/explosions/enemyExplosion/15.png"));
                    break;
                case 17:
                    image.ImageSource = new BitmapImage(new Uri("pack://application:,,,/graphics/explosions/enemyExplosion/16.png"));
                    MainWindow.explosionTimers[i].Stop();
                    MainWindow.seconds[i] = 0;
                    break;
            }
            MainWindow.explosions[i].Fill = image;
        }
    }
}
