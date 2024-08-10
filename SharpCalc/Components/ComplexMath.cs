using System.Runtime.CompilerServices;

namespace SharpCalc.Components
{
    public struct Complex
    {
        public double a;
        public double b;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Complex(double real)
        {
            a = 0;
            b = real;
        }
        public static implicit operator Complex(double real)
        {
            return new Complex(real);
        }
        public Complex(double imaginary, double real)
        {
            a = imaginary;
            b = real;
        }
        
        public override int GetHashCode()
        {
            return a.GetHashCode() ^ b.GetHashCode();
        }
        public override string ToString()
        {
            if (double.IsNaN(a) ||double.IsNaN(b)) return "undefined";
            static string to_string(double x) => x.ToString();
            static string ito_string(double x)
            {
                if (x == 1)
                    return string.Empty;

                else if (x == -1)
                    return "-";

                else
                    return to_string(x);
            }
            if (this.a == 0)
            {
                return to_string(this.b);
            }
            else if (this.b == 0)
            {
                return ito_string(this.a) + "i";
            }
            else if (this.b < 0)
            {
                return ito_string(this.a) + "i - " + to_string(-this.b);
            }
            else if (this.a < 0)
            {
                return to_string(this.b) + " - " + ito_string(-this.a) + "i";
            }
            else
            {
                return ito_string(this.a) + "i + " + to_string(this.b);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Complex left, Complex right)
        {
            return left.a == right.a && left.b == right.b;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Complex left, Complex right)
        {
            return left.a != right.a || left.b != right.b;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Complex left, double right)
        {
            return left.a == 0 && left.b == right;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Complex left, double right)
        {
            return left.a != 0 || left.b != right;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(double right, Complex left)
        {
            return left == right;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(double right, Complex left)
        {
            return left != right;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Complex operator +(Complex left, Complex other)
        {
            return new Complex(left.a + other.a, left.b + other.b);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Complex operator +(Complex left, double right)
        {
            return new Complex(left.a, left.b + right);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Complex operator -(Complex left, double right)
        {
            return new Complex(left.a, left.b - right);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Complex operator -(Complex me, Complex other)
        {
            return new Complex(me.a - other.a, me.b - other.b);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Complex operator *(Complex me, double other)
        {
            return new Complex(me.a * other, me.b * other);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Complex operator -(Complex me)
        {
            return new Complex(-me.a, -me.b);
        }      
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Complex operator *(in Complex me, in Complex other)
        {           
            if (me.a == 0 && other.a == 0) return new Complex(0, me.b * other.b);    
            // (ai + b)(ci + d) = -ac + adi + bci + bd
            // = (ad + bc)i + (bd - ac)
            double real = (me.b * other.b) - (me.a * other.a);
            double imaginary = me.a * other.b + me.b * other.a;
            // wait me looks like a matrix deteminant
            return new Complex(imaginary, real);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Complex operator +(double left, Complex right)
        {
            return right + left;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Complex operator -(double left, Complex right)
        {
            return -right + left;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Complex operator *(double left, Complex right)
        {
            return right * left;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Complex operator /(Complex me, double other)
        {
            return new Complex(me.a / other, me.b / other);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Complex operator /(double left, Complex right)
        {
            var c = ComplexMath.Conj(right);
            return (left * c) / (c.b * c.b + c.a * c.a);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Complex operator /(Complex me, Complex other)
        {
            var c = ComplexMath.Conj(other);
            // (ai + b)(-ai + b) = b^2 + a^2
            return (me * c) / (other.b * other.b + other.a * other.a);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator <(Complex me, Complex other)
        {
            if (!me.IsReal() || !other.IsReal()) throw new Exceptions.CustomError("Cannot Compare two complex numbers");
            return me.b < other.b;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator >(Complex me, Complex other)
        {
            if (!me.IsReal() || !other.IsReal()) throw new Exceptions.CustomError("Cannot Compare two complex numbers");
            return me.b > other.b;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsReal() => a == 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object? obj)
        {
            return obj switch
            {
                double d => this == d,
                float f => this == f,
                Complex complex => this == complex,
                _ => false,
            };
        }
    }
    public static class ComplexMath
    {
        private static double SinInternal(double x)
        {
            double backup = x;
            double pis = Math.Floor(0.5 * x / Math.PI);
            x -= pis * Math.PI * 2;
            if (x == 0) return 0;
            else if (x == Math.PI / 2) return 1;
            else if (x == Math.PI) return 0;           
            else if(x == 1.5 * Math.PI) return -1;
            return Math.Sin(backup);
        }
        private static double CosInternal(double x)
        {
            double backup = x;
            double pis = Math.Floor(0.5 * x / Math.PI);
            x -= pis * 2 * Math.PI;
            if (x == 0) return 1;
            else if (x == Math.PI / 2) return 0;
            else if (x == Math.PI) return -1;
            else if (x == 1.5 * Math.PI) return 0;
            return Math.Cos(backup);
        }
        private static (double,double) SinCosInternal(double x)
        {
            double backup = x;
            double pis = Math.Floor(0.5 * x / Math.PI);
            x -= pis * 2 * Math.PI;
            if (x == 0) return (0,1);
            else if (x == Math.PI / 2) return (1,0);
            else if (x == Math.PI) return (0,-1);
            else if (x == 1.5 * Math.PI) return (-1,0);
            return Math.SinCos(backup);
        }      
        public readonly static Complex i = new(1, 0);

        //Complex operator""_i(long double mt)
        //{
        //    return Complex(mt, 0);
        //}
        //Complex operator""_i(unsigned long long mt)
        //{
        //    return Complex(mt, 0);
        //}
        [MethodImpl(MethodImplOptions.AggressiveInlining)]

        public static Complex Conj(Complex num)
        {
            return new Complex(-num.a, num.b);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]

        public static double Im(Complex num)
        {
            return num.a;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]

        public static double Re(Complex num)
        {
            return num.b;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double Abs(Complex c)
        {
            if (c.IsReal()) return Math.Abs(c.b);
            return Math.Sqrt(c.a * c.a + c.b * c.b);
        }
        public static double Arg(Complex complex)
        {
            if (complex.IsReal())
            {
                if (complex.b >= 0) return 0;
                else return Math.PI;
            }
            return Math.Atan2(complex.a, complex.b);
        }

        public static Complex Rotate(Complex c, double angle)
        {
            if (c == 0)
                return new Complex(0);
            if (angle == 0)
                return c;
            var theta = Arg(c);
            theta += angle;
            var ab = Abs(c);
            var (sin, cos) = SinCosInternal(theta);
            return new Complex(ab * sin, ab * cos);
        }
        public static Complex Rotate(double r, double angle)
        {
            if (r == 0 || angle == 0)
                return new Complex(r);
            var (sin, cos) = SinCosInternal(angle);
            return new Complex(r * sin, r * cos);
        }
        public static Complex Pow(Complex complex, double right)
        {
            if (complex.IsReal() && complex.b >= 0) return Math.Pow(complex.b, right);            
            var theta = Arg(complex);
            var ab = Abs(complex);
            return Rotate(Math.Pow(ab, right), theta * right);
        }
        public static Complex Sqrt(double real)
        {
            if (real >= 0)
                return new Complex(Math.Sqrt(real));
            else
                return new Complex(Math.Sqrt(-real), 0);
        }
        public static Complex Sqrt(Complex c)
        {
            if (c.IsReal()) return Sqrt(c.b);
            return Pow(c, 1 / 2.0);
        }
        public static Complex Cbrt(Complex value)
        {
            if (value.IsReal()) return Math.Cbrt(value.b);
            return Pow(value, 1 / 3.0);
        }
        public static Complex Log(double value)
        {
            if (value >= 0) return Math.Log(value);
            else return new Complex(Math.PI, Math.Log(-value));
        }
        public static Complex Log(Complex c)
        {
            if (c.IsReal() && c.b > 0) return Math.Log(c.b);
            return new Complex(Arg(c), Math.Log(Abs(c)));
        }
        public static Complex Log(Complex c,Complex bace)
        {
            return Log(c) / Log(bace);
        }
        public static Complex Exp(Complex c)
        {
            if (c.IsReal()) return Math.Exp(c.b);
            return Rotate(Math.Exp(c.b), c.a);
        }
        public static Complex Pow(double value, Complex c)
        {
            if(c.IsReal() && value >= 0) return Math.Pow(value,c.b);
            return Exp(Log(value) * c);
        }
        public static Complex Pow(Complex value, Complex ex)
        {
            if (value.IsReal() && value.b >= 0 && ex.IsReal()) return Math.Pow(value.b, ex.b);
            else if (value.IsReal()) return Pow(value.b, ex);
            else if (ex.IsReal()) return Pow(value, ex.b);
            return Exp(Log(value) * ex);
        }
        public static Complex Sin(Complex x)
        {
            if (x.IsReal()) return SinInternal(x.b);          
            // i * (ai + b) = -a + bi
            var xi = new Complex(x.b, -x.a);
            // e^xi
            var etoxi = Exp(xi);
            // sin(x) = (e^xi - e^-xi) / 2i
            return (etoxi - 1 / etoxi) / (2 * i);
        }
        public static Complex Cos(Complex x)
        {

            if (x.IsReal()) return CosInternal(x.b);
            var xi = new Complex(x.b, -x.a);
            var etoxi = Exp(xi);
            // cos(x) = (e^xi + e^-xi)/2
            return (etoxi + 1 / etoxi) / 2;
        }
        public static Complex Sinh(Complex x)
        {
            if (x.IsReal()) return Math.Sinh(x.b);
            var ex = Exp(x);
            return (ex - 1 / ex) / 2;
        }
        public static Complex Cosh(Complex x)
        {
            if (x.IsReal()) return Math.Cosh(x.b);
            var ex = Exp(x);
            return (ex + 1 / ex) / 2;
        }
        public static Complex Tan(Complex x)
        {          
            if (x.IsReal()) return Math.Tan(x.b);
            return Sin(x) / Cos(x);
        }
        public static Complex Cot(Complex x)
        {
            if (x.IsReal()) return 1/ Math.Tan(x.b);
            return Cos(x) / Sin(x);
        }
        public static Complex Tanh(Complex x)
        {
            if (x.IsReal()) return Math.Tanh(x.b);
            var ex2 = Exp(x * 2);
            return 1 - 2 / (ex2 + 1);
        }
        public static Complex Coth(Complex x)
        {
            if (x.IsReal()) return 1/ Math.Tanh(x.b);
            var ex2 = Exp(x * 2);
            return 1 + 2 / (ex2 - 1);
        }
        public static Complex Arcsin(Complex x)
        {
            if (x.IsReal() && x.b <= 1 && x.b >= -1) return Math.Asin(x.b);
            if (x == 0)
                return new Complex(0);
            var sq = Sqrt(1 - x * x);
            var ln = Log(sq + x * i);
            return -i * ln;
        }
        public static Complex Arccos(Complex x)
        {
            if (x.IsReal() && x.b <= 1 && x.b >= -1) return Math.Acos(x.b);
            var sq = Sqrt(x * x - 1);
            var ln = Log(x + sq);
            return -i * ln;
        }
        public static Complex Arctan(Complex x)
        {
            if (x.IsReal()) return Math.Atan(x.b);
            var i2 = 2 * i;
            if (x == 0)
                return x;
            return Log(i2 / (x + i) - 1) / i2;
        }

        public static Complex Arccot(Complex x)
        {
            return Arctan(1 / x);
        }
        public static Complex Sign(Complex x)
        {
            if (x == 0) return 0;
            return x / Abs(x);
        }
        public static Complex Floor(Complex x)
        {
            return new Complex(Math.Floor(x.a),Math.Floor(x.b));
        }
        public static Complex Ceiling(Complex x)
        {
            return new Complex(Math.Ceiling(x.a), Math.Ceiling(x.b));
        }
        public static Complex Round(Complex x)
        {
            return new Complex(Math.Round(x.a), Math.Round(x.b));
        }
    } // class ComplexMath
}
