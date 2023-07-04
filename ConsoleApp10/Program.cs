using System;
using ConsoleApp10;

internal class Program
{
    static void Main()
    {
        double x0 = -2;
        double xn = -1;
        int n = 2;
        Generate a = new Generate();
        a.Generated(xn, x0, n);
        int n1 = 8;
        int n2 = 16;
        Generate b = new Generate();
        Generate c = new Generate();
        b.Generated(xn, x0, n1);
        c.Generated(xn, x0, n2);
    }
}
