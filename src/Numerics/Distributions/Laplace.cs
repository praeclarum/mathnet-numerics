﻿// <copyright file="Laplace.cs" company="Math.NET">
// Math.NET Numerics, part of the Math.NET Project
// http://numerics.mathdotnet.com
// http://github.com/mathnet/mathnet-numerics
// http://mathnetnumerics.codeplex.com
//
// Copyright (c) 2009-2013 Math.NET
//
// Permission is hereby granted, free of charge, to any person
// obtaining a copy of this software and associated documentation
// files (the "Software"), to deal in the Software without
// restriction, including without limitation the rights to use,
// copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following
// conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.
// </copyright>

using System;
using System.Collections.Generic;
using MathNet.Numerics.Properties;
using MathNet.Numerics.Random;

namespace MathNet.Numerics.Distributions
{
    /// <summary>
    /// Continuous Univariate Laplace distribution.
    /// The Laplace distribution is a distribution over the real numbers parameterized by a mean and
    /// scale parameter. The PDF is:
    ///     p(x) = \frac{1}{2 * scale} \exp{- |x - mean| / scale}.
    /// <a href="http://en.wikipedia.org/wiki/Laplace_distribution">Wikipedia - Laplace distribution</a>.
    /// </summary>
    /// <remarks>The distribution will use the <see cref="System.Random"/> by default. 
    /// <para>Users can set the random number generator by using the <see cref="RandomSource"/> property.</para>
    /// <para>The statistics classes will check all the incoming parameters whether they are in the allowed
    /// range. This might involve heavy computation. Optionally, by setting Control.CheckDistributionParameters
    /// to <c>false</c>, all parameter checks can be turned off.</para></remarks>
    public class Laplace : IContinuousDistribution
    {
        System.Random _random;

        double _location;
        double _scale;

