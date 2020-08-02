using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Reflection;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Windows.Threading;
using System.Collections.Generic;
using System.Windows.Media.Imaging;

namespace Astromaders
{
    public partial class MainWindow : Window
    {
        //sound
        private static readonly MediaPlayer menuMusic = new MediaPlayer();
        private static readonly MediaPlayer titleSlide = new MediaPlayer();
        private static readonly MediaPlayer bullet = new MediaPlayer();
        private static readonly MediaPlayer fly = new MediaPlayer();
        private static readonly MediaPlayer gamePlayMusic = new MediaPlayer();
        private static readonly MediaPlayer dead = new MediaPlayer();
        private static readonly MediaPlayer boom = new MediaPlayer();
        private static readonly MediaPlayer boom1 = new MediaPlayer();
        private static readonly MediaPlayer boom2 = new MediaPlayer();
        private static readonly MediaPlayer boom3 = new MediaPlayer();
        private static readonly MediaPlayer boom4 = new MediaPlayer();
        private static readonly MediaPlayer boom5 = new MediaPlayer();
        private static readonly MediaPlayer boom6 = new MediaPlayer();
        private static readonly MediaPlayer boom7 = new MediaPlayer();
        private static readonly MediaPlayer boom8 = new MediaPlayer();
        private static readonly MediaPlayer boom9 = new MediaPlayer();

        private static readonly MediaPlayer[] boomSounds = new MediaPlayer[10];
        private static String[] mp3Strings = new String[11];

        //main menu data
        ImageBrush playerSkin, backgroundSkin, leftSkin, rightSkin, aircraftSkin, playButtonBackgroundSkin, helpButtonBackgroundSkin,
        exitButtonBackgroundSkin, helpBackgroundSkin;
        DispatcherTimer titleTimer, aircraftTimer;

        //gameplay data
        bool goLeft, goRight, timeToAdd, replay;
        List<Rectangle> itemstoremove, bullets;
        DispatcherTimer dispatcherTimer, bulletSoundTimer, exTimer, ex1Timer, ex2Timer, ex3Timer, ex4Timer, ex5Timer, ex6Timer, ex7Timer, ex8Timer, ex9Timer;
        public static int[] seconds;
        public static Rectangle[] enemies, explosions;
        public static DispatcherTimer[] explosionTimers;
        double enemySpeed;
        int playerSpeed, bulletIndex, rocketSpeed, bulletSpeed, enemiesSpacing, pts, lvl, s, s1, s2, s3, s4, s5, s6, s7, s8, s9;
        public static Random rand;

        //game over data
        DispatcherTimer deathTimer;
        ImageBrush gameOverSkin;
        ImageBrush deathSkin;
        int deathSeconds;

