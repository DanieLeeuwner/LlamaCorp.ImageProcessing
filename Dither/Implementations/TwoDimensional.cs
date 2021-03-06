﻿using LlamaCorp.ImageProcessing.Attributes;
using LlamaCorp.ImageProcessing.Dither.Entities;
using LlamaCorp.ImageProcessing.Dither.Enums;
using LlamaCorp.ImageProcessing.Dither.Interfaces;

namespace LlamaCorp.ImageProcessing.Dither.Implementations
{
  [EnumLink(typeof(DitherAlgorithm), (int)DitherAlgorithm.TwoDimensional)]
  public class TwoDimensional : IDither
  {
    public void PerformWork(Image image, Parameters parameters = null)
    {
      if (parameters == null)
      {
        parameters = new Parameters();
      }

      int error = 0;

      for (var i = 0; i < image.Pixels.Length; i++)
      {
        var pixel = image.Pixels[i];
        var avg = (pixel.R * parameters.R_Multiplier + pixel.G * parameters.G_Multiplier + pixel.B * parameters.B_Multiplier);

        avg += pixel.Error;

        if (avg < parameters.Threshold)
        {
          error = (int)(avg);
          avg = 0;
        }
        else
        {
          error = (int)(avg - 255);
          avg = 255;
        }

        if ((i + 1) % image.Width != 0)
        {
          image.Pixels[i + 1].Error = (error);
        }

        if (i < image.Pixels.Length - image.Width - 1)
        {
          image.Pixels[i + image.Width].Error = (error / 2);
        }

        image.Pixels[i] = new Pixel()
        {
          R = (byte)avg,
          G = (byte)avg,
          B = (byte)avg,
          A = 255,
        };
      }
    }
  }
}