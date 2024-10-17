using System.Diagnostics;
using DotNext;
using McProtoNet;

public class BallisticSolver
{
    // SolveQuadric, SolveCubic, and SolveQuartic were ported from C as written for Graphics Gems I
    // Original Author: Jochen Schwarze (schwarze@isa.de)
    // https://github.com/erich666/GraphicsGems/blob/240a34f2ad3fa577ef57be74920db6c4b00605e4/gems/Roots3And4.c

    // Utility function used by SolveQuadratic, SolveCubic, and SolveQuartic
    private static bool IsZero(double d)
    {
        const double eps = 1e-9;
        return d > -eps && d < eps;
    }

    private static double GetCubicRoot(double value)
    {
        if (value > 0.0)
        {
            return System.Math.Pow(value, 1.0 / 3.0);
        }
        else if (value < 0)
        {
            return -System.Math.Pow(-value, 1.0 / 3.0);
        }
        else
        {
            return 0.0;
        }
    }

    // Solve quadratic equation: c0*X^2 + c1*X + c2. 
    // Returns number of solutions.
    public static int SolveQuadric(double c0, double c1, double c2, out double s0, out double s1)
    {
        s0 = double.NaN;
        s1 = double.NaN;

        double p, q, D;

        /* normal form: X^2 + px + q = 0 */
        p = c1 / (2 * c0);
        q = c2 / c0;

        D = p * p - q;

        if (IsZero(D))
        {
            s0 = -p;
            return 1;
        }

        if (D < 0)
        {
            return 0;
        }

        double sqrt_D = Math.Sqrt(D);

        s0 = sqrt_D - p;
        s1 = -sqrt_D - p;
        return 2;
    }

    // Solve cubic equation: c0*X^3 + c1*X^2 + c2*X + c3. 
    // Returns number of solutions.
    public static int SolveCubic(double c0, double c1, double c2, double c3, out double s0, out double s1,
        out double s2)
    {
        s0 = double.NaN;
        s1 = double.NaN;
        s2 = double.NaN;

        int num;
        double sub;
        double A, B, C;
        double sq_A, p, q;
        double cb_p, D;

        /* normal form: X^3 + Ax^2 + Bx + C = 0 */
        A = c1 / c0;
        B = c2 / c0;
        C = c3 / c0;

        /*  substitute X = Y - A/3 to eliminate quadric term:  X^3 +px + q = 0 */
        sq_A = A * A;
        p = 1.0 / 3 * (-1.0 / 3 * sq_A + B);
        q = 1.0 / 2 * (2.0 / 27 * A * sq_A - 1.0 / 3 * A * B + C);

        /* use Cardano's formula */
        cb_p = p * p * p;
        D = q * q + cb_p;

        if (IsZero(D))
        {
            if (IsZero(q)) /* one triple solution */
            {
                s0 = 0;
                num = 1;
            }
            else /* one single and one double solution */
            {
                double u = GetCubicRoot(-q);
                s0 = 2 * u;
                s1 = -u;
                num = 2;
            }
        }
        else if (D < 0) /* Casus irreducibilis: three real solutions */
        {
            double phi = 1.0 / 3 * System.Math.Acos(-q / System.Math.Sqrt(-cb_p));
            double t = 2 * System.Math.Sqrt(-p);

            s0 = t * System.Math.Cos(phi);
            s1 = -t * System.Math.Cos(phi + System.Math.PI / 3);
            s2 = -t * System.Math.Cos(phi - System.Math.PI / 3);
            num = 3;
        }
        else /* one real solution */
        {
            double sqrt_D = System.Math.Sqrt(D);
            double u = GetCubicRoot(sqrt_D - q);
            double v = -GetCubicRoot(sqrt_D + q);

            s0 = u + v;
            num = 1;
        }

        /* resubstitute */
        sub = 1.0 / 3 * A;

        if (num > 0) s0 -= sub;
        if (num > 1) s1 -= sub;
        if (num > 2) s2 -= sub;

        return num;
    }