        public MainWindow()
        {
            InitializeComponent();
         
            mp3Strings[0] = "menuMusic.mp3";
            mp3Strings[1] = "titleSlide.mp3";
            mp3Strings[2] = "fly.mp3";
            mp3Strings[3] = "bullet.mp3";
            mp3Strings[4] = "soundtrack1.mp3";
            mp3Strings[5] = "soundtrack2.mp3";
            mp3Strings[6] = "soundtrack3.mp3";
            mp3Strings[7] = "soundtrack4.mp3";
            mp3Strings[8] = "soundtrack5.mp3";
            mp3Strings[9] = "dead.mp3";
            mp3Strings[10] = "boom.mp3";

            saveMusicToDisk();
            menuMusic.Open(new Uri(System.IO.Path.Combine(System.IO.Path.GetTempPath(), "menuMusic.mp3")));
            menuMusic.Play();
            titleSlide.Open(new Uri(System.IO.Path.Combine(System.IO.Path.GetTempPath(), "titleSlide.mp3")));
            bullet.Open(new Uri(System.IO.Path.Combine(System.IO.Path.GetTempPath(), "bullet.mp3")));
            fly.Open(new Uri(System.IO.Path.Combine(System.IO.Path.GetTempPath(), "fly.mp3")));
            dead.Open(new Uri(System.IO.Path.Combine(System.IO.Path.GetTempPath(), "dead.mp3")));
            boomSounds[0] = boom;
            boomSounds[1] = boom1;
            boomSounds[2] = boom2;
            boomSounds[3] = boom3;
            boomSounds[4] = boom4;
            boomSounds[5] = boom5;
            boomSounds[6] = boom6;
            boomSounds[7] = boom7;
            boomSounds[8] = boom8;
            boomSounds[9] = boom9;

            for (int i = 0; i < boomSounds.Length; i++)            {
                boomSounds[i].Open(new Uri(System.IO.Path.Combine(System.IO.Path.GetTempPath(), "boom.mp3")));
                boomSounds[i].Volume = 0.3;
            }

            //assign volumes
            titleSlide.Volume = 1;
            fly.Volume = 1;
            menuMusic.Volume = 0.7;
            bullet.Volume = 0.4;
            gamePlayMusic.Volume = 0.6;
            dead.Volume = 0.4;

            //assign images
            playerSkin = new ImageBrush();
            backgroundSkin = new ImageBrush();
            leftSkin = new ImageBrush();
            rightSkin = new ImageBrush();
            aircraftSkin = new ImageBrush();
            playButtonBackgroundSkin = new ImageBrush();
            helpButtonBackgroundSkin = new ImageBrush();
            exitButtonBackgroundSkin = new ImageBrush();
            helpBackgroundSkin = new ImageBrush();

            backgroundSkin.ImageSource = new BitmapImage(new Uri("pack://application:,,,/graphics/backgrounds/menuBackground.jpg"));
            leftSkin.ImageSource = new BitmapImage(new Uri("pack://application:,,,/graphics/extra/left.png"));
            rightSkin.ImageSource = new BitmapImage(new Uri("pack://application:,,,/graphics/extra/right.png"));
            aircraftSkin.ImageSource = new BitmapImage(new Uri("pack://application:,,,/graphics/characters/player.png"));
            playButtonBackgroundSkin.ImageSource = new BitmapImage(new Uri("pack://application:,,,/graphics/extra/green.jpg"));
            helpButtonBackgroundSkin.ImageSource = new BitmapImage(new Uri("pack://application:,,,/graphics/extra/green.jpg"));
            exitButtonBackgroundSkin.ImageSource = new BitmapImage(new Uri("pack://application:,,,/graphics/extra/green.jpg"));
            playerSkin.ImageSource = new BitmapImage(new Uri("pack://application:,,,/graphics/characters/player.png"));

            aircraft.Fill = aircraftSkin;
            background.Fill = backgroundSkin;
            left.Fill = leftSkin;
            right.Fill = rightSkin;
            background.Fill = backgroundSkin;
            player1.Fill = playerSkin;
            playButtonBackground.Fill = playButtonBackgroundSkin;
            helpButtonBackground.Fill = playButtonBackgroundSkin;
            exitButtonBackground.Fill = playButtonBackgroundSkin;

            aircraftTimer = new DispatcherTimer();
            titleTimer = new DispatcherTimer();

            titleTimer.Tick += animateTitle;
            aircraftTimer.Tick += animateTakeoff;
            aircraftTimer.Interval = TimeSpan.FromMilliseconds(20);
            titleTimer.Interval = TimeSpan.FromMilliseconds(20);

            //gamePlay stuff   
            explosionTimers = new DispatcherTimer[10];
            dispatcherTimer = new DispatcherTimer();
            bulletSoundTimer = new DispatcherTimer();
            bullets = new List<Rectangle>();
            itemstoremove = new List<Rectangle>();
            explosions = new Rectangle[10];
            enemies = new Rectangle[10];
            seconds = new int[10];
            dispatcherTimer.Interval = TimeSpan.FromMilliseconds(20);
            bulletSoundTimer.Interval = TimeSpan.FromMilliseconds(3500);
            dispatcherTimer.Tick += gameEngine;
            bulletSoundTimer.Tick += bulletSound;

            exTimer = new DispatcherTimer(); ex1Timer = new DispatcherTimer();
            ex2Timer = new DispatcherTimer(); ex3Timer = new DispatcherTimer();
            ex4Timer = new DispatcherTimer(); ex5Timer = new DispatcherTimer();
            ex6Timer = new DispatcherTimer(); ex7Timer = new DispatcherTimer();
            ex8Timer = new DispatcherTimer(); ex9Timer = new DispatcherTimer();

            exTimer.Tick += Explosion.animateExplosion; ex1Timer.Tick += Explosion.animateExplosion1;
            ex2Timer.Tick += Explosion.animateExplosion2; ex3Timer.Tick += Explosion.animateExplosion3;
            ex4Timer.Tick += Explosion.animateExplosion4; ex5Timer.Tick += Explosion.animateExplosion5;
            ex6Timer.Tick += Explosion.animateExplosion6; ex7Timer.Tick += Explosion.animateExplosion7;
            ex8Timer.Tick += Explosion.animateExplosion8; ex9Timer.Tick += Explosion.animateExplosion9;

            explosionTimers[0] = exTimer; explosionTimers[1] = ex1Timer;
            explosionTimers[2] = ex2Timer; explosionTimers[3] = ex3Timer;
            explosionTimers[4] = ex4Timer; explosionTimers[5] = ex5Timer;
            explosionTimers[6] = ex6Timer; explosionTimers[7] = ex7Timer;
            explosionTimers[8] = ex8Timer; explosionTimers[9] = ex9Timer;

            for (int i = 0; i < 10; i++) explosionTimers[i].Interval = TimeSpan.FromMilliseconds(45);

            explosions[0] = ex; explosions[1] = ex1;
            explosions[2] = ex2; explosions[3] = ex3;
            explosions[4] = ex4; explosions[5] = ex5;
            explosions[6] = ex6; explosions[7] = ex7;
            explosions[8] = ex8; explosions[9] = ex9;

            seconds[0] = s; seconds[1] = s1;
            seconds[2] = s2; seconds[3] = s3;
            seconds[4] = s4; seconds[5] = s5;
            seconds[6] = s6; seconds[7] = s7;
            seconds[8] = s8; seconds[9] = s9;

            enemiesSpacing = 95;
            playerSpeed = 12;
            rocketSpeed = 12;
            enemySpeed = 1.5;
            bulletSpeed = 5;
            lvl = 1;

            rand = new Random();
        
            //gameover stuff
            deathTimer = new DispatcherTimer();
            deathTimer.Tick += animateDeath;
            deathTimer.Interval = TimeSpan.FromMilliseconds(65);
            deathSkin = new ImageBrush();
            gameOverSkin = new ImageBrush();
            gameOverSkin.ImageSource = new BitmapImage(new Uri("pack://application:,,,/graphics/extra/gameOver.png"));
            gameOverLabel.Fill = gameOverSkin;
        }

