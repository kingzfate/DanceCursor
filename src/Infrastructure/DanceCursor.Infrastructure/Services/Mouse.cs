﻿using DanceCursor.Domain.Abstractions;
using Scrutor.AspNetCore;
using System.Runtime.InteropServices;
using System.Windows;

namespace DanceCursor.Infrastructure.Services
{
    public class Mouse : IMouse, ISingletonLifetime
    {
        PeriodicTimer timer;

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool GetCursorPos(ref Win32Point pt);

        [DllImport("User32.dll")]
        private static extern bool SetCursorPos(int X, int Y);

        [StructLayout(LayoutKind.Sequential)]
        internal struct Win32Point
        {
            public int X;
            public int Y;
        };

        private void SetPosition(int a, int b)
        {
            SetCursorPos(a, b);
        }

        public static Point GetMousePosition()
        {
            Win32Point w32Mouse = new();
            GetCursorPos(ref w32Mouse);

            return new Point(w32Mouse.X, w32Mouse.Y);
        }

        public void Move()
        {
            timer = new(TimeSpan.FromMilliseconds(5000));
            bool isDown = true;

            Task statisticsUploader = PeriodicAsync(async () =>
            {
                try
                {
                    UploadStatisticsAsync(isDown);
                    isDown = isDown ? false : true;
                }
                catch (Exception ex)
                {
                    // Log the exception
                }
            }, TimeSpan.FromMilliseconds(5000));
        }

        public void SmartMove()
        {
            timer = new(TimeSpan.FromMilliseconds(5000));
            bool isDown = true;
            Point previousPoint = GetMousePosition();
            Point currentPoint;
            bool isMoved;

            Task statisticsUploader = PeriodicAsync(async () =>
            {
                currentPoint = GetMousePosition();
                if (previousPoint == currentPoint)
                {
                    UploadStatisticsAsync(isDown);
                    isDown = isDown ? false : true;
                }
                else
                {
                    previousPoint = currentPoint;
                    currentPoint = GetMousePosition();
                }
            }, TimeSpan.FromMinutes(1));
        }

        public async Task PeriodicAsync(Func<Task> action, TimeSpan interval, CancellationToken cancellationToken = default)

        {
            using PeriodicTimer timer = new(interval);
            while (true)
            {
                await action();
                await timer.WaitForNextTickAsync(cancellationToken);
            }
        }

        public void UploadStatisticsAsync(bool isDown)
        {
            Point a = GetMousePosition();
            if (isDown)
            {
                SetPosition(Convert.ToInt32(a.X - 50), Convert.ToInt32(a.Y - 50));
            }
            else
            {
                SetPosition(Convert.ToInt32(a.X + 50), Convert.ToInt32(a.Y + 50));
            }
        }
    }
}