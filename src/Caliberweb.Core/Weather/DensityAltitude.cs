/*
 * Copyright 1998-2010  All Rights Reserved Richard Shelquist,  January 1998 - July 2010
 * 
 * This calculator is the copyrighted intellectual property of Richard Shelquist, Shelquist Engineering.
 * 
 * This calculator may be freely used for an individual's personal use via my web site,
 * or by means of one copy on an individual's own computer for that person's personal use,
 * but may not be copied or republished in any form on any web site, bulletin board or
 * any other means.
 */
using System;

namespace Caliberweb.Core.Weather
{
    public static class DensityAltitude
    {
        /// <summary>
        /// Calculate the ISA altitude (meters) for a given density (kg/m3)
        /// </summary>
        /// <param name="d">The d.</param>
        /// <returns></returns>
        private static double CalcAltitude(double d)
        {
            const double G = 9.80665;
            const double PO = 101325;
            const double TO = 288.15;
            const double L = 6.5;
            const double R = 8.314320;
            const double M = 28.9644;

            double d1 = d*1000;

            double p2 = ((L*R)/(G*M - L*R))*Math.Log((R*TO*d1)/(M*PO));

            double h = -(TO/L)*(Math.Exp(p2) - 1);

            return (h*1000);
        }

        /// <summary>
        /// Calculate the actual pressure (mb)from the altimeter setting (mb) and geopotential altitude (m).
        /// </summary>
        /// <param name="as">The @as.</param>
        /// <param name="h">The h.</param>
        /// <returns></returns>
        private static double CalcAs2Press(double @as, double h)
        {
            const double K1 = 0.190263;
            const double K2 = 8.417286E-5;

            double p = Math.Pow((Math.Pow(@as, K1) - (K2*h)), (1/K1));

            return (p);
        }

        /// <summary>
        /// Calculate the air density in kg/m3
        /// </summary>
        /// <param name="abspressmb">The abspressmb.</param>
        /// <param name="e">The e.</param>
        /// <param name="tc">The tc.</param>
        /// <returns></returns>
        private static double CalcDensity(double abspressmb, double e, double tc)
        {
            const double RV = 461.4964;
            const double RD = 287.0531;

            double tk = tc + 273.15;
            double pv = e*100;
            double pd = (abspressmb - e)*100;
            double d = (pv/(RV*tk)) + (pd/(RD*tk));
            return (d);
        }

        /// <summary>
        /// Calculate the H altitude (meters), given the Z altitide (meters)
        /// </summary>
        /// <param name="z">The z.</param>
        /// <returns></returns>
        private static double CalcH(double z)
        {
            const double R = 6369E3;

            return ((R*z)/(R + z));
        }

        /// <summary>
        /// Calculate the saturation vapor pressure given the temperature(celsius)
        /// </summary>
        /// <param name="t">The t.</param>
        /// <returns></returns>
        private static double CalcVaporPressureWobus(double t)
        {
            const double ESO = 6.1078;
            const double C0 = 0.99999683;
            const double C1 = -0.90826951E-02;
            const double C2 = 0.78736169E-04;
            const double C3 = -0.61117958E-06;
            const double C4 = 0.43884187E-08;
            const double C5 = -0.29883885E-10;
            const double C6 = 0.21874425E-12;
            const double C7 = -0.17892321E-14;
            const double C8 = 0.11112018E-16;
            const double C9 = -0.30994571E-19;

            double pol = C0 + t*(C1 + t*(C2 + t*(C3 + t*(C4 + t*(C5 + t*(C6 + t*(C7 + t*(C8 + t*(C9)))))))));

            double es = ESO/Math.Pow(pol, 8);

            return (es);
        }

        /// <summary>
        /// Calculate the Z altitude (meters), given the H altitide (meters)
        /// </summary>
        /// <param name="h">The h.</param>
        /// <returns></returns>
        private static double CalcZ(double h)
        {
            const double R = 6369E3;

            return ((R*h)/(R - h));
        }

        public static double Calculate(double elevation, Atmosphere atmosphere)
        {
            double temperature = atmosphere.Temperature;
            double pressure = atmosphere.Pressure;
            double humidity = atmosphere.Humidity;

            // Process the input values
            double tf = 1.0*temperature; // temperature, deg F
            humidity = 1.0*humidity; // relative humidity, %
            pressure = 1.0*pressure; // altimeter setting, in-Hg
            double z = 1.0*elevation; // geometric elevation, feet

            // Convert to metric units
            const double MB_PER_IN = 33.86389;
            const double M_PER_FT = 0.304800;
            double tc = (5.0/9.0)*(tf - 32);
            double altsetmb = pressure*MB_PER_IN;
            double zm = z*M_PER_FT;

            // Calculate the vapor pressures (mb) given the ambient temperature (c) and relative humidity (c)
            double esmb = CalcVaporPressureWobus(tc);
            double emb = esmb*humidity/100;

            // Calculate geopotential altitude H (m) from geometric altitude (m) Z

            double hm = CalcH(zm);

            // Calculate the absolute pressure given the altimeter setting(mb) and geopotential elevation(meters)

            double actpressmb = CalcAs2Press(altsetmb, hm);

            // Calculate the air density (kg/m3) from absolute pressure (mb) vapor pressure (mb) and temp (c)

            double density = CalcDensity(actpressmb, emb, tc);
//            double relden = 100*(density/1.225);

            // Calculate the geopotential altitude (m) in ISA with that same density (kg/m3)

            double densaltm = CalcAltitude(density);

            // Calculate geometric altitude Z (m) from geopotential altitude (m) H

            double densaltzm = CalcZ(densaltm);

            // Convert Units for output

//            const double IN_PER_MB = (1/33.86389);
//            double actpress = actpressmb*IN_PER_MB;
            const double FT_PER_M = (1/0.304800);
            double densalt = densaltzm*FT_PER_M;

            return RoundNum(densalt, 0);
        }

        private static double RoundNum(double num, int places)
        {
            double rounder = Math.Pow(10, places);
            return Math.Round(num*rounder)/rounder;
        }
    }
}