    // Solve quartic function: c0*X^4 + c1*X^3 + c2*X^2 + c3*X + c4. 
    // Returns number of solutions.
    public static int SolveQuartic(double c0, double c1, double c2, double c3, double c4, out double s0, out double s1,
        out double s2, out double s3)
    {
        s0 = double.NaN;
        s1 = double.NaN;
        s2 = double.NaN;
        s3 = double.NaN;

        Span<double> coeffs = stackalloc double[4];
        double Z, u, v, sub;
        double A, B, C, D;
        double sq_A, p, q, r;
        int num;

        /* normal form: X^4 + Ax^3 + Bx^2 + Cx + D = 0 */
        A = c1 / c0;
        B = c2 / c0;
        C = c3 / c0;
        D = c4 / c0;

        /*  substitute X = Y - A/4 to eliminate cubic term: X^4 + px^2 + qx + r = 0 */
        sq_A = A * A;
        p = -3.0 / 8 * sq_A + B;
        q = 1.0 / 8 * sq_A * A - 1.0 / 2 * A * B + C;
        r = -3.0 / 256 * sq_A * sq_A + 1.0 / 16 * sq_A * B - 1.0 / 4 * A * C + D;

        if (IsZero(r))
        {
            /* no absolute term: Y(Y^3 + py + q) = 0 */

            coeffs[3] = q;
            coeffs[2] = p;
            coeffs[1] = 0;
            coeffs[0] = 1;

            num = BallisticSolver.SolveCubic(coeffs[0], coeffs[1], coeffs[2], coeffs[3], out s0, out s1, out s2);
        }
        else
        {
            /* solve the resolvent cubic ... */
            coeffs[3] = 1.0 / 2 * r * p - 1.0 / 8 * q * q;
            coeffs[2] = -r;
            coeffs[1] = -1.0 / 2 * p;
            coeffs[0] = 1;

            SolveCubic(coeffs[0], coeffs[1], coeffs[2], coeffs[3], out s0, out s1, out s2);

            /* ... and take the one real solution ... */
            Z = s0;

            /* ... to build two quadric equations */
            u = Z * Z - r;
            v = 2 * Z - p;

            if (IsZero(u))
                u = 0;
            else if (u > 0)
                u = System.Math.Sqrt(u);
            else
                return 0;

            if (IsZero(v))
                v = 0;
            else if (v > 0)
                v = System.Math.Sqrt(v);
            else
                return 0;

            coeffs[2] = Z - u;
            coeffs[1] = q < 0 ? -v : v;
            coeffs[0] = 1;

            num = BallisticSolver.SolveQuadric(coeffs[0], coeffs[1], coeffs[2], out s0, out s1);

            coeffs[2] = Z + u;
            coeffs[1] = q < 0 ? v : -v;
            coeffs[0] = 1;

            if (num == 0) num += BallisticSolver.SolveQuadric(coeffs[0], coeffs[1], coeffs[2], out s0, out s1);
            else if (num == 1) num += BallisticSolver.SolveQuadric(coeffs[0], coeffs[1], coeffs[2], out s1, out s2);
            else if (num == 2) num += BallisticSolver.SolveQuadric(coeffs[0], coeffs[1], coeffs[2], out s2, out s3);
        }

        /* resubstitute */
        sub = 1.0 / 4 * A;

        if (num > 0) s0 -= sub;
        if (num > 1) s1 -= sub;
        if (num > 2) s2 -= sub;
        if (num > 3) s3 -= sub;

        return num;
    }



