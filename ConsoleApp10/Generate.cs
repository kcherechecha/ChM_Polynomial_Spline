namespace ConsoleApp10;

public class Generate
{
    static double f(double x)
    {
        return Math.Pow(x, 5) - 5 * x + 2;
    }

    public void Generated(double xn, double x0, int n)
    {
        double h = (xn - x0) / n;
        
        double[] xi = new double[n + 1];
        double[] yi = new double[n + 1];

        for (int i = 0; i <= n; i++)
        {
            xi[i] = x0 + i * h;
            yi[i] = f(xi[i]);
        }

        Console.WriteLine("\nСистема точок на площині:");
        for (int i = 0; i <= n; i++)
        {
            Console.WriteLine($"x{i} = {xi[i]}, y{i} = {yi[i]}");
        }

        double x = -1;
        LagrangePolynomial(xi, yi);
        NewtonInterpolation(xi, yi);
        double[] b, c, d;
        Spline(xi, yi, out b, out c, out d);
    }
    
    static void LagrangePolynomial(double[] xi, double[] yi)
    {
        string result = "";
        for (int i = 0; i < xi.Length; i++)
        {
            string term = $"{yi[i]}";
            for (int j = 0; j < xi.Length; j++)
            {
                if (j != i)
                {
                    term += $" * (x - {xi[j]}) / ({xi[i]} - {xi[j]})";
                }
            }
            result += term;
            if (i < xi.Length - 1)
            {
                result += ") + (";
            }
        }
        Console.WriteLine($"\nІнтерполяційний поліном Лагранжа:\nL(x) = {result}");
    }

    static void NewtonInterpolation(double[] xi, double[] yi)
    {
        int n = xi.Length - 1;
        double[] b = new double[n + 1];
        for (int i = 0; i <= n; i++)
        {
            b[i] = yi[i];
        }

        for (int j = 1; j <= n; j++)
        {
            for (int i = n; i >= j; i--)
            {
                b[i] = (b[i] - b[i - 1]) / (xi[i] - xi[i - j]);
            }
        }

        string result = "P(x) = ";
        for (int i = 0; i <= n; i++)
        {
            string term = b[i].ToString();
            for (int j = 0; j < i; j++)
            {
                term += " * (x - " + xi[j].ToString() + ")";
            }
            result += term;
            if (i < n)
            {
                result += " + ";
            }
        }

        Console.WriteLine($"\nІнтерполяційний поліном Ньютона:\n{result}");
    }
    private void Spline(double[] x, double[] y, out double[] b, out double[] c, out double[] d)
    {
        int n = x.Length;
        double[] a = (double[])y.Clone();
        b = new double[x.Length];
        c = new double[x.Length];
        d = new double[x.Length];
        double[] h = new double[n - 1];
        double[] alpha = new double[n - 1];
        double[] l = new double[n];
        double[] mu = new double[n];
        double[] z = new double[n];

        for (int i = 0; i < n - 1; i++)
        {
            h[i] = x[i + 1] - x[i];
        }

        for (int i = 1; i < n - 1; i++)
        {
            alpha[i] = 6 * ((a[i + 1] - a[i]) / h[i] - (a[i] - a[i - 1]) / h[i - 1]);
        }

        l[0] = 1;
        mu[0] = 0;
        z[0] = 0;

        for (int i = 1; i < n - 1; i++)
        {
            l[i] = 2 * (x[i + 1] - x[i - 1]) - h[i - 1] * mu[i - 1];
            mu[i] = h[i] / l[i];
            z[i] = (alpha[i] - h[i - 1] * z[i - 1]) / l[i];
        }

        l[n - 1] = 1;
        z[n - 1] = 0;
        c[n - 1] = 0;

        for (int j = n - 2; j >= 0; j--)
        {
            c[j] = z[j] - mu[j] * c[j + 1];
            d[j] = (c[j + 1] - c[j]) / (h[j]);
        }

        for (int i = 1; i < b.Length; i++)
        {
            b[i] = c[i] * h[i - 1] / 2 - Math.Pow(h[i - 1], 2) * d[i - 1] / 6 + (a[i] - a[i - 1]) / h[i - 1];
        }
        Console.WriteLine("\nКоефіцієнти та сплайн:");
        for (int i = 0; i < x.Length - 1; i++)
        {
            Console.WriteLine($"a{i+1}={y[i+1]} | b{i+1}={b[i+1]} | c{i+1}={c[i+1]} | d{i+1}={d[i+1]}");
            Console.WriteLine($"s{i+1} = {y[i + 1]} + {b[i + 1]}(x - {x[i + 1]}) +  {c[i + 1] / 2}(x - {x[i + 1]})^2 + {d[i] / 6}(x - {x[i + 1]})^3 на інтервалі [{x[i]};{x[i + 1]}]  ");
        }
    }
}