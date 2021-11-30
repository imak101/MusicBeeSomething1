﻿using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.Diagnostics.CodeAnalysis;

namespace MusicBeePlugin.Drawing
{
    public class PaintManager
    {
        private PaintEventArgs _eventArgs;
        private readonly Plugin.MusicBeeApiInterface _mbAPI;
        private readonly PluginSettings _settings;

        private static readonly Size _pfpSize = new Size(60,60);

        private Color _bgColor;
        private Color _fgColor;

        private string _pfpPath;
        private string _username;
        private string _oldPfpPath;

        private Point _usernamePoint; //= new Point(105,83);     < -- Fallback values
        private Point _pfpPoint; //= new Point(100, 20);

        private Image _pfp;

        private Control _controlMain;

        private PictureBox _picBox;

        private bool _drawRounded;
        
        public PaintManager(ref Plugin.MusicBeeApiInterface mbAPI, ref PluginSettings settings)
        {
            _mbAPI = mbAPI;
            _settings = settings;

            SetColorsFromSkin();
            LoadSettings();
        }

        public void SetArgs(ref PaintEventArgs args)
        {
            _eventArgs = args;
        }

        private void SetColorsFromSkin()
        {
            _bgColor = Color.FromArgb(_mbAPI.Setting_GetSkinElementColour.Invoke(Plugin.SkinElement.SkinSubPanel,Plugin.ElementState.ElementStateDefault, Plugin.ElementComponent.ComponentBackground)); 
            _fgColor = Color.FromArgb(_mbAPI.Setting_GetSkinElementColour.Invoke(Plugin.SkinElement.SkinSubPanel,Plugin.ElementState.ElementStateDefault,Plugin.ElementComponent.ComponentForeground));
        }

        private void LoadSettings()
        {
            _pfpPath = _settings.GetFromKey("pfpPath");
            _username = _settings.GetFromKey("username");

            try
            {
                _drawRounded = Convert.ToBoolean(_settings.GetFromKey("roundPfpCheck"));
            }
            catch (FormatException)
            {
                _settings.SetFromKey("roundPfpCheck", false.ToString(), true); //TODO: safety key
                _drawRounded = false;
            }
        }

        public void MainPainter()
        {
            _oldPfpPath = _pfpPath;
            LoadSettings();

            _eventArgs.Graphics.Clear(_bgColor);

            CalculateCenter_Point();
            
            TextRenderer.DrawText(_eventArgs.Graphics, _username, SystemFonts.CaptionFont, _usernamePoint, _fgColor);
            
            // timer.Stop();
            // timerFrames = new GifHandler(_pfpPath, GifHandler.GifScope.MainPanel).MakeGifArray();
            // timer.Start();
            _picBox.Image = ImageHandler(); // Make async somehow 
        }

        private Image ImageHandler()
        {
            string currentPath = _pfpPath;

            //bool canAnimate = ImageAnimator.CanAnimate(new Bitmap(_pfpPath));
            using (GifHandler handler = new GifHandler(currentPath, GifHandler.GifScope.MainPanel))
            {
                timer.Stop();
                if (currentPath == null || _pfp == null || _pfpPath != _oldPfpPath || !_drawRounded)
                {
                    if (handler.IsGif)
                    {
                        frameCount = 0;
                        timerFrames = null;

                        timerFrames = handler.RawFramesResizeGif(_pfpSize.Width, _pfpSize.Height);
                        timer.Interval = handler.GetFrameDelayMs();


                        timer.Start();
                        _pfp = null;
                        return null;
                    
                        if (_drawRounded) goto DrawRounded;
                        _pfp = new Bitmap(new GifHandler(currentPath, GifHandler.GifScope.MainPanel).ResizeGif(_pfpSize.Width, _pfpSize.Height));
                        return _pfp;
                    }

                    _pfp = ResizeImage(Image.FromFile(_pfpPath), _pfpSize.Width, _pfpSize.Height);
                }
            
                DrawRounded:
                if (_drawRounded)
                {
                    if (handler.IsGif)
                    {
                        if ((string)_pfp.Tag != currentPath) _pfp = new Bitmap(new GifHandler(currentPath, GifHandler.GifScope.MainPanel).ResizeAndRoundGifCorners(_pfpSize.Width, _pfpSize.Height));
                        return _pfp;
                    }
                
                    if ((string)_pfp.Tag != currentPath) _pfp = ApplyRoundCorners();
                }
                
            }
            
            return _pfp;
        }