        //save music temporarily
        public static void saveMusicToDisk()
        {
            for (int i = 0; i < mp3Strings.Length; i++)
            {
                using (FileStream fileStream = File.Create(System.IO.Path.GetTempPath() + mp3Strings[i]))
                {
                    Assembly.GetExecutingAssembly().GetManifestResourceStream("Astromaders.sound."+mp3Strings[i]).CopyTo(fileStream);
                }
            }
        }

        //play button events
        private void play(object sender, RoutedEventArgs e)
        {
            menuMusic.Stop();
            titleSlide.Play();
            titleTimer.Start();
        }

        //help button events
        private void help(object sender, RoutedEventArgs e)
        {
            hideGameContent();
            helpBackground.Visibility = Visibility.Visible;
            backButton.Visibility = Visibility.Visible;
        }

        //back button events
        private void back(object sender, RoutedEventArgs e){
            backButton.Visibility = Visibility.Hidden;
            helpBackground.Visibility = Visibility.Hidden;
        }

        //exit button event
        private void exit(object sender, RoutedEventArgs e)
        {
            menuMusic.Stop();
            this.Close();
            Environment.Exit(1);
        }

        //restart button events
        private void restart(object sender, RoutedEventArgs e)
        {
            hideGameOverContent();
            player1.Visibility = Visibility.Visible;
            initEnemies();
            addBullet();
            dispatcherTimer.Start();
        }

