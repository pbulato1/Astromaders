using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Astromaders
{
    class Randomize
    {
        //randomize enemy type
        public static void randomizeEnemySkin(int i)
        {
            ImageBrush enemySkin = new ImageBrush();
            int r = MainWindow.rand.Next(5);
            switch (r)
            {
                case 0:
                    enemySkin.ImageSource = new BitmapImage(new Uri("pack://application:,,,/graphics/characters/E1.png"));
                    break;
                case 1:
                    enemySkin.ImageSource = new BitmapImage(new Uri("pack://application:,,,/graphics/characters/E2.png"));
                    break;
                case 2:
                    enemySkin.ImageSource = new BitmapImage(new Uri("pack://application:,,,/graphics/characters/E3.png"));
                    break;
                case 3:
                    enemySkin.ImageSource = new BitmapImage(new Uri("pack://application:,,,/graphics/characters/E4.png"));
                    break;
                case 4:
                    enemySkin.ImageSource = new BitmapImage(new Uri("pack://application:,,,/graphics/characters/E5.png"));
                    break;
            }
            MainWindow.enemies[i].Fill = enemySkin;
        }

        //randomize bullet type
        public static void randomizeBulletSkin(ImageBrush bulletSkin)
        {
            int r = MainWindow.rand.Next(4);
            switch (r)
            {
                case 0:
                    bulletSkin.ImageSource = new BitmapImage(new Uri("pack://application:,,,/graphics/weapons/yellowBullet.png"));
                    break;
                case 1:
                    bulletSkin.ImageSource = new BitmapImage(new Uri("pack://application:,,,/graphics/weapons/greenBullet.png"));
                    break;
                case 2:
                    bulletSkin.ImageSource = new BitmapImage(new Uri("pack://application:,,,/graphics/weapons/bluebullet.png"));
                    break;
                case 3:
                    bulletSkin.ImageSource = new BitmapImage(new Uri("pack://application:,,,/graphics/weapons/pinkBullet.png"));
                    break;
            }
        }

        //randomize gameplay background image
        public static void randomizeBackground(ImageBrush backgroundSkin)
        {
            int r = MainWindow.rand.Next(4);
            switch (r)
            {
                case 0:
                    backgroundSkin.ImageSource = new BitmapImage(new Uri("pack://application:,,,/graphics/backgrounds/background1.jpg"));
                    break;
                case 1:
                    backgroundSkin.ImageSource = new BitmapImage(new Uri("pack://application:,,,/graphics/backgrounds/background2.jpg"));
                    break;
                case 2:
                    backgroundSkin.ImageSource = new BitmapImage(new Uri("pack://application:,,,/graphics/backgrounds/background3.jpg"));
                    break;
                case 3:
                    backgroundSkin.ImageSource = new BitmapImage(new Uri("pack://application:,,,/graphics/backgrounds/background4.jpg"));
                    break;
            }
        }
    }
}