    public static int solve_ballistic_arc(Vector3 proj_pos, float proj_speed, Vector3 target, float gravity,
        out Vector3 s0, out Vector3 s1)
    {
        // Handling these cases is up to your project's coding standards
        Debug.Assert(proj_pos != target && proj_speed > 0 && gravity > 0,
            "fts.solve_ballistic_arc called with invalid data");

        // C# requires out variables be set
        s0 = Vector3.Zero;
        s1 = Vector3.Zero;

        // Derivation
        //   (1) X = v*t*cos O
        //   (2) Y = v*t*sin O - .5*g*t^2
        // 
        //   (3) t = X/(cos O*v)                                        [solve t from (1)]
        //   (4) Y = v*X*sin O/(cos O * v) - .5*g*X^2/(cos^2 O*v^2)     [plug t into Y=...]
        //   (5) Y = X*tan O - g*X^2/(2*v^2*cos^2 O)                    [reduce; cos/sin = tan]
        //   (6) Y = X*tan O - (g*X^2/(2*v^2))*(1+tan^2 O)              [reduce; 1+tan O = 1/cos^2 O]
        //   (7) 0 = ((-g*X^2)/(2*v^2))*tan^2 O + X*tan O - (g*X^2)/(2*v^2) - Y    [re-arrange]
        //   Quadratic! a*p^2 + b*p + c where p = tan O
        //
        //   (8) let gxv = -g*X*X/(2*v*v)
        //   (9) p = (-X +- sqrt(X*X - 4gxv*(gxv - Y)))/2*gxv           [quadratic formula]
        //   (10) p = (v^2 +- sqrt(v^4 - g(g*X^2 + 2*Y*v^2)))/gx        [multiply top/bottom by -2*v*v/X; move 4*v^4/X^2 into root]
        //   (11) O = atan(p)

        Vector3 diff = target - proj_pos;
        Vector3 diffXZ = new Vector3(diff.X, 0f, diff.Z);
        float groundDist = diffXZ.Magnitude;

        float speed2 = proj_speed * proj_speed;
        float speed4 = proj_speed * proj_speed * proj_speed * proj_speed;
        float Y = (float)diff.Y;
        float X = groundDist;
        float gx = gravity * X;

        float root = speed4 - gravity * (gravity * X * X + 2 * Y * speed2);

        // No solution
        if (root < 0)
            return 0;

        root = MathF.Sqrt(root);

        float lowAng = MathF.Atan2(speed2 - root, gx);
        float highAng = MathF.Atan2(speed2 + root, gx);
        int numSolutions = lowAng != highAng ? 2 : 1;

        Vector3 groundDir = diffXZ.Normalize();
        s0 = groundDir * MathF.Cos(lowAng) * proj_speed + Vector3.Up * MathF.Sin(lowAng) * proj_speed;
        if (numSolutions > 1)
            s1 = groundDir * MathF.Cos(highAng) * proj_speed + Vector3.Up * MathF.Sin(highAng) * proj_speed;

        return numSolutions;
    }

    
    public static int solve_ballistic_arc(Vector3 proj_pos, float proj_speed, Vector3 target_pos,
        Vector3 target_velocity, float gravity, out Vector3 s0, out Vector3 s1)
    {
        // Initialize output parameters
        s0 = Vector3.Zero;
        s1 = Vector3.Zero;


        double G = gravity;

        double A = proj_pos.X;
        double B = proj_pos.Y;
        double C = proj_pos.Z;
        double M = target_pos.X;
        double N = target_pos.Y;
        double O = target_pos.Z;
        double P = target_velocity.X;
        double Q = target_velocity.Y;
        double R = target_velocity.Z;
        double S = proj_speed;

        double H = M - A;
        double J = O - C;
        double K = N - B;
        double L = -.5f * G;

        // Quartic Coeffecients
        double c0 = L * L;
        double c1 = -2 * Q * L;
        double c2 = Q * Q - 2 * K * L - S * S + P * P + R * R;
        double c3 = 2 * K * Q + 2 * H * P + 2 * J * R;
        double c4 = K * K + H * H + J * J;

        // Solve quartic
        Span<double> times = stackalloc double[4];
        int numTimes = SolveQuartic(c0, c1, c2, c3, c4, out times[0], out times[1], out times[2], out times[3]);
       
        
        
        times.Sort();
        // Plug quartic solutions into base equations
        // There should never be more than 2 positive, real roots.
        Span<Vector3> solutions = stackalloc Vector3[2];
        int numSolutions = 0;

        for (int i = 0; i < times.Length && numSolutions < 2; ++i)
        {
            double t = times[i];
            if (t <= 0 || double.IsNaN(t))
                continue;

            solutions[numSolutions].X = (float)((H + P * t) / t);
            solutions[numSolutions].Y = (float)((K + Q * t - L * t * t) / t);
            solutions[numSolutions].Z = (float)((J + R * t) / t);
            ++numSolutions;
        }

        // Write out solutions
        if (numSolutions > 0) s0 = solutions[0];
        if (numSolutions > 1) s1 = solutions[1];

        return numSolutions;
    }


}