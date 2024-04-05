using System;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

namespace Amrv.ConfigurableCompany.API.Accesors
{
    public sealed class BuildColor
    {
        private readonly byte R;
        private readonly byte G;
        private readonly byte B;
        private readonly byte A;

        private BuildColor(Color color)
        {
            R = (byte)(color.r * byte.MaxValue);
            G = (byte)(color.g * byte.MaxValue);
            B = (byte)(color.b * byte.MaxValue);
            A = (byte)(color.a * byte.MaxValue);
        }

        private BuildColor(byte r, byte g, byte b, byte a)
        {
            R = r;
            G = g;
            B = b;
            A = a;
        }

        public static implicit operator BuildColor(Color color)
        {
            return new(color);
        }

        public static implicit operator Color(BuildColor color)
        {
            return new(color.R / byte.MaxValue, color.G / byte.MaxValue, color.B / byte.MaxValue, color.A / byte.MaxValue);
        }

        public static implicit operator BuildColor(Color32 color)
        {
            return new(color);
        }

        public static implicit operator Color32(BuildColor color)
        {
            return new Color32(color.R, color.G, color.B, color.A);
        }

        public static implicit operator BuildColor(System.Drawing.Color color)
        {
            return new(new Color32(color.R, color.G, color.B, color.A));
        }

        public static implicit operator System.Drawing.Color(BuildColor color)
        {
            return System.Drawing.Color.FromArgb(color.A, color.R, color.G, color.B);
        }

        public static implicit operator BuildColor((byte r, byte g, byte b) color)
        {
            return new(new Color32(color.r, color.g, color.b, 255));
        }

        public static implicit operator (byte r, byte g, byte b)(BuildColor color)
        {
            return (color.R, color.G, color.B);
        }

        public static implicit operator BuildColor((byte r, byte g, byte b, byte a) color)
        {
            return new(new Color32(color.r, color.g, color.b, color.a));
        }

        public static implicit operator (byte r, byte g, byte b, byte a)(BuildColor color)
        {
            return (color.R, color.G, color.B, color.A);
        }

        public static implicit operator BuildColor(int hex)
        {
            return new((byte)(hex >> 16 & 0xff), (byte)(hex >> 8 & 0xff), (byte)(hex & 0xff), (byte)(hex >> 24 & 0xff));
        }
    }
}