        private Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);
            
            //destImage.SetResolution(image.HorizontalResolution >= 72? 95 : image.HorizontalResolution, image.VerticalResolution >= 72? 95 : image.VerticalResolution);
            //destImage.SetResolution(image.HorizontalResolution,image.VerticalResolution); //TODO: Remove if above solution suffices
            destImage.SetResolution(96,96);
            
            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceOver;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
        
                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width,image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }
        
            return destImage;
        }

        private Bitmap ApplyRoundCorners()
        {
            Rectangle plaster = new Rectangle(0, 0, _pfpSize.Width, _pfpSize.Height);
            Bitmap pfpBmp = ResizeImage(Image.FromFile(_pfpPath), _pfpSize.Width,_pfpSize.Height);
            Bitmap targetBmp = new Bitmap(_pfpSize.Width, _pfpSize.Height);

            
            Point pPlasterCenterRelative = new Point(plaster.Width / 2, plaster.Height / 2);
            Point pImageCenterRelative = new Point(pfpBmp.Width / 2, pfpBmp.Height / 2);
            Point pOffSetRelative = new Point(pPlasterCenterRelative.X - pImageCenterRelative.X, pPlasterCenterRelative.Y - pImageCenterRelative.Y);

            Point xAbsolutePixel = pOffSetRelative + new Size(plaster.Location); //Find the absolute location

            using (Graphics graphics = Graphics.FromImage(targetBmp))
            {
                using (GraphicsPath path = new GraphicsPath())
                {
                    using (TextureBrush texture = new TextureBrush(pfpBmp, WrapMode.Clamp))
                    {
                        graphics.CompositingMode = CompositingMode.SourceOver;
                        graphics.CompositingQuality = CompositingQuality.HighQuality;
                        graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        graphics.SmoothingMode = SmoothingMode.HighQuality;
                        graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                        
                        graphics.FillRectangle(new SolidBrush(_bgColor), plaster);

                        texture.TranslateTransform(xAbsolutePixel.X, xAbsolutePixel.Y);
                        
                        path.AddEllipse(plaster);
                        graphics.FillEllipse(texture, plaster);

                        path.CloseFigure();
                        
                        targetBmp.Tag = _pfpPath;

                        return targetBmp;
                    }
                }
            }
        }

        public static Image P_ResizeImage(Image image, int width, int height)
        {
            if (ImageAnimator.CanAnimate(image) && (string)image.Tag != "gifFrame")
            {
                using (GifHandler handler = new GifHandler((string)image.Tag, GifHandler.GifScope.Form))
                {
                    return new Bitmap(handler.ResizeGif(width,height));
                }
            }
            
            Rectangle destRect = new Rectangle(0, 0, width, height);
            Bitmap destImage = new Bitmap(width, height);
            
            destImage.SetResolution(image.HorizontalResolution >= 72? 95 : image.HorizontalResolution, image.VerticalResolution >= 72? 95 : image.VerticalResolution);
            
            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
        
                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width,image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }
            destImage.Tag = image.Tag;
            
            return destImage;
        }

        public static Image P_ApplyRoundedCorners(Image image, int width, int height, bool useMenuColor = true)
        {
            if (ImageAnimator.CanAnimate(image) && (string)image.Tag != "gifFrame")
            {
                using (GifHandler handler = new GifHandler((string)image.Tag, GifHandler.GifScope.Form, true))
                {
                    return new Bitmap(handler.ResizeAndRoundGifCorners(width, height));
                }
            }
            
            Rectangle plaster = new Rectangle(0, 0, width, height);
            Image pfpBmp = P_ResizeImage(image, width, height);
            Bitmap targetBmp = new Bitmap(width, height);

            
            Point pPlasterCenterRelative = new Point(plaster.Width / 2, plaster.Height / 2);
            Point pImageCenterRelative = new Point(pfpBmp.Width / 2, pfpBmp.Height / 2);
            Point pOffSetRelative = new Point(pPlasterCenterRelative.X - pImageCenterRelative.X, pPlasterCenterRelative.Y - pImageCenterRelative.Y);

            Point xAbsolutePixel = pOffSetRelative + new Size(plaster.Location); //Find the absolute location

            using (Graphics graphics = Graphics.FromImage(targetBmp))
            {
                using (GraphicsPath path = new GraphicsPath())
                {
                    using (TextureBrush texture = new TextureBrush(pfpBmp, WrapMode.Clamp))
                    {
                        graphics.CompositingMode = CompositingMode.SourceOver;
                        graphics.CompositingQuality = CompositingQuality.HighQuality;
                        graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        graphics.SmoothingMode = SmoothingMode.HighQuality;
                        graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                        graphics.FillRectangle(!useMenuColor ? new SolidBrush(Color.FromArgb(Plugin._mbApiInterface.Setting_GetSkinElementColour.Invoke(Plugin.SkinElement.SkinSubPanel, Plugin.ElementState.ElementStateDefault, Plugin.ElementComponent.ComponentBackground))) : new SolidBrush(SystemColors.Menu), plaster);

                        texture.TranslateTransform(xAbsolutePixel.X, xAbsolutePixel.Y);
                        
                        path.AddEllipse(plaster);
                        graphics.FillEllipse(texture, plaster);

                        path.CloseFigure();
                        
                        targetBmp.Tag = image.Tag;

                        return targetBmp;
                    }
                }
            }
        }

        public void MakePicBox()
        {
            _controlMain = Plugin.FormControlMain;

            void PicBoxMake()
            {
                _picBox = new PictureBox { Parent = _controlMain, Name = "picBox" };
                _controlMain.Controls.Add(_picBox);
            }

            _controlMain.Invoke((Action)PicBoxMake);
            _picBox.Paint += picBox_Paint;
        }

        private static System.Timers.Timer timer;
        private static Bitmap[] timerFrames;
        private static int frameCount;
        public void MakeTimer()
        {
            timer = new System.Timers.Timer();
            timer.Interval = 60;

            timer.Elapsed += Timer_Elapsed;
            
            // timer.Start();
        }
        
        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            _picBox.Image = timerFrames[frameCount];
            frameCount++;
            if (frameCount >= timerFrames.Length) frameCount = 0;
        }

        private void picBox_Paint(object sender, PaintEventArgs e)
        {
            if (_drawRounded)
            {
                using (GraphicsPath gp = new GraphicsPath())
                {
                    e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
                    gp.AddEllipse(0, 0, _picBox.Size.Width, _picBox.Size.Height);
                    _picBox.Region = new Region(gp);
                    e.Graphics.DrawEllipse(new Pen(new SolidBrush(_bgColor)), 0, 0, _picBox.Size.Width, _picBox.Size.Height);
                    
                    return;
                }
            }

            _picBox.Region = null;
        }

        private void CalculateCenter_Point()
        {
            _picBox = (PictureBox) _controlMain.Controls["picBox"];
            
            if (_pfp == null)
            {
                _pfp = ResizeImage(Image.FromFile(_pfpPath), _pfpSize.Width,_pfpSize.Height);
            }
            
            _pfpPoint.X = _controlMain.Size.Width / 2 - _pfp.Width / 2;
            _pfpPoint.Y = _controlMain.Size.Height / 2 - _pfp.Height / 2;

            _usernamePoint.X = Convert.ToInt32((_controlMain.Size.Width - _eventArgs.Graphics.MeasureString(_username, SystemFonts.CaptionFont).Width) / 2); // calculate text center relative to text and control size
            _usernamePoint.Y = (_controlMain.Height / 2) + _pfp.Height / 2 + 3;
            
            _picBox.Location = _pfpPoint;
            _picBox.Size = _pfpSize;
            _picBox.SizeMode = PictureBoxSizeMode.Zoom;
        }
    }
}