        //main menu button events
        private void menu(object sender, RoutedEventArgs e)
        {
            hideGameOverContent();
            hideGameContent();
            backgroundSkin.ImageSource = new BitmapImage(new Uri("pack://application:,,,/graphics/backgrounds/menuBackground.jpg"));
            background.Fill = backgroundSkin;
            showMenuContent();
            menuMusic.Play();
            Canvas.SetLeft(left, 180);
            Canvas.SetLeft(right, 530);
            Canvas.SetLeft(aircraft, 465);
            Canvas.SetTop(left, 15);
            Canvas.SetTop(right, 15);
            Canvas.SetTop(aircraft, 140);
        }

        //animate title break
        private void animateTitle(object sender, EventArgs e)
        {
            Canvas.SetLeft(left, Canvas.GetLeft(left) - 2);//move "astro" to the left
            Canvas.SetLeft(right, Canvas.GetLeft(right) + 2);//move "maders" to the right
            if (Canvas.GetLeft(left) < 60)
            {
                titleTimer.Stop();
                aircraftTimer.Start();
                fly.Play();
                titleSlide.Stop();
            }
        }

        //animate spaceship menu takeoff
        private void animateTakeoff(object sender, EventArgs e)
        {
            if (Canvas.GetTop(aircraft) < -65)
            {
                aircraftTimer.Stop();
                hideMenuContent();
                showGameContent();
                Canvas.SetLeft(player1, 480);
                Canvas.SetTop(player1, 510);
                dispatcherTimer.Start();
                bullet.Play();
                bulletSoundTimer.Start();
                initEnemies();
                addBullet();
                //randomize gameplay music
                int r = rand.Next(5);
                switch (r)
                {
                    case 0:
                        gamePlayMusic.Open(new Uri(System.IO.Path.Combine(System.IO.Path.GetTempPath(), "soundtrack1.mp3")));
                        break;
                    case 1:
                        gamePlayMusic.Open(new Uri(System.IO.Path.Combine(System.IO.Path.GetTempPath(), "soundtrack2.mp3")));
                        break;
                    case 2:
                        gamePlayMusic.Open(new Uri(System.IO.Path.Combine(System.IO.Path.GetTempPath(), "soundtrack3.mp3")));
                        break;
                    case 3:
                        gamePlayMusic.Open(new Uri(System.IO.Path.Combine(System.IO.Path.GetTempPath(), "soundtrack4.mp3")));
                        break;
                    case 4:
                        gamePlayMusic.Open(new Uri(System.IO.Path.Combine(System.IO.Path.GetTempPath(), "soundtrack5.mp3")));
                        break;
                }
                gamePlayMusic.Play();
            }
            Canvas.SetTop(aircraft, Canvas.GetTop(aircraft) - 6);
        }

