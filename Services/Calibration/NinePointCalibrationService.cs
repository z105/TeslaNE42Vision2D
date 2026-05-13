using System;
using System.Collections.Generic;
using TeslaNE42Vision2D.Entity;
using TeslaNE42Vision2D.Utils;

namespace TeslaNE42Vision2D.Services.Calibration
{
    /// <summary>
    /// 九点标定服务，使用最小二乘法计算仿射变换矩阵。
    /// X_world = a*X_pix + b*Y_pix + c
    /// Y_world = d*X_pix + e*Y_pix + f
    /// </summary>
    public class NinePointCalibrationService
    {
        private double[] _affineMatrix = new double[6]; // [a, b, c, d, e, f]
        public bool IsCalibrated { get; private set; } = false;
        public List<CalibrationPoint> Points { get; } = new List<CalibrationPoint>();

        public void AddPoint(double pixelX, double pixelY, double physicalX, double physicalY)
        {
            Points.Add(new CalibrationPoint
            {
                PixelX = pixelX,
                PixelY = pixelY,
                PhysicalX = physicalX,
                PhysicalY = physicalY,
            });
        }

        public void ClearPoints()
        {
            Points.Clear();
            IsCalibrated = false;
        }

        /// <summary>
        /// 用当前采集的点计算仿射矩阵（最少3点，推荐9点）
        /// </summary>
        public bool Calibrate()
        {
            if (Points.Count < 3)
            {
                LogHelper.Warning("标定点数量不足，至少需要3个点");
                return false;
            }

            try
            {
                // 构建 Ax = b 最小二乘系统
                // 每个点贡献2行：
                // [Xpix Ypix 1  0    0   0] [a]   [Xworld]
                // [0    0   0  Xpix Ypix 1] [b] = [Yworld]
                //                           [c]
                //                           [d]
                //                           [e]
                //                           [f]
                int n = Points.Count;
                double[,] A = new double[2 * n, 6];
                double[] b = new double[2 * n];

                for (int i = 0; i < n; i++)
                {
                    double px = Points[i].PixelX;
                    double py = Points[i].PixelY;
                    double wx = Points[i].PhysicalX;
                    double wy = Points[i].PhysicalY;

                    // 第 2i 行（X方向）
                    A[2 * i, 0] = px; A[2 * i, 1] = py; A[2 * i, 2] = 1;
                    A[2 * i, 3] = 0;  A[2 * i, 4] = 0;  A[2 * i, 5] = 0;
                    b[2 * i] = wx;

                    // 第 2i+1 行（Y方向）
                    A[2 * i + 1, 0] = 0; A[2 * i + 1, 1] = 0; A[2 * i + 1, 2] = 0;
                    A[2 * i + 1, 3] = px; A[2 * i + 1, 4] = py; A[2 * i + 1, 5] = 1;
                    b[2 * i + 1] = wy;
                }

                // 最小二乘：x = (A^T A)^{-1} A^T b
                _affineMatrix = SolveLeastSquares(A, b, 2 * n, 6);
                IsCalibrated = true;
                LogHelper.Info($"标定完成，使用 {n} 个点");
                return true;
            }
            catch (Exception ex)
            {
                LogHelper.Error("标定计算失败", ex);
                return false;
            }
        }

        /// <summary>
        /// 将像素坐标转换为物理坐标
        /// </summary>
        public (double PhysicalX, double PhysicalY) Transform(double pixelX, double pixelY)
        {
            if (!IsCalibrated)
                throw new InvalidOperationException("尚未完成标定");

            double a = _affineMatrix[0], b = _affineMatrix[1], c = _affineMatrix[2];
            double d = _affineMatrix[3], e = _affineMatrix[4], f = _affineMatrix[5];

            double wx = a * pixelX + b * pixelY + c;
            double wy = d * pixelX + e * pixelY + f;
            return (wx, wy);
        }

        public void LoadFromConfig(CalibConfig config)
        {
            if (config == null) return;
            Points.Clear();
            if (config.Points != null)
                Points.AddRange(config.Points);
            if (config.IsCalibrated && config.AffineMatrix != null && config.AffineMatrix.Length == 6)
            {
                _affineMatrix = config.AffineMatrix;
                IsCalibrated = true;
            }
        }

        public CalibConfig ToConfig()
        {
            return new CalibConfig
            {
                Points = new List<CalibrationPoint>(Points),
                AffineMatrix = _affineMatrix,
                IsCalibrated = IsCalibrated,
            };
        }

        // 高斯-约当法求解最小二乘 (A^T A) x = A^T b
        private double[] SolveLeastSquares(double[,] A, double[] b, int rows, int cols)
        {
            // 构建法方程 AtA * x = Atb
            double[,] AtA = new double[cols, cols];
            double[] Atb = new double[cols];

            for (int i = 0; i < cols; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    double sum = 0;
                    for (int k = 0; k < rows; k++)
                        sum += A[k, i] * A[k, j];
                    AtA[i, j] = sum;
                }
                double sumB = 0;
                for (int k = 0; k < rows; k++)
                    sumB += A[k, i] * b[k];
                Atb[i] = sumB;
            }

            // 高斯消元 + 回代
            return GaussianElimination(AtA, Atb, cols);
        }

        private double[] GaussianElimination(double[,] A, double[] b, int n)
        {
            // 增广矩阵 [A | b]
            double[,] M = new double[n, n + 1];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++) M[i, j] = A[i, j];
                M[i, n] = b[i];
            }

            for (int col = 0; col < n; col++)
            {
                // 选主元
                int maxRow = col;
                for (int row = col + 1; row < n; row++)
                    if (Math.Abs(M[row, col]) > Math.Abs(M[maxRow, col]))
                        maxRow = row;

                // 交换行
                for (int j = 0; j <= n; j++)
                {
                    double tmp = M[col, j];
                    M[col, j] = M[maxRow, j];
                    M[maxRow, j] = tmp;
                }

                if (Math.Abs(M[col, col]) < 1e-10)
                    throw new InvalidOperationException("矩阵奇异，无法求解");

                double pivot = M[col, col];
                for (int j = col; j <= n; j++) M[col, j] /= pivot;

                for (int row = 0; row < n; row++)
                {
                    if (row == col) continue;
                    double factor = M[row, col];
                    for (int j = col; j <= n; j++)
                        M[row, j] -= factor * M[col, j];
                }
            }

            double[] x = new double[n];
            for (int i = 0; i < n; i++) x[i] = M[i, n];
            return x;
        }
    }
}
