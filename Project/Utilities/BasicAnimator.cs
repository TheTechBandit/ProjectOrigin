using System;
using System.Collections.Generic;
using System.Drawing;

namespace ProjectOrigin
{
    public class BasicAnimator
    {
        public List<Bitmap> Frames { get; set; }
        public virtual Bitmap Background { get; set; }
        public ImageCriteria Criteria { get; set; }
        public List<(int i, string name)> Keyframes { get; set; }
        public int CurrentFrame { get; set; }
        public int Speed { get; set; }

        public BasicAnimator()
        {

        }

        public BasicAnimator(bool newanim)
        {
            Frames = new List<Bitmap>();
            Criteria = new ImageCriteria();
            Keyframes = new List<(int i, string name)>();
            Keyframes.Add((0, "Start"));
            CurrentFrame = 0;
            Speed = 10;
        }

        public void AddFrame(Bitmap bitmap)
        {
            Frames.Add(bitmap);
        }

        public void AddFrame(string url)
        {
            Frames.Add(ImageGenerator.UrlToBitmap(url));
        }

        public void ReturnToLastKeyframe()
        {
            if (CurrentFrame != 0)
            {
                int diff = 999;
                int originalFrame = CurrentFrame;
                foreach ((int indx, string name) in Keyframes)
                {
                    //Console.WriteLine($"Checking Keyframe {indx} {name} against CurrentFrame {CurrentFrame}"); 
                    if (originalFrame > indx && diff > originalFrame - indx)
                    {
                        diff = originalFrame - indx;
                        CurrentFrame = indx;
                        //Console.WriteLine($"Returning to keyframe: {indx} {name}");
                    }
                }
            }
            //Console.WriteLine("Job's Done :)");
        }

        public void ReturnToKeyframe(string keyframe)
        {
            foreach ((int indx, string name) in Keyframes)
            {
                if (name == keyframe)
                {
                    CurrentFrame = indx;
                    break;
                }
            }
        }

        public Bitmap AddImageToFrame(ImageCriteria criteria, int i)
        {
            //Ensure there are enough frames
            while (Frames.Count < i + 1)
            {
                Frames.Add(Background);
            }

            //Get the background image
            Image backImage = Frames[i];

            //Get the overlay image
            Image overlayImage = criteria.GetImage();

            if (criteria.Flip)
                overlayImage.RotateFlip(RotateFlipType.RotateNoneFlipX);

            if (backImage == null)
                throw new ArgumentNullException("backImage");
            if (overlayImage == null)
                throw new ArgumentNullException("overlayImage");

            Bitmap outputImage = new Bitmap(backImage.Width, backImage.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            if (criteria.XWidth == 0)
                criteria.XWidth = overlayImage.Width;
            if (criteria.YHeight == 0)
                criteria.YHeight = overlayImage.Height;

            using (Graphics graphics = Graphics.FromImage(outputImage))
            {
                graphics.DrawImage(backImage, new Rectangle(new Point(), backImage.Size),
                    new Rectangle(new Point(), backImage.Size), GraphicsUnit.Pixel);
                graphics.DrawImage(overlayImage, new Rectangle(criteria.GetXLoc(), criteria.GetYLoc(), criteria.GetXSize(), criteria.GetYSize()),
                    new Rectangle(new Point(), overlayImage.Size), GraphicsUnit.Pixel);
            }

            Frames[i] = outputImage;
            return outputImage;
        }

        public void AddAllImagesToFrames(ImageCriteria Criteria, List<Bitmap> images, int i)
        {
            foreach (Bitmap img in images)
            {
                Criteria.AddImage(img);
                AddImageToFrame(Criteria, i);
                CurrentFrame++;
                i++;
            }
        }

    }
}