        //death explosion animation
        private void animateDeath(object send, EventArgs e)
        {
            deathSeconds++;
            switch (deathSeconds)
            {
                case 1:
                    deathSkin.ImageSource = new BitmapImage(new Uri("pack://application:,,,/graphics/explosions/playerExplosion/1.png"));
                    break;
                case 2:
                    deathSkin.ImageSource = new BitmapImage(new Uri("pack://application:,,,/graphics/explosions/playerExplosion/2.png"));
                    break;
                case 3:
                    deathSkin.ImageSource = new BitmapImage(new Uri("pack://application:,,,/graphics/explosions/playerExplosion/3.png"));
                    break;
                case 4:
                    deathSkin.ImageSource = new BitmapImage(new Uri("pack://application:,,,/graphics/explosions/playerExplosion/4.png"));
                    break;
                case 5:
                    deathSkin.ImageSource = new BitmapImage(new Uri("pack://application:,,,/graphics/explosions/playerExplosion/5.png"));
                    break;
                case 6:
                    deathSkin.ImageSource = new BitmapImage(new Uri("pack://application:,,,/graphics/explosions/playerExplosion/6.png"));
                    break;
                case 7:
                    deathSkin.ImageSource = new BitmapImage(new Uri("pack://application:,,,/graphics/explosions/playerExplosion/7.png"));
                    break;
                case 8:
                    deathSkin.ImageSource = new BitmapImage(new Uri("pack://application:,,,/graphics/explosions/playerExplosion/8.png"));
                    break;
                case 9:
                    deathSkin.ImageSource = new BitmapImage(new Uri("pack://application:,,,/graphics/explosions/playerExplosion/9.png"));
                    break;
                case 10:
                    deathSkin.ImageSource = new BitmapImage(new Uri("pack://application:,,,/graphics/explosions/playerExplosion/10.png"));
                    break;
                case 11:
                    deathSkin.ImageSource = new BitmapImage(new Uri("pack://application:,,,/graphics/explosions/playerExplosion/11.png"));
                    break;
                case 12:
                    deathSkin.ImageSource = new BitmapImage(new Uri("pack://application:,,,/graphics/explosions/playerExplosion/12.png"));
                    break;
                case 13:
                    deathSkin.ImageSource = new BitmapImage(new Uri("pack://application:,,,/graphics/explosions/playerExplosion/13.png"));
                    break;
                case 14:
                    deathSkin.ImageSource = new BitmapImage(new Uri("pack://application:,,,/graphics/explosions/playerExplosion/14.png"));
                    break;
                case 15:
                    deathSkin.ImageSource = new BitmapImage(new Uri("pack://application:,,,/graphics/explosions/playerExplosion/15.png"));
                    break;
                case 16:
                    deathSkin.ImageSource = new BitmapImage(new Uri("pack://application:,,,/graphics/explosions/playerExplosion/16.png"));
                    break;
                case 17:
                    deathSkin.ImageSource = new BitmapImage(new Uri("pack://application:,,,/graphics/explosions/playerExplosion/17.png"));
                    break;
                case 18:
                    deathSkin.ImageSource = new BitmapImage(new Uri("pack://application:,,,/graphics/explosions/playerExplosion/18.png"));
                    break;
                case 19:
                    deathSkin.ImageSource = new BitmapImage(new Uri("pack://application:,,,/graphics/explosions/playerExplosion/19.png"));
                    break;
                case 20:
                    deathSkin.ImageSource = new BitmapImage(new Uri("pack://application:,,,/graphics/explosions/playerExplosion/20.png"));
                    break;
                case 21:
                    deathSkin.ImageSource = new BitmapImage(new Uri("pack://application:,,,/graphics/explosions/playerExplosion/21.png"));
                    break;
                case 22:
                    deathSkin.ImageSource = new BitmapImage(new Uri("pack://application:,,,/graphics/explosions/playerExplosion/22.png"));
                    break;
                case 23:
                    deathSkin.ImageSource = new BitmapImage(new Uri("pack://application:,,,/graphics/explosions/playerExplosion/23.png"));
                    break;
                case 24:
                    deathSkin.ImageSource = new BitmapImage(new Uri("pack://application:,,,/graphics/explosions/playerExplosion/24.png"));
                    break;
                case 25:
                    deathSkin.ImageSource = new BitmapImage(new Uri("pack://application:,,,/graphics/explosions/playerExplosion/25.png"));
                    break;
                case 26:
                    deathSkin.ImageSource = new BitmapImage(new Uri("pack://application:,,,/graphics/explosions/playerExplosion/26.png"));
                    death.Visibility = Visibility.Hidden;
                    deathTimer.Stop();
                    deathSeconds = 0;
                    break;
            }
            death.Fill = deathSkin;
        }