        /// <summary>
        /// Initializes a new instance of the <see cref="Laplace"/> class (location = 0, scale = 1). 
        /// </summary>
        public Laplace()
            : this(0.0, 1.0)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Laplace"/> class. 
        /// </summary>
        /// <param name="location">The location (μ) of the distribution.</param>
        /// <param name="scale">The scale (b) of the distribution. Range: b > 0.</param>
        /// <exception cref="ArgumentException">If <paramref name="scale"/> is negative.</exception>
        public Laplace(double location, double scale)
        {
            _random = SystemRandomSource.Default;
            SetParameters(location, scale);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Laplace"/> class. 
        /// </summary>
        /// <param name="location">The location (μ) of the distribution.</param>
        /// <param name="scale">The scale (b) of the distribution. Range: b > 0.</param>
        /// <param name="randomSource">The random number generator which is used to draw random samples.</param>
        /// <exception cref="ArgumentException">If <paramref name="scale"/> is negative.</exception>
        public Laplace(double location, double scale, System.Random randomSource)
        {
            _random = randomSource ?? SystemRandomSource.Default;
            SetParameters(location, scale);
        }

        /// <summary>
        /// A string representation of the distribution.
        /// </summary>
        /// <returns>a string representation of the distribution.</returns>
        public override string ToString()
        {
            return "Laplace(μ = " + _location + ", b = " + _scale + ")";
        }

        /// <summary>
        /// Sets the parameters of the distribution after checking their validity.
        /// </summary>
        /// <param name="location">The location (μ) of the distribution.</param>
        /// <param name="scale">The scale (b) of the distribution. Range: b > 0.</param>
        /// <exception cref="ArgumentOutOfRangeException">When the parameters are out of range.</exception>
        void SetParameters(double location, double scale)
        {
            if (scale <= 0.0 || Double.IsNaN(location) || Double.IsNaN(scale))
            {
                throw new ArgumentOutOfRangeException(Resources.InvalidDistributionParameters);
            }

            _location = location;
            _scale = scale;
        }

        /// <summary>
        /// Gets or sets the location (μ) of the Laplace distribution.
        /// </summary>
        public double Location
        {
            get { return _location; }
            set { SetParameters(value, _scale); }
        }

        /// <summary>
        /// Gets or sets the scale (b) of the Laplace distribution. Range: b > 0.
        /// </summary>
        public double Scale
        {
            get { return _scale; }
            set { SetParameters(_location, value); }
        }

        /// <summary>
        /// Gets or sets the random number generator which is used to draw random samples.
        /// </summary>
        public System.Random RandomSource
        {
            get { return _random; }
            set { _random = value ?? SystemRandomSource.Default; }
        }

        /// <summary>
        /// Gets the mean of the distribution.
        /// </summary>
        public double Mean
        {
            get { return _location; }
        }

        /// <summary>
        /// Gets the variance of the distribution.
        /// </summary>
        public double Variance
        {
            get { return 2.0*_scale*_scale; }
        }

        /// <summary>
        /// Gets the standard deviation of the distribution.
        /// </summary>
        public double StdDev
        {
            get { return Constants.Sqrt2*_scale; }
        }

        /// <summary>
        /// Gets the entropy of the distribution.
        /// </summary>
        public double Entropy
        {
            get { return Math.Log(2.0*Constants.E*_scale); }
        }

        /// <summary>
        /// Gets the skewness of the distribution.
        /// </summary>
        public double Skewness
        {
            get { return 0.0; }
        }

        /// <summary>
        /// Gets the mode of the distribution.
        /// </summary>
        public double Mode
        {
            get { return _location; }
        }

        /// <summary>
        /// Gets the median of the distribution.
        /// </summary>
        public double Median
        {
            get { return _location; }
        }

        /// <summary>
        /// Gets the minimum of the distribution.
        /// </summary>
        public double Minimum
        {
            get { return Double.NegativeInfinity; }
        }

        /// <summary>
        /// Gets the maximum of the distribution.
        /// </summary>
        public double Maximum
        {
            get { return Double.PositiveInfinity; }
        }

        /// <summary>
        /// Computes the probability density of the distribution (PDF) at x, i.e. ∂P(X ≤ x)/∂x.
        /// </summary>
        /// <param name="x">The location at which to compute the density.</param>
        /// <returns>the density at <paramref name="x"/>.</returns>
        /// <seealso cref="PDF"/>
        public double Density(double x)
        {
            return Math.Exp(-Math.Abs(x - _location)/_scale)/(2.0*_scale);
        }

        /// <summary>
        /// Computes the log probability density of the distribution (lnPDF) at x, i.e. ln(∂P(X ≤ x)/∂x).
        /// </summary>
        /// <param name="x">The location at which to compute the log density.</param>
        /// <returns>the log density at <paramref name="x"/>.</returns>
        /// <seealso cref="PDFLn"/>
        public double DensityLn(double x)
        {
            return -Math.Abs(x - _location)/_scale - Math.Log(2.0*_scale);
        }

        /// <summary>
        /// Computes the cumulative distribution (CDF) of the distribution at x, i.e. P(X ≤ x).
        /// </summary>
        /// <param name="x">The location at which to compute the cumulative distribution function.</param>
        /// <returns>the cumulative distribution at location <paramref name="x"/>.</returns>
        /// <seealso cref="CDF"/>
        public double CumulativeDistribution(double x)
        {
            return 0.5*(1.0 + (Math.Sign(x - _location)*(1.0 - Math.Exp(-Math.Abs(x - _location)/_scale))));
        }

        /// <summary>
        /// Samples a Laplace distributed random variable.
        /// </summary>
        /// <returns>a sample from the distribution.</returns>
        public double Sample()
        {
            return SampleUnchecked(_random, _location, _scale);
        }

        /// <summary>
        /// Generates a sample from the Laplace distribution.
        /// </summary>
        /// <returns>a sample from the distribution.</returns>
        public IEnumerable<double> Samples()
        {
            while (true)
            {
                yield return SampleUnchecked(_random, _location, _scale);
            }
        }

        /// <summary>
        /// Samples the distribution.
        /// </summary>
        /// <param name="rnd">The random number generator to use.</param>
        /// <param name="location">The location (μ) of the distribution.</param>
        /// <param name="scale">The scale (b) of the distribution. Range: b > 0.</param>
        /// <returns>a random number from the distribution.</returns>
        static double SampleUnchecked(System.Random rnd, double location, double scale)
        {
            var u = rnd.NextDouble() - 0.5;
            return location - (scale * Math.Sign(u) * Math.Log(1.0 - (2.0 * Math.Abs(u))));
        }

        /// <summary>
        /// Computes the probability density of the distribution (PDF) at x, i.e. ∂P(X ≤ x)/∂x.
        /// </summary>
        /// <param name="location">The location (μ) of the distribution.</param>
        /// <param name="scale">The scale (b) of the distribution. Range: b > 0.</param>
        /// <param name="x">The location at which to compute the density.</param>
        /// <returns>the density at <paramref name="x"/>.</returns>
        /// <seealso cref="Density"/>
        public static double PDF(double location, double scale, double x)
        {
            if (scale <= 0.0) throw new ArgumentOutOfRangeException(Resources.InvalidDistributionParameters);

            return Math.Exp(-Math.Abs(x - location)/scale)/(2.0*scale);
        }

        /// <summary>
        /// Computes the log probability density of the distribution (lnPDF) at x, i.e. ln(∂P(X ≤ x)/∂x).
        /// </summary>
        /// <param name="location">The location (μ) of the distribution.</param>
        /// <param name="scale">The scale (b) of the distribution. Range: b > 0.</param>
        /// <param name="x">The location at which to compute the density.</param>
        /// <returns>the log density at <paramref name="x"/>.</returns>
        /// <seealso cref="DensityLn"/>
        public static double PDFLn(double location, double scale, double x)
        {
            if (scale <= 0.0) throw new ArgumentOutOfRangeException(Resources.InvalidDistributionParameters);

            return -Math.Abs(x - location)/scale - Math.Log(2.0*scale);
        }

        /// <summary>
        /// Computes the cumulative distribution (CDF) of the distribution at x, i.e. P(X ≤ x).
        /// </summary>
        /// <param name="x">The location at which to compute the cumulative distribution function.</param>
        /// <param name="location">The location (μ) of the distribution.</param>
        /// <param name="scale">The scale (b) of the distribution. Range: b > 0.</param>
        /// <returns>the cumulative distribution at location <paramref name="x"/>.</returns>
        /// <seealso cref="CumulativeDistribution"/>
        public static double CDF(double location, double scale, double x)
        {
            if (scale <= 0.0) throw new ArgumentOutOfRangeException(Resources.InvalidDistributionParameters);

            return 0.5*(1.0 + (Math.Sign(x - location)*(1.0 - Math.Exp(-Math.Abs(x - location)/scale))));
        }

        /// <summary>
        /// Generates a sample from the distribution.
        /// </summary>
        /// <param name="rnd">The random number generator to use.</param>
        /// <param name="location">The location (μ) of the distribution.</param>
        /// <param name="scale">The scale (b) of the distribution. Range: b > 0.</param>
        /// <returns>a sample from the distribution.</returns>
        public static double Sample(System.Random rnd, double location, double scale)
        {
            if (scale <= 0.0) throw new ArgumentOutOfRangeException(Resources.InvalidDistributionParameters);

            return SampleUnchecked(rnd, location, scale);
        }

        /// <summary>
        /// Generates a sequence of samples from the distribution.
        /// </summary>
        /// <param name="rnd">The random number generator to use.</param>
        /// <param name="location">The location (μ) of the distribution.</param>
        /// <param name="scale">The scale (b) of the distribution. Range: b > 0.</param>
        /// <returns>a sequence of samples from the distribution.</returns>
        public static IEnumerable<double> Samples(System.Random rnd, double location, double scale)
        {
            if (scale <= 0.0) throw new ArgumentOutOfRangeException(Resources.InvalidDistributionParameters);

            while (true)
            {
                yield return SampleUnchecked(rnd, location, scale);
            }
        }
    }
}
