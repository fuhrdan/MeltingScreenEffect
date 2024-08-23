//*****************************************************************************
//** Melting Screen Effect for Windows.                                      **
//** Simple program to make the screen melt.  Kind of Works, but it was an   **
//** Exercise in windows forms.                                              **
//*****************************************************************************


using System;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Imaging;

namespace MeltingScreenEffect
{
    public class MainForm : Form
    {
        private Bitmap screenCapture;
        private System.Windows.Forms.Timer timer; // Explicit reference to WinForms Timer
        private int currentLine;

        public MainForm()
        {
            // No need for InitializeComponent since we aren't using the designer

            this.BackColor = Color.Black;
            this.WindowState = FormWindowState.Maximized;

            // Capture the screen
            screenCapture = CaptureScreen();

            // Set up the PictureBox
            PictureBox pictureBox = new PictureBox
            {
                Image = screenCapture,
                Dock = DockStyle.Fill,
                SizeMode = PictureBoxSizeMode.StretchImage
            };
            this.Controls.Add(pictureBox);

            // Set up the Timer
            timer = new System.Windows.Forms.Timer(); // Explicit reference to WinForms Timer
            timer.Interval = 1; // Adjust for speed
            timer.Tick += OnTick;
            currentLine = screenCapture.Height;

            timer.Start();
        }

        private void OnTick(object sender, EventArgs e)
        {
            using (Graphics g = Graphics.FromImage(screenCapture))
            {
                // For each tick, shrink a line of the image
                if (currentLine > 0)
                {
                    int lineHeight = (currentLine == screenCapture.Height) ? 1 : currentLine / screenCapture.Height;
                    Rectangle srcRect = new Rectangle(0, screenCapture.Height - currentLine, screenCapture.Width, 1);
                    Rectangle destRect = new Rectangle(0, screenCapture.Height - currentLine, screenCapture.Width, lineHeight);

                    // Clear the previous section with black
                    g.FillRectangle(Brushes.Black, destRect);

                    // Draw the shrinking line
                    g.DrawImage(screenCapture, destRect, srcRect, GraphicsUnit.Pixel);
                    currentLine--;
                }
                else
                {
                    timer.Stop();
                }
                this.Invalidate();
            }
        }

        private Bitmap CaptureScreen()
        {
            Rectangle bounds = Screen.PrimaryScreen.Bounds;
            Bitmap bitmap = new Bitmap(bounds.Width, bounds.Height);

            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.CopyFromScreen(Point.Empty, Point.Empty, bounds.Size);
            }

            return bitmap;
        }

        [STAThread]
        public static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