        //play enemy bullet sound
        private void bulletSound(object send, EventArgs e)
        {
            int span = rand.Next(3500, 6500);
            bullet.Stop();
            bullet.Play();
            bulletSoundTimer.Interval = TimeSpan.FromMilliseconds(span);
        }

        //when key pressed
        private void Canvas_KeyisDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left) goLeft = true;
            if (e.Key == Key.Right) goRight = true;
        }

        //when key released
        private void Canvas_KeyIsUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left) goLeft = false;
            if (e.Key == Key.Right) goRight = false;

            if (e.Key == Key.Space && dispatcherTimer.IsEnabled)
            {
                itemstoremove.Clear();
                ImageBrush rocket = new ImageBrush();
                rocket.ImageSource = new BitmapImage(new Uri("pack://application:,,,/graphics/weapons/rocket.png"));

                Rectangle newRocket = new Rectangle
                {
                    Tag = "rocket",
                    Height = 72,
                    Width = 12,
                    Fill = rocket,
                };

                Canvas.SetTop(newRocket, Canvas.GetTop(player1) - newRocket.Height);
                Canvas.SetLeft(newRocket, Canvas.GetLeft(player1) + player1.Width / 2 - 5);
                myCanvas.Children.Add(newRocket);
            }
        }
        //generate enemy bullets
        private void addBullet()
        {
            int random = rand.Next(10);
            double x = 0;
            double y = 0;

            ImageBrush bulletSkin = new ImageBrush();
            Randomize.randomizeBulletSkin(bulletSkin);

            Rectangle newBullet = new Rectangle
            {
                Tag = "bullet",
                Height = 28,
                Width = 12,
                MinWidth = bulletIndex,
            };

            foreach (var r in myCanvas.Children.OfType<Rectangle>())
            {
                if (r.Tag == "enemy" && r.MinWidth == random)
                {
                    x = Canvas.GetLeft(r);
                    y = Canvas.GetTop(r);
                    break;
                }
            }
            newBullet.Fill = bulletSkin;
            bullets.Add(newBullet);
            Canvas.SetTop(newBullet, y + 10);
            Canvas.SetLeft(newBullet, x + 21);
            myCanvas.Children.Add(newBullet);
            bulletIndex++;
        }

        //initialize 10 enemies
        private void initEnemies()
        {
            dead.Stop();
            for (int i = 0; i < enemies.Length; i++)
            {
                ImageBrush enemySkin = new ImageBrush();
                Rectangle newEnemy = new Rectangle
                {
                    Tag = "enemy",
                    Height = 55,
                    Width = 55,
                    MinWidth = i,
                };

                enemies[i] = newEnemy;
                int x = rand.Next(100);
                int m = rand.Next(5);

                Canvas.SetTop(enemies[i], x);
                Canvas.SetLeft(enemies[i], ((i + 1) * enemiesSpacing));
                myCanvas.Children.Add(enemies[i]);

                switch (m)
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
                enemies[i].Fill = enemySkin;
            }
        }

        //make enemy reappear at the top once it goes out of bounds or gets destroyed
        private void regenerateEnemies(int index)
        {
            int x = rand.Next(100);
            Canvas.SetTop(enemies[index], -200 + x);
            Canvas.SetLeft(enemies[index], ((index + 1) * enemiesSpacing));
            Randomize.randomizeEnemySkin(index);
        }

        //make enemy bullet reappear at the top in a random lane
        public void regenerateBullet(Rectangle rec)
        {
            int random = rand.Next(10);
            int index = (int)rec.MinWidth;
            double x = 0;
            double y = 0;

            ImageBrush bulletSkin = new ImageBrush();
            Randomize.randomizeBulletSkin(bulletSkin);

            rec.Fill = bulletSkin;
            foreach (var r in myCanvas.Children.OfType<Rectangle>())
            {
                if (r.Tag == "enemy" && r.MinWidth == random)
                {
                    x = Canvas.GetLeft(r);
                    y = Canvas.GetTop(r);
                }
            }
            Canvas.SetTop(rec, y + 10);
            Canvas.SetLeft(rec, x + 21);
        }

        //destroy objects once game is done
        private void clearTheScreen()
        {
            foreach (var rec in myCanvas.Children.OfType<Rectangle>())
            {
                if (rec is Rectangle && ((string)rec.Tag == "bullet" || (string)rec.Tag == "enemy" || (string)rec.Tag == "rocket")) itemstoremove.Add(rec);
            }
        }

        //game loop, invoked every 20 miliseconds
        private void gameEngine(object sender, EventArgs e)
        {
            if (replay)
            {
                bulletSoundTimer.Start();
                enemySpeed = 1.5;
                bulletIndex = 0;
                rocketSpeed = 12;
                bulletSpeed = 5;
                pts = 0;
                lvl = 1;
                timeToAdd = false;
                points.Content = pts.ToString();
                levelPts.Content = lvl.ToString();
                Randomize.randomizeBackground(backgroundSkin);
                replay = false;
            }

            //check if enemies are out of bounds, if so, respawn them.
            for (int i = 0; i < enemies.Length; i++)
            {
                if (Canvas.GetTop(enemies[i]) > background.Height) regenerateEnemies(i);
                if (Canvas.GetTop(enemies[i]) > 20) boomSounds[i].Stop();
            }

            //increase difficulty
            if (timeToAdd)
            {
                addBullet();
                timeToAdd = false;
                enemySpeed++;
                bulletSpeed++;
            }

            //make player bounding box      
            Rect player = new Rect(Canvas.GetLeft(player1), Canvas.GetTop(player1), player1.Width, player1.Height);

            //check if player(aircraft) is supposed to move
            if (goLeft && Canvas.GetLeft(player1) > 0) Canvas.SetLeft(player1, Canvas.GetLeft(player1) - 10);
            else if (goRight && Canvas.GetLeft(player1) + 80 < 1035.67) Canvas.SetLeft(player1, Canvas.GetLeft(player1) + 10);

            foreach (var x in myCanvas.Children.OfType<Rectangle>())
            {
                // if any rectangle has the "bullet" tag
                if (x is Rectangle && (string)x.Tag == "rocket")
                {
                    Canvas.SetTop(x, Canvas.GetTop(x) - rocketSpeed);       // move the rocket towards top of the screen
                    Rect rocket = new Rect(Canvas.GetLeft(x), Canvas.GetTop(x), x.Width, x.Height);// make a rocket bounding box
                    if (Canvas.GetTop(x) < 10) itemstoremove.Add(x);        // check if rocket is almost out of bounds

                    // run another for each loop inside the game loop
                    foreach (var y in myCanvas.Children.OfType<Rectangle>())
                    {
                        if (y is Rectangle && (string)y.Tag == "enemy")//if y is a rectangle with an "enemy" tag
                        {
                            Rect enemy = new Rect(Canvas.GetLeft(y), Canvas.GetTop(y), y.Width, y.Height);//make an enemy bounding box

                            if (rocket.IntersectsWith(enemy))//now check if rocket and enemy are colliding
                            {
                                pts++;                           //points +1
                                points.Content = pts.ToString();
                                if (pts != 0 && pts % 30 == 0)
                                {  //increase difficulty every 30 points and add another bullet                             
                                    lvl++;
                                    timeToAdd = true;
                                }
                                levelPts.Content = lvl.ToString(); //update points count
                                int i = (int)y.MinWidth;
                                boomSounds[i].Play(); //play                            
                                Canvas.SetTop(explosions[i], Canvas.GetTop(y));
                                Canvas.SetLeft(explosions[i], Canvas.GetLeft(y) - 10);
                                explosionTimers[i].Start();
                                regenerateEnemies(i); //respwan the destroyed enemy                            
                                itemstoremove.Add(x);//destroy the rocket as soon as it hits an enemy
                            }
                        }
                    }
                }
                //check again if the any rectangle has the "enemy" tag
                if (x is Rectangle && (string)x.Tag == "enemy")
                {
                    Canvas.SetTop(x, Canvas.GetTop(x) + enemySpeed);//enemy falling speed
                    Rect enemy = new Rect(Canvas.GetLeft(x), Canvas.GetTop(x), x.Width, x.Height);// make another enemy bounding box
                    if (player.IntersectsWith(enemy)) showgameOverContent(); //check for collision between player and enemy, if found, game is over   
                }

                // check if any rectangle has the "enemyBullet"
                if (x is Rectangle && (string)x.Tag == "bullet")
                {
                    Canvas.SetTop(x, Canvas.GetTop(x) + bulletSpeed);//if so drop it by bulletSpeed
                    if (Canvas.GetTop(x) > 600) regenerateBullet(x);//check if bullet is out of bounds, if so put it back at the top
                    Rect enemyBullets = new Rect(Canvas.GetLeft(x), Canvas.GetTop(x), x.Width, x.Height);//make an enemy bullet bounding box
                    if (enemyBullets.IntersectsWith(player)) showgameOverContent(); //if any bullet hits the player, game is over           
                }
            }
            //destroy all garbage
            foreach (Rectangle y in itemstoremove)
            {
                myCanvas.Children.Remove(y);
            }
        }

        //game over events
        private void showgameOverContent()
        {
            fly.Stop();
            dead.Play();
            clearTheScreen();
            dispatcherTimer.Stop();
            restartButton.Visibility = Visibility.Visible;
            menuButton.Visibility = Visibility.Visible;
            gameOverLabel.Visibility = Visibility.Visible;
            bulletSoundTimer.Stop();
            Canvas.SetLeft(death, Canvas.GetLeft(player1));
            Canvas.SetTop(death, Canvas.GetTop(player1) - 50);
            death.Visibility = Visibility.Visible;
            deathTimer.Start();
            player1.Visibility = Visibility.Hidden;
            replay = true;
        }

        //show and hide content...
        private void hideGameOverContent()
        {
            if (deathTimer.IsEnabled)
            {
                deathTimer.Stop();
                deathSeconds = 0;
            }
            death.Visibility = Visibility.Hidden;
            restartButton.Visibility = Visibility.Hidden;
            menuButton.Visibility = Visibility.Hidden;
            gameOverLabel.Visibility = Visibility.Hidden;
        }

        private void hideMenuContent()
        {
            left.Visibility = Visibility.Hidden;
            right.Visibility = Visibility.Hidden;
            aircraft.Visibility = Visibility.Hidden;
            podium.Visibility = Visibility.Hidden;
            playButton.Visibility = Visibility.Hidden;
            helpButton.Visibility = Visibility.Hidden;
            exitButton.Visibility = Visibility.Hidden;
        }

        private void showGameContent()
        {
            Randomize.randomizeBackground(backgroundSkin);
            background.Fill = backgroundSkin;
            player1.Visibility = Visibility.Visible;
            score.Visibility = Visibility.Visible;
            points.Visibility = Visibility.Visible;
            level.Visibility = Visibility.Visible;
            levelPts.Visibility = Visibility.Visible;
        }

        private void showMenuContent()
        {
            left.Visibility = Visibility.Visible;
            right.Visibility = Visibility.Visible;
            aircraft.Visibility = Visibility.Visible;
            podium.Visibility = Visibility.Visible;
            playButton.Visibility = Visibility.Visible;
            helpButton.Visibility = Visibility.Visible;
            exitButton.Visibility = Visibility.Visible;
        }

        private void hideGameContent()
        {
            bulletSoundTimer.Stop();
            gamePlayMusic.Stop();
            player1.Visibility = Visibility.Hidden;
            score.Visibility = Visibility.Hidden;
            points.Visibility = Visibility.Hidden;
            level.Visibility = Visibility.Hidden;
            levelPts.Visibility = Visibility.Hidden;
        }
    }
}