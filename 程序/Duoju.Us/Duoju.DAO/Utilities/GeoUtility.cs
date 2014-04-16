using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Duoju.DAO.Utilities
{
    /// <summary>
    /// <Author>jary.zhang</Author>
    /// <Date>2012.12.10</Date>
    /// 地理数据计算，包括球面距离，两geohash值间的球面距离，矩形边界点，过滤圆范围外的点等
    /// http://www.cnblogs.com/three-zone/
    /// </summary>
    public static class GeoUtility
    {
        private static Char[] _Digits = { '0', '1', '2', '3', '4', '5', '6', '7', '8',
                        '9', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'j', 'k', 'm', 'n', 'p',
                        'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' };
        //相邻块集合
        private static readonly string[][] Neighbors = {
            new[]{
                    "p0r21436x8zb9dcf5h7kjnmqesgutwvy", // Top
                    "bc01fg45238967deuvhjyznpkmstqrwx", // Right
                    "14365h7k9dcfesgujnmqp0r2twvyx8zb", // Bottom
                    "238967debc01fg45kmstqrwxuvhjyznp", // Left
            },
            new[]{
                    "bc01fg45238967deuvhjyznpkmstqrwx", // Top
                    "p0r21436x8zb9dcf5h7kjnmqesgutwvy", // Right
                    "238967debc01fg45kmstqrwxuvhjyznp", // Bottom
                    "14365h7k9dcfesgujnmqp0r2twvyx8zb", // Left
            }
        };
        //边界
        private static readonly string[][] Borders = {
            new[] {"prxz", "bcfguvyz", "028b", "0145hjnp"},
            new[] {"bcfguvyz", "prxz", "0145hjnp", "028b"}
        };

        //方位
        private const int Top = 0;
        private const int Right = 1;
        private const int Bottom = 2;
        private const int Left = 3;

        //以5位为一组，共11组，分组按照经纬度的精度-1
        private static int _NumberOfBits = 5 * 5;
        //对应表
        private static Dictionary<Char, Int32> _LookupTable = CreateLookup();

        /// <summary>
        /// 创建对应关系
        /// </summary>
        /// <returns></returns>
        private static Dictionary<Char, Int32> CreateLookup()
        {
            Dictionary<Char, Int32> result = new Dictionary<char, Int32>();
            Int32 i = 0;
            foreach (Char c in _Digits)
            {
                result[c] = i;
                i++;
            }
            return result;
        }

        /// <summary>
        /// 将二进制的GeoHash编码转换为double型
        /// </summary>
        /// <param name="bits"></param>
        /// <param name="floorValue"></param>
        /// <param name="ceilingValue"></param>
        /// <returns></returns>
        private static double GeoHashDecode(BitArray bits, double floorValue, double ceilingValue)
        {
            Double middle = 0;
            Double floor = floorValue;
            Double ceiling = ceilingValue;
            for (Int32 i = 0; i < bits.Length; i++)
            {
                middle = (floor + ceiling) / 2;
                if (bits[i])
                {
                    floor = middle;
                }
                else
                {
                    ceiling = middle;
                }
            }
            return middle;
        }

        /// <summary>
        /// 将经纬度转换为GeoHash的二进制编码
        /// </summary>
        /// <param name="value"></param>
        /// <param name="floorValue"></param>
        /// <param name="ceilingValue"></param>
        /// <returns></returns>
        private static BitArray GeoHashEncode(double value, double floorValue, double ceilingValue)
        {
            BitArray result = new BitArray(_NumberOfBits);
            Double floor = floorValue;
            Double ceiling = ceilingValue;
            for (Int32 i = 0; i < _NumberOfBits; i++)
            {
                Double middle = (floor + ceiling) / 2;
                if (value >= middle)
                {
                    result[i] = true;
                    floor = middle;
                }
                else
                {
                    result[i] = false;
                    ceiling = middle;
                }
            }
            return result;
        }

        /// <summary>
        /// 对字符串进行Base32编码
        /// </summary>
        /// <param name="binaryStringValue"></param>
        /// <returns></returns>
        private static String EncodeBase32(String binaryStringValue)
        {
            StringBuilder buffer = new StringBuilder();
            String binaryString = binaryStringValue;
            while (binaryString.Length > 0)
            {
                String currentBlock = binaryString.Substring(0, 5).PadLeft(5, '0');
                if (binaryString.Length > 5)
                {
                    binaryString = binaryString.Substring(5, binaryString.Length - 5);
                }
                else
                {
                    binaryString = String.Empty;
                }
                Int32 value = Convert.ToInt32(currentBlock, 2);
                buffer.Append(_Digits[value]);
            }

            String result = buffer.ToString();
            return result;
        }

        /// <summary>
        /// 将角度转换为弧度
        /// </summary>
        /// <param name="angle"></param>
        /// <returns></returns>
        private static double DegreeToRadian(double angle)
        {
            return Math.PI * angle / 180.0;
        }

        /// <summary>
        /// 将弧度转换为角度
        /// </summary>
        /// <param name="angle"></param>
        /// <returns></returns>
        private static double RadianToDegree(double angle)
        {
            return angle * (180.0 / Math.PI);
        }

        /// <summary>
        /// 解码GeoHash，将一串GeoHash解析为一个GeoPoint（经纬度对象）
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        internal static GeoPoint DecodeGeoHash(String value)
        {
            StringBuilder lBuffer = new StringBuilder();
            foreach (Char c in value)
            {
                if (!_LookupTable.ContainsKey(c))
                {
                    throw new ArgumentException("Invalid character " + c);
                }
                Int32 i = _LookupTable[c] + 32;
                lBuffer.Append(Convert.ToString(i, 2).Substring(1));
            }

            BitArray lonset = new BitArray(_NumberOfBits);
            BitArray latset = new BitArray(_NumberOfBits);

            //偶数位
            int j = 0;
            for (int i = 0; i < _NumberOfBits * 2; i += 2)
            {
                Boolean isSet = false;
                if (i < lBuffer.Length)
                {
                    isSet = lBuffer[i] == '1';
                }
                lonset[j] = isSet;
                j++;
            }

            //奇数位
            j = 0;
            for (int i = 1; i < _NumberOfBits * 2; i += 2)
            {
                Boolean isSet = false;
                if (i < lBuffer.Length)
                {
                    isSet = lBuffer[i] == '1';
                }
                latset[j] = isSet;
                j++;
            }

            double lLongitude = GeoHashDecode(lonset, -180, 180);
            double lLatitude = GeoHashDecode(latset, -90, 90);

            GeoPoint lResult = new GeoPoint(lLatitude, lLongitude);

            return lResult;
        }

        /// <summary>
        /// 将一组GeoPoint转换为GeoHash编码
        /// </summary>
        /// <param name="data"></param>
        /// <param name="accuracy"></param>
        /// <returns></returns>
        public static String EncodeGeoHash(GeoPoint data, Int32 accuracy)
        {
            BitArray latitudeBits = GeoHashEncode(data.Lat, -90, 90);
            BitArray longitudeBits = GeoHashEncode(data.Lng, -180, 180);
            StringBuilder buffer = new StringBuilder();
            for (Int32 i = 0; i < _NumberOfBits; i++)
            {
                buffer.Append((longitudeBits[i]) ? '1' : '0');
                buffer.Append((latitudeBits[i]) ? '1' : '0');
            }
            String binaryValue = buffer.ToString();
            String result = EncodeBase32(binaryValue);
            result = result.Substring(0, accuracy);
            return result;
        }

        /// <summary>
        /// 过滤不在以radius为半径的圆范围内的点
        /// </summary>
        /// <param name="pointList"></param>
        /// <param name="centerPoint"></param>
        /// <param name="radius"></param>
        /// <returns></returns>
        public static List<GeoPoint> FilterPointOutOfCycl(List<GeoPoint> pointList, GeoPoint centerPoint, double radius)
        {
            var points = new List<GeoPoint>();

            foreach (var point in pointList)
            {
                if (GetDistanceByLatLng(centerPoint, point) <= radius)
                {
                    points.Add(point);
                }
            }
            return points;
        }

        /// <summary>
        /// 过滤不在以radius为半径的圆范围内的点
        /// </summary>
        /// <param name="geohashList"></param>
        /// <param name="centerGeoHash"></param>
        /// <param name="radius"></param>
        /// <returns></returns>
        internal static List<string> FilterGeoHashOutOfCycl(List<string> geohashList, string centerGeoHash, double radius)
        {
            foreach (var geohash in geohashList)
            {
                if (GetDistanceByGeoHash(centerGeoHash, geohash) > radius)
                {
                    geohashList.Remove(geohash);
                }
            }
            return geohashList;
        }

        /// <summary>
        /// 根据输入的经纬度，计算两点间的距离(m)
        /// </summary>
        /// <param name="startPoint"></param>
        /// <param name="endPoint"></param>
        /// <returns></returns>
        public static double GetDistanceByLatLng(GeoPoint startPoint, GeoPoint endPoint)
        {
            const double RADIUS = 6378100.0;
            var dLat = (startPoint.Lat - endPoint.Lat) * Math.PI / 180;
            var dLng = (startPoint.Lng - endPoint.Lng) * Math.PI / 180;
            var angle = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) + Math.Cos(startPoint.Lat * Math.PI / 180) * Math.Cos(endPoint.Lat * Math.PI / 180) * Math.Sin(dLng / 2) * Math.Sin(dLng / 2);
            var cosine = 2 * Math.Atan2(Math.Sqrt(angle), Math.Sqrt(1 - angle));
            var distance = Math.Round(RADIUS * cosine, 15);
            return distance;
        }

        /// <summary>
        /// 计算一组GeoHash编码间的距离(m)
        /// </summary>
        /// <param name="startGeoHash"></param>
        /// <param name="endGeoHash"></param>
        /// <returns></returns>
        internal static double GetDistanceByGeoHash(string startGeoHash, string endGeoHash)
        {
            var startPoint = DecodeGeoHash(startGeoHash);
            var endPoint = DecodeGeoHash(endGeoHash);
            return GetDistanceByLatLng(startPoint, endPoint);
        }

        /// <summary>
        /// 获取正方形区域内的四个边界点
        /// </summary>
        /// <param name="centerPoint"></param>
        /// <param name="scope"></param>
        /// <returns>
        /// boundaries'order:leftTop,leftBottom,rightTop,rightBottom
        /// </returns>
        public static GeoPoint[] GetRectBoundaryPoints(GeoPoint centerPoint, double scope)
        {
            const double RADIUS = 6378100.0;
            GeoPoint[] boundarys = new GeoPoint[4];
            var tmpPoint = new GeoPoint();
            tmpPoint.Lng = RadianToDegree(2 * Math.Asin(Math.Sin(scope / (2 * RADIUS)) / Math.Cos(centerPoint.Lat)));
            tmpPoint.Lat = RadianToDegree(scope / RADIUS);
            //left top point
            boundarys[0] = new GeoPoint(centerPoint.Lat + tmpPoint.Lat, centerPoint.Lng - tmpPoint.Lng);
            //left bottom point
            boundarys[1] = new GeoPoint(centerPoint.Lat - tmpPoint.Lat, centerPoint.Lng - tmpPoint.Lng);
            //right top point
            boundarys[2] = new GeoPoint(centerPoint.Lat + tmpPoint.Lat, centerPoint.Lng + tmpPoint.Lng);
            //right bottom point
            boundarys[3] = new GeoPoint(centerPoint.Lat - tmpPoint.Lat, centerPoint.Lng + tmpPoint.Lng);
            return boundarys;
        }

        internal static string[] GetExpandNeighbourByGeoHash(string geoHash)
        {
            string[] neighbours = new string[8];
            //topGeoHash
            neighbours[0] = GetAdjacentGeoHash(geoHash, Top);
            //rightGeoHash
            neighbours[1] = GetAdjacentGeoHash(geoHash, Right);
            //bottomGeoHash
            neighbours[2] = GetAdjacentGeoHash(geoHash, Bottom);
            //leftGeoHash
            neighbours[3] = GetAdjacentGeoHash(geoHash, Left);
            //leftTopGeoHash
            neighbours[4] = GetAdjacentGeoHash(neighbours[3], Top);
            //leftBottomGeoHash
            neighbours[5] = GetAdjacentGeoHash(neighbours[3], Bottom);
            //rightTopGeoHash
            neighbours[6] = GetAdjacentGeoHash(neighbours[1], Top);
            //rightBottomGeoHash
            neighbours[7] = GetAdjacentGeoHash(neighbours[1], Bottom);
            return neighbours;
        }

        /// <summary>
        /// 计算相邻geoHash的geohash值
        /// </summary>
        /// <param name="geoHash">当前geoHash值</param>
        /// <param name="direction">方位(Top,Right,Bottom,Left)</param>
        /// <returns></returns>
        internal static string GetAdjacentGeoHash(string geoHash, int direction)
        {
            geoHash = geoHash.ToLower();
            char lastChr = geoHash[geoHash.Length - 1];
            int type = geoHash.Length % 2;
            string nHash = geoHash.Substring(0, geoHash.Length - 1);

            if (Borders[type][direction].IndexOf(lastChr) != -1)
            {
                nHash = GetAdjacentGeoHash(nHash, direction);
            }
            return nHash + _Digits[Neighbors[type][direction].IndexOf(lastChr)];
        }
    }

    /// <summary>
    /// 地理信息点
    /// </summary>
    public class GeoPoint
    {
        //latitude
        public double Lat { get; set; }
        //longitude
        public double Lng { get; set; }

        public GeoPoint()
        { }

        public GeoPoint(double lat, double lng)
        {
            this.Lat = lat;
            this.Lng = lng;
        }

        public override string ToString()
        {
            return this.Lat + " " + this.Lng;
        }
    }
}
