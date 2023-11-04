using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace CarGame
{
    public partial class Form1 : Form
    {
        int roadSpeed = 15;
        int trafficSpeed = 18;
        int playerSpeed = 15;
        int score = 0;
        int carImage;

        Bitmap[] images = {
            Properties.Resources.car1,
            Properties.Resources.car2,
            Properties.Resources.car3,
            Properties.Resources.car4,
            Properties.Resources.car5,
            Properties.Resources.car6,
            Properties.Resources.car7,
            Properties.Resources.car8,
        };

        Random rand = new Random();
        Random carPosition = new Random();

        bool goLeft, goRight, goUp;

        public Form1()
        {
            InitializeComponent();
            resetGame();
        }
        
        private void keyIsDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Left)
            {
                goLeft = true;
            }
            if(e.KeyCode == Keys.Right)
            {
                goRight = true;
            }
            if (e.KeyCode == Keys.Up)
            {
                goUp = true;
            }
            
        }

        private void keyIsUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                goLeft = false;
            }
            if (e.KeyCode == Keys.Right)
            {
                goRight = false;
            }
            if (e.KeyCode == Keys.Up)
            {
                goUp = false;
            }
        }

        private void gameTimerEvent(object sender, EventArgs e)

        {
            incrementScore();
            playerMovesHandler();
            roadParallaxMovement();
            enemieMovesHandler();
            checkCollision();
            winAward();
        }

        private void changeEnemyCars(PictureBox tempCar)
        {
            carImage = rand.Next(1, 8);
            tempCar.Image = images[carImage];

            tempCar.Top = carPosition.Next(100, 400) * -1;

            if((string)tempCar.Tag == "carLeft")
            {
                Enemy1.Left = carPosition.Next(12, 234);
            }
            if ((string)tempCar.Tag == "carRight")
            {
                Enemy2.Left = carPosition.Next(306, 360);
            }
        }

        private void gameOver()
        {
            playSound();
            gameTimer.Stop();
            explosion.Visible = true;
            player.Controls.Add(explosion);
            explosion.Location = new Point(-8, 5);
            explosion.BackColor = Color.Transparent;

            award.Visible = true;
            award.BringToFront();
            startButton.Enabled = true;
        }

        private void resetGame()
        {
            startButton.Enabled = false;
            explosion.Visible = false;
            award.Visible= false;
            goLeft = false; 
            goRight = false ;
            score = 0;

            Image originalImage = Properties.Resources.lose1;
            int newWidth = 250;
            int newHeight = 100;
            Bitmap resizedImage = new Bitmap(originalImage, newWidth, newHeight);

            award.Image = resizedImage;

            Enemy1.Top = carPosition.Next(20, 450) * -1;
            Enemy1.Left = carPosition.Next(12, 234);

            Enemy2.Top = carPosition.Next(200, 450) * -1;
            Enemy2.Left = carPosition.Next(306, 360);

            gameTimer.Start();
        }
        
        private void restartGame(object sender, EventArgs e)
        {
            resetGame();
        }

        private void playSound()
        {
            System.Media.SoundPlayer playCrash = new System.Media.SoundPlayer(Properties.Resources.hit);
            playCrash.Play();
        }

        private void winAward()
        {
            if (score > 100 && score < 500)
            {
                award.Image = Properties.Resources.bronze;
                roadSpeed = 15;
                trafficSpeed = 20;
            }
            else if (score > 500 && score < 1000)
            {
                award.Image = Properties.Resources.silver;
                roadSpeed = 23;
                trafficSpeed = 26;
            }
            else if (score > 1000)
            {
                award.Image = Properties.Resources.gold;
                roadSpeed = 27;
                trafficSpeed = 32;
            }
        }

        private void enemieMovesHandler()
        {
            Enemy1.Top += trafficSpeed;
            Enemy2.Top += trafficSpeed;

            if (Enemy1.Top > 520)
            {
                changeEnemyCars(Enemy1);
            }
            if (Enemy2.Top > 520)
            {
                changeEnemyCars(Enemy2);
            }
        }
        private void roadParallaxMovement()
        {
            roadTrack1.Top += roadSpeed;
            roadTruck2.Top += roadSpeed;

            if (roadTruck2.Top > 425)
            {
                roadTruck2.Top = -425;
            }
            if (roadTrack1.Top > 425)
            {
                roadTrack1.Top = -425;
            }
        }

        private void playerMovesHandler()
        {
            if (goLeft == true && player.Left > 5)
            {
                player.Left -= playerSpeed;
            }
            if (goRight == true && player.Left < 350)
            {
                player.Left += playerSpeed;
            }

            if (goUp == true)
            {
                gameTimer.Interval = 15;
            }
            else
            {
                gameTimer.Interval = 20;
            }

        }

        private void checkCollision()
        {
            if (player.Bounds.IntersectsWith(Enemy1.Bounds) || player.Bounds.IntersectsWith(Enemy2.Bounds))
            {
                gameOver();
            }
        }

        private void incrementScore()
        {
            txtScore.Text = $"Score: {score}";
            score++;
        }
    }
}