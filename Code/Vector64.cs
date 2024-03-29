/*

Author: Dan Lodholm (github: orbitusii)
Copyright: 2022

*/

using System;
using UnityEngine;

[System.Serializable]
public struct Vector64
{
    public double x, y, z;

    public Vector64(double x, double y, double z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public static Vector64 zero
    {
        get
        {
            // Tuples are fun
            return (0, 0, 0);
        }
    }

    public double magnitude
    {
        get
        {
            return zero.Distance(this);
        }
    }

    public Vector64 normalized
    {
        get
        {
            return this / magnitude;
        }
    }

    public Vector3 normalizedVector3
    {
        get
        {
            return (Vector3)normalized;
        }
    }

    public static double Distance(Vector64 to, Vector64 from)
    {
        return from.Distance(to);
    }

    public double Distance(Vector64 to)
    {
        double sqrMag = (to - this).SquareMagnitude;
        return Math.Sqrt(sqrMag);
    }

    public double SquareMagnitude
    {
        get
        {
            double sqrMag = x * x + y * y + z * z;
            return sqrMag;
        }
    }

    public float AngleBetween(Vector64 to)
    {
        Vector3 v1 = this.normalizedVector3;
        Vector3 v2 = to.normalizedVector3;

        return Vector3.Angle(v1, v2);
    }

    public static Vector64 WeightedAverage(Vector64[] vectors, double[] weights)
    {
        if (weights.Length != vectors.Length)
        {
            double[] fixedWeights = new double[vectors.Length];
            for (int i = 0; i < vectors.Length; i++)
            {
                try
                {
                    fixedWeights[i] = weights[i];
                }
                catch (ArgumentOutOfRangeException)
                {
                    fixedWeights[i] = 1;
                }
            }

            weights = fixedWeights;
        }

        Vector64 totalVec = Vector64.zero;
        double sum = 0;

        for (int i = 0; i < vectors.Length; i++)
        {
            totalVec += vectors[i] * weights[i];
            sum += weights[i];
        }

        return totalVec / sum;
    }

    /// <summary>
    /// Shifts ALL THREE AXES by amount passed in.
    /// e.g. (5, 5, 3).ShiftAll(-2) returns (3, 3, 1)
    /// </summary>
    /// <param name="amount"></param>
    /// <returns>A shifted copy of the original vector (DOES NOT AFFECT THE ORIGINAL VECTOR!)</returns>
    public Vector64 ShiftAll(double amount)
    {
        Vector64 copy = this;
        for (int i = 0; i <= 2; i++)
        {
            copy[i] += amount;
        }
        return copy;
    }

    /// <summary>
    /// Returns the maximum value along each axis.
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns></returns>
    public static Vector64 MaxComposite(Vector64 left, Vector64 right)
    {
        return (
            Math.Max(left.x, right.x),
            Math.Max(left.y, right.y),
            Math.Max(left.z, right.z)
        );
    }

    public static Vector64 MaxComposite(Vector64[] vectors)
    {
        Vector64 maxAll = vectors[0];

        for (int i = 1; i < vectors.Length; i++)
        {
            maxAll = MaxComposite(maxAll, vectors[i]);
        }

        return maxAll;
    }

    /// <summary>
    /// Returns the minimum value along each axis.
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns></returns>
    public static Vector64 MinComposite(Vector64 left, Vector64 right)
    {
        return (
            Math.Min(left.x, right.x),
            Math.Min(left.y, right.y),
            Math.Min(left.z, right.z)
        );
    }
    public static Vector64 MinComposite(Vector64[] vectors)
    {
        Vector64 minAll = vectors[0];

        for (int i = 1; i < vectors.Length; i++)
        {
            minAll = MinComposite(minAll, vectors[i]);
        }

        return minAll;
    }
    public static Vector64 operator +(Vector64 left, Vector64 right)
    {
        return new Vector64(left.x + right.x, left.y + right.y, left.z + right.z);
    }

    public static Vector64 operator -(Vector64 left, Vector64 right)
    {
        return new Vector64(left.x - right.x, left.y - right.y, left.z - right.z);
    }

    public static Vector64 operator *(Vector64 left, double right)
    {
        return new Vector64(left.x * right, left.y * right, left.z * right);
    }

    public static Vector64 operator /(Vector64 left, double right)
    {
        return new Vector64(left.x / right, left.y / right, left.z / right);
    }
    
    // Negative sign operator, duh?
    public static Vector64 operator -(Vector64 vec)
    {
        return new Vector64(-vec.x, -vec.y, -vec.z);
    }

    public static explicit operator Vector3(Vector64 vec)
    {
        return new Vector3((float)vec.x, (float)vec.y, (float)vec.z);
    }

    public static implicit operator Vector64(Vector3 vec)
    {
        return new Vector64(vec.x, vec.y, vec.z);
    }


    /// <summary>
    /// Implicit cast from a tuple of doubles to make code look clean?
    /// </summary>
    /// <param name="tuple"></param>
    public static implicit operator Vector64 ((double x, double y, double z) tuple)
    {
        return new Vector64(tuple.x, tuple.y, tuple.z);
    }

    public double this[int i]
    {
        get
        {
            switch(i)
            {
                case 0:
                    return x;
                case 1:
                    return y;
                case 2:
                    return z;
                default:
                    throw new IndexOutOfRangeException("Attempted to get a nonexistent value from a Vector64!");
            }
        }
        set
        {
            switch (i)
            {
                case 0:
                    x = value;
                    return;
                case 1:
                    y = value;
                    return;
                case 2:
                    z = value;
                    return;
                default:
                    throw new IndexOutOfRangeException("Attempted to set a nonexistent value from a Vector64!");
            }
        }
    }

    public override string ToString()
    {
        return $"V64({Math.Round(x, 3)}, {Math.Round(y, 3)}, {Math.Round(z, 3)})";
    }
}
