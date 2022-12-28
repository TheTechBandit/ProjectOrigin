using System.Drawing;

namespace ProjectOrigin
{
    public class ImageCriteria
    {
        public string ImageBack { get; set; }
        public Bitmap BImageBack { get; set; }
        public bool IsBitmapBack { get; set; }
        public string Image { get; set; }
        public Bitmap BImage { get; set; }
        public bool IsBitmap { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public double XPerc { get; set; }
        public double YPerc { get; set; }
        public bool Flip { get; set; }
        public bool Centralize { get; set; }
        public int XWidth { get; set; }
        public int YHeight { get; set; }
        public double Scale { get; set; }

        public ImageCriteria()
        {
            Image = null;
            BImage = null;
            IsBitmap = false;
            X = 0;
            Y = 0;
            XPerc = 0.0;
            YPerc = 0.0;
            Flip = false;
            Centralize = false;
            XWidth = 0;
            YHeight = 0;
            Scale = 1.0;
        }

        public ImageCriteria AddBack(string img)
        {
            ImageBack = img;
            IsBitmapBack = false;
            return this;
        }

        public ImageCriteria AddBack(Bitmap img)
        {
            BImageBack = img;
            IsBitmapBack = true;
            return this;
        }

        public ImageCriteria ClearBack()
        {
            ImageBack = null;
            BImageBack = null;
            IsBitmapBack = true;
            return this;
        }

        public ImageCriteria AddImage(string img)
        {
            Image = img;
            IsBitmap = false;
            return this;
        }

        public ImageCriteria AddImage(Bitmap img)
        {
            BImage = img;
            IsBitmap = true;
            return this;
        }

        public ImageCriteria ClearImage()
        {
            Image = null;
            BImage = null;
            IsBitmap = true;
            return this;
        }

        public Bitmap GetImage()
        {
            if (IsBitmap)
                return BImage;
            else
                return ImageGenerator.UrlToBitmap(Image);
        }

        public ImageCriteria SetX(int x)
        {
            X = x;
            return this;
        }

        public ImageCriteria SetY(int y)
        {
            Y = y;
            return this;
        }

        public ImageCriteria SetXPerc(double x)
        {
            XPerc = x;
            return this;
        }

        public ImageCriteria SetYPerc(int y)
        {
            YPerc = y;
            return this;
        }

        public ImageCriteria SetFlip(bool flip)
        {
            Flip = flip;
            return this;
        }

        public ImageCriteria SetCentralize(bool centralize)
        {
            Centralize = centralize;
            return this;
        }

        public ImageCriteria SetXWidth(int x)
        {
            XWidth = x;
            return this;
        }

        public ImageCriteria SetYHeight(int y)
        {
            YHeight = y;
            return this;
        }

        public ImageCriteria SetScale(double scale)
        {
            Scale = scale;
            return this;
        }

        public int GetXLoc()
        {
            int x = X;
            if (Centralize)
                x -= GetXSize() / 2;
            return x;
        }

        public int GetYLoc()
        {
            int y = Y;
            if (Centralize)
                y -= GetYSize() / 2;
            return y;
        }

        public int GetXSize()
        {
            return (int)(XWidth * Scale);
        }

        public int GetYSize()
        {
            return (int)(YHeight * Scale);
        }

        public ImageCriteria DefaultReset()
        {
            ClearBack();
            ClearImage();
            X = 0;
            Y = 0;
            XPerc = 0.0;
            YPerc = 0.0;
            Flip = false;
            Centralize = false;
            XWidth = 0;
            YHeight = 0;
            Scale = 1.0;
            return this;
        }

    }
}